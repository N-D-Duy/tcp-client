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

    public Message messageNotLogin(sbyte command)
	{
		Message message = new Message(-29);
		message.writer().writeByte(command);
		return message;
	}

    public void login(string username, string pass, string version)
	{
		// gI().setClientType();
		try
		{
			Message message = messageNotLogin(-127);
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
}