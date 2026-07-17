using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;

public sealed class OwnerChoice
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class DocumentTypeChoice
{
    public Document.DocumentType Value { get; init; }
    public string Name { get; init; } = string.Empty;
}

public sealed class OwnerTypeChoice
{
    public DocumentOwnerType Value { get; init; }
    public string Name { get; init; } = string.Empty;
}

public class DocumentsViewModel : BaseViewModel
{
    private readonly IDocumentService _documentService;
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private readonly IEventService _eventService;
    private readonly IApplicationSettingsService _settings;

    public ObservableCollection<DocumentDTO> Documents { get; } = new();
    public ObservableCollection<OwnerChoice> Owners { get; } = new();
    public ObservableCollection<DocumentTypeChoice> DocumentTypes { get; } = new();
    public ObservableCollection<OwnerTypeChoice> OwnerTypes { get; } = new();

    private DocumentTypeChoice? _selectedDocumentType;
    public DocumentTypeChoice? SelectedDocumentType { get => _selectedDocumentType; set { _selectedDocumentType = value; OnPropertyChanged(); } }
    private OwnerTypeChoice? _selectedOwnerType;
    public OwnerTypeChoice? SelectedOwnerType { get => _selectedOwnerType; set { _selectedOwnerType = value; OnPropertyChanged(); LoadOwners(); } }
    private OwnerChoice? _selectedOwner;
    public OwnerChoice? SelectedOwner { get => _selectedOwner; set { _selectedOwner = value; OnPropertyChanged(); } }
    private string _documentPath = string.Empty;
    public string DocumentPath { get => _documentPath; set { _documentPath = value; OnPropertyChanged(); } }
    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; set { _statusMessage = value; OnPropertyChanged(); } }
    private bool _isSuccessMessage;
    public bool IsSuccessMessage { get => _isSuccessMessage; private set { _isSuccessMessage = value; OnPropertyChanged(); OnPropertyChanged(nameof(StatusBrush)); } }
    public Brush StatusBrush => IsSuccessMessage
        ? (Brush)Application.Current.Resources["SuccessText"]
        : Brushes.Firebrick;
    public ICommand SaveCommand { get; }
    public ICommand SelectFileCommand { get; }
    public ICommand OpenDocumentCommand { get; }
    private DocumentDTO? _selectedDocument;
    public DocumentDTO? SelectedDocument { get => _selectedDocument; set { _selectedDocument = value; OnPropertyChanged(); } }

    public DocumentsViewModel(IDocumentService documentService, IChildService childService, IParentService parentService, IEventService eventService, IApplicationSettingsService settings)
    {
        _documentService = documentService;
        _childService = childService;
        _parentService = parentService;
        _eventService = eventService;
        _settings = settings;
        DocumentTypes.Add(new() { Value = Document.DocumentType.Passport, Name = "Паспорт" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.SNILS, Name = "СНИЛС" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Diagnosis, Name = "Диагноз" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Letter, Name = "Письмо" });
        DocumentTypes.Add(new() { Value = Document.DocumentType.Order, Name = "Приказ" });
        OwnerTypes.Add(new() { Value = DocumentOwnerType.Child, Name = "Ребёнок" });
        OwnerTypes.Add(new() { Value = DocumentOwnerType.Parent, Name = "Родитель" });
        OwnerTypes.Add(new() { Value = DocumentOwnerType.Event, Name = "Мероприятие" });
        _selectedDocumentType = DocumentTypes.First();
        _selectedOwnerType = OwnerTypes.First();
        SaveCommand = new RelayCommand(Save);
        SelectFileCommand = new RelayCommand(SelectFile);
        OpenDocumentCommand = new RelayCommand(OpenDocument);
        LoadDocuments();
        LoadOwners();
    }

    private void LoadDocuments()
    {
        Documents.Clear();
        foreach (var document in _documentService.GetAll()) Documents.Add(document);
    }

    public void PrepareForOwner(DocumentOwnerType ownerType, Guid ownerId)
    {
        SelectedOwnerType = OwnerTypes.First(type => type.Value == ownerType);
        SelectedOwner = Owners.FirstOrDefault(owner => owner.Id == ownerId);
        DocumentPath = string.Empty;
        StatusMessage = string.Empty;
    }

    private void LoadOwners()
    {
        Owners.Clear();
        if (SelectedOwnerType is null) return;
        IEnumerable<OwnerChoice> owners = SelectedOwnerType.Value switch
        {
            DocumentOwnerType.Child => _childService.GetAllChildren().Select(c => new OwnerChoice { Id = c.Id, Name = $"Ребенок: {c.Surname} {c.Name}" }),
            DocumentOwnerType.Parent => _parentService.GetAllParents().Select(p => new OwnerChoice { Id = p.Id, Name = $"Родитель: {p.Surname} {p.Name}" }),
            DocumentOwnerType.Event => _eventService.GetAll().Select(e => new OwnerChoice { Id = e.Id, Name = $"Мероприятие: {e.Name}" }),
            _ => []
        };
        foreach (var owner in owners) Owners.Add(owner);
        SelectedOwner = Owners.FirstOrDefault();
    }

    private void Save()
    {
        try
        {
            if (SelectedOwner is null) throw new ArgumentException("Выберите владельца документа.");
            if (string.IsNullOrWhiteSpace(DocumentPath)) throw new ArgumentException("Укажите путь к документу.");
            if (SelectedDocumentType is null || SelectedOwnerType is null) throw new ArgumentException("Выберите тип и владельца документа.");
            if (!File.Exists(DocumentPath)) throw new FileNotFoundException("Выбранный файл не найден.", DocumentPath);
            var copiedFilePath = CopyToDocumentsDirectory(DocumentPath);
            _documentService.AddToOwner(new DocumentDTO(Guid.Empty, SelectedDocumentType.Value, copiedFilePath), SelectedOwnerType.Value, SelectedOwner.Id);
            DocumentPath = string.Empty;
            StatusMessage = "Документ добавлен и привязан к выбранной записи.";
            IsSuccessMessage = true;
            LoadDocuments();
        }
        catch (Exception ex) { StatusMessage = ex.Message; IsSuccessMessage = false; }
    }

    private void SelectFile()
    {
        var dialog = new OpenFileDialog { Title = "Выберите документ" };
        if (dialog.ShowDialog() == true) DocumentPath = dialog.FileName;
    }

    private string CopyToDocumentsDirectory(string sourcePath)
    {
        var originalName = Path.GetFileName(sourcePath);
        var targetPath = Path.Combine(_settings.DocumentsDirectory, originalName);
        var number = 2;
        while (File.Exists(targetPath))
        {
            targetPath = Path.Combine(_settings.DocumentsDirectory, $"{Path.GetFileNameWithoutExtension(originalName)} ({number++}){Path.GetExtension(originalName)}");
        }
        File.Copy(sourcePath, targetPath);
        return targetPath;
    }

    private void OpenDocument()
    {
        if (SelectedDocument is null) { StatusMessage = "Выберите документ в списке."; IsSuccessMessage = false; return; }
        if (!File.Exists(SelectedDocument.Path)) { StatusMessage = "Файл документа не найден."; IsSuccessMessage = false; return; }
        Process.Start(new ProcessStartInfo(SelectedDocument.Path) { UseShellExecute = true });
    }
}
