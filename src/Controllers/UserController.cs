using Microsoft.AspNetCore.Mvc;
using backend.Services;
using Microsoft.AspNetCore.Authorization;

using backend.Helper;

namespace backend.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UserService _userService;
    private readonly JwtHelper _jwt = new();

    public UsersController(UserService userService)
    {
        _userService = userService;
    }

    
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(string username, string password)
    {
        var user = await _userService.RegisterUser(username, password);
        if (user == null) return BadRequest("Registration failed.");
        return Ok(user);
    }

   
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _userService.AuthenticateUser(username, password);
        if (user == null) return Unauthorized();

        var accessToken = _jwt.GenerateAccessToken(username, "User");
        var refreshToken = _jwt.GenerateRefreshToken();
        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }


    [Authorize]
    [HttpGet("hello")]
    public IActionResult Hello()
    {
        return Ok(new { message = "Hello :D you are authorized" });
    }

    [Authorize]
    [HttpGet("getAll")]
    public IActionResult GetAll()
    {
        return Ok(new { users = _userService.GetAllUsers() });
    }

    [Authorize]
    [HttpGet("getByUsername")]
    public IActionResult GetByUsername(string username)
    {
        return Ok(new { users = _userService.GetByUsername(username) });
    }
}
