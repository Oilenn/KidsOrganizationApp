using KidsOrganizationApp.Service.DTO;
using System.Diagnostics;
using System.IO;

namespace KidsOrganizationApp.Service;

public interface IDocumentFileService
{
    string? Open(DocumentDTO document);
    string? ShowInExplorer(DocumentDTO document);
    string? OpenFolder(string directoryPath);
    Task CopyAsync(
        string sourcePath,
        string destinationPath,
        bool overwrite = false,
        CancellationToken cancellationToken = default);
    Task<string> CopyToDirectoryAsync(
        string sourcePath,
        string directoryPath,
        CancellationToken cancellationToken = default);
}

public sealed class DocumentFileService : IDocumentFileService
{
    private readonly SemaphoreSlim _copyLock = new(1, 1);

    public Task CopyAsync(
        string sourcePath,
        string destinationPath,
        bool overwrite = false,
        CancellationToken cancellationToken = default)
    {
        // File.Copy сохраняет метаданные файла и не предоставляет асинхронную версию.
        return Task.Run(() => File.Copy(sourcePath, destinationPath, overwrite), cancellationToken);
    }

    public async Task<string> CopyToDirectoryAsync(
        string sourcePath,
        string directoryPath,
        CancellationToken cancellationToken = default)
    {
        await _copyLock.WaitAsync(cancellationToken);
        try
        {
            var name = Path.GetFileName(sourcePath);
            var destination = Path.Combine(directoryPath, name);
            var number = 2;
            while (File.Exists(destination))
            {
                destination = Path.Combine(
                    directoryPath,
                    $"{Path.GetFileNameWithoutExtension(name)} ({number++}){Path.GetExtension(name)}");
            }

            await CopyAsync(sourcePath, destination, cancellationToken: cancellationToken);
            return destination;
        }
        finally
        {
            _copyLock.Release();
        }
    }

    public string? Open(DocumentDTO document)
    {
        var validationError = Validate(document);
        if (validationError is not null)
            return validationError;

        try
        {
            Process.Start(new ProcessStartInfo(document.Path) { UseShellExecute = true });
            return null;
        }
        catch
        {
            return "Файл поврежден";
        }
    }

    public string? ShowInExplorer(DocumentDTO document)
    {
        if (string.IsNullOrWhiteSpace(document.Path) || !File.Exists(document.Path))
        {
            return "Файл не найден";
        }

        try
        {
            Process.Start(new ProcessStartInfo
            {
                FileName = "explorer.exe",
                Arguments = $"/select,\"{document.Path}\"",
                UseShellExecute = true
            });
            return null;
        }
        catch
        {
            return "Файл поврежден";
        }
    }

    public string? OpenFolder(string directoryPath)
    {
        try
        {
            Directory.CreateDirectory(directoryPath);
            Process.Start(new ProcessStartInfo(directoryPath) { UseShellExecute = true });
            return null;
        }
        catch
        {
            return "Не удалось открыть папку документов";
        }
    }

    private static string? Validate(DocumentDTO document)
    {
        if (string.IsNullOrWhiteSpace(document.Path) || !File.Exists(document.Path))
        {
            return "Файл не найден";
        }

        try
        {
            using var stream = new FileStream(document.Path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return null;
        }
        catch
        {
            return "Файл поврежден";
        }
    }
}
