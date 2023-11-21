using AttendanceService.Models;
using Grpc.Net.Client;
using StudentService;

namespace AttendanceService.Services;

public class GrpcStudentClient
{
    private readonly IConfiguration _config;
    private readonly ILogger<GrpcStudentClient> _logger;

    public GrpcStudentClient(IConfiguration config, ILogger<GrpcStudentClient> logger)
    {
        _config = config;
        _logger = logger;
    }

    public Student GetStudent(string id)
    {
        _logger.LogInformation("==> Calling GetStudent GRPC Service");

        var channel = GrpcChannel.ForAddress(_config["GrpcStudent"] ?? throw new InvalidOperationException());
        var client = new GrpcStudent.GrpcStudentClient(channel);
        var request = new GetStudentRequest { Id = id };

        try
        {
            var reply = client.GetStudent(request);
            var auction = new Student
            {
                ID = reply.Student.Id,
                Name = reply.Student.Name,
                ClassName = reply.Student.ClassName,
                ClassId = reply.Student.ClassId,
                StudentCode = reply.Student.StudentCode,
                DepartmentId = reply.Student.DepartmentId,
                DepartmentName = reply.Student.DepartmentName,
                GradeId = reply.Student.GradeId,
                GradeName = reply.Student.GradeName
            };

            return auction;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not call GRPC Service");
            return null;
        }
    }
}