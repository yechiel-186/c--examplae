using WebApplication1.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// In-memory storage for demonstration (replace with database in production)
var products = new List<Product>
{
    new Product { Id = 1, Name = "Laptop", Price = 999.99m },
    new Product { Id = 2, Name = "Mouse", Price = 25.50m },
    new Product { Id = 3, Name = "Keyboard", Price = 75.00m }
};

// Product API Endpoints
var productsApi = app.MapGroup("/api/products")
    .WithTags("Products")
    .WithOpenApi();

// GET /api/products - Get all products
productsApi.MapGet("/", () => Results.Ok(products))
    .WithName("GetProducts")
    .WithSummary("Get all products")
    .Produces<IEnumerable<Product>>(200);

// GET /api/products/{id} - Get product by ID
productsApi.MapGet("/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
    .WithName("GetProductById")
    .WithSummary("Get a product by ID")
    .Produces<Product>(200)
    .Produces(404);

// POST /api/products - Create new product
productsApi.MapPost("/", (Product product) =>
{
    // Generate new ID
    product.Id = products.Count > 0 ? products.Max(p => p.Id) + 1 : 1;
    products.Add(product);
    return Results.Created($"/api/products/{product.Id}", product);
})
    .WithName("CreateProduct")
    .WithSummary("Create a new product")
    .Produces<Product>(201)
    .Produces(400);

// PUT /api/products/{id} - Update product
productsApi.MapPut("/{id:int}", (int id, Product updatedProduct) =>
{
    var existingProduct = products.FirstOrDefault(p => p.Id == id);
    if (existingProduct is null)
        return Results.NotFound();

    existingProduct.Name = updatedProduct.Name;
    existingProduct.Price = updatedProduct.Price;
    return Results.Ok(existingProduct);
})
    .WithName("UpdateProduct")
    .WithSummary("Update an existing product")
    .Produces<Product>(200)
    .Produces(404);

// DELETE /api/products/{id} - Delete product
productsApi.MapDelete("/{id:int}", (int id) =>
{
    var product = products.FirstOrDefault(p => p.Id == id);
    if (product is null)
        return Results.NotFound();

    products.Remove(product);
    return Results.NoContent();
})
    .WithName("DeleteProduct")
    .WithSummary("Delete a product")
    .Produces(204)
    .Produces(404);

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
