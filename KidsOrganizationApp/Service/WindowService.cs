using KidsOrganizationApp.UI.View;
using KidsOrganizationApp.UI.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KidsOrganizationApp.Service
{
    public class WindowService
    {
        private readonly ParentChildView _parentChildView;
        private readonly MainViewModel _mainViewModel;

        public WindowService(ParentChildView parentChildView, MainViewModel mainViewModel) 
        {
            _parentChildView = parentChildView;
            _mainViewModel = mainViewModel;
        }

        public void ShowParentChildWindow()
        {

            _parentChildView.SetViewModel(_mainViewModel);

            _parentChildView.Show();
        }
    }
}
