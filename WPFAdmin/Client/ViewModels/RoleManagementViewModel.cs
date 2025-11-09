using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Client.Services;

namespace Client.ViewModels;

public class RoleManagementViewModel : INotifyPropertyChanged
{
    private readonly RBACService.RBACServiceClient _rbacClient;
    private ObservableCollection<Role> _roles = new();
    private Role? _selectedRole;
    private bool _isDialogOpen;
    private string _dialogTitle = string.Empty;
    private string _roleName = string.Empty;
    private string _roleDescription = string.Empty;
    private Role? _editingRole;
    private ObservableCollection<MenuItem> _menuItems = new();
    private Dictionary<string, int> _menuCodeToIdMap = new();
    private bool _isAssignMenusDialogOpen;
    private string _assignMenusDialogTitle = string.Empty;

    public RoleManagementViewModel()
    {
        _rbacClient = GrpcClientService.Instance.RbacClient;

        // 初始化命令
        AddRoleCommand = new RelayCommand(AddRole);
        EditRoleCommand = new RelayCommand<Role?>(EditRole);
        DeleteRoleCommand = new RelayCommand<Role?>(DeleteRole);
        SaveRoleCommand = new RelayCommand(SaveRole);
        CloseDialogCommand = new RelayCommand(CloseDialog);
        AssignMenusCommand = new RelayCommand<Role?>(AssignMenus);
        SaveAssignMenusCommand = new RelayCommand(SaveAssignMenus);
        CloseAssignMenusDialogCommand = new RelayCommand(CloseAssignMenusDialog);

        // 加载数据
        _ = LoadRolesAsync();
        _ = LoadMenuItemsAsync();
    }

    public ObservableCollection<Role> Roles
    {
        get => _roles;
        set
        {
            _roles = value;
            OnPropertyChanged();
        }
    }

    public Role? SelectedRole
    {
        get => _selectedRole;
        set
        {
            _selectedRole = value;
            OnPropertyChanged();
        }
    }

    public bool IsDialogOpen
    {
        get => _isDialogOpen;
        set
        {
            _isDialogOpen = value;
            OnPropertyChanged();
        }
    }

    public string DialogTitle
    {
        get => _dialogTitle;
        set
        {
            _dialogTitle = value;
            OnPropertyChanged();
        }
    }

    public string RoleName
    {
        get => _roleName;
        set
        {
            _roleName = value;
            OnPropertyChanged();
        }
    }

    public string RoleDescription
    {
        get => _roleDescription;
        set
        {
            _roleDescription = value;
            OnPropertyChanged();
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

    public bool IsAssignMenusDialogOpen
    {
        get => _isAssignMenusDialogOpen;
        set
        {
            _isAssignMenusDialogOpen = value;
            OnPropertyChanged();
        }
    }

    public string AssignMenusDialogTitle
    {
        get => _assignMenusDialogTitle;
        set
        {
            _assignMenusDialogTitle = value;
            OnPropertyChanged();
        }
    }

    // Commands
    public ICommand AddRoleCommand { get; }
    public ICommand EditRoleCommand { get; }
    public ICommand DeleteRoleCommand { get; }
    public ICommand SaveRoleCommand { get; }
    public ICommand CloseDialogCommand { get; }
    public ICommand AssignMenusCommand { get; }
    public ICommand SaveAssignMenusCommand { get; }
    public ICommand CloseAssignMenusDialogCommand { get; }

    private async Task LoadRolesAsync()
    {
        try
        {
            var request = new GetAllRolesRequest();
            var response = await _rbacClient.GetAllRolesAsync(request);

            if (response.Success)
            {
                Roles = new ObservableCollection<Role>(response.Roles);
            }
            else
            {
                MessageBox.Show($"加载角色失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadMenuItemsAsync()
    {
        try
        {
            // 加载菜单树
            var menuRequest = new GetMenuTreeRequest();
            var menuResponse = await _rbacClient.GetMenuTreeAsync(menuRequest);

            // 加载所有菜单以构建Code到ID的映射
            var allMenusRequest = new GetAllMenusRequest();
            var allMenusResponse = await _rbacClient.GetAllMenusAsync(allMenusRequest);

            if (menuResponse.Success && allMenusResponse.Success)
            {
                var menuItems = MapGrpcMenusToClient(menuResponse.Menus);
                MenuItems = new ObservableCollection<MenuItem>(menuItems);
                
                // 构建菜单Code到ID的映射
                _menuCodeToIdMap = allMenusResponse.Menus.ToDictionary(m => m.Code, m => m.Id);
            }
            else
            {
                var errorMessage = menuResponse.Success ? allMenusResponse.Message : menuResponse.Message;
                MessageBox.Show($"加载菜单失败: {errorMessage}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载菜单时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private List<MenuItem> MapGrpcMenusToClient(Google.Protobuf.Collections.RepeatedField<Menu> grpcMenus, MenuItem? parent = null)
    {
        var menuItems = new List<MenuItem>();

        foreach (var grpcMenu in grpcMenus)
        {
            var menuItem = new MenuItem
            {
                Id = grpcMenu.Code,
                Code = grpcMenu.Code,
                Name = grpcMenu.Name,
                Icon = grpcMenu.Icon,
                Parent = parent
            };

            if (grpcMenu.Children.Count > 0)
            {
                var children = MapGrpcMenusToClient(grpcMenu.Children, menuItem);
                foreach (var child in children)
                {
                    menuItem.Children.Add(child);
                }
            }

            menuItems.Add(menuItem);
        }

        return menuItems;
    }

    private void AddRole()
    {
        _editingRole = null;
        DialogTitle = "添加角色";
        RoleName = string.Empty;
        RoleDescription = string.Empty;
        IsDialogOpen = true;
    }

    private void EditRole(Role? role)
    {
        if (role == null) return;
        
        _editingRole = role;
        DialogTitle = "编辑角色";
        RoleName = role.Name;
        RoleDescription = role.Description;
        IsDialogOpen = true;
    }

    private async void DeleteRole(Role? role)
    {
        if (role == null) return;
        
        var result = MessageBox.Show($"确定要删除角色 '{role.Name}' 吗？", "确认删除", 
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            try
            {
                var request = new DeleteRoleRequest { Id = role.Id };
                var response = await _rbacClient.DeleteRoleAsync(request);

                if (response.Success)
                {
                    Roles.Remove(role);
                    MessageBox.Show("角色删除成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"删除角色失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"删除角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    private async void SaveRole()
    {
        if (string.IsNullOrWhiteSpace(RoleName))
        {
            MessageBox.Show("请填写角色名称。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            if (_editingRole == null)
            {
                // 添加新角色
                var request = new CreateRoleRequest
                {
                    Name = RoleName,
                    Description = RoleDescription
                };

                var response = await _rbacClient.CreateRoleAsync(request);

                if (response.Success)
                {
                    Roles.Add(response.Role);
                    MessageBox.Show("角色创建成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"创建角色失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // 更新现有角色
                var request = new UpdateRoleRequest
                {
                    Role = new Role
                    {
                        Id = _editingRole.Id,
                        Name = RoleName,
                        Description = RoleDescription
                    }
                };

                var response = await _rbacClient.UpdateRoleAsync(request);

                if (response.Success)
                {
                    _editingRole.Name = RoleName;
                    _editingRole.Description = RoleDescription;
                    MessageBox.Show("角色更新成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show($"更新角色失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            CloseDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CloseDialog()
    {
        IsDialogOpen = false;
    }

    private void AssignMenus(Role? role)
    {
        if (role == null) return;
        
        SelectedRole = role;
        AssignMenusDialogTitle = "分配菜单";
        
        // 重置所有菜单项为未分配状态
        ResetMenuItemsAssignment(MenuItems);
        
        // 加载该角色已分配的菜单
        _ = LoadRoleMenusAsync(role.Id);
        
        IsAssignMenusDialogOpen = true;
    }

    private async void SaveAssignMenus()
    {
        if (SelectedRole == null)
        {
            MessageBox.Show("请先选择一个角色。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            // 先获取角色当前的菜单
            var getRequest = new GetRoleMenusRequest { RoleId = SelectedRole.Id };
            var getResponse = await _rbacClient.GetRoleMenusAsync(getRequest);

            if (!getResponse.Success)
            {
                MessageBox.Show($"获取角色当前菜单失败: {getResponse.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var currentMenuCodes = getResponse.Menus.Select(m => m.Code).ToHashSet();

            // 处理需要添加的菜单
            var menusToAssign = new List<MenuItem>();
            CollectAssignedMenuItems(MenuItems, menusToAssign);
            
            // 分配新选择的菜单
            foreach (var menuItem in menusToAssign.Where(m => !currentMenuCodes.Contains(m.Code)))
            {
                if (_menuCodeToIdMap.TryGetValue(menuItem.Code, out int menuId))
                {
                    var request = new AssignMenuToRoleRequest
                    {
                        RoleId = SelectedRole.Id,
                        MenuId = menuId
                    };

                    var response = await _rbacClient.AssignMenuToRoleAsync(request);
                    if (!response.Success)
                    {
                        MessageBox.Show($"分配菜单 '{menuItem.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"无法找到菜单 '{menuItem.Name}' 的ID", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // 处理需要移除的菜单
            var menuCodesToAssign = menusToAssign.Select(m => m.Code).ToHashSet();
            var menusToRemove = getResponse.Menus.Where(m => !menuCodesToAssign.Contains(m.Code)).ToList();
            foreach (var menu in menusToRemove)
            {
                if (_menuCodeToIdMap.TryGetValue(menu.Code, out int menuId))
                {
                    var request = new RemoveMenuFromRoleRequest
                    {
                        RoleId = SelectedRole.Id,
                        MenuId = menuId
                    };

                    var response = await _rbacClient.RemoveMenuFromRoleAsync(request);
                    if (!response.Success)
                    {
                        MessageBox.Show($"移除菜单 '{menu.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"无法找到菜单 '{menu.Name}' 的ID", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageBox.Show("菜单分配保存成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            CloseAssignMenusDialog();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存菜单分配时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CloseAssignMenusDialog()
    {
        IsAssignMenusDialogOpen = false;
    }

    private void CollectAssignedMenuItems(ObservableCollection<MenuItem> menuItems, List<MenuItem> assignedItems)
    {
        foreach (var menuItem in menuItems)
        {
            if (menuItem.IsAssigned)
            {
                assignedItems.Add(menuItem);
            }
            CollectAssignedMenuItems(menuItem.Children, assignedItems);
        }
    }

    private async Task LoadRoleMenusAsync(int roleId)
    {
        try
        {
            // 重置所有菜单项为未分配状态
            ResetMenuItemsAssignment(MenuItems);

            // 获取角色的菜单
            var request = new GetRoleMenusRequest { RoleId = roleId };
            var response = await _rbacClient.GetRoleMenusAsync(request);

            if (response.Success)
            {
                // 标记已分配的菜单
                var roleMenuCodes = response.Menus.Select(m => m.Code).ToHashSet();
                MarkMenuItemsAsAssigned(MenuItems, roleMenuCodes);
            }
            else
            {
                MessageBox.Show($"加载角色菜单失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载角色菜单时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ResetMenuItemsAssignment(ObservableCollection<MenuItem> menuItems)
    {
        foreach (var menuItem in menuItems)
        {
            menuItem.IsAssigned = false;
            ResetMenuItemsAssignment(menuItem.Children);
        }
    }

    private void MarkMenuItemsAsAssigned(ObservableCollection<MenuItem> menuItems, HashSet<string> roleMenuCodes)
    {
        foreach (var menuItem in menuItems)
        {
            menuItem.IsAssigned = roleMenuCodes.Contains(menuItem.Code);
            MarkMenuItemsAsAssigned(menuItem.Children, roleMenuCodes);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}