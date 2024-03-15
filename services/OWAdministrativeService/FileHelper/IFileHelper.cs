namespace OWAdministrativeService.FileHelper;

public interface IFileHelper
{
    public Task<string> SaveImage(IFormFile file);
    public Task<bool> RemoveFile(string fileName);

    public Task<bool> RemoveFolder(string folder);

    public Task<(Stream Stream, string ContentType)> DownloadImage(string fileName);
}