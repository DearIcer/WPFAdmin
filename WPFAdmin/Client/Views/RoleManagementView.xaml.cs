using System.Windows.Controls;
using Client.ViewModels;

namespace Client.Views;

public partial class RoleManagementView : UserControl
{
    public RoleManagementView()
    {
        InitializeComponent();
        DataContext = new RoleManagementViewModel();
    }
}