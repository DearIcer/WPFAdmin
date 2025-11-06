using System.Collections.ObjectModel;

namespace Client.Models;

public class MenuItem
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsExpanded { get; set; } = false;
    public bool IsSelected { get; set; } = false;
    public ObservableCollection<MenuItem> Children { get; set; } = new();
}