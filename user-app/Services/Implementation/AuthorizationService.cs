using user_app.Services.Interfaces;

namespace user_app.Services.Implementation;

public class AuthorizationService: IAuthService
{
    public bool HasUserAccess(string username, string password)
    {
        return username == "admin" && password == "123";
    }
}