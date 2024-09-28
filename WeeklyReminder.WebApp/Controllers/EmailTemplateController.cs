using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WeeklyReminder.Application.Services.Abstracts;
using WeeklyReminder.Domain.Entities;

namespace WeeklyReminder.WebApp.Controllers;

[ApiController, Authorize]
[Route("api/[controller]")]
public class EmailTemplateController : ControllerBase
{
    private readonly IEmailTemplateService _emailTemplateService;

    public EmailTemplateController(IEmailTemplateService emailTemplateService)
    {
        _emailTemplateService = emailTemplateService;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<EmailTemplateEntity>> GetEmailTemplate(Guid id)
    {
        var emailTemplate = await _emailTemplateService.GetByIdAsync(id);
        if (emailTemplate == null)
        {
            return NotFound();
        }
        return emailTemplate;
    }

    [HttpGet("activity/{activityId}")]
    public async Task<ActionResult<IEnumerable<EmailTemplateEntity>>> GetEmailTemplatesByActivity(Guid activityId)
    {
        var emailTemplates = await _emailTemplateService.GetByActivityIdAsync(activityId);
        return Ok(emailTemplates);
    }

    [HttpPost]
    public async Task<ActionResult<EmailTemplateEntity>> CreateEmailTemplate(EmailTemplateEntity emailTemplate)
    {
        await _emailTemplateService.CreateEmailTemplateAsync(emailTemplate);
        return CreatedAtAction(nameof(GetEmailTemplate), new { id = emailTemplate.Id }, emailTemplate);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateEmailTemplate(Guid id, EmailTemplateEntity emailTemplate)
    {
        if (id != emailTemplate.Id)
        {
            return BadRequest();
        }
        await _emailTemplateService.UpdateEmailTemplateAsync(emailTemplate);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteEmailTemplate(Guid id)
    {
        await _emailTemplateService.DeleteEmailTemplateAsync(id);
        return NoContent();
    }
}