using ClosedXML.Excel;
using KidsOrganizationApp.Service.DTO;
using System.IO;
using System.IO.Compression;

namespace KidsOrganizationApp.Service;
public interface IExportService { void ExportArchive(string archivePath); }
public class ExportService : IExportService
{
    private readonly IChildService _children; private readonly IParentService _parents; private readonly IEventService _events; private readonly IDocumentService _documents;
    public ExportService(IChildService children, IParentService parents, IEventService events, IDocumentService documents) { _children = children; _parents = parents; _events = events; _documents = documents; }
    public void ExportArchive(string archivePath)
    {
        var directory = Path.Combine(Path.GetTempPath(), $"kids-export-{Guid.NewGuid():N}"); Directory.CreateDirectory(directory);
        try { Participants(Path.Combine(directory, "Участники.xlsx")); Events(Path.Combine(directory, "Мероприятия.xlsx")); CopyDocuments(Path.Combine(directory, "Документы")); if (File.Exists(archivePath)) File.Delete(archivePath); ZipFile.CreateFromDirectory(directory, archivePath); }
        finally { if (Directory.Exists(directory)) Directory.Delete(directory, true); }
    }
    private void Participants(string path)
    {
        using var book = new XLWorkbook(); var sheet = book.AddWorksheet("Участники"); var headers = new[] { "Роль", "Фамилия", "Имя", "Отчество", "Дата рождения", "Телефон", "Адрес", "Электронная почта" };
        for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i]; var row = 2;
        foreach (var c in _children.GetAllChildren()) Write(sheet, row++, "Ребёнок", c.Surname, c.Name, c.Patronymic, c.DateBirth, c.MobileNumber, c.LivingPlace, c.Email);
        foreach (var p in _parents.GetAllParents()) Write(sheet, row++, "Родитель", p.Surname, p.Name, p.Patronymic, p.DateBirth, p.MobileNumber, p.LivingPlace, p.Email);
        Style(sheet, headers.Length); book.SaveAs(path);
    }
    private void Events(string path) { using var book = new XLWorkbook(); var sheet = book.AddWorksheet("Мероприятия"); sheet.Cell(1, 1).Value = "Название"; sheet.Cell(1, 2).Value = "Дата"; var row = 2; foreach (var e in _events.GetAll()) { sheet.Cell(row, 1).Value = e.Name; sheet.Cell(row++, 2).Value = e.Date; } Style(sheet, 2); book.SaveAs(path); }
    private static void Write(IXLWorksheet s, int r, string role, string surname, string name, string patronymic, DateTime birth, string phone, string place, string? email) { s.Cell(r, 1).Value = role; s.Cell(r, 2).Value = surname; s.Cell(r, 3).Value = name; s.Cell(r, 4).Value = patronymic; s.Cell(r, 5).Value = birth; s.Cell(r, 5).Style.DateFormat.Format = "dd.MM.yyyy"; s.Cell(r, 6).Value = phone; s.Cell(r, 7).Value = place; s.Cell(r, 8).Value = email ?? string.Empty; }
    private static void Style(IXLWorksheet s, int columns) { s.Range(1, 1, 1, columns).Style.Font.Bold = true; s.Range(1, 1, 1, columns).Style.Fill.BackgroundColor = XLColor.FromHtml("243B6B"); s.Range(1, 1, 1, columns).Style.Font.FontColor = XLColor.White; s.SheetView.FreezeRows(1); s.Columns().AdjustToContents(); }
    private void CopyDocuments(string directory) { Directory.CreateDirectory(directory); var missing = new List<string>(); foreach (var doc in _documents.GetAll()) { if (File.Exists(doc.Path)) File.Copy(doc.Path, Path.Combine(directory, Path.GetFileName(doc.Path)), true); else missing.Add($"{doc.TypeName}: {doc.Path}"); } if (missing.Count > 0) File.WriteAllLines(Path.Combine(directory, "Не найдено.txt"), missing); }
}
