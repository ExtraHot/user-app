using Microsoft.EntityFrameworkCore;
using user_app.Context;
using user_app.Models;
using user_app.Repositories.Interfaces;

namespace user_app.Repositories.Implementations;

public class UserGroupRepository: IUserGroupRepository
{
    private readonly ApplicationContext _context;

    public UserGroupRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    private IQueryable<UserGroup> UserGroups => _context.UserGroups;
    
    public async Task<UserGroup> GetUserGroupByCode(GroupCode code)
    {
        return await UserGroups.FirstAsync(userGroup => userGroup.Code == code);
    }
}