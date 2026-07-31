using System.Windows;
using System.Windows.Controls;

namespace KidsOrganizationApp.UI.View;

public partial class AddFamilyView : UserControl
{
    public AddFamilyView()
    {
        InitializeComponent();
    }

    private void OnFilesDropped(object sender, DragEventArgs e)
    {
        if (DataContext is AddFamilyViewModel viewModel &&
            e.Data.GetDataPresent(DataFormats.FileDrop) &&
            e.Data.GetData(DataFormats.FileDrop) is string[] files)
        {
            viewModel.AcceptDroppedFiles(files);
            e.Handled = true;
        }
    }
}
