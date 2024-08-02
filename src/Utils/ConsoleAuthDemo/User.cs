public class User{
    public string Username{get;}
    public string Salt {get;}
    public string HashedPassword {get;}

    public User(string username, string salt, string hashedPassword)
    {
        Username = username;
        Salt = salt;
        HashedPassword = hashedPassword;
    }
}
