using user_app.Dtos;
using user_app.Exceptions;
using user_app.Models;
using user_app.Repositories.Interfaces;
using user_app.Services.Interfaces;

namespace user_app.Services.Implementation;

using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;

public class UserService: IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IUserGroupRepository _userGroupRepository;
    private readonly IUserStateRepository _userStateRepository;

    private readonly ILogger _logger;
    private readonly ConcurrentDictionary<string, bool> _reserved = new();
    private const int Delay = 5000;

    public UserService(IUserRepository userRepository, IUserGroupRepository userGroupRepository, 
                       IUserStateRepository userStateRepository, ILogger<UserService> logger)
    {
        _userRepository = userRepository;
        _userGroupRepository = userGroupRepository;
        _userStateRepository = userStateRepository;
        _logger = logger;
    }
    
    private DateTime GetCurrentTime() => DateTime.Now;

    private void ReserveLogin(string login)
    {
        var isAdded = _reserved.TryAdd(login, default);
        if (!isAdded)
            throw new InvalidOperationException($"Login {login} was reserved by other users");
    }

    private void Cancel(string login)
    {
        var isSuccessfullyRemoved = _reserved.TryRemove(login, out _);
        if (!isSuccessfullyRemoved)
            throw new InvalidOperationException($"No reserved login {login}");
    }

    private bool IsLoginReservedByOtherUser(string login)
    {
        _logger.LogInformation("IsLoginReservedByOtherUser");
        return _reserved.ContainsKey(login);
    }

    public async Task<UserDto> GetUserById(Guid id)
    {
        _logger.LogInformation($"Trying get user by id {id}");
        var user = await _userRepository.GetUserById(id);
        if (user is null)
            throw new UserNotFoundException(id);
        var userDto = new UserDto
        {
            UserId = user.UserId,
            Login = user.Login,
            CreatedDate = user.CreatedDate,
            UserGroup = user.UserGroup.DisplayCode,
            UserState = user.UserState.DisplayCode
        };
        return userDto;
    }

    public async Task<IEnumerable<UserDto>> GetAllUsers()
    {
        _logger.LogInformation("Getting all users");
        var users = await _userRepository.GetAllUsers();
        var userDtos = users.Select(user =>
            new UserDto
            {
                UserId = user.UserId,
                Login = user.Login,
                CreatedDate = user.CreatedDate,
                UserGroup = user.UserGroup.DisplayCode,
                UserState = user.UserState.DisplayCode
            }
        );
        return userDtos;
    }

    public async Task<IEnumerable<UserDto>> GetPagedUsers(Page page)
    {
        _logger.LogInformation("Getting paged users");
        var pagedUser = await _userRepository.GetPagedUsers(page.Offset, page.PageSize);
        var userDtos = pagedUser.Select(user =>
            new UserDto
            {
                UserId = user.UserId,
                Login = user.Login,
                CreatedDate = user.CreatedDate,
                UserGroup = user.UserGroup.DisplayCode,
                UserState = user.UserState.DisplayCode
            }
        );
        return userDtos;
    }

    public async Task<UserDto> CreateUser(CreateUserDto createUserDto)
    {
        _logger.LogInformation("Trying to create new user...");
        _logger.LogInformation("Login reserve checking...");
        if (IsLoginReservedByOtherUser(createUserDto.Login))
            throw new LoginAlreadyReservedException(createUserDto.Login);
        
        ReserveLogin(createUserDto.Login);
        await Task.Delay(Delay);
        
        _logger.LogInformation("Login exist checking...");
        if (await _userRepository.IsLoginExist(createUserDto.Login))
            throw new LoginAlreadyExistException(createUserDto.Login);

        _logger.LogInformation("Admin checking...");
        if (createUserDto.GetGroup() == GroupCode.Admin)
        {
            var admin = await _userRepository.IsAdminExist();
            if (admin)
                throw new AdminAlreadyExistException();
        }
        var sha256 = SHA256.Create();
        var hashedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(createUserDto.Password)));
        var userState = await _userStateRepository.GetUserStateByCode(StateCode.Active);
        var userGroup = await _userGroupRepository.GetUserGroupByCode(createUserDto.GetGroup());
        var user = new User
        {
            Login = createUserDto.Login,
            Password = hashedPassword,
            CreatedDate = GetCurrentTime(),
            UserState = userState,
            UserGroup = userGroup
        };
        var createdUser = await _userRepository.CreateUser(user);
        _logger.LogInformation("New user created    ");
        Cancel(createUserDto.Login);
        var userDto = new UserDto
        {
            UserId = createdUser.UserId,
            Login = createdUser.Login,
            CreatedDate = createdUser.CreatedDate,
            UserGroup = createdUser.UserGroup.DisplayCode,
            UserState = createdUser.UserState.DisplayCode
        };
        return userDto;
    }

    public async Task<UserDto> DeleteUser(Guid id)
    {
        _logger.LogInformation($"Trying to delete user with id {id}");
        var user = await _userRepository.GetUserById(id);
        if (user is null)
            throw new UserNotFoundException(id);
        var userState = await _userStateRepository.GetUserStateByCode(StateCode.Blocked);
        user.UserState = userState;
        user.UserState.Description = "Deleted";
        var removedUser = await _userRepository.UpdateUser(user);
        var userDto = new UserDto
        {
            UserId = removedUser.UserId,
            Login = removedUser.Login,
            CreatedDate = removedUser.CreatedDate,
            UserGroup = removedUser.UserGroup.DisplayCode,
            UserState = removedUser.UserState.DisplayCode
        };
        return userDto;
    }
}