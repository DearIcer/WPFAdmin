using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Grpc.Net.Client;

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
    
    private readonly RBACService.RBACServiceClient _rbacClient;

    public UserManagementViewModel()
    {
        _users = new ObservableCollection<User>();
        
        // 初始化gRPC客户端
        var channel = GrpcChannel.ForAddress("http://localhost:5101");
        _rbacClient = new RBACService.RBACServiceClient(channel);
        
        LoadUsersCommand = new RelayCommand(async () => await LoadUsersAsync());
        AddUserCommand = new RelayCommand(async () => await AddUserAsync());
        EditUserCommand = new RelayCommand(EditUser, CanEditOrDeleteUser);
        DeleteUserCommand = new RelayCommand(async () => await DeleteUserAsync(), CanEditOrDeleteUser);
        SaveUserCommand = new RelayCommand(async () => await SaveUserAsync());
        CancelEditCommand = new RelayCommand(CancelEdit);
        
        // 加载用户数据
        _ = LoadUsersAsync();
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

    private void EditUser()
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
                
                IsEditing = false;
                EditingUser = null;
                
                MessageBox.Show("用户信息更新成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
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

    private void CancelEdit()
    {
        IsEditing = false;
        EditingUser = null;
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}