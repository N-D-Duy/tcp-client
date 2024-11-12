public interface IMessageHandler
{
	void OnMessage(Message message);

	void OnConnectionFail();

	void OnDisconnected();

	void OnConnectOK();
}
