using user_app.Services.Interfaces;

namespace user_app.Middlewares;

using System.Text;

public class BasicAuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthService _authService;

    public BasicAuthMiddleware(RequestDelegate next, IAuthService authService)
    {
        _next = next;
        _authService = authService;
    }

    public async Task Invoke(HttpContext context)
    {
        var authHeader = context.Request.Headers.Authorization.ToString();
        if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic "))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        string authData;
        try
        {
            var bytesAuthData = Convert.FromBase64String(authHeader.Split(' ')[1]);
            authData = Encoding.UTF8.GetString(bytesAuthData);
        }
        catch
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        if (!authData.Contains(':'))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return;
        }

        var splittedData = authData.Split(':');
        var (login, password) = (splittedData[0], splittedData[1]);

        if (!_authService.HasUserAccess(login, password))
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            return;
        }
        await _next.Invoke(context);
    }
}