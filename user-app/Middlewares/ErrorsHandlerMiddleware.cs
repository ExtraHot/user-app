namespace user_app.Middlewares;

public class ErrorsHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ErrorsHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next.Invoke(context);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync(e.Message);
        }
    }
}