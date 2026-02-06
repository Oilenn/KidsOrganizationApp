using KidsOrganizationApp.UI.View;
using KidsOrganizationApp.UI.ViewModels;

namespace KidsOrganizationApp.Service
{
    /// <summary>
    /// Сервис для контроля за окнами и их состоянием
    /// </summary>
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
