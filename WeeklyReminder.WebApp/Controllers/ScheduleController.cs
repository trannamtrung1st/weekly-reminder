using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ScheduleController : ControllerBase
{
    private readonly IScheduleService _scheduleService;

    public ScheduleController(IScheduleService scheduleService)
    {
        _scheduleService = scheduleService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ScheduleEntity>> GetSchedule(Guid id)
    {
        var schedule = await _scheduleService.GetScheduleByIdAsync(id);
        if (schedule == null)
        {
            return NotFound();
        }
        return schedule;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ScheduleEntity>>> GetAllSchedules()
    {
        return Ok(await _scheduleService.GetAllSchedulesAsync());
    }

    [HttpPost]
    public async Task<ActionResult<ScheduleEntity>> CreateSchedule(ScheduleEntity schedule)
    {
        await _scheduleService.CreateScheduleAsync(schedule);
        return CreatedAtAction(nameof(GetSchedule), new { id = schedule.Id }, schedule);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSchedule(Guid id, ScheduleEntity schedule)
    {
        if (id != schedule.Id)
        {
            return BadRequest();
        }

        await _scheduleService.UpdateScheduleAsync(schedule);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSchedule(Guid id)
    {
        await _scheduleService.DeleteScheduleAsync(id);
        return NoContent();
    }

    [HttpPost("upload")]
    public async Task<IActionResult> UploadTimetable(Guid userId, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded");
        }

        using var stream = file.OpenReadStream();
        await _scheduleService.CreateScheduleFromTimetableAsync(userId, stream);
        return Ok("Timetable processed successfully");
    }
}
