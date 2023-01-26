using Core.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Core.Entities;

public class ShippingItem:IEntity
{
    public int Id { get; set; }
    [Required]
    public string? Image { get; set; }
    [Required(ErrorMessage ="Qaqa bosh buraxma"), MaxLength(100)]
    public string? Title { get; set; }
    public string? Description { get; set; }
}
