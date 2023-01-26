using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.ViewComponents;

public class ProductViewComponent : ViewComponent
{
    private readonly AppDbContext _context;

    public ProductViewComponent(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var products = await _context.Products.Take(8).AsNoTracking().ToListAsync();

        return View(products);
    }
}
