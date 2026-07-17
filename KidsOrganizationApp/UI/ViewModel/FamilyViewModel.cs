using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

public class FamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IDocumentService _documentService;
    public event Action<ChildDTO?>? AddRequested;
    public event Action<DocumentOwnerType, Guid>? DocumentAddRequested;
    public ObservableCollection<ChildDTO> Children { get; } = new();
    public ObservableCollection<ParentDTO> Parents { get; } = new();
    public ObservableCollection<DocumentDTO> AttachedDocuments { get; } = new();
    private ChildDTO? _selectedChild;
    public ChildDTO? SelectedChild { get => _selectedChild; set { _selectedChild = value; OnPropertyChanged(); OnPropertyChanged(nameof(SelectedChildVisibility)); LoadParents(); LoadDocuments(); } }
    private ParentDTO? _selectedParent;
    public ParentDTO? SelectedParent { get => _selectedParent; set { _selectedParent = value; OnPropertyChanged(); OnPropertyChanged(nameof(SelectedParentVisibility)); OnPropertyChanged(nameof(DocumentsTitle)); LoadDocuments(); } }
    public Visibility SelectedChildVisibility => SelectedChild is null ? Visibility.Collapsed : Visibility.Visible;
    public Visibility SelectedParentVisibility => SelectedParent is null ? Visibility.Collapsed : Visibility.Visible;
    public string DocumentsTitle => SelectedParent is not null ? "Документы родителя" : "Документы ребёнка";
    public ICommand AddChildCommand { get; }
    public ICommand AddParentCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand OpenDocumentCommand { get; }
    private DocumentDTO? _selectedDocument;
    public DocumentDTO? SelectedDocument { get => _selectedDocument; set { _selectedDocument = value; OnPropertyChanged(); } }

    public FamilyViewModel(IChildService childService, IParentService parentService, AddFamilyViewModel addFamilyViewModel, IDocumentService documentService)
    {
        _childService = childService; _documentService = documentService;
        AddChildCommand = new RelayCommand(() => AddRequested?.Invoke(null));
        AddParentCommand = new RelayCommand(() => { if (SelectedChild is not null) AddRequested?.Invoke(SelectedChild); });
        AddDocumentCommand = new RelayCommand(AddDocument);
        OpenDocumentCommand = new RelayCommand(OpenDocument);
        Refresh();
    }
    public void Refresh()
    {
        var selectedId = SelectedChild?.Id;
        Children.Clear(); 
        foreach (var child in _childService.GetAllChildren()) Children.Add(child);

        SelectedChild = selectedId is null ? null : Children.FirstOrDefault(c => c.Id == selectedId);
    }
    private void LoadParents()
    {
        Parents.Clear(); SelectedParent = null;
        if (SelectedChild is null) return;
        foreach (var parent in _childService.GetParents(SelectedChild)) Parents.Add(parent);
    }
    private void LoadDocuments()
    {
        AttachedDocuments.Clear(); SelectedDocument = null;
        var ids = SelectedParent?.DocumentIds ?? SelectedChild?.DocumentIds;
        if (ids is null) return;
        foreach (var document in _documentService.GetByIds(ids)) AttachedDocuments.Add(document);
    }
    private void AddDocument()
    {
        if (SelectedParent is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Parent, SelectedParent.Id);
        else if (SelectedChild is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Child, SelectedChild.Id);
    }
    private void OpenDocument()
    {
        if (SelectedDocument is not null && File.Exists(SelectedDocument.Path)) Process.Start(new ProcessStartInfo(SelectedDocument.Path) { UseShellExecute = true });
    }
}
