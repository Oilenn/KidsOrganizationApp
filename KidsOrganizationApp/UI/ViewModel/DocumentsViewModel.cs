using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Domain;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace KidsOrganizationApp;

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
    private readonly IDocumentFileService _documentFiles;
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
        : (Brush)Application.Current.Resources["ErrorText"];
    public Visibility DocumentsEmptyVisibility => Documents.Count == 0
        ? Visibility.Visible
        : Visibility.Collapsed;

    public ICommand SaveCommand { get; }
    public ICommand SelectFileCommand { get; }
    public ICommand OpenDocumentCommand { get; }
    public ICommand ShowInExplorerCommand { get; }
    public ICommand OpenDocumentsFolderCommand { get; }

    public DocumentsViewModel(
        IDocumentService documentService,
        IDocumentFileService documentFiles,
        IChildService childService,
        IParentService parentService,
        IEventService eventService,
        IApplicationSettingsService settings)
    {
        _documentService = documentService;
        _documentFiles = documentFiles;
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
        OwnerTypes.Add(new() { Value = DocumentOwnerType.Parent, Name = "Родитель или законный представитель" });
        OwnerTypes.Add(new() { Value = DocumentOwnerType.Event, Name = "Мероприятие" });

        _selectedDocumentType = DocumentTypes.First();
        _selectedOwnerType = OwnerTypes.First();
        SaveCommand = new RelayCommand(Save);
        SelectFileCommand = new RelayCommand(SelectFile);
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        OpenDocumentsFolderCommand = new RelayCommand(OpenDocumentsFolder);
        LoadDocuments();
        LoadOwners();
    }

    public void AcceptDroppedFiles(IEnumerable<string> paths)
    {
        var path = paths.FirstOrDefault(File.Exists);
        if (path is null)
        {
            SetError("Файл не найден");
            return;
        }
        DocumentPath = path;
        StatusMessage = "Файл выбран. Проверьте тип и владельца документа.";
        IsSuccessMessage = true;
    }

    public void PrepareForOwner(DocumentOwnerType ownerType, Guid ownerId)
    {
        SelectedOwnerType = OwnerTypes.First(type => type.Value == ownerType);
        SelectedOwner = Owners.FirstOrDefault(owner => owner.Id == ownerId);
        DocumentPath = string.Empty;
        StatusMessage = string.Empty;
    }

    private void LoadDocuments()
    {
        Documents.Clear();
        foreach (var document in _documentService.GetAll()) Documents.Add(document);
        OnPropertyChanged(nameof(DocumentsEmptyVisibility));
    }

    private void LoadOwners()
    {
        Owners.Clear();
        if (SelectedOwnerType is null) return;
        IEnumerable<OwnerChoice> owners = SelectedOwnerType.Value switch
        {
            DocumentOwnerType.Child => _childService.GetAllChildren().Select(child =>
                new OwnerChoice { Id = child.Id, Name = $"Ребёнок: {child.Surname} {child.Name}" }),
            DocumentOwnerType.Parent => _parentService.GetAllParents().Select(parent =>
                new OwnerChoice { Id = parent.Id, Name = $"Представитель: {parent.Surname} {parent.Name}" }),
            DocumentOwnerType.Event => _eventService.GetAll().Select(item =>
                new OwnerChoice { Id = item.Id, Name = $"Мероприятие: {item.Name}" }),
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
            if (SelectedDocumentType is null || SelectedOwnerType is null) throw new ArgumentException("Выберите тип и владельца документа.");
            if (string.IsNullOrWhiteSpace(DocumentPath) || !File.Exists(DocumentPath)) throw new FileNotFoundException("Файл не найден");

            var copiedFilePath = CopyToDocumentsDirectory(DocumentPath);
            _documentService.AddToOwner(
                new DocumentDTO(Guid.Empty, SelectedDocumentType.Value, copiedFilePath),
                SelectedOwnerType.Value,
                SelectedOwner.Id);
            DocumentPath = string.Empty;
            StatusMessage = "Документ добавлен и привязан к выбранной записи.";
            IsSuccessMessage = true;
            LoadDocuments();
        }
        catch (FileNotFoundException)
        {
            SetError("Файл не найден");
        }
        catch
        {
            SetError("Файл поврежден");
        }
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
            targetPath = Path.Combine(
                _settings.DocumentsDirectory,
                $"{Path.GetFileNameWithoutExtension(originalName)} ({number++}){Path.GetExtension(originalName)}");
        }
        File.Copy(sourcePath, targetPath);
        return targetPath;
    }

    private void OpenDocument(DocumentDTO? document)
    {
        if (document is null) return;
        var error = _documentFiles.Open(document);
        if (error is not null) SetError(error);
        else StatusMessage = string.Empty;
    }

    private void ShowInExplorer(DocumentDTO? document)
    {
        if (document is null) return;
        var error = _documentFiles.ShowInExplorer(document);
        if (error is not null) SetError(error);
        else StatusMessage = string.Empty;
    }

    private void OpenDocumentsFolder()
    {
        var error = _documentFiles.OpenFolder(_settings.DocumentsDirectory);
        if (error is not null) SetError(error);
        else StatusMessage = string.Empty;
    }

    private void SetError(string message)
    {
        StatusMessage = message;
        IsSuccessMessage = false;
    }
}
