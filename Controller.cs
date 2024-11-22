using System;

public class Controller : IMessageHandler
{

    private static Controller me;
    private Service service = Service.gI();
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
                case CMD.NOT_IN_GAME:
                    {
                        messageNotInGame(message);
                        break;
                    }
                case CMD.IN_GAME:
                    {
                        messageInGame(message);
                        break;
                    }
                case CMD.SERVER_MESSAGE:
                    {
                        Out.Log(message.reader().ReadUTF());
                        break;
                    }
                case CMD.SERVER_DIALOG:
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

    private void messageInGame(Message message)
    {
        try
        {
            switch(message.reader().ReadByte())
            {
                case CMD.CREATE_PLAYER:
                    {
                        service.CreatePlayer("duynguyen");
                        break;
                    }
                case CMD.PLAYER_LOAD_ALL:
                    {
                        service.LoadAllPlayer(message);
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
        finally
        {
            message?.cleanup();
        }
    }

    public void messageNotInGame(Message msg)
    {
        try
        {
            switch (msg.reader().ReadByte())
            {
                case CMD.LOGIN_OK: 
                    {
                        service.clientOK();
                        break;
                    }
                case CMD.REGISTER_OK:
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