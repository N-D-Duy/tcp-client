using System;

public class Controller : IMessageHandler
{

    private static Controller me;
    public static Controller gI()
    {
        me ??= new Controller();
        return me;
    }
    public void OnConnectionFail()
    {
        Out.Log("OnConnectionFail");
    }

    public void OnConnectOK()
    {
        Out.Log("OnConnectOK");
    }

    public void OnDisconnected()
    {
        Out.Log("OnDisconnected");
    }

    public void OnMessage(Message message)
    {
        Out.Log("OnMessage: " + message.command);
        try{
            switch (message.command)
            {
                case -127:
                    {
                        Out.Log("Login OK");
                        break;
                    }
                case -29:
                    {
                        Out.Log("Login Fail");
                        break;
                    }
                default:
                    {
                        Out.Log("Command not found");
                        break;
                    }
            }
        } catch (Exception ex)
        {
            Out.LogError(ex.ToString());
        }
    }
}