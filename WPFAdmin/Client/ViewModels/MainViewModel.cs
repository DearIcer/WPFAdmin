using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Client.Services;
using Client.Views;

namespace Client.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly RBACService.RBACServiceClient _rbacClient;
    private User? _currentUser;
    private ObservableCollection<MenuItem> _menuItems;
    private MenuItem? _selectedItem;
    private object? _currentView;
    
    public MainViewModel()
    {
        // 初始化gRPC客户端
        _rbacClient = GrpcClientService.Instance.RbacClient;
        
        // 初始化菜单项
        _menuItems = new ObservableCollection<MenuItem>();
        
        // 初始化命令
        LogoutCommand = new RelayCommand(Logout);
        NavigateCommand = new RelayCommand<MenuItem>(Navigate);
        
        // 默认显示仪表盘
        CurrentView = new DashboardView();
    }

    public User? CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            OnPropertyChanged();
            if (_currentUser != null)
            {
                _ = LoadUserMenusAsync(_currentUser.Id);
            }
        }
    }

    public ObservableCollection<MenuItem> MenuItems
    {
        get => _menuItems;
        set
        {
            _menuItems = value;
            OnPropertyChanged();
        }
    }

    public MenuItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            Navigate(_selectedItem);
        }
    }

    public object? CurrentView
    {
        get => _currentView;
        set
        {
            _currentView = value;
            OnPropertyChanged();
        }
    }

    public ICommand LogoutCommand { get; }
    public ICommand NavigateCommand { get; }

    private async Task LoadUserMenusAsync(int userId)
    {
        try
        {
            var request = new GetUserMenuTreeRequest { UserId = userId };
            var response = await _rbacClient.GetUserMenuTreeAsync(request);

            if (response.Success)
            {
                var menuItems = MapGrpcMenusToClient(response.Menus);
                MenuItems = new ObservableCollection<MenuItem>(menuItems);
            }
            else
            {
                MessageBox.Show($"加载用户菜单失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载用户菜单时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private List<MenuItem> MapGrpcMenusToClient(Google.Protobuf.Collections.RepeatedField<Menu> grpcMenus)
    {
        var menuItems = new List<MenuItem>();

        foreach (var grpcMenu in grpcMenus)
        {
            var menuItem = new MenuItem
            {
                Id = grpcMenu.Code,
                Code = grpcMenu.Code,
                Name = grpcMenu.Name,
                Icon = grpcMenu.Icon
            };

            if (grpcMenu.Children.Count > 0)
            {
                var children = MapGrpcMenusToClient(grpcMenu.Children);
                foreach (var child in children)
                {
                    menuItem.Children.Add(child);
                }
            }

            menuItems.Add(menuItem);
        }

        return menuItems;
    }

    private void Logout()
    {
        // 触发登出事件
        LoggedOut?.Invoke(this, EventArgs.Empty);
    }

    private void Navigate(MenuItem? menuItem)
    {
        if (menuItem == null) return;

        // 根据菜单项导航到相应的视图
        CurrentView = menuItem.Code switch
        {
            "dashboard" => new DashboardView(),
            "product_list" => new ProductsView(),  
            "user_management" => new UserManagementView(),
            "role_management" => new RoleManagementView(),
            "menu_management" => new MenuManagementView(),
            _ => CurrentView  // 对于父级菜单或其他未指定的菜单项，保持当前视图不变
        };
    }

    public event EventHandler? LoggedOut;
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}