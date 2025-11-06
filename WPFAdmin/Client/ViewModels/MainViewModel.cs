using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Client.Models;
using Client.Services;
using Client.Views;

namespace Client.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Dictionary<string, Func<object>> _viewFactory;
    private readonly PermissionService _permissionService;
    private object? _currentView;
    private ObservableCollection<MenuItem> _menuItems;
    private MenuItem? _selectedItem;
    private int _currentUserId;

    public MainViewModel(int userId, PermissionService permissionService) // 从登录信息中获取用户ID和权限服务
    {
        // 初始化权限服务
        _permissionService = permissionService;
        _currentUserId = userId;
        
        // 初始化视图工厂
        _viewFactory = new Dictionary<string, Func<object>>(StringComparer.OrdinalIgnoreCase)
        {
            { "dashboard", () => new DashboardView() },
            { "products", () => new ProductsView() }
            // 可以在这里添加更多视图
            // { "orders", () => new OrdersView() }
        };

        MenuItems = new ObservableCollection<MenuItem>();
        LoadMenuData();
        // 默认显示仪表盘
        CurrentView = new DashboardView();
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
            if (_selectedItem != null) NavigateToView(_selectedItem.Id);
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

    public event PropertyChangedEventHandler? PropertyChanged;

    private async void LoadMenuData()
    {
        try
        {
            // 从后端加载用户权限
            await _permissionService.LoadUserPermissionsAsync(_currentUserId);
            
            // 使用权限服务获取基于权限的菜单数据
            var menuItems = _permissionService.GetMenuItemsByPermissions();
            MenuItems = new ObservableCollection<MenuItem>(menuItems);
        }
        catch (Exception ex)
        {
            // 异常处理：输出日志并加载默认数据
            Debug.WriteLine($"加载菜单数据时出错: {ex.Message}");
            CreateDefaultMenuData();
        }
    }

    private void CreateDefaultMenuData()
    {
        // 使用权限服务获取菜单数据
        var menuItems = _permissionService.GetMenuItemsByPermissions();
        MenuItems = new ObservableCollection<MenuItem>(menuItems);
    }

    private void NavigateToView(string viewId)
    {
        if (_viewFactory.TryGetValue(viewId, out var viewFactory))
            CurrentView = viewFactory();
        else
            // 默认显示仪表盘
            CurrentView = new DashboardView();
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}