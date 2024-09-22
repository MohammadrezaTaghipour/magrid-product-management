using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagementApp.Framework.Persistence;

namespace ProductManagementApp.Products;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateProductCommand command,
        [FromServices] ProductManagementDbContext context)
    {
        var product = new Product(Guid.NewGuid(), command.ProductName, command.Price, command.StockQuanity);
        context.Products.Add(product);
        await context.SaveChangesAsync();

        return Ok(product.ProductId);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, [FromServices] ProductManagementDbContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null)
            return NotFound();

        product.Delete();
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put([FromRoute] Guid id, [FromBody] CreateProductCommand command,
        [FromServices] ProductManagementDbContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null)
            return NotFound();

        product.Update(command.ProductName, command.Price, command.StockQuanity);
        await context.SaveChangesAsync();

        return NoContent();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get([FromRoute] Guid id, [FromServices] ProductManagementDbContext context)
    {
        var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);
        if (product is null)
            return NotFound();

        return Ok(product);
    }
}