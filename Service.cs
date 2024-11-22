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

	public Message messageInGame(sbyte command){
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

	public void LoadAllPlayer(Message mss){
		try{
			int id = mss.reader().ReadInt();
			string name = mss.reader().ReadUTF();
			int energy = mss.reader().ReadInt();
			int maxEnergy = mss.reader().ReadInt();
			sbyte level = mss.reader().ReadByte();

			Out.Log("ID: " + id + " Name: " + name + " Energy: " + energy + " MaxExp: " + maxEnergy + " Level: " + level);
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

	public void clientOK(){
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

	public void CreatePlayer(string name){
		try{
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
}