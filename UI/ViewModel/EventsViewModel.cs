using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class EventsViewModel : BaseViewModel
{
    private readonly IEventService _eventService;
    private readonly IDocumentService _documentService;
    private readonly IDocumentFileService _documentFiles;
    private readonly AsyncRelayCommand _loadDocumentsCommand;

    public event Func<DocumentOwnerType, Guid, Task>? DocumentAddRequested;
    public ObservableCollection<EventDTO> Events { get; } = new();
    public ObservableCollection<DocumentDTO> AttachedDocuments { get; } = new();

    private EventDTO? _selectedEvent;
    public EventDTO? SelectedEvent
    {
        get => _selectedEvent;
        set
        {
            if (_selectedEvent == value)
                return;
            _selectedEvent = value;
            OnPropertyChanged();
            _loadDocumentsCommand.Execute(null);
        }
    }

    private string _eventName = string.Empty;
    public string EventName
    {
        get => _eventName;
        set
        {
            _eventName = value;
            OnPropertyChanged();
        }
    }

    private DateTime? _eventDate = DateTime.Today;
    public DateTime? EventDate
    {
        get => _eventDate;
        set
        {
            _eventDate = value;
            OnPropertyChanged();
        }
    }

    private string _statusMessage = string.Empty;
    public string StatusMessage
    {
        get => _statusMessage;
        private set
        {
            _statusMessage = value;
            OnPropertyChanged();
        }
    }

    private string _documentStatusMessage = string.Empty;
    public string DocumentStatusMessage
    {
        get => _documentStatusMessage;
        private set
        {
            _documentStatusMessage = value;
            OnPropertyChanged();
        }
    }

    public Visibility EventDocumentsEmptyVisibility =>
        SelectedEvent is not null && AttachedDocuments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    public string EventDocumentsEmptyMessage => SelectedEvent is null
        ? string.Empty
        : $"Нет документов у мероприятия «{SelectedEvent.Name}»";

    public ICommand SaveCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand OpenDocumentCommand { get; }
    public ICommand ShowInExplorerCommand { get; }

    public EventsViewModel(
        IEventService eventService,
        IDocumentService documentService,
        IDocumentFileService documentFiles)
    {
        _eventService = eventService;
        _documentService = documentService;
        _documentFiles = documentFiles;
        SaveCommand = new AsyncRelayCommand(SaveAsync);
        AddDocumentCommand = new AsyncRelayCommand(AddDocumentAsync);
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        _loadDocumentsCommand = new AsyncRelayCommand(LoadDocumentsAsync);
    }

    public async Task RefreshAsync()
    {
        var events = await _eventService.GetAllAsync();
        var selectedId = SelectedEvent?.Id;
        Events.Clear();
        foreach (var item in events)
            Events.Add(item);
        _selectedEvent = selectedId is null ? null : Events.FirstOrDefault(item => item.Id == selectedId);
        OnPropertyChanged(nameof(SelectedEvent));
        await LoadDocumentsAsync();
    }

    private async Task LoadDocumentsAsync()
    {
        var selectedEvent = SelectedEvent;
        AttachedDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (selectedEvent is not null)
        {
            var documents = await _documentService.GetByIdsAsync(selectedEvent.Documents);
            if (SelectedEvent != selectedEvent)
                return;
            foreach (var document in documents)
                AttachedDocuments.Add(document);
        }
        OnPropertyChanged(nameof(EventDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(EventDocumentsEmptyMessage));
    }

    private async Task SaveAsync()
    {
        try
        {
            if (EventDate is null)
                throw new ArgumentException("Укажите дату мероприятия.");
            await _eventService.AddAsync(new EventDTO(Guid.Empty, EventName, EventDate.Value));
            EventName = string.Empty;
            EventDate = DateTime.Today;
            StatusMessage = "Мероприятие добавлено.";
            await RefreshAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private async Task AddDocumentAsync()
    {
        var selectedEvent = SelectedEvent;
        if (selectedEvent is not null && DocumentAddRequested is not null)
        {
            foreach (Func<DocumentOwnerType, Guid, Task> handler in DocumentAddRequested.GetInvocationList())
                await handler(DocumentOwnerType.Event, selectedEvent.Id);
        }
    }

    private void OpenDocument(DocumentDTO? document)
    {
        if (document is null)
            return;
        DocumentStatusMessage = _documentFiles.Open(document) ?? string.Empty;
    }

    private void ShowInExplorer(DocumentDTO? document)
    {
        if (document is null)
            return;
        DocumentStatusMessage = _documentFiles.ShowInExplorer(document) ?? string.Empty;
    }
}
