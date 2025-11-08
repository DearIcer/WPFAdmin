using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Client.Models;

public class RoleItem : INotifyPropertyChanged
{
    private bool _isAssigned;
    
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public bool IsAssigned
    {
        get => _isAssigned;
        set
        {
            _isAssigned = value;
            OnPropertyChanged();
        }
    }
    
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}