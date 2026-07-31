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

    public event Action<DocumentOwnerType, Guid>? DocumentAddRequested;
    public ObservableCollection<EventDTO> Events { get; } = new();
    public ObservableCollection<DocumentDTO> AttachedDocuments { get; } = new();

    private EventDTO? _selectedEvent;
    public EventDTO? SelectedEvent { get => _selectedEvent; set { _selectedEvent = value; OnPropertyChanged(); LoadDocuments(); } }
    private string _eventName = string.Empty;
    public string EventName { get => _eventName; set { _eventName = value; OnPropertyChanged(); } }
    private DateTime? _eventDate = DateTime.Today;
    public DateTime? EventDate { get => _eventDate; set { _eventDate = value; OnPropertyChanged(); } }
    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }
    private string _documentStatusMessage = string.Empty;
    public string DocumentStatusMessage { get => _documentStatusMessage; private set { _documentStatusMessage = value; OnPropertyChanged(); } }
    public Visibility EventDocumentsEmptyVisibility =>
        SelectedEvent is not null && AttachedDocuments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    public string EventDocumentsEmptyMessage => SelectedEvent is null
        ? string.Empty
        : $"Нет документов у мероприятия «{SelectedEvent.Name}»";

    public ICommand SaveCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand OpenDocumentCommand { get; }
    public ICommand ShowInExplorerCommand { get; }

    public EventsViewModel(IEventService eventService, IDocumentService documentService, IDocumentFileService documentFiles)
    {
        _eventService = eventService;
        _documentService = documentService;
        _documentFiles = documentFiles;
        SaveCommand = new RelayCommand(Save);
        AddDocumentCommand = new RelayCommand(() =>
        {
            if (SelectedEvent is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Event, SelectedEvent.Id);
        });
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        Refresh();
    }

    public void Refresh()
    {
        var selectedId = SelectedEvent?.Id;
        Events.Clear();
        foreach (var item in _eventService.GetAll()) Events.Add(item);
        SelectedEvent = selectedId is null ? null : Events.FirstOrDefault(item => item.Id == selectedId);
    }

    private void LoadDocuments()
    {
        AttachedDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (SelectedEvent is not null)
        {
            foreach (var document in _documentService.GetByIds(SelectedEvent.Documents)) AttachedDocuments.Add(document);
        }
        OnPropertyChanged(nameof(EventDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(EventDocumentsEmptyMessage));
    }

    private void Save()
    {
        try
        {
            if (EventDate is null) throw new ArgumentException("Укажите дату мероприятия.");
            _eventService.Add(new EventDTO(Guid.Empty, EventName, EventDate.Value));
            EventName = string.Empty;
            EventDate = DateTime.Today;
            StatusMessage = "Мероприятие добавлено.";
            Refresh();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private void OpenDocument(DocumentDTO? document)
    {
        if (document is null) return;
        DocumentStatusMessage = _documentFiles.Open(document) ?? string.Empty;
    }

    private void ShowInExplorer(DocumentDTO? document)
    {
        if (document is null) return;
        DocumentStatusMessage = _documentFiles.ShowInExplorer(document) ?? string.Empty;
    }
}
