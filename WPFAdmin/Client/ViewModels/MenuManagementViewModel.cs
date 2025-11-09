using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Backed.Grpc;
using Client.Models;
using Client.Services;

namespace Client.ViewModels;

public class MenuManagementViewModel : INotifyPropertyChanged
{
    private ObservableCollection<MenuItem> _menuItems;
    private MenuItem? _selectedItem;
    private string _newItemName = string.Empty;
    private string _newItemIcon = string.Empty;
    private string _newItemId = string.Empty;
    private MenuItem? _parentItem;
    private bool _isEditing;
    private Dictionary<string, int> _menuCodeToIdMap = new();
    private MenuItem? _editingParentItem; 

    private readonly RBACService.RBACServiceClient _rbacClient;

    public MenuManagementViewModel()
    {
        _menuItems = new ObservableCollection<MenuItem>();
        
        _rbacClient = GrpcClientService.Instance.RbacClient;
        
        LoadCommand = new RelayCommand(async () => await LoadMenuDataAsync());
        AddCommand = new RelayCommand(async () => await AddMenuItemAsync());
        EditCommand = new RelayCommand(EditMenuItem, CanEditOrDelete);
        DeleteCommand = new RelayCommand(async () => await DeleteMenuItemAsync(), CanEditOrDelete);
        SaveCommand = new RelayCommand(async () => await SaveMenuItemAsync());
        CancelCommand = new RelayCommand(CancelEdit);
        
        _ = LoadMenuDataAsync();
    }

    public ObservableCollection<MenuItem> MenuItems
    {
        get => _menuItems;
        set
        {
            _menuItems = value;
            OnPropertyChanged();
            MenuItemsChanged?.Invoke(this, _menuItems);
        }
    }

    public MenuItem? SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsItemSelected));
            (EditCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (DeleteCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public bool IsItemSelected => SelectedItem != null;

    public string NewItemName
    {
        get => _newItemName;
        set
        {
            _newItemName = value;
            OnPropertyChanged();
        }
    }

    public string NewItemIcon
    {
        get => _newItemIcon;
        set
        {
            _newItemIcon = value;
            OnPropertyChanged();
        }
    }

    public string NewItemId
    {
        get => _newItemId;
        set
        {
            _newItemId = value;
            OnPropertyChanged();
        }
    }

    public MenuItem? ParentItem
    {
        get => _parentItem;
        set
        {
            _parentItem = value;
            OnPropertyChanged();
        }
    }

    public MenuItem? EditingParentItem
    {
        get => _editingParentItem;
        set
        {
            _editingParentItem = value;
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
            OnPropertyChanged(nameof(IsEditingParentEnabled));
        }
    }

    public bool IsEditingParentEnabled => IsEditing && SelectedItem != null;

    // 事件，当菜单项更改时触发
    public event EventHandler<ObservableCollection<MenuItem>>? MenuItemsChanged;

    public ICommand LoadCommand { get; }
    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    private async Task LoadMenuDataAsync()
    {
        try
        {
            var request = new GetMenuTreeRequest();
            var response = await _rbacClient.GetMenuTreeAsync(request);

            if (response.Success)
            {
                var menuItems = MapGrpcMenusToClient(response.Menus);
                MenuItems = new ObservableCollection<MenuItem>(menuItems);
                
                // 构建菜单Code到ID的映射
                BuildMenuCodeToIdMap(response.Menus);
            }
            else
            {
                MessageBox.Show($"加载菜单失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"加载菜单时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void BuildMenuCodeToIdMap(Google.Protobuf.Collections.RepeatedField<Menu> grpcMenus)
    {
        _menuCodeToIdMap.Clear();
        foreach (var menu in grpcMenus)
        {
            AddMenuToCodeToIdMap(menu);
        }
    }

    private void AddMenuToCodeToIdMap(Menu menu)
    {
        _menuCodeToIdMap[menu.Code] = menu.Id;
        
        // 递归处理子菜单
        foreach (var child in menu.Children)
        {
            AddMenuToCodeToIdMap(child);
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

    private async Task AddMenuItemAsync()
    {
        if (string.IsNullOrWhiteSpace(NewItemName) || string.IsNullOrWhiteSpace(NewItemId))
        {
            MessageBox.Show("请填写菜单项ID和名称。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var request = new CreateMenuRequest
            {
                Name = NewItemName,
                Code = NewItemId,
                Icon = NewItemIcon,
                SortOrder = 0,
                IsActive = true
            };

            // 如果选择了父级菜单，则设置ParentId
            if (ParentItem != null && _menuCodeToIdMap.ContainsKey(ParentItem.Id))
            {
                request.ParentId = _menuCodeToIdMap[ParentItem.Id];
            }

            var response = await _rbacClient.CreateMenuAsync(request);

            if (response.Success)
            {
                MessageBox.Show("菜单项创建成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadMenuDataAsync(); // 重新加载菜单数据

                // 清空输入框
                NewItemId = string.Empty;
                NewItemName = string.Empty;
                NewItemIcon = string.Empty;
                ParentItem = null;
            }
            else
            {
                MessageBox.Show($"创建菜单项失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"创建菜单项时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void EditMenuItem()
    {
        if (SelectedItem == null) return;

        IsEditing = true;
        NewItemId = SelectedItem.Code;
        NewItemName = SelectedItem.Name;
        NewItemIcon = SelectedItem.Icon;
        
        EditingParentItem = FindParentMenuItem(SelectedItem);
    }

    // 查找指定菜单项的父级菜单项
    public MenuItem? FindParentMenuItem(MenuItem item)
    {
        return FindParentMenuItemRecursive(MenuItems, item);
    }

    private MenuItem? FindParentMenuItemRecursive(ObservableCollection<MenuItem> items, MenuItem targetItem)
    {
        foreach (var item in items)
        {
            // 检查当前项的子项是否包含目标项
            foreach (var child in item.Children)
            {
                if (child.Id == targetItem.Id)
                {
                    return item; // 找到父级
                }
            }

            // 递归检查子项
            var found = FindParentMenuItemRecursive(item.Children, targetItem);
            if (found != null)
            {
                return found;
            }
        }

        return null; // 未找到父级（可能是顶级菜单）
    }

    private bool CanEditOrDelete()
    {
        return SelectedItem != null;
    }

    private async Task DeleteMenuItemAsync()
    {
        if (SelectedItem == null) return;

        var result = MessageBox.Show($"确定要删除菜单项 '{SelectedItem.Name}' 吗？这将删除所有子菜单项。", "确认删除",
            MessageBoxButton.YesNo, MessageBoxImage.Question);

        if (result == MessageBoxResult.No) return;

        try
        {
            // 获取要删除的菜单项ID
            if (!_menuCodeToIdMap.ContainsKey(SelectedItem.Id))
            {
                MessageBox.Show("无法找到要删除的菜单项。", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var menuId = _menuCodeToIdMap[SelectedItem.Id];
            var request = new DeleteMenuRequest { Id = menuId };
            var response = await _rbacClient.DeleteMenuAsync(request);

            if (response.Success)
            {
                MessageBox.Show("菜单项删除成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadMenuDataAsync(); // 重新加载菜单数据
                SelectedItem = null;
            }
            else
            {
                MessageBox.Show($"删除菜单项失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"删除菜单项时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private async Task SaveMenuItemAsync()
    {
        if (SelectedItem == null || !_menuCodeToIdMap.ContainsKey(SelectedItem.Id)) return;

        try
        {
            var menuId = _menuCodeToIdMap[SelectedItem.Id];
            var request = new UpdateMenuRequest
            {
                Menu = new Menu
                {
                    Id = menuId,
                    Name = NewItemName,
                    Code = NewItemId,
                    Icon = NewItemIcon,
                    SortOrder = 0,
                    IsActive = true
                }
            };

            // 如果选择了父级菜单，则设置ParentId
            if (EditingParentItem != null && _menuCodeToIdMap.ContainsKey(EditingParentItem.Id) && 
                EditingParentItem.Id != SelectedItem.Id) // 避免自己成为自己的父级
            {
                request.Menu.ParentId = _menuCodeToIdMap[EditingParentItem.Id];
            }
            // 如果没有选择父级菜单，则设置ParentId为0（表示顶级菜单）
            else if (EditingParentItem == null)
            {
                request.Menu.ParentId = 0;
            }

            var response = await _rbacClient.UpdateMenuAsync(request);

            if (response.Success)
            {
                MessageBox.Show("菜单项更新成功。", "成功", MessageBoxButton.OK, MessageBoxImage.Information);
                await LoadMenuDataAsync(); // 重新加载菜单数据
                IsEditing = false;
            }
            else
            {
                MessageBox.Show($"更新菜单项失败: {response.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"更新菜单项时发生错误: {ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void CancelEdit()
    {
        IsEditing = false;
        if (SelectedItem != null)
        {
            NewItemId = SelectedItem.Code;
            NewItemName = SelectedItem.Name;
            NewItemIcon = SelectedItem.Icon;
            EditingParentItem = FindParentMenuItem(SelectedItem);
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}