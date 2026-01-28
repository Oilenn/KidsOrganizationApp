using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.UI.ViewModels
{
    using KidsOrganizationApp.Domain;
    using System.Collections.ObjectModel;

    public class MainViewModel
    {
        public ObservableCollection<ParentViewModel> Parents { get; }
        public ObservableCollection<ChildViewModel> Children { get; }

        public MainViewModel()
        {
            Parents = new ObservableCollection<ParentViewModel>();
            Children = new ObservableCollection<ChildViewModel>();

            LoadTestData(); // пока тест
        }

        private void LoadTestData()
        {
            var parent = new Parent("Иван", "Иванов", "Иванович", new DateTime(1980, 1, 1));
            var child = new Child("Петя", "Иванов", "Иванович", new DateTime(2010, 5, 12));

            parent.Children.Add(child);
            child.Parents.Add(parent);

            Parents.Add(new ParentViewModel(parent));
            Children.Add(new ChildViewModel(child));
        }
    }

}
