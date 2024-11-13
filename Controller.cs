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
        try
        {
            switch (message.command)
            {
                case CMD.NOT_LOGIN:
                    {
                        messageNotLogin(message);
                        break;
                    }
                case CMD.SUB_COMMAND:
                    {
                        Out.Log("Login OK");
                        break;
                    }
                case CMD.SERVER_MESSAGE:
                    {
                        Out.Log(message.reader().ReadUTF());
                        break;
                    }
                default:
                    {
                        Out.Log("Command not found");
                        break;
                    }
            }
        }
        catch (Exception ex)
        {
            Out.LogError(ex.ToString());
        }
    }

    public void messageNotLogin(Message msg)
    {
        try
        {
            switch (msg.reader().ReadByte())
            {
                case CMD.LOGIN: 
                    {
                        Out.Log("Login OK");
                        break;
                    }
                case CMD.REGISTER:
                    {
                        Out.Log("Register OK");
                        break;
                    }
                default:
                    {
                        Out.Log("Command not found");
                        break;
                    }

            }
        }
        catch (Exception)
        {
        }
        finally
        {
            msg?.cleanup();
        }
    }


}