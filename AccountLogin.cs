public class AccountLogin
{
    public int ID;
    public string User;
    public string Password;
    public bool[] Config;
    public string IP;
    public int Port;

    public AccountLogin()
    {

    }

    public AccountLogin(int _ID, string _User, string _Password, bool[] _Config, string _IP, int _Port)
    {
        ID = _ID;
        User = _User;
        Password = _Password;
        IP = _IP;
        Port = _Port;
        Config = _Config;
    }
}

