using Microsoft.AspNetCore.Mvc;
using SampleApi.Models;
using SampleApi.Services;

namespace SampleApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IProductService _service;

    public ProductsController(IProductService service)
    {
        _service = service;
    }

    /// <summary>Get all products</summary>
    [HttpGet]
    public IActionResult GetAll() =>
        Ok(_service.GetAll());

    /// <summary>Get product by id</summary>
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var product = _service.GetById(id);
        return product is null ? NotFound() : Ok(product);
    }

    /// <summary>Create a new product</summary>
    [HttpPost]
    public IActionResult Create([FromBody] Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
            return BadRequest("Product name is required.");

        if (product.Price < 0)
            return BadRequest("Price cannot be negative.");

        var created = _service.Create(product);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Update an existing product</summary>
    [HttpPut("{id:int}")]
    public IActionResult Update(int id, [FromBody] Product product)
    {
        var updated = _service.Update(id, product);
        return updated is null ? NotFound() : Ok(updated);
    }

    /// <summary>Delete a product</summary>
    [HttpDelete("{id:int}")]
    public IActionResult Delete(int id)
    {
        var deleted = _service.Delete(id);
        return deleted ? NoContent() : NotFound();
    }
}
