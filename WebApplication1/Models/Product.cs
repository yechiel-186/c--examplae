using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models;

/// <summary>
/// Represents a product in the system
/// </summary>
public class Product
{
    /// <summary>
    /// Unique identifier for the product
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the product
    /// </summary>
    [Required(ErrorMessage = "Product name is required")]
    [StringLength(100, ErrorMessage = "Product name cannot exceed 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Price of the product
    /// </summary>
    [Required(ErrorMessage = "Product price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "Product price must be greater than 0")]
    public decimal Price { get; set; }
}