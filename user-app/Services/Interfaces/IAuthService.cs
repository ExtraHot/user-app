namespace user_app.Services.Interfaces;

public interface IAuthService
{
    public bool HasUserAccess(string username, string password);
}