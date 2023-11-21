using Microsoft.AspNetCore.Mvc;
using StudentService.Data;
using StudentService.DTOs;

namespace StudentService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly IStudentRepository _repo;

    public StudentsController(IStudentRepository repo)
    {
        _repo = repo;
    }

    [HttpGet]
    public async Task<ActionResult<List<StudentDto>>> GetAllStudents()
    {
        return await _repo.GetStudentsAsync();
    }
}