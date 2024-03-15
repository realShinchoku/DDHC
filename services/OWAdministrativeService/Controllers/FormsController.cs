using System.Text.Json;
using ApplicationBase.Security;
using AutoMapper;
using Contracts.Notifications;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using MongoDB.Entities;
using OWAdministrativeService.DTOs;
using OWAdministrativeService.FileHelper;
using OWAdministrativeService.Models;

namespace OWAdministrativeService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FormsController(
    IUserAccessor userAccessor,
    IMapper mapper,
    IFileHelper fileHelper,
    IPublishEndpoint publishEndpoint) : ControllerBase
{
    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    [HttpPost("student-card")]
    public async Task<IActionResult> CreateStudentCard([FromForm] StudentCardFormDto studentCardFormDto)
    {
        var email = userAccessor.GetUserEmail();
        if (email == null) return BadRequest("User email is null");
        var studentCode = email.Split("@")[0];

        var form = new Form
        {
            Email = email,
            Type = FormType.Card
        };

        var studentCardForm = mapper.Map<StudentCardForm>(studentCardFormDto);

        if (studentCardFormDto.Photo3X4 != null)
        {
            var photo3X4 = await fileHelper.SaveImage(studentCardFormDto.Photo3X4);
            studentCardForm.Photo3X4 = photo3X4;
        }

        if (studentCardFormDto.FrontIdPhoto != null)
        {
            var frontIdPhoto = await fileHelper.SaveImage(studentCardFormDto.FrontIdPhoto);
            studentCardForm.FrontIdPhoto = frontIdPhoto;
        }

        if (studentCardFormDto.BackIdPhoto != null)
        {
            var backIdPhoto = await fileHelper.SaveImage(studentCardFormDto.BackIdPhoto);
            studentCardForm.BackIdPhoto = backIdPhoto;
        }

        form.Body = JsonSerializer.Serialize(studentCardForm);

        await form.SaveAsync();

        return Ok(mapper.Map<FormDto>(form));
    }


    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    [HttpPost("student-verify")]
    public async Task<IActionResult> CreateStudentVerify(StudentVerifyFormDto studentVerifyFormDto)
    {
        var email = userAccessor.GetUserEmail();
        if (email == null) return BadRequest("User email is null");

        var form = new Form
        {
            Email = email,
            Type = FormType.Verify
        };

        var studentVerifyForm = mapper.Map<StudentVerifyForm>(studentVerifyFormDto);

        form.Body = JsonSerializer.Serialize(studentVerifyForm);

        await form.SaveAsync();

        return Ok(mapper.Map<FormDto>(form));
    }

    [Authorize]
    [AuthorizeForScopes(ScopeKeySection = "DownstreamApi:Scopes")]
    [AuthorizeForScopes(ScopeKeySection = "AzureAd:Scopes")]
    [HttpGet("student")]
    public async Task<IActionResult> GetFormsForStudent()
    {
        var email = userAccessor.GetUserEmail();
        if (email == null) return BadRequest("User email is null");

        var forms = await DB.Find<Form>()
            .Sort(x => x.UpdatedAt, Order.Descending)
            .Sort(x => x.CreatedAt, Order.Descending)
            .Match(f => f.Email == email)
            .ExecuteAsync();

        return Ok(mapper.Map<List<FormDto>>(forms));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteForm(Guid id)
    {
        var form = await DB.Find<Form>()
            .OneAsync(id);

        if (form == null) return NotFound();

        await form.DeleteAsync();

        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetForms(int? page, int? pageSize, FormStatus? status)
    {
        var query = DB.PagedSearch<Form>()
            .Sort(x => x.UpdatedAt, Order.Descending)
            .Sort(x => x.CreatedAt, Order.Descending);

        if (status.HasValue)
            query = query.Match(x => x.Status == status);

        var forms = await query
            .PageNumber(page ?? 1)
            .PageSize(pageSize ?? 10)
            .ExecuteAsync();
        var approvedForms = await DB.CountAsync<Form>(x => x.Status != FormStatus.Pending);
        var totalForms = await DB.CountAsync<Form>();
        return Ok(new
        {
            items = mapper.Map<List<FormDto>>(forms.Results),
            count = forms.TotalCount,
            totalForms,
            approvedForms
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetForm(string id)
    {
        var form = await DB.Find<Form>()
            .OneAsync(id);

        return Ok(mapper.Map<FormDto>(form));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateForm(string id, FormUpdateDto formUpdateDto)
    {
        var form = await DB.Find<Form>()
            .OneAsync(id);

        if (form == null) return NotFound();

        form.Status = formUpdateDto.Status switch
        {
            "1" => FormStatus.Approved,
            "2" => FormStatus.Rejected,
            _ => form.Status
        };

        form.Note = formUpdateDto.Note;
        form.DateToGetResult = formUpdateDto.DateToGetResult;
        form.UpdatedAt = DateTime.UtcNow;

        if (formUpdateDto.Status == "0")
        {
            var studentCardForm = JsonSerializer.Deserialize<StudentCardForm>(form.Body);
            studentCardForm.Code = formUpdateDto.Code;
            studentCardForm.CardReturnDate = formUpdateDto.DateToGetResult?.ToString("yyyy-MM-dd") ?? "";
            form.Body = JsonSerializer.Serialize(studentCardForm);
        }

        var title = form.Status switch
        {
            FormStatus.Approved => "Yêu cầu đã được chấp nhận",
            FormStatus.Rejected => "Yêu cầu đã bị từ chối",
            _ => ""
        };

        var message = $"{form.Note}";

        if (formUpdateDto.DateToGetResult.HasValue)
            message += $"\nNgày nhận kết quả: {formUpdateDto.DateToGetResult.Value:hh:mm dd/MM/yyyy}";

        await publishEndpoint.Publish(new NotificationCreated
        {
            Title = title,
            Email = form.Email,
            Message = message,
            Type = NotificationType.Normal
        });

        await form.SaveAsync();

        return Ok(mapper.Map<FormDto>(form));
    }
}