public interface ISession
{
	bool IsConnected();

	void SetHandler(IMessageHandler messageHandler);

	void Connect(string host, int port);

	void SendMessage(Message message);

	void Close();
}