using System.Windows;
using Client.Models;
using Client.ViewModels;
using Client.Services;

namespace Client;

/// <summary>
///     Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void MenuTree_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
        if (DataContext is MainViewModel viewModel) 
            viewModel.SelectedItem = e.NewValue as MenuItem;
    }
}