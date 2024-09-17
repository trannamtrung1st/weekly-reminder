using Microsoft.AspNetCore.Mvc;
using WeeklyReminder.WebApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace WeeklyReminder.WebApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromForm] LoginModel model)
    {
        var credentialsPath = Path.Combine(AppContext.BaseDirectory, "credentials.txt");
        var credentials = System.IO.File.ReadAllLines(credentialsPath)
            .Select(line => line.Split('='))
            .ToDictionary(parts => parts[0], parts => parts[1]);

        if (model.Username == credentials["username"] && model.Password == credentials["password"])
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.Username),
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

        return Redirect("/login?error=Invalid username or password");
    }

    [Route("logout")]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return Redirect("/login");
    }
}
