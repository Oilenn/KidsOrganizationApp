using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace KidsOrganizationApp;

public class FamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private readonly IDocumentService _documentService;
    private readonly IDocumentFileService _documentFiles;
    private readonly RelayCommand _addChildCommand;
    private readonly RelayCommand _addParentDocumentCommand;
    private readonly RelayCommand _addChildDocumentCommand;

    public event Action<ParentDTO>? AddChildRequested;
    public event Action? AddParentRequested;
    public event Action<DocumentOwnerType, Guid>? DocumentAddRequested;

    public ObservableCollection<ParentDTO> Parents { get; } = new();
    public ObservableCollection<ChildDTO> Children { get; } = new();
    public ObservableCollection<DocumentDTO> ParentDocuments { get; } = new();
    public ObservableCollection<DocumentDTO> ChildDocuments { get; } = new();

    private ParentDTO? _selectedParent;
    public ParentDTO? SelectedParent
    {
        get => _selectedParent;
        set
        {
            _selectedParent = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(SelectedParentVisibility));
            _addChildCommand.NotifyCanExecuteChanged();
            _addParentDocumentCommand.NotifyCanExecuteChanged();
            LoadChildren();
            LoadParentDocuments();
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
            _addChildDocumentCommand.NotifyCanExecuteChanged();
            LoadChildDocuments();
        }
    }

    public Visibility SelectedParentVisibility => SelectedParent is null ? Visibility.Collapsed : Visibility.Visible;
    public Visibility SelectedChildVisibility => SelectedChild is null ? Visibility.Collapsed : Visibility.Visible;
    public Visibility ParentDocumentsEmptyVisibility =>
        SelectedParent is not null && ParentDocuments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    public Visibility ChildDocumentsEmptyVisibility =>
        SelectedChild is not null && ChildDocuments.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
    public string ParentDocumentsEmptyMessage => SelectedParent is null
        ? string.Empty
        : $"Нет документов у {SelectedParent.Name} {SelectedParent.Surname}";
    public string ChildDocumentsEmptyMessage => SelectedChild is null
        ? string.Empty
        : $"Нет документов у {SelectedChild.Name} {SelectedChild.Surname}";

    public ICommand AddChildCommand => _addChildCommand;
    public ICommand AddParentCommand { get; }
    public ICommand AddParentDocumentCommand => _addParentDocumentCommand;
    public ICommand AddChildDocumentCommand => _addChildDocumentCommand;
    public ICommand OpenDocumentCommand { get; }
    public ICommand ShowInExplorerCommand { get; }

    private string _documentStatusMessage = string.Empty;
    public string DocumentStatusMessage { get => _documentStatusMessage; private set { _documentStatusMessage = value; OnPropertyChanged(); } }

    public FamilyViewModel(
        IChildService childService,
        IParentService parentService,
        IDocumentService documentService,
        IDocumentFileService documentFiles)
    {
        _childService = childService;
        _parentService = parentService;
        _documentService = documentService;
        _documentFiles = documentFiles;
        _addChildCommand = new RelayCommand(() => AddChildRequested?.Invoke(SelectedParent!), () => SelectedParent is not null);
        _addParentDocumentCommand = new RelayCommand(
            () => DocumentAddRequested?.Invoke(DocumentOwnerType.Parent, SelectedParent!.Id),
            () => SelectedParent is not null);
        _addChildDocumentCommand = new RelayCommand(
            () => DocumentAddRequested?.Invoke(DocumentOwnerType.Child, SelectedChild!.Id),
            () => SelectedChild is not null);
        AddParentCommand = new RelayCommand(() => AddParentRequested?.Invoke());
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        Refresh();
    }

    public void Refresh()
    {
        var parentId = SelectedParent?.Id;
        var childId = SelectedChild?.Id;
        Parents.Clear();
        foreach (var parent in _parentService.GetAllParents()) Parents.Add(parent);
        SelectedParent = parentId is null ? null : Parents.FirstOrDefault(parent => parent.Id == parentId);
        if (SelectedParent is not null && childId is not null)
        {
            SelectedChild = Children.FirstOrDefault(child => child.Id == childId);
        }
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
        if (selectedChildId is not null)
        {
            SelectedChild = Children.FirstOrDefault(child => child.Id == selectedChildId);
        }
    }

    private void LoadParentDocuments()
    {
        ParentDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (SelectedParent is not null)
        {
            foreach (var document in _documentService.GetByIds(SelectedParent.DocumentIds)) ParentDocuments.Add(document);
        }
        OnPropertyChanged(nameof(ParentDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(ParentDocumentsEmptyMessage));
    }

    private void LoadChildDocuments()
    {
        ChildDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (SelectedChild is not null)
        {
            foreach (var document in _documentService.GetByIds(SelectedChild.DocumentIds)) ChildDocuments.Add(document);
        }
        OnPropertyChanged(nameof(ChildDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(ChildDocumentsEmptyMessage));
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
