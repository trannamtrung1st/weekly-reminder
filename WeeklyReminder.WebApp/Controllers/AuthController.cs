using Microsoft.AspNetCore.Mvc;
using WeeklyReminder.WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using WeeklyReminder.Application.Services.Abstracts;

namespace WeeklyReminder.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController, Authorize]
public class AuthController : ControllerBase
{
    private readonly ISettingsService _settingsService;

    public AuthController(ISettingsService settingsService)
    {
        _settingsService = settingsService;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
        var credentials = await _settingsService.GetSystemCredentialsAsync();

        if ((credentials.Username is null || model.Username == credentials.Username)
            && (credentials.Password is null || model.Password == credentials.Password))
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
                new Claim(ClaimTypes.Email, credentials.Email),
                new Claim(ClaimTypes.Role, "User")
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7)
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            return Redirect("/");
        }

        return Redirect("/auth/login?error=Invalid username or password");
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/auth/login");
    }
}
