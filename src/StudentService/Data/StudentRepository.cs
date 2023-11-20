using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using StudentService.DTOs;
using StudentService.Entities;

namespace StudentService.Data;

public class StudentRepository : IStudentRepository
{
    private readonly DataContext _context;
    private readonly IMapper _mapper;

    public StudentRepository(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<StudentDto>> GetStudentsAsync()
    {
        var query = _context.Students.OrderBy(x => x.StudentCode).AsQueryable();

        return await query.ProjectTo<StudentDto>(_mapper.ConfigurationProvider).ToListAsync();
    }

    public async Task<StudentDto> GetStudentByIdAsync(Guid id)
    {
        return await _context.Students
            .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<Student> GetStudentEntityByIdAsync(Guid id)
    {
        return await _context.Students
            .Include(x => x.Class)
            .ThenInclude(x => x.Department)
            .Include(x => x.Class)
            .ThenInclude(x => x.Course)
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddStudent(Student auction)
    {
        await _context.Students.AddAsync(auction);
    }

    public void RemoveStudent(Student auction)
    {
        _context.Students.Remove(auction);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync() > 0;
    }
}