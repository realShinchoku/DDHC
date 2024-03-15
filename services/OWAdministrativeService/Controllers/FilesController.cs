using System.Text.Json;
using DocumentFormat.OpenXml.Packaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using OWAdministrativeService.FileHelper;
using OWAdministrativeService.Models;

namespace OWAdministrativeService.Controllers;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class FilesController(IFileHelper fileHelper, IWebHostEnvironment webHostEnvironment) : ControllerBase
{
    [HttpGet("images/{fileName}")]
    public async Task<IActionResult> DownloadImages(string fileName)
    {
        var file = await fileHelper.DownloadImage(fileName);
        if (file.Stream == null) return NotFound();
        return File(file.Stream, file.ContentType, fileName);
    }

    [HttpGet("docs/{id:guid}")]
    public async Task<IActionResult> DownloadDoc(Guid id)
    {
        var form = await DB.Find<Form>().OneAsync(id);
        if (form == null) return NotFound();
        var studentCardForm = JsonSerializer.Deserialize<StudentCardForm>(form.Body);
        var stream = await CreateDocFromTemplateOpenXml(studentCardForm);
        return File(stream, "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            $"{form.Email}.docx");
    }


    public async Task<Stream> CreateDocFromTemplateOpenXml(StudentCardForm studentCardForm)
    {
        var templatePath = Path.Combine(webHostEnvironment.WebRootPath, "docs", "student-card.docx");

        var memoryStream = new MemoryStream();

        await using (var fs = new FileStream(templatePath, FileMode.Open, FileAccess.Read, FileShare.Read))
        {
            await fs.CopyToAsync(memoryStream);
        }

        using (var wordDoc = WordprocessingDocument.Open(memoryStream, true))
        {
            string docText;

            if (wordDoc.MainDocumentPart is null) throw new ArgumentNullException(nameof(templatePath));

            using (var sr = new StreamReader(wordDoc.MainDocumentPart.GetStream()))
            {
                docText = await sr.ReadToEndAsync();
            }

            docText = ReplaceValue(docText, studentCardForm);

            await using (var sw = new StreamWriter(wordDoc.MainDocumentPart.GetStream(FileMode.Create)))
            {
                await sw.WriteAsync(docText);
            }

            wordDoc.Save();
        }

        memoryStream.Position = 0;
        return memoryStream;
    }

    private static string ReplaceValue(string input, StudentCardForm studentCardForm)
    {
        input = input.Replace("FullName", studentCardForm.FullName);
        input = input.Replace("BirthDay", studentCardForm.BirthDay);
        input = input.Replace("CurrentClass", studentCardForm.CurrentClass);
        input = input.Replace("FirstClass", studentCardForm.FirstClass);
        input = input.Replace("StudentCode", studentCardForm.StudentCode);
        input = input.Replace("Course", studentCardForm.Course);
        input = input.Replace("StudentType", studentCardForm.StudentType);
        input = input.Replace("Code", studentCardForm.Code);
        input = input.Replace("CardReturnDate", studentCardForm.CardReturnDate);
        input = input.Replace("CreatedDate", studentCardForm.CreatedDate);

        var count = 0;
        var arr = input.ToCharArray();

        for (var i = 0; i < arr.Length; i++)
        {
            if (arr[i] != '□') continue;

            if (count == (int)studentCardForm.Reason || count == (int)studentCardForm.Reason + 5) arr[i] = '■';

            count++;
        }

        return new string(arr);
    }
}