using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class EventsViewModel : BaseViewModel
{
    private readonly IEventService _eventService;
    private readonly IDocumentService _documentService;
    public event Action<DocumentOwnerType, Guid>? DocumentAddRequested;
    public ObservableCollection<EventDTO> Events { get; } = new();
    public ObservableCollection<DocumentDTO> AttachedDocuments { get; } = new();
    private EventDTO? _selectedEvent;
    public EventDTO? SelectedEvent { get => _selectedEvent; set { _selectedEvent = value; OnPropertyChanged(); LoadDocuments(); } }
    private DocumentDTO? _selectedDocument;
    public DocumentDTO? SelectedDocument { get => _selectedDocument; set { _selectedDocument = value; OnPropertyChanged(); } }
    private string _eventName = string.Empty;
    public string EventName { get => _eventName; set { _eventName = value; OnPropertyChanged(); } }
    private DateTime? _eventDate = DateTime.Today;
    public DateTime? EventDate { get => _eventDate; set { _eventDate = value; OnPropertyChanged(); } }
    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }
    public ICommand SaveCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand OpenDocumentCommand { get; }

    public EventsViewModel(IEventService eventService, IDocumentService documentService)
    {
        _eventService = eventService; _documentService = documentService;
        SaveCommand = new RelayCommand(Save);
        AddDocumentCommand = new RelayCommand(() => { if (SelectedEvent is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Event, SelectedEvent.Id); });
        OpenDocumentCommand = new RelayCommand(OpenDocument); Refresh();
    }
    public void Refresh() { Events.Clear(); foreach (var item in _eventService.GetAll()) Events.Add(item); }
    private void LoadDocuments() { AttachedDocuments.Clear(); SelectedDocument = null; if (SelectedEvent is null) return; foreach (var document in _documentService.GetByIds(SelectedEvent.Documents)) AttachedDocuments.Add(document); }
    private void Save()
    {
        try { if (EventDate is null) throw new ArgumentException("Укажите дату мероприятия."); _eventService.Add(new EventDTO(Guid.Empty, EventName, EventDate.Value)); EventName = string.Empty; EventDate = DateTime.Today; StatusMessage = "Мероприятие добавлено."; Refresh(); }
        catch (Exception ex) { StatusMessage = ex.Message; }
    }
    private void OpenDocument() { if (SelectedDocument is not null && File.Exists(SelectedDocument.Path)) Process.Start(new ProcessStartInfo(SelectedDocument.Path) { UseShellExecute = true }); }
}
