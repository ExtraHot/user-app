namespace user_app.Exceptions;

public class LoginAlreadyReservedException: Exception
{
    public override string Message => $"Login already reserved {_login}";

    private readonly string _login;

    public LoginAlreadyReservedException(string login)
    {
        _login = login;
    }
}