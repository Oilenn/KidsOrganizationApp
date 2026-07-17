using ClosedXML.Excel;
using KidsOrganizationApp.Service.DTO;
using System.IO;
using System.IO.Compression;

namespace KidsOrganizationApp.Service;

public interface IExportService
{
    void ExportArchive(string archivePath);
}

public class ExportService : IExportService
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private readonly IEventService _eventService;
    private readonly IDocumentService _documentService;

    public ExportService(IChildService childService, IParentService parentService,
        IEventService eventService, IDocumentService documentService)
    {
        _childService = childService;
        _parentService = parentService;
        _eventService = eventService;
        _documentService = documentService;
    }

    public void ExportArchive(string archivePath)
    {
        var workDirectory = Path.Combine(Path.GetTempPath(), $"kids-organization-export-{Guid.NewGuid():N}");
        Directory.CreateDirectory(workDirectory);
        try
        {
            CreateParticipantsWorkbook(Path.Combine(workDirectory, "Участники.xlsx"));
            CreateEventsWorkbook(Path.Combine(workDirectory, "Мероприятия.xlsx"));
            CopyDocuments(Path.Combine(workDirectory, "Документы"));

            if (File.Exists(archivePath)) File.Delete(archivePath);
            ZipFile.CreateFromDirectory(workDirectory, archivePath, CompressionLevel.Optimal, false);
        }
        finally
        {
            if (Directory.Exists(workDirectory)) Directory.Delete(workDirectory, true);
        }
    }

    private void CreateParticipantsWorkbook(string path)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Участники");
        var headers = new[] { "Роль", "Фамилия", "Имя", "Отчество", "Дата рождения", "Телефон", "Место жительства", "Электронная почта" };
        for (var column = 0; column < headers.Length; column++) sheet.Cell(1, column + 1).Value = headers[column];

        var row = 2;
        foreach (var child in _childService.GetAllChildren())
        {
            WriteParticipant(sheet, row++, "Ребёнок", child.Surname, child.Name, child.Patronymic, child.DateBirth, child.MobileNumber, child.LivingPlace, child.Email);
        }
        foreach (var parent in _parentService.GetAllParents())
        {
            WriteParticipant(sheet, row++, "Родитель", parent.Surname, parent.Name, parent.Patronymic, parent.DateBirth, parent.MobileNumber, parent.LivingPlace, parent.Email);
        }
        StyleWorksheet(sheet, headers.Length, row - 1);
        workbook.SaveAs(path);
    }

    private void CreateEventsWorkbook(string path)
    {
        using var workbook = new XLWorkbook();
        var sheet = workbook.AddWorksheet("Мероприятия");
        sheet.Cell(1, 1).Value = "Название";
        sheet.Cell(1, 2).Value = "Дата";
        var row = 2;
        foreach (var item in _eventService.GetAll())
        {
            sheet.Cell(row, 1).Value = item.Name;
            sheet.Cell(row, 2).Value = item.Date;
            sheet.Cell(row, 2).Style.DateFormat.Format = "dd.MM.yyyy";
            row++;
        }
        StyleWorksheet(sheet, 2, row - 1);
        workbook.SaveAs(path);
    }

    private static void WriteParticipant(IXLWorksheet sheet, int row, string role, string surname, string name,
        string patronymic, DateTime birthDate, string phone, string address, string? email)
    {
        sheet.Cell(row, 1).Value = role;
        sheet.Cell(row, 2).Value = surname;
        sheet.Cell(row, 3).Value = name;
        sheet.Cell(row, 4).Value = patronymic;
        sheet.Cell(row, 5).Value = birthDate;
        sheet.Cell(row, 5).Style.DateFormat.Format = "dd.MM.yyyy";
        sheet.Cell(row, 6).Value = phone;
        sheet.Cell(row, 7).Value = address;
        sheet.Cell(row, 8).Value = email ?? string.Empty;
    }

    private static void StyleWorksheet(IXLWorksheet sheet, int columns, int rows)
    {
        var header = sheet.Range(1, 1, 1, columns);
        header.Style.Font.Bold = true;
        header.Style.Fill.BackgroundColor = XLColor.FromHtml("243B6B");
        header.Style.Font.FontColor = XLColor.White;
        sheet.Range(1, 1, Math.Max(rows, 1), columns).SetAutoFilter();
        sheet.SheetView.FreezeRows(1);
        sheet.Columns().AdjustToContents();
        sheet.Column(7).Width = Math.Min(Math.Max(sheet.Column(7).Width, 22), 42);
        sheet.Column(8).Width = Math.Min(Math.Max(sheet.Column(8).Width, 22), 42);
    }

    private void CopyDocuments(string documentsDirectory)
    {
        Directory.CreateDirectory(documentsDirectory);
        var missing = new List<string>();
        var usedNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var document in _documentService.GetAll())
        {
            if (!File.Exists(document.Path))
            {
                missing.Add($"{document.TypeName}: {document.Path}");
                continue;
            }
            var fileName = Path.GetFileName(document.Path);
            var uniqueName = fileName;
            var number = 2;
            while (!usedNames.Add(uniqueName))
                uniqueName = $"{Path.GetFileNameWithoutExtension(fileName)} ({number++}){Path.GetExtension(fileName)}";
            File.Copy(document.Path, Path.Combine(documentsDirectory, uniqueName));
        }
        if (missing.Count > 0)
            File.WriteAllLines(Path.Combine(documentsDirectory, "Не найдено.txt"), missing);
    }
}
