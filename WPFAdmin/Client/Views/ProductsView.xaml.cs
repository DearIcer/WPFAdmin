using System.Windows.Controls;
using Client.Models;

namespace Client.Views;

/// <summary>
///     ProductsView.xaml 的交互逻辑
/// </summary>
public partial class ProductsView : UserControl
{
    public ProductsView()
    {
        InitializeComponent();
        LoadProducts();
        DataContext = this;
    }

    public List<Product> Products { get; set; }

    private void LoadProducts()
    {
        Products = new List<Product>
        {
            new() { Id = "P001", Name = "iPhone 15 Pro", Category = "数码产品", Price = 7999, Stock = 50, Status = "上架" },
            new() { Id = "P002", Name = "MacBook Air M2", Category = "数码产品", Price = 8999, Stock = 30, Status = "上架" },
            new() { Id = "P003", Name = "Nike Air Max", Category = "服装", Price = 899, Stock = 100, Status = "上架" },
            new() { Id = "P004", Name = "咖啡豆 1kg", Category = "食品", Price = 99, Stock = 200, Status = "上架" },
            new() { Id = "P005", Name = "办公椅", Category = "家具", Price = 599, Stock = 20, Status = "下架" }
        };
    }
}