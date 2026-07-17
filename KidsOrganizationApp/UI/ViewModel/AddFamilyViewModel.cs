using CommunityToolkit.Mvvm.Input;
using KidsOrganizationApp.Service;
using KidsOrganizationApp.Service.DTO;
using System.Windows;
using System.Windows.Input;

public class AddFamilyViewModel : BaseViewModel
{
    private readonly IChildService _childService;
    private readonly IParentService _parentService;
    private ChildDTO? _childToAttachParent;

    public event Action? Saved;
    public event Action? Cancelled;

    public string Title => _childToAttachParent is null
        ? "Добавление ребенка и родителя"
        : $"Добавление родителя для: {_childToAttachParent.Surname} {_childToAttachParent.Name}";
    public bool IsAddingParentOnly => _childToAttachParent is not null;
    public Visibility ChildFieldsVisibility => IsAddingParentOnly ? Visibility.Collapsed : Visibility.Visible;

    private string _childName = string.Empty;
    public string ChildName { get => _childName; set { _childName = value; OnPropertyChanged(); } }
    private string _childSurname = string.Empty;
    public string ChildSurname { get => _childSurname; set { _childSurname = value; OnPropertyChanged(); } }
    private string _childPatronymic = string.Empty;
    public string ChildPatronymic { get => _childPatronymic; set { _childPatronymic = value; OnPropertyChanged(); } }
    private string _childPhone = string.Empty;
    public string ChildPhone { get => _childPhone; set { _childPhone = value; OnPropertyChanged(); } }
    private string _childLivingPlace = string.Empty;
    public string ChildLivingPlace { get => _childLivingPlace; set { _childLivingPlace = value; OnPropertyChanged(); } }
    private DateTime? _childBirthDate;
    public DateTime? ChildBirthDate { get => _childBirthDate; set { _childBirthDate = value; OnPropertyChanged(); } }

    private string _parentName = string.Empty;
    public string ParentName { get => _parentName; set { _parentName = value; OnPropertyChanged(); } }
    private string _parentSurname = string.Empty;
    public string ParentSurname { get => _parentSurname; set { _parentSurname = value; OnPropertyChanged(); } }
    private string _parentPatronymic = string.Empty;
    public string ParentPatronymic { get => _parentPatronymic; set { _parentPatronymic = value; OnPropertyChanged(); } }
    private string _parentPhone = string.Empty;
    public string ParentPhone { get => _parentPhone; set { _parentPhone = value; OnPropertyChanged(); } }
    private string _parentLivingPlace = string.Empty;
    public string ParentLivingPlace { get => _parentLivingPlace; set { _parentLivingPlace = value; OnPropertyChanged(); } }
    private DateTime? _parentBirthDate;
    public DateTime? ParentBirthDate { get => _parentBirthDate; set { _parentBirthDate = value; OnPropertyChanged(); } }

    private string _statusMessage = string.Empty;
    public string StatusMessage { get => _statusMessage; private set { _statusMessage = value; OnPropertyChanged(); } }

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    public AddFamilyViewModel(IChildService childService, IParentService parentService)
    {
        _childService = childService;
        _parentService = parentService;
        SaveCommand = new RelayCommand(Save);
        CancelCommand = new RelayCommand(() => Cancelled?.Invoke());
    }

    public void PrepareForNewChild()
    {
        _childToAttachParent = null;
        ClearForm();
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(IsAddingParentOnly));
        OnPropertyChanged(nameof(ChildFieldsVisibility));
    }

    public void PrepareForParent(ChildDTO child)
    {
        _childToAttachParent = child;
        ClearForm();
        OnPropertyChanged(nameof(Title));
        OnPropertyChanged(nameof(IsAddingParentOnly));
        OnPropertyChanged(nameof(ChildFieldsVisibility));
    }

    private void Save()
    {
        try
        {
            if (_childToAttachParent is null)
            {
                var savedParent = _parentService.Add(CreateParent());
                var child = new ChildDTO(ChildName, ChildSurname, ChildPatronymic, ChildPhone,
                    ChildLivingPlace, RequireDate(ChildBirthDate, "ребенка"), new List<Guid> { savedParent.Id });
                _childService.AddChild(child);
                StatusMessage = "Ребенок и родитель сохранены.";
            }
            else
            {
                _childService.AddParent(CreateParent(), _childToAttachParent);
                StatusMessage = "Родитель добавлен.";
            }

            Saved?.Invoke();
        }
        catch (Exception ex)
        {
            StatusMessage = ex.Message;
        }
    }

    private ParentDTO CreateParent() => new(ParentName, ParentSurname, ParentPatronymic,
        ParentPhone, ParentLivingPlace, RequireDate(ParentBirthDate, "родителя"));

    private static DateTime RequireDate(DateTime? date, string person) =>
        date ?? throw new ArgumentException($"Укажите дату рождения {person}.");

    private void ClearForm()
    {
        ChildName = ChildSurname = ChildPatronymic = ChildPhone = ChildLivingPlace = string.Empty;
        ChildBirthDate = null;
        ParentName = ParentSurname = ParentPatronymic = ParentPhone = ParentLivingPlace = string.Empty;
        ParentBirthDate = null;
        StatusMessage = string.Empty;
    }
}
