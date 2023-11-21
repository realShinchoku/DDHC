using StudentService.DTOs;
using StudentService.Models;

namespace StudentService.Data;

public interface IStudentRepository
{
    Task<List<StudentDto>> GetStudentsAsync();
    Task<StudentDto> GetStudentByIdAsync(Guid id);
    Task<Student> GetStudentEntityByIdAsync(Guid id);
    Task AddStudent(Student auction);
    void RemoveStudent(Student auction);
    Task<bool> SaveChangesAsync();
}