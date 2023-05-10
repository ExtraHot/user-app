using user_app.Models;

namespace user_app.Repositories.Interfaces;


public interface IUserGroupRepository
{
    public Task<UserGroup> GetUserGroupByCode(GroupCode code);
}