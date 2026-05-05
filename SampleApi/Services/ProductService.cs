using SampleApi.Models;

namespace SampleApi.Services;

public class ProductService : IProductService
{
    private readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop",     Price = 999.99m,  Stock = 10 },
        new Product { Id = 2, Name = "Mouse",      Price = 29.99m,   Stock = 50 },
        new Product { Id = 3, Name = "Keyboard",   Price = 49.99m,   Stock = 30 },
    };

    private int _nextId = 4;

    public IEnumerable<Product> GetAll() => _products;

    public Product? GetById(int id) =>
        _products.FirstOrDefault(p => p.Id == id);

    public Product Create(Product product)
    {
        product.Id = _nextId++;
        _products.Add(product);
        return product;
    }

    public Product? Update(int id, Product product)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is null) return null;

        existing.Name  = product.Name;
        existing.Price = product.Price;
        existing.Stock = product.Stock;
        return existing;
    }

    public bool Delete(int id)
    {
        var existing = _products.FirstOrDefault(p => p.Id == id);
        if (existing is null) return false;
        _products.Remove(existing);
        return true;
    }
}
