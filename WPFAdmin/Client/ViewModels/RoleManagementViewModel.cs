using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Grpc.Net.Client;

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
    private ObservableCollection<PermissionItem> _permissions = new();
    private bool _isPermissionsLoaded = false;

    public RoleManagementViewModel()
    {
        // 初始化gRPC客户端
        var channel = GrpcChannel.ForAddress("http://localhost:5101");
        _rbacClient = new RBACService.RBACServiceClient(channel);

        // 初始化命令
        AddRoleCommand = new RelayCommand(AddRole);
        EditRoleCommand = new RelayCommand<Role?>(EditRole);
        DeleteRoleCommand = new RelayCommand<Role?>(DeleteRole);
        SaveRoleCommand = new RelayCommand(SaveRole);
        CloseDialogCommand = new RelayCommand(CloseDialog);
        SavePermissionsCommand = new RelayCommand(SavePermissions);

        // 加载数据
        _ = LoadRolesAsync();
        _ = LoadPermissionsAsync();
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
            if (_selectedRole != null && _isPermissionsLoaded)
            {
                _ = LoadRolePermissionsAsync(_selectedRole.Id);
            }
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

    public ObservableCollection<PermissionItem> Permissions
    {
        get => _permissions;
        set
        {
            _permissions = value;
            OnPropertyChanged();
        }
    }

    // Commands
    public ICommand AddRoleCommand { get; }
    public ICommand EditRoleCommand { get; }
    public ICommand DeleteRoleCommand { get; }
    public ICommand SaveRoleCommand { get; }
    public ICommand CloseDialogCommand { get; }
    public ICommand SavePermissionsCommand { get; }

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

    private async Task LoadPermissionsAsync()
    {
        try
        {
            var request = new GetAllPermissionsRequest();
            var response = await _rbacClient.GetAllPermissionsAsync(request);

            if (response.Success)
            {
                var permissionItems = response.Permissions.Select(p => new PermissionItem
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    IsAssigned = false
                }).ToList();

                Permissions = new ObservableCollection<PermissionItem>(permissionItems);
                _isPermissionsLoaded = true;
            }
            else
            {
                MessageBox.Show($"加载权限失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载权限时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadRolePermissionsAsync(int roleId)
    {
        try
        {
            // 重置所有权限为未分配状态
            foreach (var permission in Permissions)
            {
                permission.IsAssigned = false;
            }

            // 获取角色的权限
            var request = new GetRolePermissionsRequest { RoleId = roleId };
            var response = await _rbacClient.GetRolePermissionsAsync(request);

            if (response.Success)
            {
                // 标记已分配的权限
                var rolePermissionIds = response.Permissions.Select(p => p.Id).ToHashSet();
                foreach (var permission in Permissions)
                {
                    permission.IsAssigned = rolePermissionIds.Contains(permission.Id);
                }
                
                // 触发UI更新
                OnPropertyChanged(nameof(Permissions));
            }
            else
            {
                MessageBox.Show($"加载角色权限失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载角色权限时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

    private async void SavePermissions()
    {
        if (SelectedRole == null)
        {
            MessageBox.Show("请先选择一个角色。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            // 先获取角色当前的权限
            var getRequest = new GetRolePermissionsRequest { RoleId = SelectedRole.Id };
            var getResponse = await _rbacClient.GetRolePermissionsAsync(getRequest);

            if (!getResponse.Success)
            {
                MessageBox.Show($"获取角色当前权限失败: {getResponse.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var currentPermissionIds = getResponse.Permissions.Select(p => p.Id).ToHashSet();

            // 处理需要添加的权限
            var permissionsToAssign = Permissions.Where(p => p.IsAssigned && !currentPermissionIds.Contains(p.Id)).ToList();
            foreach (var permission in permissionsToAssign)
            {
                var request = new AssignPermissionToRoleRequest
                {
                    RoleId = SelectedRole.Id,
                    PermissionId = permission.Id
                };

                var response = await _rbacClient.AssignPermissionToRoleAsync(request);
                if (!response.Success)
                {
                    MessageBox.Show($"分配权限 '{permission.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // 处理需要移除的权限
            var permissionsToRemove = Permissions.Where(p => !p.IsAssigned && currentPermissionIds.Contains(p.Id)).ToList();
            foreach (var permission in permissionsToRemove)
            {
                var request = new RemovePermissionFromRoleRequest
                {
                    RoleId = SelectedRole.Id,
                    PermissionId = permission.Id
                };

                var response = await _rbacClient.RemovePermissionFromRoleAsync(request);
                if (!response.Success)
                {
                    MessageBox.Show($"移除权限 '{permission.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MessageBox.Show("权限配置保存成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存权限时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadRolePermissionsWithDelayAsync(int roleId)
    {
        // 等待一小段时间确保权限列表加载完成
        await Task.Delay(100);
        await LoadRolePermissionsAsync(roleId);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}