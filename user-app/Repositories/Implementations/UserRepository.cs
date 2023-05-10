using Microsoft.EntityFrameworkCore;
using user_app.Context;
using user_app.Models;
using user_app.Repositories.Interfaces;

namespace user_app.Repositories.Implementations;

public class UserRepository : IUserRepository
{
    private readonly ApplicationContext _context;

    public UserRepository(ApplicationContext context)
    {
        _context = context;
    }
    
    private IQueryable<User> Users => _context.Users
                                              .Include(x => x.UserState)
                                              .Where(x => x.UserState.Code != StateCode.Blocked)
                                              .Include(x => x.UserGroup);

    public async Task<User> GetUserById(Guid id)
    {
        return await Users.FirstAsync(user => user.UserId == id);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await Users.ToListAsync();
    }
    
    public async Task<IEnumerable<User>> GetPagedUsers(int offset, int pageSize) 
    {
        return await Users.OrderByDescending(user => user.UserId)
                          .Skip(offset)
                          .Take(pageSize)
                          .ToListAsync();
    }
    
    public async Task<User> CreateUser(User user) 
    {
        var createdUser = _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return createdUser.Entity;
    }
    
    public async Task<User> UpdateUser(User user) 
    {
        var updatedEntry = _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return updatedEntry.Entity;
    }
    
    public async Task<User> DeleteUser(User user) 
    {
        var removedEntry = _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return removedEntry.Entity;
    }

    public async Task<User?> TryGetById(Guid id) 
    {
        return await Users.FirstOrDefaultAsync(user => user.UserId == id);
    }
    
    public async Task<bool> IsAdminExist() 
    {
        return await Users.AnyAsync(user => user.UserGroup.Code == GroupCode.Admin);
    }
    
    public async Task<bool> IsLoginExist(string login) 
    {
        return await Users.AnyAsync(user => user.Login == login);
    }
}