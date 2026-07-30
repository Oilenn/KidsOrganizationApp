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
    public string Name => $"{Type}: {System.IO.Path.GetFileName(Path)}";
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
    private DateTime? _parentBirthDate;
    public DateTime? ParentBirthDate { get => _parentBirthDate; set { _parentBirthDate = value; OnPropertyChanged(); } }

    private DocumentTypeChoice? _selectedDocumentType;
    public DocumentTypeChoice? SelectedDocumentType { get => _selectedDocumentType; set { _selectedDocumentType = value; OnPropertyChanged(); } }
    private string _documentPath = string.Empty;
    public string DocumentPath { get => _documentPath; set { _documentPath = value; OnPropertyChanged(); } }
    private PendingDocument? _selectedPendingDocument;
    public PendingDocument? SelectedPendingDocument { get => _selectedPendingDocument; set { _selectedPendingDocument = value; OnPropertyChanged(); } }
    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }
    public ICommand SelectDocumentCommand { get; }
    public ICommand AddDocumentCommand { get; }
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
        SelectDocumentCommand = new RelayCommand(SelectDocument);
        AddDocumentCommand = new RelayCommand(AddDocument);
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
        ParentPhone, ParentLivingPlace, RequireDate(ParentBirthDate, "родителя"));

    private static DateTime RequireDate(DateTime? date, string person) =>
        date ?? throw new ArgumentException($"Укажите дату рождения {person}.");

    private void SelectDocument()
    {
        var dialog = new OpenFileDialog { Title = "Выберите документ" };
        if (dialog.ShowDialog() == true) DocumentPath = dialog.FileName;
    }

    private void AddDocument()
    {
        if (SelectedDocumentType is null) { StatusMessage = "Выберите тип документа."; return; }
        if (string.IsNullOrWhiteSpace(DocumentPath) || !File.Exists(DocumentPath)) { StatusMessage = "Выберите существующий файл документа."; return; }
        PendingDocuments.Add(new PendingDocument { Type = SelectedDocumentType.Value, Path = DocumentPath });
        DocumentPath = string.Empty;
        StatusMessage = string.Empty;
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
        ParentName = ParentSurname = ParentPatronymic = ParentPhone = ParentLivingPlace = string.Empty;
        ParentBirthDate = null;
        DocumentPath = string.Empty;
        PendingDocuments.Clear();
        SelectedPendingDocument = null;
        StatusMessage = string.Empty;
    }
}
