namespace user_app.Exceptions;

public class LoginAlreadyExistException: Exception
{
    public override string Message => $"Login already exists {_login}";
    private readonly string _login;
    
    public LoginAlreadyExistException(string login)
    {
        _login = login;
    }
}