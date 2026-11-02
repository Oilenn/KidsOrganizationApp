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
    private readonly AsyncRelayCommand _loadOwnersCommand;

    public ObservableCollection<DocumentDTO> Documents { get; } = new();
    public ObservableCollection<OwnerChoice> Owners { get; } = new();
    public ObservableCollection<DocumentTypeChoice> DocumentTypes { get; } = new();
    public ObservableCollection<OwnerTypeChoice> OwnerTypes { get; } = new();

    private DocumentTypeChoice? _selectedDocumentType;
    public DocumentTypeChoice? SelectedDocumentType
    {
        get => _selectedDocumentType;
        set
        {
            _selectedDocumentType = value;
            OnPropertyChanged();
        }
    }

    private OwnerTypeChoice? _selectedOwnerType;
    public OwnerTypeChoice? SelectedOwnerType
    {
        get => _selectedOwnerType;
        set
        {
            if (_selectedOwnerType == value)
                return;
            _selectedOwnerType = value;
            OnPropertyChanged();
            _loadOwnersCommand.Execute(null);
        }
    }

    private OwnerChoice? _selectedOwner;
    public OwnerChoice? SelectedOwner
    {
        get => _selectedOwner;
        set
        {
            _selectedOwner = value;
            OnPropertyChanged();
        }
    }

    private string _documentPath = string.Empty;
    public string DocumentPath
    {
        get => _documentPath;
        set
        {
            _documentPath = value;
            OnPropertyChanged();
        }
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    private bool _isSuccessMessage;
    public bool IsSuccessMessage
    {
        get => _isSuccessMessage;
        private set
        {
            _isSuccessMessage = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(StatusBrush));
        }
    }

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
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        SelectFileCommand = new RelayCommand(SelectFile);
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        OpenDocumentsFolderCommand = new RelayCommand(OpenDocumentsFolder);
        _loadOwnersCommand = new AsyncRelayCommand(LoadOwnersAsync);
    }

    public async Task InitializeAsync()
    {
        await LoadDocumentsAsync();
        await LoadOwnersAsync();
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

    public async Task PrepareForOwnerAsync(DocumentOwnerType ownerType, Guid ownerId)
    {
        _selectedOwnerType = OwnerTypes.First(type => type.Value == ownerType);
        OnPropertyChanged(nameof(SelectedOwnerType));
        await LoadOwnersAsync();
        SelectedOwner = Owners.FirstOrDefault(owner => owner.Id == ownerId);
        DocumentPath = string.Empty;
        StatusMessage = string.Empty;
    }

    private async Task LoadDocumentsAsync()
    {
        Documents.Clear();
        foreach (var document in await _documentService.GetAllAsync())
            Documents.Add(document);
        OnPropertyChanged(nameof(DocumentsEmptyVisibility));
    }

    private async Task LoadOwnersAsync()
    {
        var ownerType = SelectedOwnerType;
        Owners.Clear();
        if (ownerType is null)
            return;
        IEnumerable<OwnerChoice> owners = ownerType.Value switch
        {
            DocumentOwnerType.Child => (await _childService.GetAllChildrenAsync()).Select(child =>
                new OwnerChoice { Id = child.Id, Name = $"Ребёнок: {child.Surname} {child.Name}" }),
            DocumentOwnerType.Parent => (await _parentService.GetAllParentsAsync()).Select(parent =>
                new OwnerChoice { Id = parent.Id, Name = $"Представитель: {parent.Surname} {parent.Name}" }),
            DocumentOwnerType.Event => (await _eventService.GetAllAsync()).Select(item =>
                new OwnerChoice { Id = item.Id, Name = $"Мероприятие: {item.Name}" }),
            _ => []
        };
        if (SelectedOwnerType != ownerType)
            return;
        foreach (var owner in owners)
            Owners.Add(owner);
        SelectedOwner = Owners.FirstOrDefault();
    }

    private async Task SaveAsync()
    {
        try
        {
            var owner = SelectedOwner;
            var documentType = SelectedDocumentType;
            var ownerType = SelectedOwnerType;
            var documentPath = DocumentPath;
            if (owner is null)
                throw new ArgumentException("Выберите владельца документа.");
            if (documentType is null || ownerType is null)
                throw new ArgumentException("Выберите тип и владельца документа.");
            if (string.IsNullOrWhiteSpace(documentPath) || !File.Exists(documentPath))
                throw new FileNotFoundException("Файл не найден");

            var copiedFilePath = await _documentFiles.CopyToDirectoryAsync(documentPath, _settings.DocumentsDirectory);
            await _documentService.AddToOwnerAsync(
                new DocumentDTO(Guid.Empty, documentType.Value, copiedFilePath),
                ownerType.Value,
                owner.Id);
            DocumentPath = string.Empty;
            StatusMessage = "Документ добавлен и привязан к выбранной записи.";
            IsSuccessMessage = true;
            await LoadDocumentsAsync();
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
        if (dialog.ShowDialog() == true)
            DocumentPath = dialog.FileName;
    }

    private void OpenDocument(DocumentDTO? document)
    {
        if (document is null)
            return;
        var error = _documentFiles.Open(document);
        if (error is not null)
            SetError(error);
        else
            StatusMessage = string.Empty;
    }

    private void ShowInExplorer(DocumentDTO? document)
    {
        if (document is null)
            return;
        var error = _documentFiles.ShowInExplorer(document);
        if (error is not null)
            SetError(error);
        else
            StatusMessage = string.Empty;
    }

    private void OpenDocumentsFolder()
    {
        var error = _documentFiles.OpenFolder(_settings.DocumentsDirectory);
        if (error is not null)
            SetError(error);
        else
            StatusMessage = string.Empty;
    }

    private void SetError(string message)
    {
        StatusMessage = message;
        IsSuccessMessage = false;
    }
}
