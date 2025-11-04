using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Client.Models;
using Client.Views;

namespace Client.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly Dictionary<string, Func<object>> _viewFactory;
    private object? _currentView;
    private ObservableCollection<MenuItem> _menuItems;
    private MenuItem? _selectedItem;

    public MainViewModel()
    {
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

    private void LoadMenuData()
    {
        try
        {
            var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "menu.json");
            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                var items = JsonSerializer.Deserialize<ObservableCollection<MenuItem>>(json);
                if (items != null) MenuItems = items;
            }
            else
            {
                // 如果文件不存在，则使用默认数据
                CreateDefaultMenuData();
            }
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
        MenuItems = new ObservableCollection<MenuItem>
        {
            new()
            {
                Id = "dashboard",
                Name = "仪表盘",
                Icon = "ViewDashboard",
                Children = new ObservableCollection<MenuItem>()
            },
            new()
            {
                Id = "products",
                Name = "商品管理",
                Icon = "PackageVariant",
                Children = new ObservableCollection<MenuItem>
                {
                    new() { Id = "product_list", Name = "商品列表", Icon = "FormatListBulleted" },
                    new() { Id = "product_category", Name = "商品分类", Icon = "Folder" }
                }
            },
            new()
            {
                Id = "orders",
                Name = "订单管理",
                Icon = "ClipboardList",
                Children = new ObservableCollection<MenuItem>
                {
                    new() { Id = "order_list", Name = "订单列表", Icon = "FormatListBulleted" },
                    new() { Id = "order_returns", Name = "退货申请", Icon = "PackageDown" }
                }
            },
            new()
            {
                Id = "members",
                Name = "会员管理",
                Icon = "AccountMultiple",
                Children = new ObservableCollection<MenuItem>
                {
                    new() { Id = "member_list", Name = "会员列表", Icon = "FormatListBulleted" },
                    new() { Id = "member_levels", Name = "会员等级", Icon = "Star" }
                }
            },
            new()
            {
                Id = "settings",
                Name = "系统设置",
                Icon = "Cog",
                Children = new ObservableCollection<MenuItem>
                {
                    new() { Id = "user_management", Name = "用户管理", Icon = "Account" },
                    new() { Id = "role_management", Name = "角色管理", Icon = "AccountArrowRight" }
                }
            }
        };
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