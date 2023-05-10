namespace user_app.Exceptions;

public class UserNotFoundException: Exception
{
    public override string Message => $"No user with id {_userId}";

    private readonly Guid _userId;

    public UserNotFoundException(Guid userId)
    {
        _userId = userId;
    }
}