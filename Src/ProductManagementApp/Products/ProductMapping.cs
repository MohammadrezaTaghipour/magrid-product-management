using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ProductManagementApp.Products;

public class ProductMapping : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasIndex(b => b.ProductId).IsUnique();

        builder.Property(b => b.ProductName).IsRequired();
        builder.Property(b => b.Price).IsRequired();
        builder.Property(b => b.StockQuantity).IsRequired();
        builder.Property(b => b.Deleted).IsRequired();

        builder.HasQueryFilter(e => !e.Deleted);
    }
}