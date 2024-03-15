using Microsoft.AspNetCore.StaticFiles;

namespace OWAdministrativeService.FileHelper;

public class FileHelper(IWebHostEnvironment webHostEnvironment) : IFileHelper
{
    private const string ImagesDirectory = "images";
    private readonly string _webRootPath = webHostEnvironment.WebRootPath;

    public async Task<string> SaveImage(IFormFile file)
    {
        CheckDirectory();

        var fileName = (DateTime.UtcNow.ToString("yyyyMMddhhmmss") + file.FileName).Replace(" ", "_");

        var filePath = Path.Combine(ImagesDirectory, fileName);

        await using var output = File.Create(Path.Combine(_webRootPath, filePath));
        await file.CopyToAsync(output);

        return fileName;
    }

    public Task<bool> RemoveFile(string fileName)
    {
        var filePath = Path.Combine(_webRootPath, ImagesDirectory, fileName);
        if (!File.Exists(filePath)) return Task.FromResult(false);

        File.Delete(filePath);
        return Task.FromResult(true);
    }

    public Task<bool> RemoveFolder(string folder)
    {
        var filePath = Path.Combine(_webRootPath, ImagesDirectory, folder);
        if (!Directory.Exists(filePath)) return Task.FromResult(false);

        Directory.Delete(filePath, true);
        return Task.FromResult(true);
    }

    public async Task<(Stream Stream, string ContentType)> DownloadImage(string fileName)
    {
        var filePath = Path.Combine(_webRootPath, ImagesDirectory, fileName);
        if (!File.Exists(filePath)) return (null, null);
        return await Task.FromResult((File.OpenRead(filePath), GetMimeTypeForFileExtension(filePath)));
    }

    private void CheckDirectory()
    {
        var path = Path.Combine(_webRootPath, ImagesDirectory);
        if (!Directory.Exists(path) && !string.IsNullOrEmpty(path))
            Directory.CreateDirectory(path);
    }

    private string GetMimeTypeForFileExtension(string filePath)
    {
        const string defaultContentType = "application/octet-stream";

        var provider = new FileExtensionContentTypeProvider();

        if (!provider.TryGetContentType(filePath, out var contentType)) contentType = defaultContentType;

        return contentType;
    }
}