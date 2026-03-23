using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using KidsOrganizationApp.UI.View;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class AddFamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;

    public ICommand AddChildCommand { get; }
    public ICommand AddParentCommand { get; }

    public AddFamilyViewModel(IChildService childService, IParentService parentService)
    {
        _childService = childService;
        _parentService = parentService;

        AddChildCommand = new RelayCommand(AddChild);
        AddParentCommand = new RelayCommand(AddParent);
    }

    public void AddChild()
    {
        
    }

    public void AddParent()
    {

    }
}