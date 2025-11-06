using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views;

/// <summary>
/// UserManagementView.xaml 的交互逻辑
/// </summary>
public partial class UserManagementView : UserControl
{
    public UserManagementView()
    {
        InitializeComponent();
        DataContext = new UserManagementViewModel();
    }

    private void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (DataContext is UserManagementViewModel viewModel)
        {
            viewModel.NewPassword = NewPasswordBox.Password;
        }
    }
}