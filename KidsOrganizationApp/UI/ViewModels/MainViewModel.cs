using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.UI.ViewModels
{
    using KidsOrganizationApp.Domain;
    using KidsOrganizationApp.Service;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        public ObservableCollection<ParentViewModel> Parents { get; }
        public ObservableCollection<ChildViewModel> Children { get; }

        public IChildService ChildService { get; set; }
        public IParentService ParentService { get; set; }

        public MainViewModel(IChildService childService, IParentService parentService)
        {
            Parents = new ObservableCollection<ParentViewModel>();
            Children = new ObservableCollection<ChildViewModel>();

            ChildService = childService;
            ParentService = parentService;

            LoadTestData(); // пока тест
        }

        private void LoadTestData()
        {
            ChildService.AddChild("Настя", "Копшева", "Ивановна", new DateTime());

            List<Parent> parents = ParentService.GetAllParents();

            var parentViewModels = parents
                .Select(p => new ParentViewModel(p))
                .ToList();

            foreach (var viewModel in parentViewModels)
            {
                Parents.Add(viewModel);
            }

            List<Child> children = ChildService.GetAllChildren();

            var childrentViewModel = children
                .Select(p => new ChildViewModel(p))
                .ToList();

            foreach (var viewModel in childrentViewModel)
            {
                Children.Add(viewModel);
            }
        }
    }

}
