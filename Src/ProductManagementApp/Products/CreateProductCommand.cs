namespace ProductManagementApp.Products;

public class CreateProductCommand
{
    public string ProductName { get; set; }
    public decimal Price { get; set; }
    public int StockQuanity { get; set; }
}