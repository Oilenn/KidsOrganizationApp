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
    private readonly AsyncRelayCommand _addParentDocumentCommand;
    private readonly AsyncRelayCommand _addChildDocumentCommand;
    private readonly AsyncRelayCommand _loadParentCommand;
    private readonly AsyncRelayCommand _loadChildDocumentsCommand;

    public event Action<ParentDTO>? AddChildRequested;
    public event Action? AddParentRequested;
    public event Func<DocumentOwnerType, Guid, Task>? DocumentAddRequested;

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
            if (_selectedParent == value)
                return;
            UpdateSelectedParent(value);
            _loadParentCommand.Execute(null);
        }
    }

    private ChildDTO? _selectedChild;
    public ChildDTO? SelectedChild
    {
        get => _selectedChild;
        set
        {
            if (_selectedChild == value)
                return;
            UpdateSelectedChild(value);
            _loadChildDocumentsCommand.Execute(null);
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
    public string DocumentStatusMessage
    {
        get => _documentStatusMessage;
        private set
        {
            _documentStatusMessage = value;
            OnPropertyChanged();
        }
    }

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
        _addParentDocumentCommand = new AsyncRelayCommand(
            () => RequestDocumentAsync(DocumentOwnerType.Parent, SelectedParent!.Id),
            () => SelectedParent is not null);
        _addChildDocumentCommand = new AsyncRelayCommand(
            () => RequestDocumentAsync(DocumentOwnerType.Child, SelectedChild!.Id),
            () => SelectedChild is not null);
        AddParentCommand = new RelayCommand(() => AddParentRequested?.Invoke());
        OpenDocumentCommand = new RelayCommand<DocumentDTO?>(OpenDocument);
        ShowInExplorerCommand = new RelayCommand<DocumentDTO?>(ShowInExplorer);
        _loadParentCommand = new AsyncRelayCommand(LoadParentAsync);
        _loadChildDocumentsCommand = new AsyncRelayCommand(LoadChildDocumentsAsync);
    }

    public async Task RefreshAsync()
    {
        var parents = await _parentService.GetAllParentsAsync();
        var parentId = SelectedParent?.Id;
        var childId = SelectedChild?.Id;
        Parents.Clear();
        foreach (var parent in parents)
            Parents.Add(parent);
        UpdateSelectedParent(parentId is null ? null : Parents.FirstOrDefault(parent => parent.Id == parentId));
        await LoadParentAsync();
        if (SelectedParent is not null && childId is not null)
        {
            UpdateSelectedChild(Children.FirstOrDefault(child => child.Id == childId));
            await LoadChildDocumentsAsync();
        }
    }

    private void UpdateSelectedParent(ParentDTO? parent)
    {
        _selectedParent = parent;
        OnPropertyChanged(nameof(SelectedParent));
        OnPropertyChanged(nameof(SelectedParentVisibility));
        _addChildCommand.NotifyCanExecuteChanged();
        _addParentDocumentCommand.NotifyCanExecuteChanged();
    }

    private void UpdateSelectedChild(ChildDTO? child)
    {
        _selectedChild = child;
        OnPropertyChanged(nameof(SelectedChild));
        OnPropertyChanged(nameof(SelectedChildVisibility));
        _addChildDocumentCommand.NotifyCanExecuteChanged();
    }

    private async Task LoadParentAsync()
    {
        var parent = SelectedParent;
        await LoadChildrenAsync();
        if (SelectedParent == parent)
            await LoadParentDocumentsAsync();
    }

    private async Task LoadChildrenAsync()
    {
        var parent = SelectedParent;
        var selectedChildId = SelectedChild?.Id;
        Children.Clear();
        UpdateSelectedChild(null);
        await LoadChildDocumentsAsync();
        if (parent is null)
            return;
        var children = await _childService.GetAllChildrenAsync();
        if (SelectedParent != parent)
            return;
        foreach (var child in children.Where(child => child.ParentIds.Contains(parent.Id)))
        {
            Children.Add(child);
        }
        if (selectedChildId is not null)
        {
            UpdateSelectedChild(Children.FirstOrDefault(child => child.Id == selectedChildId));
            await LoadChildDocumentsAsync();
        }
    }

    private async Task LoadParentDocumentsAsync()
    {
        var parent = SelectedParent;
        ParentDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (parent is not null)
        {
            var documents = await _documentService.GetByIdsAsync(parent.DocumentIds);
            if (SelectedParent != parent)
                return;
            foreach (var document in documents)
                ParentDocuments.Add(document);
        }
        OnPropertyChanged(nameof(ParentDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(ParentDocumentsEmptyMessage));
    }

    private async Task LoadChildDocumentsAsync()
    {
        var child = SelectedChild;
        ChildDocuments.Clear();
        DocumentStatusMessage = string.Empty;
        if (child is not null)
        {
            var documents = await _documentService.GetByIdsAsync(child.DocumentIds);
            if (SelectedChild != child)
                return;
            foreach (var document in documents)
                ChildDocuments.Add(document);
        }
        OnPropertyChanged(nameof(ChildDocumentsEmptyVisibility));
        OnPropertyChanged(nameof(ChildDocumentsEmptyMessage));
    }

    private async Task RequestDocumentAsync(DocumentOwnerType ownerType, Guid ownerId)
    {
        if (DocumentAddRequested is not null)
        {
            foreach (Func<DocumentOwnerType, Guid, Task> handler in DocumentAddRequested.GetInvocationList())
                await handler(ownerType, ownerId);
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
