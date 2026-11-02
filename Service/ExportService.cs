using ClosedXML.Excel;
using KidsOrganizationApp.Service.DTO;
using System.IO;
using System.IO.Compression;

namespace KidsOrganizationApp.Service;

public interface IExportService
{
    Task ExportArchiveAsync(string archivePath, CancellationToken cancellationToken = default);
}

public class ExportService : IExportService
{
    private readonly IChildService _children;
    private readonly IParentService _parents;
    private readonly IEventService _events;
    private readonly IDocumentService _documents;
    private readonly IDocumentFileService _documentFiles;

    public ExportService(
        IChildService children,
        IParentService parents,
        IEventService events,
        IDocumentService documents,
        IDocumentFileService documentFiles)
    {
        _children = children;
        _parents = parents;
        _events = events;
        _documents = documents;
        _documentFiles = documentFiles;
    }

    public async Task ExportArchiveAsync(string archivePath, CancellationToken cancellationToken = default)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"kids-export-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);
        try
        {
            await ParticipantsAsync(Path.Combine(directory, "Участники.xlsx"), cancellationToken);
            await EventsAsync(Path.Combine(directory, "Мероприятия.xlsx"), cancellationToken);
            await CopyDocumentsAsync(Path.Combine(directory, "Документы"), cancellationToken);
            if (File.Exists(archivePath))
                File.Delete(archivePath);
            await ZipFile.CreateFromDirectoryAsync(directory, archivePath, cancellationToken);
        }
        finally
        {
            if (Directory.Exists(directory))
                await Task.Run(() => Directory.Delete(directory, true));
        }
    }

    private async Task ParticipantsAsync(string path, CancellationToken cancellationToken)
    {
        var children = await _children.GetAllChildrenAsync(cancellationToken);
        var parents = await _parents.GetAllParentsAsync(cancellationToken);
        await Task.Run(() => SaveParticipants(path, children, parents), cancellationToken);
    }

    private static void SaveParticipants(string path, List<ChildDTO> children, List<ParentDTO> parents)
    {
        using var book = new XLWorkbook();
        var sheet = book.AddWorksheet("Участники");
        var headers = new[] { "Роль", "Фамилия", "Имя", "Отчество", "Дата рождения", "Телефон", "Адрес", "Электронная почта" };
        for (var i = 0; i < headers.Length; i++)
            sheet.Cell(1, i + 1).Value = headers[i];
        var row = 2;
        foreach (var c in children)
            Write(
                sheet,
                row++,
                "Ребёнок",
                c.Surname,
                c.Name,
                c.Patronymic,
                c.DateBirth,
                c.MobileNumber,
                c.LivingPlace,
                c.Email);
        foreach (var p in parents)
            Write(
                sheet,
                row++,
                "Родитель",
                p.Surname,
                p.Name,
                p.Patronymic,
                p.DateBirth,
                p.MobileNumber,
                p.LivingPlace,
                p.Email);
        Style(sheet, headers.Length);
        book.SaveAs(path);
    }

    private async Task EventsAsync(string path, CancellationToken cancellationToken)
    {
        var events = await _events.GetAllAsync(cancellationToken);
        await Task.Run(() => SaveEvents(path, events), cancellationToken);
    }

    private static void SaveEvents(string path, List<EventDTO> events)
    {
        using var book = new XLWorkbook();
        var sheet = book.AddWorksheet("Мероприятия");
        sheet.Cell(1, 1).Value = "Название";
        sheet.Cell(1, 2).Value = "Дата";
        var row = 2;
        foreach (var e in events)
        {
            sheet.Cell(row, 1).Value = e.Name;
            sheet.Cell(row++, 2).Value = e.Date;
        }

        Style(sheet, 2);
        book.SaveAs(path);
    }

    private static void Write(
        IXLWorksheet s,
        int r,
        string role,
        string surname,
        string name,
        string patronymic,
        DateTime birth,
        string phone,
        string place,
        string? email)
    {
        s.Cell(r, 1).Value = role;
        s.Cell(r, 2).Value = surname;
        s.Cell(r, 3).Value = name;
        s.Cell(r, 4).Value = patronymic;
        s.Cell(r, 5).Value = birth;
        s.Cell(r, 5).Style.DateFormat.Format = "dd.MM.yyyy";
        s.Cell(r, 6).Value = phone;
        s.Cell(r, 7).Value = place;
        s.Cell(r, 8).Value = email ?? string.Empty;
    }

    private static void Style(IXLWorksheet s, int columns)
    {
        s.Range(1, 1, 1, columns).Style.Font.Bold = true;
        s.Range(1, 1, 1, columns).Style.Fill.BackgroundColor = XLColor.FromHtml("243B6B");
        s.Range(1, 1, 1, columns).Style.Font.FontColor = XLColor.White;
        s.SheetView.FreezeRows(1);
        s.Columns().AdjustToContents();
    }

    private async Task CopyDocumentsAsync(string directory, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(directory);
        var missing = new List<string>();
        foreach (var doc in await _documents.GetAllAsync(cancellationToken))
        {
            if (File.Exists(doc.Path))
                await _documentFiles.CopyAsync(
                    doc.Path,
                    Path.Combine(directory, Path.GetFileName(doc.Path)),
                    overwrite: true,
                    cancellationToken);
            else
                missing.Add($"{doc.TypeName}: {doc.Path}");
        }

        if (missing.Count > 0)
            await File.WriteAllLinesAsync(Path.Combine(directory, "Не найдено.txt"), missing, cancellationToken);
    }
}
