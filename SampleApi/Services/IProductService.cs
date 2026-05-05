using SampleApi.Models;

namespace SampleApi.Services;

public interface IProductService
{
    IEnumerable<Product> GetAll();
    Product? GetById(int id);
    Product Create(Product product);
    Product? Update(int id, Product product);
    bool Delete(int id);
}
