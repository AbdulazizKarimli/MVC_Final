using Core.Entities;

namespace Business.Interfaces;

public interface IShippingItemService
{
    IEnumerable<ShippingItem> GetAll();
    Task<ShippingItem> GetAsync(int id);
    Task<bool> CreateAsync(ShippingItem item);
    Task<bool> UpdateAsync(int Id);
    Task<bool> DeleteAsync(int Id);
}
