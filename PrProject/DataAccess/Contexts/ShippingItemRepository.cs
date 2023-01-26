using Core.Entities;
using DataAccess.Interfaces;

namespace DataAccess.Contexts;

public class ShippingItemRepository : Repository<ShippingItem>, IShippingItemRepository
{
    public ShippingItemRepository(AppDbContext context) : base(context)
    {
    }
}
