namespace ProductManagementApp.Products;

public class Product
{
    private Product()
    {
    }

    public Product(Guid productId, string productName, decimal price, int stockQuantity)
    {
        //Rule
        if (string.IsNullOrEmpty(productName.Trim()))
            throw new Exception("Invalid properties.");

        if (price <= 0)
            throw new Exception("Invalid properties.");

        if (stockQuantity < 0)
            throw new Exception("Invalid properties.");

        ProductId = productId;
        ProductName = productName;
        Price = price;
        StockQuantity = stockQuantity;
    }

    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; }
    public decimal Price { get; private set; }
    public int StockQuantity { get; private set; }
    public bool Deleted { get; private set; }

    public void Delete()
    {
        Deleted = true;
    }

    public void Update(string productName, decimal price, int stockQuantity)
    {
        //Rule
        if (string.IsNullOrEmpty(productName.Trim()))
            throw new Exception("Invalid properties.");

        if (price <= 0)
            throw new Exception("Invalid properties.");

        if (stockQuantity < 0)
            throw new Exception("Invalid properties.");
        
        ProductName = productName;
        Price = price;
        StockQuantity = stockQuantity;
    }
}