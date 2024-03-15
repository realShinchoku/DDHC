using AttendanceService.Models;
using MongoDB.Entities;

namespace AttendanceService.Services;

public class StudentServiceHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<List<Lesson>> GetLessons()
    {
        var lastUpdated = await DB.Find<Lesson, string>()
            .Sort(x => x.Descending(l => l.UpdatedAt))
            .Project(x => x.UpdatedAt.ToString())
            .ExecuteFirstAsync();

        return await httpClient.GetFromJsonAsync<List<Lesson>>(
            $"{config["StudentServiceUrl"]}/api/lessons?updatedAt={lastUpdated}");
    }
}