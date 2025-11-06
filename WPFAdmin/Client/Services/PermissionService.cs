using System.Text.Json;
using Client.Models;
using Grpc.Net.Client;
using Backed.Grpc;

namespace Client.Services;

public class PermissionService
{
    // 模拟当前用户的权限列表
    // 在实际应用中，这些权限应该从后端API获取
    private HashSet<string> _userPermissions;
    private readonly RBACService.RBACServiceClient? _rbacClient;

    public PermissionService()
    {
        // 默认给管理员所有权限
        _userPermissions = new HashSet<string>
        {
            "ViewDashboard",
            "ManageProducts",
            "ManageOrders",
            "ManageMembers",
            "ManageUsers",
            "ManageRoles",
            "ManageMenus"
        };
        
        // 初始化gRPC客户端
        try
        {
            var channel = GrpcChannel.ForAddress("http://localhost:5101");
            _rbacClient = new RBACService.RBACServiceClient(channel);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"初始化gRPC客户端失败: {ex.Message}");
        }
    }

    // 从后端获取用户权限
    public async Task LoadUserPermissionsAsync(int userId)
    {
        try
        {
            if (_rbacClient == null)
            {
                Console.WriteLine("gRPC客户端未初始化");
                return;
            }

            // 获取用户权限
            var request = new GetUserPermissionsRequest { UserId = userId };
            var response = await _rbacClient.GetUserPermissionsAsync(request);
            
            if (response.Success)
            {
                _userPermissions = new HashSet<string>();
                foreach (var permission in response.Permissions)
                {
                    _userPermissions.Add(permission.Name);
                }
            }
            else
            {
                Console.WriteLine($"获取用户权限失败: {response.Message}");
            }
        }
        catch (Exception ex)
        {
            // 如果获取权限失败，使用默认权限
            Console.WriteLine($"获取用户权限失败: {ex.Message}");
        }
    }

    // 根据用户权限设置创建菜单
    public List<MenuItem> GetMenuItemsByPermissions()
    {
        var allMenuItems = LoadAllMenuItems();
        var filteredMenuItems = new List<MenuItem>();

        foreach (var menuItem in allMenuItems)
        {
            var filteredItem = FilterMenuItemByPermissions(menuItem);
            if (filteredItem != null)
            {
                filteredMenuItems.Add(filteredItem);
            }
        }

        return filteredMenuItems;
    }

    // 递归过滤菜单项
    private MenuItem? FilterMenuItemByPermissions(MenuItem item)
    {
        // 检查是否有权限访问当前菜单项
        // 菜单ID与权限名称对应关系：
        // dashboard -> ViewDashboard
        // products, product_list, product_category -> ManageProducts
        // orders, order_list, order_returns -> ManageOrders
        // members, member_list, member_levels -> ManageMembers
        // user_management -> ManageUsers
        // role_management -> ManageRoles
        // menu_management -> ManageMenus

        var hasPermission = item.Code switch
        {
            "dashboard" => _userPermissions.Contains("ViewDashboard"),
            "products" or "product_list" or "product_category" => _userPermissions.Contains("ManageProducts"),
            "orders" or "order_list" or "order_returns" => _userPermissions.Contains("ManageOrders"),
            "members" or "member_list" or "member_levels" => _userPermissions.Contains("ManageMembers"),
            "user_management" => _userPermissions.Contains("ManageUsers"),
            "role_management" => _userPermissions.Contains("ManageRoles"),
            "menu_management" => _userPermissions.Contains("ManageMenus"),
            _ => false
        };

        // 如果当前项没有权限且没有子项，则不显示
        if (!hasPermission && (item.Children == null || item.Children.Count == 0))
        {
            return null;
        }

        // 创建新的菜单项
        var filteredItem = new MenuItem
        {
            Id = item.Id,
            Code = item.Code,
            Name = item.Name,
            Icon = item.Icon,
            IsExpanded = item.IsExpanded,
            IsSelected = item.IsSelected,
            Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>()
        };

        // 如果有子项，递归过滤子项
        if (item.Children != null && item.Children.Count > 0)
        {
            foreach (var child in item.Children)
            {
                var filteredChild = FilterMenuItemByPermissions(child);
                if (filteredChild != null)
                {
                    filteredItem.Children.Add(filteredChild);
                }
            }

            // 如果当前项没有权限但有可见的子项，则显示该项
            if (!hasPermission && filteredItem.Children.Count == 0)
            {
                return null;
            }
        }

        return filteredItem;
    }

    // 加载所有菜单项
    public List<MenuItem> LoadAllMenuItems()
    {
        return new List<MenuItem>
        {
            new()
            {
                Id = "dashboard",
                Code = "dashboard",
                Name = "仪表盘",
                Icon = "ViewDashboard",
                Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>()
            },
            new()
            {
                Id = "products",
                Code = "products",
                Name = "商品管理",
                Icon = "PackageVariant",
                Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>
                {
                    new() { Id = "product_list", Code = "product_list", Name = "商品列表", Icon = "FormatListBulleted" },
                    new() { Id = "product_category", Code = "product_category", Name = "商品分类", Icon = "Folder" }
                }
            },
            new()
            {
                Id = "orders",
                Code = "orders",
                Name = "订单管理",
                Icon = "ClipboardList",
                Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>
                {
                    new() { Id = "order_list", Code = "order_list", Name = "订单列表", Icon = "FormatListBulleted" },
                    new() { Id = "order_returns", Code = "order_returns", Name = "退货申请", Icon = "PackageDown" }
                }
            },
            new()
            {
                Id = "members",
                Code = "members",
                Name = "会员管理",
                Icon = "AccountMultiple",
                Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>
                {
                    new() { Id = "member_list", Code = "member_list", Name = "会员列表", Icon = "FormatListBulleted" },
                    new() { Id = "member_levels", Code = "member_levels", Name = "会员等级", Icon = "Star" }
                }
            },
            new()
            {
                Id = "settings",
                Code = "settings",
                Name = "系统设置",
                Icon = "Cog",
                Children = new System.Collections.ObjectModel.ObservableCollection<MenuItem>
                {
                    new() { Id = "user_management", Code = "user_management", Name = "用户管理", Icon = "Account" },
                    new() { Id = "role_management", Code = "role_management", Name = "角色管理", Icon = "AccountArrowRight" },
                    new() { Id = "menu_management", Code = "menu_management", Name = "菜单管理", Icon = "Menu" }
                }
            }
        };
    }
    
}