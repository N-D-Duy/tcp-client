using System;

public class Service
{
	private ISession session = Session.gI();

	protected static Service instance;

	public static Service gI()
	{
		instance ??= new Service();
		return instance;
	}

	public Message messageNotInGame(sbyte command)
	{
		Message message = new Message(CMD.NOT_IN_GAME);
		message.writer().writeByte(command);
		return message;
	}

	public Message messageInGame(sbyte command)
	{
		Message message = new Message(CMD.IN_GAME);
		message.writer().writeByte(command);
		return message;
	}

	public void login(string username, string pass, string version)
	{
		// gI().setClientType();
		try
		{
			Message message = messageNotInGame(CMD.LOGIN);
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			message.writer().writeUTF(version);
			message.writer().writeUTF(string.Empty);
			message.writer().writeUTF(string.Empty);
			message.writer().writeUTF("random");
			message.writer().writeByte(0);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void LoadAllPlayer(Message mss)
	{
		try
		{
			int id = mss.reader().ReadInt();
			string name = mss.reader().ReadUTF();
			int energy = mss.reader().ReadInt();
			int maxEnergy = mss.reader().ReadInt();
			sbyte level = mss.reader().ReadByte();

			//load wave data
			int wave = mss.reader().ReadInt();
			int exp = mss.reader().ReadInt();
			int size = mss.reader().ReadByte();
			Wave waveState = new Wave(wave, exp, new string[size]);
			for (int i = 0; i < size; i++)
			{
				waveState.inventory[i] = mss.reader().ReadUTF();
			}

			for (int i = 0; i < size; i++)
			{
				Out.Log(waveState.inventory[i]);
			}
			// AddItem(3, "item_1", "item_2", "item_3");
			// AddCoin();
			// RemoveItem(2);
			// SaveWaveState();
			useEnergy();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	private void AddItem(int number, params string[] items)
	{
		try
		{
			Message message = messageInGame(CMD.ITEM_BAG_ADD);
			message.writer().writeByte(number);
			for (int i = 0; i < items.Length; i++)
			{
				message.writer().writeUTF(items[i]);
			}
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void SaveWaveState()
	{
		try
		{
			Message message = messageInGame(CMD.PLAYER_SAVE_WAVE);
			message.writer().writeInt(1);
			message.writer().writeInt(1000);
			message.writer().writeByte(3);
			for (int i = 0; i < 3; i++)
			{
				message.writer().writeUTF("item_1");
			}
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}

	}

	public void RemoveItem(int index)
	{
		try
		{
			Message message = messageInGame(CMD.ITEM_BAG_CLEAR);
			message.writer().writeByte(index);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	private void AddCoin()
	{
		try
		{
			Message message = messageInGame(CMD.ME_UP_COIN_BAG);
			message.writer().writeLong(10000);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void register(string username, string pass)
	{
		try
		{
			Message message = messageNotInGame(CMD.REGISTER);
			message.writer().writeUTF(username);
			message.writer().writeUTF(pass);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void clientOK()
	{
		try
		{
			Message message = messageNotInGame(CMD.CLIENT_OK);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void CreatePlayer(string name)
	{
		try
		{
			Message message = messageInGame(CMD.CREATE_PLAYER);
			message.writer().writeUTF(name);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}



	public void getClientInfo()
	{
		try
		{
			Message message = messageNotInGame(CMD.CLIENT_INFO);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void MeUpCoinBag(Message message)
	{
		try
		{
			int coin = message.reader().ReadInt();
			Out.Log("Coin add: " + coin);
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void MeLoadInfo(Message message)
	{
		try
		{
			int coin = message.reader().ReadInt();
			Out.Log("Coin: " + coin);
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void LoadEnergy(Message message)
	{
		try
		{
			int energy = message.reader().ReadInt();
			int maxEnergy = message.reader().ReadInt();
			long lastTimeUpdate = message.reader().ReadLong();
			Out.Log("Energy: " + energy + "/" + maxEnergy + " Last time update: " + lastTimeUpdate);
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}
	}

	public void useEnergy(){
		try
		{
			Message message = messageInGame(CMD.UPDATE_ENERGY);
			message.writer().writeInt(-20);
			session.SendMessage(message);
			message.cleanup();
		}
		catch (Exception ex)
		{
			Out.LogError(ex.ToString());
		}

	}
}