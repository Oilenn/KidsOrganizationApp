using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KidsOrganizationApp;

public sealed class PendingDocument
{
    public Document.DocumentType Type { get; init; }
    public string Path { get; init; } = string.Empty;
    public string TypeName => Type switch
    {
        Document.DocumentType.Passport => "Паспорт",
        Document.DocumentType.SNILS => "СНИЛС",
        Document.DocumentType.Diagnosis => "Диагноз",
        Document.DocumentType.Letter => "Письмо",
        Document.DocumentType.Order => "Приказ",
        _ => "Документ"
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
    public string FileName => System.IO.Path.GetFileName(Path);
}

public class AddFamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private readonly IDocumentService _documentService;
    private readonly IApplicationSettingsService _settings;
    private ParentDTO? _selectedParent;
    private bool _isParentForm;

    public event Action? Saved;
    public event Action? Cancelled;

    public ObservableCollection<DocumentTypeChoice> DocumentTypes { get; } = new();
    public ObservableCollection<PendingDocument> PendingDocuments { get; } = new();

    public string Title => _isParentForm
        ? "Добавление родителя или законного представителя"
        : $"Добавление ребёнка для: {_selectedParent?.Surname} {_selectedParent?.Name}";
    public Visibility ChildFieldsVisibility => _isParentForm ? Visibility.Collapsed : Visibility.Visible;
    public Visibility ParentFieldsVisibility => _isParentForm ? Visibility.Visible : Visibility.Collapsed;

    private string _childName = string.Empty;
    public string ChildName { get => _childName; set { _childName = value; OnPropertyChanged(); } }
    private string _childSurname = string.Empty;
    public string ChildSurname { get => _childSurname; set { _childSurname = value; OnPropertyChanged(); } }
    private string _childPatronymic = string.Empty;
    public string ChildPatronymic { get => _childPatronymic; set { _childPatronymic = value; OnPropertyChanged(); } }
    private string _childPhone = string.Empty;
    public string ChildPhone { get => _childPhone; set { _childPhone = value; OnPropertyChanged(); } }
    private string _childLivingPlace = string.Empty;
    public string ChildLivingPlace { get => _childLivingPlace; set { _childLivingPlace = value; OnPropertyChanged(); } }
    private DateTime? _childBirthDate;
    public DateTime? ChildBirthDate { get => _childBirthDate; set { _childBirthDate = value; OnPropertyChanged(); } }

    private string _parentName = string.Empty;
    public string ParentName { get => _parentName; set { _parentName = value; OnPropertyChanged(); } }
    private string _parentSurname = string.Empty;
    public string ParentSurname { get => _parentSurname; set { _parentSurname = value; OnPropertyChanged(); } }
    private string _parentPatronymic = string.Empty;
    public string ParentPatronymic { get => _parentPatronymic; set { _parentPatronymic = value; OnPropertyChanged(); } }
    private string _parentPhone = string.Empty;
    public string ParentPhone { get => _parentPhone; set { _parentPhone = value; OnPropertyChanged(); } }
    private string _parentLivingPlace = string.Empty;
    public string ParentLivingPlace { get => _parentLivingPlace; set { _parentLivingPlace = value; OnPropertyChanged(); } }
    private string _parentEmail = string.Empty;
    public string ParentEmail { get => _parentEmail; set { _parentEmail = value; OnPropertyChanged(); } }
    private DateTime? _parentBirthDate;
    public DateTime? ParentBirthDate { get => _parentBirthDate; set { _parentBirthDate = value; OnPropertyChanged(); } }

    private DocumentTypeChoice? _selectedDocumentType;
    public DocumentTypeChoice? SelectedDocumentType { get => _selectedDocumentType; set { _selectedDocumentType = value; OnPropertyChanged(); } }
    private PendingDocument? _selectedPendingDocument;
    public PendingDocument? SelectedPendingDocument { get => _selectedPendingDocument; set { _selectedPendingDocument = value; OnPropertyChanged(); } }
    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SelectDocumentsCommand { get; }
    public ICommand RemoveDocumentCommand { get; }

    public AddFamilyViewModel(IChildService childService, IParentService parentService, IDocumentService documentService, IApplicationSettingsService settings)
    {
        _childService = childService;
        _parentService = parentService;
        _documentService = documentService;
        _settings = settings;
        DocumentTypes.Add(new() { Value = Document.DocumentType.Passport, Name = "Паспорт" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.SNILS, Name = "СНИЛС" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Diagnosis, Name = "Диагноз" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Letter, Name = "Письмо" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Order, Name = "Приказ" });
        SelectedDocumentType = DocumentTypes.First();
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(() => Cancelled?.Invoke());
        SelectDocumentsCommand = new RelayCommand(SelectDocuments);
        RemoveDocumentCommand = new RelayCommand(RemoveDocument);
    }

    public void PrepareForChild(ParentDTO parent)
    {
        _selectedParent = parent;
        _isParentForm = false;
        ClearForm();
        NotifyModeChanged();
    }

    public void PrepareForParent()
    {
        _selectedParent = null;
        _isParentForm = true;
        ClearForm();
        NotifyModeChanged();
    }

    public void AcceptDroppedFiles(IEnumerable<string> paths)
    {
        if (SelectedDocumentType is null)
        {
            StatusMessage = "Выберите тип документа.";
            return;
        }

        var added = AddPendingDocuments(paths, SelectedDocumentType.Value);
        StatusMessage = added > 0 ? $"Добавлено файлов: {added}." : "Файл не найден";
    }

    private void NotifyModeChanged()
    {
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(ChildFieldsVisibility));
        OnPropertyChanged(nameof(ParentFieldsVisibility));
    }

    private void Save()
    {
        try
        {
            if (_isParentForm)
            {
                var parent = _parentService.Add(CreateParent());
                SaveDocuments(DocumentOwnerType.Parent, parent.Id);
                StatusMessage = "Родитель или законный представитель сохранён.";
            }
            else
            {
                if (_selectedParent is null) throw new InvalidOperationException("Выберите родителя или законного представителя.");
                var child = _childService.AddChild(new ChildDTO(
                    ChildName, ChildSurname, ChildPatronymic, ChildPhone, ChildLivingPlace,
                    RequireDate(ChildBirthDate, "ребёнка"), [_selectedParent.Id]));
                SaveDocuments(DocumentOwnerType.Child, child.Id);
                StatusMessage = "Ребёнок сохранён.";
            }
            Saved?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private ParentDTO CreateParent() => new(ParentName, ParentSurname, ParentPatronymic,
        ParentPhone, ParentLivingPlace, RequireDate(ParentBirthDate, "родителя"), ParentEmail);

    private static DateTime RequireDate(DateTime? date, string person) =>
        date ?? throw new ArgumentException($"Укажите дату рождения {person}.");

    private void SelectDocuments()
    {
        if (SelectedDocumentType is null) { StatusMessage = "Выберите тип документа."; return; }
        var dialog = new OpenFileDialog
        {
            Title = "Выберите документы",
            Multiselect = true
        };
        if (dialog.ShowDialog() != true) return;

        var added = AddPendingDocuments(dialog.FileNames, SelectedDocumentType.Value);
        StatusMessage = added > 0 ? $"Добавлено файлов: {added}." : "Файл не найден";
    }

    private int AddPendingDocuments(IEnumerable<string> paths, Document.DocumentType type)
    {
        var added = 0;
        foreach (var path in paths.Where(File.Exists))
        {
            if (PendingDocuments.Any(document =>
                    string.Equals(document.Path, path, StringComparison.OrdinalIgnoreCase))) continue;

            PendingDocuments.Add(new PendingDocument { Type = type, Path = path });
            added++;
        }
        return added;
    }

    private void RemoveDocument()
    {
        if (SelectedPendingDocument is not null) PendingDocuments.Remove(SelectedPendingDocument);
    }

    private void SaveDocuments(DocumentOwnerType ownerType, Guid ownerId)
    {
        foreach (var document in PendingDocuments)
        {
            var destination = CopyToDocumentsDirectory(document.Path);
            _documentService.AddToOwner(new DocumentDTO(Guid.Empty, document.Type, destination), ownerType, ownerId);
        }
    }

    private string CopyToDocumentsDirectory(string sourcePath)
    {
        var name = Path.GetFileName(sourcePath);
        var destination = Path.Combine(_settings.DocumentsDirectory, name);
        var number = 2;
        while (File.Exists(destination))
        {
            destination = Path.Combine(_settings.DocumentsDirectory, $"{Path.GetFileNameWithoutExtension(name)} ({number++}){Path.GetExtension(name)}");
        }
        File.Copy(sourcePath, destination);
        return destination;
    }

    private void ClearForm()
    {
        ChildName = ChildSurname = ChildPatronymic = ChildPhone = ChildLivingPlace = string.Empty;
        ChildBirthDate = null;
        ParentName = ParentSurname = ParentPatronymic = ParentPhone = ParentLivingPlace = ParentEmail = string.Empty;
        ParentBirthDate = null;
        PendingDocuments.Clear();
        SelectedPendingDocument = null;
        StatusMessage = string.Empty;
    }
}
