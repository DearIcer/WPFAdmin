using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;
using Client.Services;
using Grpc.Net.Client;
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

        // 简单验证
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            ErrorTextBlock.Text = "请输入用户名和密码";
            return;
        }

        try
        {
            // 使用 gRPC 进行身份验证
            var channel = GrpcChannel.ForAddress("http://localhost:5101");
            var client = new RBACService.RBACServiceClient(channel);

            var request = new AuthenticateUserRequest
            {
                Username = username,
                Password = password
            };

            var response = await client.AuthenticateUserAsync(request);

            if (response.Success)
            {
                // 登录成功，加载用户权限并显示主窗口
                var permissionService = new PermissionService();
                await permissionService.LoadUserPermissionsAsync(response.User.Id);

                var mainWindow = new MainWindow();
                mainWindow.DataContext = new MainViewModel(response.User.Id, permissionService);
                mainWindow.Show();

                // 关闭当前登录窗口
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