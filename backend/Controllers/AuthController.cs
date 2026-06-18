using Microsoft.AspNetCore.Mvc;
using NoteAppApi.DTOs.Auth;
using NoteAppApi.Services;

namespace NoteAppApi.Controllers;

[Route("auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await _authService.RegisterAsync(request);
        
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }
}
