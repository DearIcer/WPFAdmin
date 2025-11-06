using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views;

/// <summary>
/// MenuManagementView.xaml 的交互逻辑
/// </summary>
public partial class MenuManagementView : UserControl
{
    public MenuManagementView()
    {
        InitializeComponent();
        DataContext = new MenuManagementViewModel();
        
        // 订阅DataContext的变化事件，以便更新父级菜单ComboBox
        if (DataContext is MenuManagementViewModel viewModel)
        {
            viewModel.MenuItemsChanged += OnMenuItemsChanged;
        }
        
        // 订阅ComboBox选择变化事件
        ParentComboBox.SelectionChanged += ParentComboBox_SelectionChanged;
        EditingParentComboBox.SelectionChanged += EditingParentComboBox_SelectionChanged;
    }

    private void MenuTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MenuManagementViewModel viewModel)
        {
            viewModel.SelectedItem = e.NewValue as Models.MenuItem;
            
            // 更新编辑区域的值
            if (viewModel.SelectedItem != null)
            {
                viewModel.NewItemId = viewModel.SelectedItem.Id;
                viewModel.NewItemName = viewModel.SelectedItem.Name;
                viewModel.NewItemIcon = viewModel.SelectedItem.Icon;
                
                // 更新编辑父级菜单的ComboBox
                UpdateEditingParentComboBox();
            }
        }
    }
    
    private void OnMenuItemsChanged(object? sender, ObservableCollection<Models.MenuItem> menuItems)
    {
        // 清空ComboBox项
        ParentComboBox.Items.Clear();
        EditingParentComboBox.Items.Clear();
        
        // 添加"无父级"选项
        ParentComboBox.Items.Add(new ComboBoxItem { Content = "无父级", Tag = null });
        EditingParentComboBox.Items.Add(new ComboBoxItem { Content = "无父级", Tag = null });
        
        // 递归添加所有菜单项到ComboBox
        AddMenuItemsToComboBox(ParentComboBox, menuItems, 0);
        AddMenuItemsToComboBox(EditingParentComboBox, menuItems, 0);
    }
    
    private void AddMenuItemsToComboBox(ComboBox comboBox, ObservableCollection<Models.MenuItem> menuItems, int level)
    {
        foreach (var item in menuItems)
        {
            // 创建缩进来表示层级关系
            var indent = new string(' ', level * 2);
            comboBox.Items.Add(new ComboBoxItem { Content = $"{indent}{item.Name}", Tag = item });
            
            // 递归添加子项
            if (item.Children.Count > 0)
            {
                AddMenuItemsToComboBox(comboBox, item.Children, level + 1);
            }
        }
    }
    
    private void ParentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MenuManagementViewModel viewModel && ParentComboBox.SelectedItem is ComboBoxItem comboBoxItem)
        {
            viewModel.ParentItem = comboBoxItem.Tag as Models.MenuItem;
        }
    }
    
    private void EditingParentComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (DataContext is MenuManagementViewModel viewModel && EditingParentComboBox.SelectedItem is ComboBoxItem comboBoxItem)
        {
            viewModel.EditingParentItem = comboBoxItem.Tag as Models.MenuItem;
        }
    }
    
    private void UpdateEditingParentComboBox()
    {
        if (DataContext is MenuManagementViewModel viewModel && viewModel.SelectedItem != null)
        {
            // 查找当前选中项的父级菜单项
            var parentItem = viewModel.FindParentMenuItem(viewModel.SelectedItem);
            
            // 在EditingParentComboBox中选择对应的项
            foreach (ComboBoxItem item in EditingParentComboBox.Items)
            {
                if (item.Tag == parentItem)
                {
                    EditingParentComboBox.SelectedItem = item;
                    break;
                }
            }
        }
    }
}