using Microsoft.AspNetCore.Mvc;
using Moq;
using user_app.Controllers;
using user_app.Dtos;
using user_app.Models;
using user_app.Services.Interfaces;

namespace user_unit_app;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetUserById_ReturnsOkObjectResult_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId };
        _userServiceMock.Setup(x => x.GetUserById(userId)).ReturnsAsync(new UserDto { UserId = userId });

        // Act
        var result = await _controller.GetUserById(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.UserId);
    }

    [Fact]
    public async Task GetAllUsers_ReturnsOkObjectResult_WhenUsersExist()
    {
        // Arrange
        var users = new List<User> { new User { UserId = Guid.NewGuid() }, new User { UserId = Guid.NewGuid() } };
        _userServiceMock.Setup(x => x.GetAllUsers()).ReturnsAsync(users.Select(u => new UserDto { UserId = u.UserId }));

        // Act
        var result = await _controller.GetAllUsers();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        Assert.Equal(users.Count, returnedUsers.Count());
    }
    
    [Fact]
    public async Task GetPagedUsers_ReturnsOkObjectResult_WhenPageIsValid()
    {
        // Arrange
        var page = new Page { PageNumber = 1, PageSize = 10 };
        var users = new List<User> { new User { UserId = Guid.NewGuid() }, new User { UserId = Guid.NewGuid() } };
        _userServiceMock.Setup(x => x.GetPagedUsers(page)).ReturnsAsync(users.Select(u => new UserDto { UserId = u.UserId }));


        // Act
        var result = await _controller.GetPagedUsers(page);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUsers = Assert.IsAssignableFrom<IEnumerable<UserDto>>(okResult.Value);
        Assert.Equal(users.Count, returnedUsers.Count());
    }

    [Fact]
    public async Task GetPagedUsers_ReturnsBadRequestResult_WhenPageIsInvalid()
    {
        // Arrange
        var page = new Page { PageNumber = -1, PageSize = -1 };

        // Act
        var result = await _controller.GetPagedUsers(page);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestResult>(result);
    }
    
    [Fact]
    public async Task CreateUser_ReturnsBadRequestObjectResult_WhenModelStateIsInvalid()
    {
        // Arrange
        var createUserDto = new CreateUserDto { Login = ""};
        _controller.ModelState.AddModelError("Name", "The Name field is required.");
        _controller.ModelState.AddModelError("Email", "The Email field is not a valid e-mail address.");

        // Act
        var result = await _controller.CreateUser(createUserDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var errorMessages = Assert.IsAssignableFrom<IEnumerable<string>>(badRequestResult.Value);
        Assert.Equal(2, errorMessages.Count());
    }

    [Fact]
    public async Task DeleteUser_ReturnsOkObjectResult_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var user = new User { UserId = userId };
        _userServiceMock.Setup(x => x.DeleteUser(userId)).ReturnsAsync(new UserDto { UserId = userId });

        // Act
        var result = await _controller.DeleteUser(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var returnedUser = Assert.IsType<UserDto>(okResult.Value);
        Assert.Equal(userId, returnedUser.UserId);
    }
}