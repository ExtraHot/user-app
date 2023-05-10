using user_app.Models;

namespace user_app.Repositories.Interfaces;

public interface IUserStateRepository
{
    public Task<UserState> GetUserStateByCode(StateCode code);
}