public class CMD
{
    // Main command for in-game feat
    public const sbyte IN_GAME = 1;

    // Sub commands
    public const sbyte ITEM_BAG_CLEAR = -1;
    public const sbyte ITEM_BAG_ADD = 1;
    public const sbyte ME_UP_COIN_BAG = -2;
    public const sbyte ME_LOAD_INFO = 2;
    public const sbyte ME_LOAD_LEVEL = -3;
    public const sbyte ME_LOAD_ALL = -3;
    public const sbyte PLAYER_UP_EXP = -4;
    public const sbyte PLAYER_LOAD_ALL = 4;
    public const sbyte CREATE_PLAYER = -5;
    public const sbyte UPDATE_ENERGY = 5;
    public const sbyte PLAYER_LOAD_ENERGY = 6;
    public const sbyte PLAYER_SAVE_WAVE = -6;

    // Main command for not in-game feat
    public const sbyte NOT_IN_GAME = -1;
    

    // Sub commands for not in-game
    public const sbyte FULL_SIZE = 0;
    public const sbyte LOGIN = -1;
    public const sbyte LOGIN_OK = 1;
    public const sbyte CLIENT_INFO = -2;
    public const sbyte GET_SESSION_ID = 2;
    public const sbyte SERVER_DIALOG = -3;
    public const sbyte SERVER_ALERT = 3;
    public const sbyte SERVER_MESSAGE = -4;
    public const sbyte SHOW_WAIT = 4;
    public const sbyte ALERT_MESSAGE = -5;
    public const sbyte REGISTER = -6;
    public const sbyte REGISTER_OK = 6;
    public const sbyte CLIENT_OK = -7;
}
