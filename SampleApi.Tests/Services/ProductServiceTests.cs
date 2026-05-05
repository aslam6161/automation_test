using SampleApi.Models;
using SampleApi.Services;
using Xunit;

namespace SampleApi.Tests.Services;

public class ProductServiceTests
{
    private readonly ProductService _service = new();

    // ── GetAll ────────────────────────────────────────────────────────────────

    [Fact]
    public void GetAll_ReturnsAllSeededProducts()
    {
        var result = _service.GetAll().ToList();
        Assert.Equal(3, result.Count);
    }

    // ── GetById ───────────────────────────────────────────────────────────────

    [Fact]
    public void GetById_ExistingId_ReturnsProduct()
    {
        var product = _service.GetById(1);
        Assert.NotNull(product);
        Assert.Equal(1, product.Id);
    }

    [Fact]
    public void GetById_NonExistingId_ReturnsNull()
    {
        var product = _service.GetById(999);
        Assert.Null(product);
    }

    // ── Create ────────────────────────────────────────────────────────────────

    [Fact]
    public void Create_ValidProduct_AssignsIdAndReturns()
    {
        var newProduct = new Product { Name = "Monitor", Price = 299.99m, Stock = 5 };

        var created = _service.Create(newProduct);

        Assert.True(created.Id > 0);
        Assert.Equal("Monitor", created.Name);
        Assert.Equal(299.99m, created.Price);
    }

    [Fact]
    public void Create_ProductAppearsInGetAll()
    {
        var before = _service.GetAll().Count();
        _service.Create(new Product { Name = "Hub", Price = 19.99m, Stock = 20 });

        Assert.Equal(before + 1, _service.GetAll().Count());
    }

    // ── Update ────────────────────────────────────────────────────────────────

    [Fact]
    public void Update_ExistingProduct_ReturnUpdatedProduct()
    {
        var updated = _service.Update(1, new Product { Name = "Gaming Laptop", Price = 1499m, Stock = 3 });

        Assert.NotNull(updated);
        Assert.Equal("Gaming Laptop", updated!.Name);
        Assert.Equal(1499m, updated.Price);
    }

    [Fact]
    public void Update_NonExistingId_ReturnsNull()
    {
        var result = _service.Update(999, new Product { Name = "Ghost", Price = 0, Stock = 0 });
        Assert.Null(result);
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    [Fact]
    public void Delete_ExistingProduct_ReturnsTrueAndRemoves()
    {
        var result = _service.Delete(1);

        Assert.True(result);
        Assert.Null(_service.GetById(1));
    }

    [Fact]
    public void Delete_NonExistingId_ReturnsFalse()
    {
        var result = _service.Delete(999);
        Assert.False(result);
    }
}
