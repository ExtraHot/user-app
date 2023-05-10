using user_app.Dtos;
using user_app.Services.Interfaces;

namespace user_app.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        var user = await _userService.GetUserById(id);
        return Ok(user);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedUsers([FromQuery] Page page)
    {
        if (!page.IsInValidState)
            return BadRequest();
        var users = await _userService.GetPagedUsers(page);
        return Ok(users);
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage)));

        var user = await _userService.CreateUser(createUserDto);
       
        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        var user = await _userService.DeleteUser(id);
        return Ok(user);
    }
}