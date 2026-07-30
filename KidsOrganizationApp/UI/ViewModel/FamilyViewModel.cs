using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class FamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private readonly IDocumentService _documentService;
    private readonly RelayCommand _addChildCommand;

    public event Action<ParentDTO>? AddChildRequested;
    public event Action? AddParentRequested;
    public event Action<DocumentOwnerType, Guid>? DocumentAddRequested;

    public ObservableCollection<ParentDTO> Parents { get; } = new();
    public ObservableCollection<ChildDTO> Children { get; } = new();
    public ObservableCollection<DocumentDTO> AttachedDocuments { get; } = new();

    private ParentDTO? _selectedParent;
    public ParentDTO? SelectedParent
    {
        get => _selectedParent;
        set
        {
            _selectedParent = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedParentVisibility));
            OnPropertyChanged(nameof(DocumentsTitle));
            _addChildCommand.NotifyCanExecuteChanged();
            LoadChildren();
            LoadDocuments();
        }
    }

    private ChildDTO? _selectedChild;
    public ChildDTO? SelectedChild
    {
        get => _selectedChild;
        set
        {
            _selectedChild = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedChildVisibility));
            OnPropertyChanged(nameof(DocumentsTitle));
            LoadDocuments();
        }
    }

    public Visibility SelectedParentVisibility => SelectedParent is null ? Visibility.Collapsed : Visibility.Visible;
    public Visibility SelectedChildVisibility => SelectedChild is null ? Visibility.Collapsed : Visibility.Visible;
    public string DocumentsTitle => SelectedChild is not null
        ? "Документы ребёнка"
        : "Документы родителя или законного представителя";

    public ICommand AddChildCommand => _addChildCommand;
    public ICommand AddParentCommand { get; }
    public ICommand AddDocumentCommand { get; }
    public ICommand OpenDocumentCommand { get; }

    private DocumentDTO? _selectedDocument;
    public DocumentDTO? SelectedDocument { get => _selectedDocument; set { _selectedDocument = value; OnPropertyChanged(); } }

    public FamilyViewModel(IChildService childService, IParentService parentService, IDocumentService documentService)
    {
        _childService = childService;
        _parentService = parentService;
        _documentService = documentService;
        _addChildCommand = new RelayCommand(() => AddChildRequested?.Invoke(SelectedParent!), () => SelectedParent is not null);
        AddParentCommand = new RelayCommand(() => AddParentRequested?.Invoke());
        AddDocumentCommand = new RelayCommand(AddDocument);
        OpenDocumentCommand = new RelayCommand(OpenDocument);
        Refresh();
    }

    public void Refresh()
    {
        var parentId = SelectedParent?.Id;
        var childId = SelectedChild?.Id;
        Parents.Clear();
        foreach (var parent in _parentService.GetAllParents()) Parents.Add(parent);
        SelectedParent = parentId is null ? null : Parents.FirstOrDefault(parent => parent.Id == parentId);
        if (SelectedParent is not null && childId is not null) SelectedChild = Children.FirstOrDefault(child => child.Id == childId);
    }

    private void LoadChildren()
    {
        var selectedChildId = SelectedChild?.Id;
        Children.Clear();
        SelectedChild = null;
        if (SelectedParent is null) return;
        foreach (var child in _childService.GetAllChildren().Where(child => child.ParentIds.Contains(SelectedParent.Id)))
        {
            Children.Add(child);
        }
        if (selectedChildId is not null) SelectedChild = Children.FirstOrDefault(child => child.Id == selectedChildId);
    }

    private void LoadDocuments()
    {
        AttachedDocuments.Clear();
        SelectedDocument = null;
        var ids = SelectedChild?.DocumentIds ?? SelectedParent?.DocumentIds;
        if (ids is null) return;
        foreach (var document in _documentService.GetByIds(ids)) AttachedDocuments.Add(document);
    }

    private void AddDocument()
    {
        if (SelectedChild is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Child, SelectedChild.Id);
        else if (SelectedParent is not null) DocumentAddRequested?.Invoke(DocumentOwnerType.Parent, SelectedParent.Id);
    }

    private void OpenDocument()
    {
        if (SelectedDocument is not null && File.Exists(SelectedDocument.Path))
        {
            Process.Start(new ProcessStartInfo(SelectedDocument.Path) { UseShellExecute = true });
        }
    }
}
