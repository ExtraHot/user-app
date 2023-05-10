using user_app.Models;

namespace user_app.Repositories.Interfaces;


public interface IUserRepository
{
    public Task<User> GetUserById(Guid id);
    public Task<IEnumerable<User>> GetAllUsers();
    public Task<IEnumerable<User>> GetPagedUsers(int pageNumber, int pageSize);
    public Task<User> CreateUser(User user);
    public Task<User> UpdateUser(User user);
    public Task<User> DeleteUser(User user);
    
    // ?
    public Task<User?> TryGetById(Guid id);
    public Task<bool> IsAdminExist();
    public Task<bool> IsLoginExist(string predicate);
}