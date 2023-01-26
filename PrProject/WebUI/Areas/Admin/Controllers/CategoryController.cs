using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.Categories);
        }

        public async Task<IActionResult> Detail(int id)
        {
            Category? category = await _context.Categories.Include(x => x.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (category == null) 
                return NotFound();

            return View(category);
        }
    }
}
