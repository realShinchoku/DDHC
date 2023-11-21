using AutoMapper;
using AutoMapper.QueryableExtensions;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using StudentService.Data;
using StudentService.DTOs;

namespace StudentService.Services;

public class GrpcStudentService : GrpcStudent.GrpcStudentBase
{
    private readonly DataContext _dbContext;
    private readonly Logger<GrpcStudentService> _logger;
    private readonly IMapper _mapper;

    public GrpcStudentService(DataContext dbContext, IMapper mapper, Logger<GrpcStudentService> logger)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _logger = logger;
    }

    public override async Task<GrpcStudentResponse> GetStudent(GetStudentRequest request, ServerCallContext context)
    {
        _logger.LogInformation("==> Received Grpc request for GetStudent");
        var student = await _dbContext.Students
                          .Include(x => x.Class)
                          .ThenInclude(x => x.Grade)
                          .Include(x => x.Class)
                          .ThenInclude(x => x.Department)
                          .ProjectTo<StudentDto>(_mapper.ConfigurationProvider)
                          .FirstOrDefaultAsync(x => x.Id == Guid.Parse(request.Id))
                      ??
                      throw new RpcException(new Status(StatusCode.NotFound,
                          $"Student with id {request.Id} not found"));

        var response = new GrpcStudentResponse
        {
            Student = new GrpcStudentModel
            {
                Id = student.Id.ToString(),
                Name = student.Name,
                DepartmentName = student.DepartmentName,
                GradeName = student.GradeName,
                ClassName = student.ClassName,
                DepartmentId = student.DepartmentId.ToString(),
                GradeId = student.GradeId.ToString(),
                ClassId = student.ClassId.ToString(),
                StudentCode = student.StudentCode
            }
        };

        return response;
    }
}