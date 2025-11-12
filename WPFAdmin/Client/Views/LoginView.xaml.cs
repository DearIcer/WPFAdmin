using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;
using Client.Services;
using Backed.Grpc;

namespace Client.Views;

/// <summary>
///     LoginView.xaml 的交互逻辑
/// </summary>
public partial class LoginView : UserControl
{
    public LoginView()
    {
        InitializeComponent();
    }

    private async void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        var username = UsernameTextBox.Text;
        var password = PasswordBox.Password;
        
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorTextBlock.Text = "请输入用户名和密码";
            return;
        }

        try
        {
            var client = GrpcClientService.Instance.RbacClient;

            var request = new AuthenticateUserRequest
            {
                Username = username,
                Password = password
            };

            var response = await client.AuthenticateUserAsync(request);

            if (response.Success)
            {
                // 设置认证token
                GrpcClientService.Instance.SetAuthToken(response.Token);
                
                var mainWindow = new MainWindow();
                var mainViewModel = new MainViewModel();
                mainViewModel.CurrentUser = response.User;
                mainWindow.DataContext = mainViewModel;
                mainWindow.Show();
                Window.GetWindow(this)?.Close();
            }
            else
            {
                ErrorTextBlock.Text = response.Message;
            }
        }
        catch (Exception ex)
        {
            ErrorTextBlock.Text = $"登录失败: {ex.Message}";
        }
    }
}