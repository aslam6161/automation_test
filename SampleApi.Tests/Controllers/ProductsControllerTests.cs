using Microsoft.AspNetCore.Mvc;
using Moq;
using SampleApi.Controllers;
using SampleApi.Models;
using SampleApi.Services;
using Xunit;

namespace SampleApi.Tests.Controllers;

public class ProductsControllerTests
{
    private readonly Mock<IProductService> _mockService = new();
    private readonly ProductsController _controller;

    public ProductsControllerTests()
    {
        _controller = new ProductsController(_mockService.Object);
    }

    // ── GetAll ────────────────────────────────────────────────────────────────

    [Fact]
    public void GetAll_Returns200WithProducts()
    {
        var products = new List<Product>
        {
            new() { Id = 1, Name = "Laptop", Price = 999m, Stock = 10 }
        };
        _mockService.Setup(s => s.GetAll()).Returns(products);

        var result = _controller.GetAll() as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result!.StatusCode);
        Assert.Equal(products, result.Value);
    }

    // ── GetById ───────────────────────────────────────────────────────────────

    [Fact]
    public void GetById_ExistingId_Returns200()
    {
        var product = new Product { Id = 1, Name = "Mouse", Price = 29.99m, Stock = 50 };
        _mockService.Setup(s => s.GetById(1)).Returns(product);

        var result = _controller.GetById(1) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result!.StatusCode);
    }

    [Fact]
    public void GetById_NonExistingId_Returns404()
    {
        _mockService.Setup(s => s.GetById(999)).Returns((Product?)null);

        var result = _controller.GetById(999);

        Assert.IsType<NotFoundResult>(result);
    }

    // ── Create ────────────────────────────────────────────────────────────────

    [Fact]
    public void Create_ValidProduct_Returns201()
    {
        var newProduct = new Product { Name = "Webcam", Price = 79.99m, Stock = 15 };
        var created    = new Product { Id = 4, Name = "Webcam", Price = 79.99m, Stock = 15 };
        _mockService.Setup(s => s.Create(newProduct)).Returns(created);

        var result = _controller.Create(newProduct) as CreatedAtActionResult;

        Assert.NotNull(result);
        Assert.Equal(201, result!.StatusCode);
        Assert.Equal(created, result.Value);
    }

    [Fact]
    public void Create_EmptyName_Returns400()
    {
        var bad = new Product { Name = "  ", Price = 10m, Stock = 1 };

        var result = _controller.Create(bad);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public void Create_NegativePrice_Returns400()
    {
        var bad = new Product { Name = "Widget", Price = -5m, Stock = 1 };

        var result = _controller.Create(bad);

        Assert.IsType<BadRequestObjectResult>(result);
    }

    // ── Update ────────────────────────────────────────────────────────────────

    [Fact]
    public void Update_ExistingProduct_Returns200()
    {
        var updated = new Product { Id = 1, Name = "Pro Laptop", Price = 1299m, Stock = 5 };
        _mockService.Setup(s => s.Update(1, It.IsAny<Product>())).Returns(updated);

        var result = _controller.Update(1, updated) as OkObjectResult;

        Assert.NotNull(result);
        Assert.Equal(200, result!.StatusCode);
    }

    [Fact]
    public void Update_NonExistingId_Returns404()
    {
        _mockService.Setup(s => s.Update(999, It.IsAny<Product>())).Returns((Product?)null);

        var result = _controller.Update(999, new Product());

        Assert.IsType<NotFoundResult>(result);
    }

    // ── Delete ────────────────────────────────────────────────────────────────

    [Fact]
    public void Delete_ExistingProduct_Returns204()
    {
        _mockService.Setup(s => s.Delete(1)).Returns(true);

        var result = _controller.Delete(1);

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_NonExistingId_Returns404()
    {
        _mockService.Setup(s => s.Delete(999)).Returns(false);

        var result = _controller.Delete(999);

        Assert.IsType<NotFoundResult>(result);
    }
}
