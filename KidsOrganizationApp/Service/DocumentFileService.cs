using KidsOrganizationApp.Service.DTO;
using System.Diagnostics;
using System.IO;

namespace KidsOrganizationApp.Service;

public interface IDocumentFileService
{
    string? Open(DocumentDTO document);
    string? ShowInExplorer(DocumentDTO document);
    string? OpenFolder(string directoryPath);
}

public sealed class DocumentFileService : IDocumentFileService
{
    public string? Open(DocumentDTO document)
    {
        var validationError = Validate(document);
        if (validationError is not null) return validationError;

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
