using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Client.Services;

namespace Client.ViewModels;

public class UserManagementViewModel : INotifyPropertyChanged
{
    private ObservableCollection<User> _users;
    private User? _selectedUser;
    private string _newUsername = string.Empty;
    private string _newEmail = string.Empty;
    private string _newPassword = string.Empty;
    private bool _isEditing;
    private User? _editingUser;
    private ObservableCollection<RoleItem> _availableRoles = new();
    private bool _isRolesLoaded = false;
    
    private readonly RBACService.RBACServiceClient _rbacClient;

    public UserManagementViewModel()
    {
        _users = new ObservableCollection<User>();
        
        _rbacClient = GrpcClientService.Instance.RbacClient;
        
        LoadUsersCommand = new RelayCommand(async () => await LoadUsersAsync());
        AddUserCommand = new RelayCommand(async () => await AddUserAsync());
        EditUserCommand = new RelayCommand(EditUser, CanEditOrDeleteUser);
        DeleteUserCommand = new RelayCommand(async () => await DeleteUserAsync(), CanEditOrDeleteUser);
        SaveUserCommand = new RelayCommand(async () => await SaveUserAsync());
        CancelEditCommand = new RelayCommand(CancelEdit);
        
        // 加载用户数据
        _ = LoadUsersAsync();
        _ = LoadRolesAsync();
    }

    public ObservableCollection<User> Users
    {
        get => _users;
        set
        {
            _users = value;
            OnPropertyChanged();
        }
    }

    public User? SelectedUser
    {
        get => _selectedUser;
        set
        {
            _selectedUser = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsUserSelected));
            (EditUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteUserCommand as RelayCommand)?.RaiseCanExecuteChanged();
            
            // 只有在非编辑模式下才加载用户角色
            if (_selectedUser != null && _isRolesLoaded && !IsEditing)
            {
                _ = LoadUserRolesAsync(_selectedUser.Id);
            }
        }
    }

    public bool IsUserSelected => SelectedUser != null;

    public string NewUsername
    {
        get => _newUsername;
        set
        {
            _newUsername = value;
            OnPropertyChanged();
        }
    }

    public string NewEmail
    {
        get => _newEmail;
        set
        {
            _newEmail = value;
            OnPropertyChanged();
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            _newPassword = value;
            OnPropertyChanged();
        }
    }

    public bool IsEditing
    {
        get => _isEditing;
        set
        {
            _isEditing = value;
            OnPropertyChanged();
        }
    }

    public User? EditingUser
    {
        get => _editingUser;
        set
        {
            _editingUser = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<RoleItem> AvailableRoles
    {
        get => _availableRoles;
        set
        {
            _availableRoles = value;
            OnPropertyChanged();
        }
    }

    public ICommand LoadUsersCommand { get; }
    public ICommand AddUserCommand { get; }
    public ICommand EditUserCommand { get; }
    public ICommand DeleteUserCommand { get; }
    public ICommand SaveUserCommand { get; }
    public ICommand CancelEditCommand { get; }

    private async Task LoadUsersAsync()
    {
        try
        {
            var request = new GetAllUsersRequest();
            var response = await _rbacClient.GetAllUsersAsync(request);
            
            if (response.Success)
            {
                Users = new ObservableCollection<User>(response.Users);
            }
            else
            {
                MessageBox.Show($"加载用户失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载用户时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task LoadRolesAsync()
    {
        try
        {
            var request = new GetAllRolesRequest();
            var response = await _rbacClient.GetAllRolesAsync(request);

            if (response.Success)
            {
                var roleItems = response.Roles.Select(r => new RoleItem
                {
                    Id = r.Id,
                    Name = r.Name,
                    IsAssigned = false
                }).ToList();

                AvailableRoles = new ObservableCollection<RoleItem>(roleItems);
                _isRolesLoaded = true;
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

    private async Task LoadUserRolesAsync(int userId)
    {
        try
        {
            // 重置所有角色为未分配状态
            foreach (var role in AvailableRoles)
            {
                role.IsAssigned = false;
            }

            // 获取用户的角色
            var request = new GetUserRolesRequest { UserId = userId };
            var response = await _rbacClient.GetUserRolesAsync(request);

            if (response.Success)
            {
                // 标记已分配的角色
                var userRoleIds = response.Roles.Select(r => r.Id).ToHashSet();
                foreach (var role in AvailableRoles)
                {
                    role.IsAssigned = userRoleIds.Contains(role.Id);
                }
            }
            else
            {
                MessageBox.Show($"加载用户角色失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载用户角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task AddUserAsync()
    {
        if (string.IsNullOrWhiteSpace(NewUsername) || string.IsNullOrWhiteSpace(NewEmail) || string.IsNullOrWhiteSpace(NewPassword))
        {
            MessageBox.Show("请填写所有必填字段。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var request = new CreateUserRequest
            {
                Username = NewUsername,
                Email = NewEmail,
                Password = NewPassword
            };

            var response = await _rbacClient.CreateUserAsync(request);
            
            if (response.Success)
            {
                Users.Add(response.User);
                
                // 为新创建的用户分配选中的角色
                if (response.User.Id > 0)
                {
                    await AssignRolesToUserAsync(response.User.Id);
                }
                
                MessageBox.Show("用户创建成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                
                // 清空输入框
                NewUsername = string.Empty;
                NewEmail = string.Empty;
                NewPassword = string.Empty;
            }
            else
            {
                MessageBox.Show($"创建用户失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"创建用户时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private async Task AssignRolesToUserAsync(int userId)
    {
        try
        {
            // 处理需要分配的角色
            var rolesToAssign = AvailableRoles.Where(r => r.IsAssigned).ToList();
            foreach (var role in rolesToAssign)
            {
                var request = new AssignRoleToUserRequest
                {
                    UserId = userId,
                    RoleId = role.Id
                };

                var response = await _rbacClient.AssignRoleToUserAsync(request);
                if (!response.Success)
                {
                    MessageBox.Show($"分配角色 '{role.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"分配用户角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async void EditUser()
    {
        if (SelectedUser == null) return;
        
        IsEditing = true;
        EditingUser = new User
        {
            Id = SelectedUser.Id,
            Username = SelectedUser.Username,
            Email = SelectedUser.Email,
            IsActive = SelectedUser.IsActive
        };
        
        // 加载用户的角色
        if (_isRolesLoaded)
        {
            await LoadUserRolesAsync(SelectedUser.Id);
        }
    }

    private bool CanEditOrDeleteUser()
    {
        return SelectedUser != null;
    }

    private async Task DeleteUserAsync()
    {
        if (SelectedUser == null) return;
        
        var result = MessageBox.Show($"确定要删除用户 '{SelectedUser.Username}' 吗？", "确认删除", 
            MessageBoxButton.YesNo, MessageBoxImage.Question);
        
        if (result == MessageBoxResult.No) return;

        try
        {
            var request = new DeleteUserRequest { Id = SelectedUser.Id };
            var response = await _rbacClient.DeleteUserAsync(request);
            
            if (response.Success)
            {
                Users.Remove(SelectedUser);
                SelectedUser = null;
                MessageBox.Show("用户删除成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"删除用户失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"删除用户时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveUserAsync()
    {
        if (EditingUser == null) return;

        try
        {
            var request = new UpdateUserRequest
            {
                User = EditingUser
            };

            var response = await _rbacClient.UpdateUserAsync(request);
            
            if (response.Success)
            {
                // 更新UI中的用户信息
                var index = Users.IndexOf(SelectedUser!);
                Users[index] = response.User;
                SelectedUser = response.User;
                
                // 保存用户角色分配
                await SaveUserRolesAsync(EditingUser.Id);
                
                IsEditing = false;
                EditingUser = null;
                
                MessageBox.Show("用户信息和角色更新成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show($"更新用户失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"更新用户时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveUserRolesAsync(int userId)
    {
        try
        {
            // 先获取用户当前的角色
            var getRequest = new GetUserRolesRequest { UserId = userId };
            var getResponse = await _rbacClient.GetUserRolesAsync(getRequest);

            if (!getResponse.Success)
            {
                MessageBox.Show($"获取用户当前角色失败: {getResponse.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var currentUserRoleIds = getResponse.Roles.Select(r => r.Id).ToHashSet();

            // 处理需要添加的角色
            var rolesToAssign = AvailableRoles.Where(r => r.IsAssigned && !currentUserRoleIds.Contains(r.Id)).ToList();
            foreach (var role in rolesToAssign)
            {
                var request = new AssignRoleToUserRequest
                {
                    UserId = userId,
                    RoleId = role.Id
                };

                var response = await _rbacClient.AssignRoleToUserAsync(request);
                if (!response.Success)
                {
                    MessageBox.Show($"分配角色 '{role.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            // 处理需要移除的角色
            var rolesToRemove = AvailableRoles.Where(r => !r.IsAssigned && currentUserRoleIds.Contains(r.Id)).ToList();
            foreach (var role in rolesToRemove)
            {
                var request = new RemoveRoleFromUserRequest
                {
                    UserId = userId,
                    RoleId = role.Id
                };

                var response = await _rbacClient.RemoveRoleFromUserAsync(request);
                if (!response.Success)
                {
                    MessageBox.Show($"移除角色 '{role.Name}' 失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"保存用户角色时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelEdit()
    {
        IsEditing = false;
        EditingUser = null;
    }

    public void NewPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
    {
        if (sender is PasswordBox passwordBox)
        {
            NewPassword = passwordBox.Password;
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}