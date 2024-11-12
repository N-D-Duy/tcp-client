using System.Text;

public class MyReader
{
	public sbyte[] buffer;

	private int posRead;

	private int posMark;

	private static readonly string fileName;

	private static readonly int status;

	public MyReader()
	{
	}

	public MyReader(sbyte[] data)
	{
		buffer = data;
	}

	// public MyReader(string filename)
	// {
	// 	TextAsset textAsset = (TextAsset)Resources.Load(filename, typeof(TextAsset));
	// 	buffer = mSystem.convertToSbyte(textAsset.bytes);
	// }

	public sbyte ReadSByte()
	{
		if (posRead < buffer.Length)
		{
			return buffer[posRead++];
		}
		posRead = buffer.Length;
		return 0;
	}

	public sbyte Readsbyte()
	{
		return ReadSByte();
	}

	public sbyte ReadByte()
	{
		return ReadSByte();
	}

	public void Mark(int readlimit)
	{
		posMark = posRead;
	}

	public void Reset()
	{
		posRead = posMark;
	}

	public byte ReadUnsignedByte()
	{
		return ConvertSbyteToByte(ReadSByte());
	}

	public short ReadShort()
	{
		short num = 0;
		for (int i = 0; i < 2; i++)
		{
			num = (short)(num << 8);
			num = (short)(num | (short)(0xFF & buffer[posRead++]));
		}
		return num;
	}

	public ushort ReadUnsignedShort()
	{
		ushort num = 0;
		for (int i = 0; i < 2; i++)
		{
			num = (ushort)(num << 8);
			num = (ushort)(num | (ushort)(0xFFu & (uint)buffer[posRead++]));
		}
		return num;
	}

	public int ReadInt()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			num <<= 8;
			num |= 0xFF & buffer[posRead++];
		}
		return num;
	}

	public long ReadLong()
	{
		long num = 0L;
		for (int i = 0; i < 8; i++)
		{
			num <<= 8;
			num |= 0xFF & buffer[posRead++];
		}
		return num;
	}

	public bool ReadBool()
	{
		return ReadSByte() > 0;
	}

	public bool ReadBoolean()
	{
		return ReadSByte() > 0;
	}

	public string ReadString()
	{
		short num = ReadShort();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = ConvertSbyteToByte(ReadSByte());
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(array);
	}

	public string ReadStringUTF()
	{
		short num = ReadShort();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = ConvertSbyteToByte(ReadSByte());
		}
		UTF8Encoding uTF8Encoding = new UTF8Encoding();
		return uTF8Encoding.GetString(array);
	}

	public string ReadUTF()
	{
		return ReadStringUTF();
	}

	public int Read()
	{
		if (posRead < buffer.Length)
		{
			return ReadSByte();
		}
		return -1;
	}

	public int Read(ref sbyte[] data)
	{
		if (data == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = ReadSByte();
			if (posRead > buffer.Length)
			{
				return -1;
			}
			num++;
		}
		return num;
	}

	public int Readz(sbyte[] data)
	{
		if (data == null)
		{
			return 0;
		}
		int num = 0;
		for (int i = 0; i < data.Length; i++)
		{
			data[i] = ReadSByte();
			if (posRead > buffer.Length)
			{
				return -1;
			}
			num++;
		}
		return num;
	}

	public void ReadFully(ref sbyte[] data)
	{
		if (data != null && data.Length + posRead <= buffer.Length)
		{
			for (int i = 0; i < data.Length; i++)
			{
				data[i] = ReadSByte();
			}
		}
	}

	public int Available()
	{
		return buffer.Length - posRead;
	}

	public static byte ConvertSbyteToByte(sbyte var)
	{
		if (var > 0)
		{
			return (byte)var;
		}
		return (byte)(var + 256);
	}

	public static byte[] ConvertSbyteToByte(sbyte[] var)
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

	public void Close()
	{
		buffer = null;
	}

	public void close()
	{
		buffer = null;
	}
}
