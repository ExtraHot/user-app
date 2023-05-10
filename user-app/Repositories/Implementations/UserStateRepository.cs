using Microsoft.EntityFrameworkCore;
using user_app.Context;
using user_app.Models;
using user_app.Repositories.Interfaces;

namespace user_app.Repositories.Implementations;

public class UserStateRepository: IUserStateRepository
{
    private readonly ApplicationContext _context;

    public UserStateRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    private IQueryable<UserState> UserStates => _context.UserStates;
    
    public async Task<UserState> GetUserStateByCode(StateCode code)
    {
        return await UserStates.FirstAsync(userState => userState.Code == code);
    }
}