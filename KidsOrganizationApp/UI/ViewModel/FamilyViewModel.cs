using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.UI.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class FamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;

    private readonly AddFamilyViewModel _addFamilyViewModel;

    public ObservableCollection<ChildDTO> Children { get; set; } = new();
    public ObservableCollection<ParentDTO> Parents { get; set; } = new();

    private ChildDTO _selectedChild;
    public ChildDTO SelectedChild
    {
        get => _selectedChild;
        set
        {
            _selectedChild = value;
            OnPropertyChanged();
            LoadParents();
        }
    }

    private ParentDTO _selectedParent;
    public ParentDTO SelectedParent
    {
        get => _selectedParent;
        set
        {
            _selectedParent = value;
            OnPropertyChanged();
        }
    }

    public ICommand AddChildCommand { get; }
    public ICommand AddParentCommand { get; }

    public FamilyViewModel(IChildService childService, IParentService parentService, AddFamilyViewModel addFamilyViewModel)
    {
        _addFamilyViewModel = addFamilyViewModel;

        _childService = childService;
        _parentService = parentService;

        AddChildCommand = new RelayCommand(AddChild);
        AddParentCommand = new RelayCommand(AddParent);

        LoadChildren();
    }

    private void LoadChildren()
    {
        Children.Clear();
        foreach (var child in _childService.GetAllChildren())
            Children.Add(child);
    }

    private void LoadParents()
    {
        Parents.Clear();

        if (SelectedChild == null) return;

        var parents = _childService.GetParents(SelectedChild);
        foreach (var parent in parents)
            Parents.Add(parent);
    }

    private void AddChild()
    {
        var window = new AddFamilyView();
        window.DataContext = _addFamilyViewModel;
        window.Show();
    }

    private void AddParent()
    {
        if (SelectedChild == null) return;

        LoadParents();
    }
}