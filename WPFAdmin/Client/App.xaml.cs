using System.Windows;
using Client.Views;

namespace Client;

/// <summary>
///     Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 显示登录窗口
        var loginWindow = new Window
        {
            Title = "管理员登录",
            Content = new LoginView(),
            Width = 1000,
            Height = 600,
            WindowStartupLocation = WindowStartupLocation.CenterScreen
        };

        loginWindow.Show();
    }
}