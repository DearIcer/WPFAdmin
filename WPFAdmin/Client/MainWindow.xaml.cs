using System.Windows;
using Client.Models;
using Client.ViewModels;
using Client.Services;

namespace Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // 确保DataContext在登录时正确设置
        // 注意：这里只是默认构造，实际使用时应该通过登录传递正确的参数
        if (DataContext == null)
        {
            var permissionService = new PermissionService();
            DataContext = new MainViewModel(1, permissionService);
        }
    }

    private void MenuTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MainViewModel viewModel) viewModel.SelectedItem = e.NewValue as MenuItem;
    }
}