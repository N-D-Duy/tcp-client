using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Session : ISession
{
	public class Sender
	{
		private readonly List<Message> sendingMessage;

		public Sender()
		{
			sendingMessage = new List<Message>();
		}

		public void AddMessage(Message message)
		{
			sendingMessage.Add(message);
		}

		public void Run()
		{
			while (connected)
			{
				try
				{
					if (getKeyComplete)
					{
						while (sendingMessage.Count > 0)
						{
							Message m = sendingMessage[0];
							doSendMessage(m);
							sendingMessage.RemoveAt(0);
						}
					}
					try
					{
						Thread.Sleep(10);
					}
					catch (Exception ex)
					{
						Out.LogError(ex.ToString());
					}
				}
				catch (Exception ex)
				{
					Out.LogError("err sender:" + ex.ToString());
				}
			}
		}
	}

	private class MessageCollector
	{
		public void Run()
		{
			try
			{
				while (connected)
				{
					Message message = readMessage();
					if (message == null)
					{
						break;
					}
					try
					{
						if (message.command == CMD.GET_SESSION_ID)
						{
							getKey(message);
						}
						else
						{
							Out.Log("onRecieveMsg: " + message.command);
							onRecieveMsg(message);
						}
					}
					catch (Exception ex)
					{
						Out.LogError("err collector:" + ex.ToString());
					}
					try
					{
						Thread.Sleep(5);
					}
					catch (Exception ex)
					{
						Out.LogError("Run err:" + ex.ToString());
					}
				}
			}
			catch (Exception ex3)
			{
				Out.LogError("err collector:" + ex3.ToString());
			}
			if (!connected)
			{
				return;
			}
			if (messageHandler != null)
			{
				if (currentTimeMillis() - timeConnected > 500)
				{
					messageHandler.OnDisconnected();
				}
				else
				{
					messageHandler.OnConnectionFail();
				}
			}
			if (sc != null)
			{
				cleanNetwork();
			}
		}

		private void getKey(Message message)
		{
			try
			{
				sbyte b = message.reader().ReadSByte();
				key = new sbyte[b];
				for (int i = 0; i < b; i++)
				{
					key[i] = message.reader().ReadSByte();
				}
				for (int j = 0; j < key.Length - 1; j++)
				{
					key[j + 1] ^= key[j];
				}
				getKeyComplete = true;
			}
			catch (Exception ex)
			{
				Out.LogError("err get key:" + ex.ToString());
			}
		}

		/*
		b: command chinh
		num2: so luong byte cua data
		array: data
		*/
		private Message readMessage()
		{
			int num = 200;
			try
			{
				sbyte b = dis.ReadSByte();
				num = b;
				if (getKeyComplete)
				{
					b = readKey(b);
				}
				int num2;
				if (b == CMD.FULL_SIZE)
				{
					b = dis.ReadSByte();
					if (getKeyComplete)
					{
						b = readKey(b);
					}
					sbyte b2 = readKey(dis.ReadSByte());
					sbyte b3 = readKey(dis.ReadSByte());
					sbyte b4 = readKey(dis.ReadSByte());
					sbyte b5 = readKey(dis.ReadSByte());
					num2 = ((b2 & 0xFF) << 24) | ((b3 & 0xFF) << 16) | ((b4 & 0xFF) << 8) | (b5 & 0xFF);
				}
				else if (getKeyComplete)
				{
					sbyte b6 = dis.ReadSByte();
					sbyte b7 = dis.ReadSByte();
					num2 = ((readKey(b6) & 0xFF) << 8) | (readKey(b7) & 0xFF);
				}
				else
				{
					sbyte b8 = dis.ReadSByte();
					sbyte b9 = dis.ReadSByte();
					num2 = (b8 & 0xFF00) | (b9 & 0xFF);
				}
				sbyte[] array = new sbyte[num2];
				byte[] src = dis.ReadBytes(num2);
				Buffer.BlockCopy(src, 0, array, 0, num2);
				recvByteCount += 5 + num2;
				int num5 = recvByteCount + sendByteCount;
				strRecvByteCount = num5 / 1024 + "." + num5 % 1024 / 102 + "Kb";
				if (getKeyComplete)
				{
					for (int i = 0; i < array.Length; i++)
					{
						array[i] = readKey(array[i]);
					}
				}
				return new Message(b, array);
			}
			catch (Exception ex)
			{
				Out.LogError(num + "-----------------:" + ex.ToString());
			}
			return null;
		}
	}

	protected static Session instance = new Session();

	private static NetworkStream dataStream;

	private static BinaryReader dis;

	private static BinaryWriter dos;

	public static IMessageHandler messageHandler;

	private static TcpClient sc;

	public static bool connected;

	public static bool connecting;

	private static Sender sender = new Sender();

	public static Thread initThread;

	public static Thread collectorThread;

	public static Thread sendThread;

	public static int sendByteCount;

	public static int recvByteCount;

	private static bool getKeyComplete;

	public static sbyte[] key = null;

	private static sbyte curR;

	private static sbyte curW;

	private static int timeConnected;

	public static string strRecvByteCount = string.Empty;

	public static bool isCancel;

	private string host;

	private int port;

	public static MyVector recieveMsg = new MyVector();

	public Session()
	{
		Out.Log("Start Session");
	}

	public static Session gI()
	{
		return instance;
	}

	public bool IsConnected()
	{
		return connected;
	}

	public void SetHandler(IMessageHandler msgHandler)
	{
		messageHandler = msgHandler;
	}

	public void Connect(string host, int port)
	{
		if (!connected && !connecting)
		{
			this.host = host;
			this.port = port;
			getKeyComplete = false;
			sc = null;
			
			initThread = new Thread(NetworkInit);
			initThread.Start();
		}
	}

	private void NetworkInit()
	{
		isCancel = false;
		connecting = true;
		Thread.CurrentThread.Priority = ThreadPriority.Highest;
		connected = true;
		try
		{
			doConnect(host, port);
			messageHandler.OnConnectOK();
		}
		catch (Exception ex)
		{
			if (messageHandler != null)
			{
				Close();
				Out.LogError("err connect:" + ex.ToString());
				messageHandler.OnConnectionFail();
			}
		}
	}

	public void doConnect(string host, int port)
	{
		sc = new TcpClient();
		sc.Connect(host, port);
		sc.ReceiveBufferSize = 128000;
		dataStream = sc.GetStream();
		dis = new BinaryReader(dataStream, new UTF8Encoding());
		dos = new BinaryWriter(dataStream, new UTF8Encoding());
		new Thread(sender.Run).Start();
		MessageCollector @object = new MessageCollector();
		collectorThread = new Thread(@object.Run);
		collectorThread.Start();
		timeConnected = currentTimeMillis();
		connecting = false;
		doSendMessage(new Message(CMD.GET_SESSION_ID));
	}

	public void SendMessage(Message message)
	{
		sender.AddMessage(message);
	}

	private static void doSendMessage(Message m)
	{
		Out.Log("doSendMessage: " + m.command);
		sbyte[] data = m.getData();
		try
		{
			if (getKeyComplete)
			{
				sbyte value = writeKey(m.command);
				dos.Write(value);
			}
			else
			{
				dos.Write(m.command);
			}
			if (data != null)
			{
				int num = data.Length;
				if (m.command == -31)
				{
					dos.Write((short)num);
				}
				else if (getKeyComplete)
				{
					int num2 = writeKey((sbyte)(num >> 8));
					dos.Write((sbyte)num2);
					int num3 = writeKey((sbyte)(num & 0xFF));
					dos.Write((sbyte)num3);
				}
				else
				{
					dos.Write((ushort)num);
				}
				if (getKeyComplete)
				{
					for (int i = 0; i < data.Length; i++)
					{
						sbyte value2 = writeKey(data[i]);
						dos.Write(value2);
					}
				}
				sendByteCount += 5 + data.Length;
			}
			else
			{
				if (getKeyComplete)
				{
					int num4 = 0;
					int num5 = writeKey((sbyte)(num4 >> 8));
					dos.Write((sbyte)num5);
					int num6 = writeKey((sbyte)(num4 & 0xFF));
					dos.Write((sbyte)num6);
				}
				else
				{
					dos.Write((ushort)0);
				}
				sendByteCount += 5;
			}
			dos.Flush();
		}
		catch (Exception ex)
		{
			Out.LogError("err send message:" + ex.ToString());
		}
	}

	public static sbyte readKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte result = (sbyte)((array[curR++] & 0xFF) ^ (b & 0xFF));
		if (curR >= key.Length)
		{
			curR %= (sbyte)key.Length;
		}
		return result;
	}

	public static sbyte writeKey(sbyte b)
	{
		sbyte[] array = key;
		sbyte result = (sbyte)((array[curW++] & 0xFF) ^ (b & 0xFF));
		if (curW >= key.Length)
		{
			curW %= (sbyte)key.Length;
		}
		return result;
	}

	public static void onRecieveMsg(Message msg)
	{
		if (Thread.CurrentThread.Name == Program.mainThreadName)
		{
			messageHandler.OnMessage(msg);
		}
		else
		{
			recieveMsg.AddElement(msg);
		}
	}

	public static void update()
	{
		while (recieveMsg.Size() > 0)
		{
			Message message = (Message)recieveMsg.ElementAt(0);
			if (message == null)
			{
				recieveMsg.RemoveElementAt(0);
				break;
			}
			messageHandler.OnMessage(message);
			recieveMsg.RemoveElementAt(0);
		}
	}

	public void Close()
	{
		recieveMsg.RemoveAllElements();
		cleanNetwork();
	}

	private static void cleanNetwork()
	{
		key = null;
		curR = 0;
		curW = 0;
		try
		{
			connected = false;
			connecting = false;
			if (sc != null)
			{
				sc.Close();
				sc = null;
			}
			if (dataStream != null)
			{
				dataStream.Close();
				dataStream = null;
			}
			if (dos != null)
			{
				dos.Close();
				dos = null;
			}
			if (dis != null)
			{
				dis.Close();
				dis = null;
			}
			sendThread = null;
			collectorThread = null;
		}
		catch (Exception)
		{
		}
	}

	public static int currentTimeMillis()
	{
		return Environment.TickCount;
	}

	public static byte convertSbyteToByte(sbyte var)
	{
		if (var > 0)
		{
			return (byte)var;
		}
		return (byte)(var + 256);
	}

	public static byte[] convertSbyteToByte(sbyte[] var)
	{
		byte[] array = new byte[var.Length];
		for (int i = 0; i < var.Length; i++)
		{
			if (var[i] > 0)
			{
				array[i] = (byte)var[i];
			}
			else
			{
				array[i] = (byte)(var[i] + 256);
			}
		}
		return array;
	}
}
