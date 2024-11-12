public class Message
{
	public sbyte command;

	private MyReader dis;

	private MyWriter dos;

	public Message(int command)
	{
		this.command = (sbyte)command;
		dos = new MyWriter();
	}

	public Message()
	{
		dos = new MyWriter();
	}

	public Message(sbyte command)
	{
		this.command = command;
		dos = new MyWriter();
	}

	public Message(sbyte command, sbyte[] data)
	{
		this.command = command;
		dis = new MyReader(data);
	}

	public sbyte[] getData()
	{
		return dos.getData();
	}

	public MyReader reader()
	{
		return dis;
	}

	public MyWriter writer()
	{
		return dos;
	}

	public void cleanup()
	{
	}
}
