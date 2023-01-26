using Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class Product : IEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public double Price { get; set; }
    public string? Image { get; set; }
    public int Raiting { get; set; }
    public int CategoryId { get; set; }
    public Category Category { get; set; }
}