using KidsOrganizationApp.Domain;
using System.IO;

namespace KidsOrganizationApp.Service.DTO;

public class DocumentDTO : IDTO
{
    public Guid Id { get; set; }
    public Document.DocumentType Type { get; set; }
    public string Path { get; set; }

    public string TypeName => Type switch
    {
        Document.DocumentType.Passport => "Паспорт",
        Document.DocumentType.SNILS => "СНИЛС",
        Document.DocumentType.Diagnosis => "Диагноз",
        Document.DocumentType.Letter => "Письмо",
        Document.DocumentType.Order => "Приказ",
        _ => "Не указан"
    };

    public string IconGlyph => Type switch
    {
        Document.DocumentType.Passport => "🪪",
        Document.DocumentType.SNILS => "▦",
        Document.DocumentType.Diagnosis => "⚕",
        Document.DocumentType.Letter => "✉",
        Document.DocumentType.Order => "📋",
        _ => "📄"
    };

    public bool IsFileMissing => !File.Exists(Path);
    public string FileName => IsFileMissing ? "Файл не найден" : System.IO.Path.GetFileName(Path);

    public DocumentDTO(Guid id, Document.DocumentType type, string path)
    {
        Id = id;
        Type = type;
        Path = path;
    }
}
