using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Models;

public class MenuItem : INotifyPropertyChanged
{
    private string _id = string.Empty;
    private string _code = string.Empty;
    private string _name = string.Empty;
    private string _icon = string.Empty;
    private bool _isExpanded = false;
    private bool _isSelected = false;
    private bool _isAssigned = false;
    private MenuItem? _parent;
    private ObservableCollection<MenuItem> _children = new();

    public string Id
    {
        get => _id;
        set
        {
            _id = value;
            OnPropertyChanged();
        }
    }

    public string Code
    {
        get => _code;
        set
        {
            _code = value;
            OnPropertyChanged();
        }
    }

    public string Name
    {
        get => _name;
        set
        {
            _name = value;
            OnPropertyChanged();
        }
    }

    public string Icon
    {
        get => _icon;
        set
        {
            _icon = value;
            OnPropertyChanged();
        }
    }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            _isExpanded = value;
            OnPropertyChanged();
        }
    }

    public bool IsSelected
    {
        get => _isSelected;
        set
        {
            _isSelected = value;
            OnPropertyChanged();
        }
    }

    public bool IsAssigned
    {
        get => _isAssigned;
        set
        {
            _isAssigned = value;
            OnPropertyChanged();
            
            // 当父级菜单被选中时，自动选中所有子菜单
            if (_isAssigned)
            {
                foreach (var child in Children)
                {
                    child.IsAssigned = true;
                }
            }
        }
    }

    public MenuItem? Parent
    {
        get => _parent;
        set
        {
            _parent = value;
            OnPropertyChanged();
        }
    }

    public ObservableCollection<MenuItem> Children
    {
        get => _children;
        set
        {
            _children = value;
            OnPropertyChanged();
            // 设置子项的父级引用
            foreach (var child in _children)
            {
                child.Parent = this;
            }
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}