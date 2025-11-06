using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using Client.Models;

namespace Client.ViewModels;

public class MenuManagementViewModel : INotifyPropertyChanged
{
    private ObservableCollection<MenuItem> _menuItems;
    private MenuItem? _selectedItem;
    private string _newItemName = string.Empty;
    private string _newItemIcon = string.Empty;
    private string _newItemId = string.Empty;
    private MenuItem? _parentItem;

    public MenuManagementViewModel()
    {
        _menuItems = new ObservableCollection<MenuItem>();
        LoadMenuData();
        
        AddCommand = new RelayCommand(AddMenuItem);
        EditCommand = new RelayCommand(EditMenuItem, CanEditOrDelete);
        DeleteCommand = new RelayCommand(DeleteMenuItem, CanEditOrDelete);
        SaveCommand = new RelayCommand(SaveMenuItems);
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

    public ICommand AddCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }
    public ICommand SaveCommand { get; }

    private void LoadMenuData()
    {
        // 从权限服务加载所有菜单项（不考虑权限过滤）
        var permissionService = new Services.PermissionService();
        var allMenuItems = permissionService.LoadAllMenuItems();
        MenuItems = new ObservableCollection<MenuItem>(allMenuItems);
    }

    private void AddMenuItem()
    {
        if (string.IsNullOrWhiteSpace(NewItemName) || string.IsNullOrWhiteSpace(NewItemId))
        {
            MessageBox.Show("请填写菜单项ID和名称。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var newItem = new MenuItem
        {
            Id = NewItemId,
            Name = NewItemName,
            Icon = NewItemIcon,
            Children = new ObservableCollection<MenuItem>()
        };

        if (ParentItem != null)
        {
            ParentItem.Children.Add(newItem);
        }
        else
        {
            MenuItems.Add(newItem);
        }

        // 清空输入框
        NewItemId = string.Empty;
        NewItemName = string.Empty;
        NewItemIcon = string.Empty;
        ParentItem = null;
    }

    private void EditMenuItem()
    {
        if (SelectedItem == null) return;

        // 在实际应用中，这里可能会打开一个编辑对话框
        // 为了简化，我们直接修改当前选中项
        SelectedItem.Name = NewItemName;
        SelectedItem.Icon = NewItemIcon;
        SelectedItem.Id = NewItemId;
        
        OnPropertyChanged(nameof(MenuItems));
    }

    private bool CanEditOrDelete()
    {
        return SelectedItem != null;
    }

    private void DeleteMenuItem()
    {
        if (SelectedItem == null) return;

        // 查找并删除选中的菜单项
        DeleteMenuItemRecursive(MenuItems, SelectedItem);
        SelectedItem = null;
    }

    private bool DeleteMenuItemRecursive(ObservableCollection<MenuItem> items, MenuItem itemToDelete)
    {
        if (items.Contains(itemToDelete))
        {
            items.Remove(itemToDelete);
            return true;
        }

        foreach (var item in items)
        {
            if (DeleteMenuItemRecursive(item.Children, itemToDelete))
                return true;
        }

        return false;
    }

    private void SaveMenuItems()
    {
        // 在实际应用中，这里会将菜单数据保存到后端
        MessageBox.Show("菜单项已保存。在实际应用中，这些更改将被保存到数据库中。", "保存成功", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public class RelayCommand : ICommand
{
    private readonly Action _execute;
    private readonly Func<bool>? _canExecute;

    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    {
        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
        _canExecute = canExecute;
    }

    public event EventHandler? CanExecuteChanged;

    public bool CanExecute(object? parameter)
    {
        return _canExecute?.Invoke() ?? true;
    }

    public void Execute(object? parameter)
    {
        _execute();
    }

    public void RaiseCanExecuteChanged()
    {
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}