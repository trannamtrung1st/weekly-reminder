using Microsoft.AspNetCore.Mvc;
using WeeklyReminder.Application.Services.Abstracts;
using System.IO;

namespace WeeklyReminder.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReminderController : ControllerBase
{
    private readonly IReminderService _reminderService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ReminderController(IReminderService reminderService, IWebHostEnvironment webHostEnvironment)
    {
        _reminderService = reminderService;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet("confirm/{token}")]
    public async Task<IActionResult> ConfirmReminder(string token)
    {
        var result = await _reminderService.ConfirmReminderAsync(token);
        if (result)
        {
            var imagePath = GetRandomImagePath();
            if (imagePath != null)
            {
                return PhysicalFile(imagePath, "image/jpeg");
            }
            return Ok("Reminder confirmed successfully.");
        }
        return NotFound("Invalid or expired confirmation token.");
    }

    private string GetRandomImagePath()
    {
        var imagesFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
        if (Directory.Exists(imagesFolder))
        {
            var imageFiles = Directory.GetFiles(imagesFolder, "*.*", SearchOption.TopDirectoryOnly)
                .Where(file => new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(file).ToLower()))
                .ToArray();

            if (imageFiles.Length > 0)
            {
                var random = new Random();
                return imageFiles[random.Next(imageFiles.Length)];
            }
        }
        return null;
    }
}