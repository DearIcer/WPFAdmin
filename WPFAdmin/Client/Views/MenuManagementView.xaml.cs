using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views;

/// <summary>
/// MenuManagementView.xaml 的交互逻辑
/// </summary>
public partial class MenuManagementView : UserControl
{
    public MenuManagementView()
    {
        InitializeComponent();
        DataContext = new MenuManagementViewModel();
    }

    private void MenuTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MenuManagementViewModel viewModel)
        {
            viewModel.SelectedItem = e.NewValue as Models.MenuItem;
            
            // 更新编辑区域的值
            if (viewModel.SelectedItem != null)
            {
                viewModel.NewItemId = viewModel.SelectedItem.Id;
                viewModel.NewItemName = viewModel.SelectedItem.Name;
                viewModel.NewItemIcon = viewModel.SelectedItem.Icon;
            }
        }
    }
}