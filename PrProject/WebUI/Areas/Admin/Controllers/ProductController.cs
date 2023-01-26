using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Areas.Admin.ViewModels.Product;

namespace WebUI.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var products = _context.Products.Include(p => p.Category).AsEnumerable();

            return View(products);
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel createProductViewModel, int categoryId)
        {
            ViewBag.Categories = _context.Categories;

            if (!ModelState.IsValid) return View(createProductViewModel);
            //all -> id=5 hamisi true
            //any -> id=5 1 dene true
            if (!_context.Categories.Any(x => x.Id == categoryId))
                return BadRequest();

            Product product = new()
            {
                Name = createProductViewModel.Name,
                Price = createProductViewModel.Price,
                Image = createProductViewModel.Image,
                Raiting = createProductViewModel.Raiting,
                CategoryId = categoryId
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
