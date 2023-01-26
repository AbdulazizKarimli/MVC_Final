using Core.Entities;
using DataAccess.Contexts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUI.Areas.Admin.ViewModels.Slider;
using WebUI.Utilities;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class SlideItemController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    private int _count;

    public SlideItemController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
        _count = _context.SlideItems.Count();
    }

    public IActionResult Index()
    {
        ViewBag.Count = _count;
        return View(_context.SlideItems);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var slide = await _context.SlideItems.FindAsync(id);
        if(slide == null) return NotFound();
        return View(slide);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SlideCreateVM slide)
    {
        if (!ModelState.IsValid) return View(slide);
        if (slide.Photo == null)
        {
            ModelState.AddModelError("Photo", "Select Photo");
            return View(slide);
        }
        if (!slide.Photo.CheckFileSize(100))
        {
            ModelState.AddModelError("Photo", "Image size must be less than 100 kb");
            return View(slide);
        }
        if (!slide.Photo.CheckFileFormat("image/"))
        {
            ModelState.AddModelError("Photo", "You must be choose image type");
            return View(slide);
        }
        var filename = string.Empty;
        try
        {
            filename=await slide.Photo.CopyFileAsync(_env.WebRootPath, "assets", "images", "website-images");
        }
        catch (Exception)
        {
            return View(slide);
        }

        SlideItem slideItem = new()
        {
            Title = slide.Title,
            Offer= slide.Offer,
            Description= slide.Description,
            Photo= filename
        };
        await _context.SlideItems.AddAsync(slideItem);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        if(_count==1) return BadRequest();
        var slide = await _context.SlideItems.FindAsync(id);
        if (slide == null) return NotFound();
        return View(slide);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeletePost(int id)
    {
        if (_count == 1) return BadRequest();
        var slide = await _context.SlideItems.FindAsync(id);
        if (slide == null) return NotFound();
        Helper.DeleteFile(_env.WebRootPath, "assets", "images", "website-images", slide.Photo);
        _context.SlideItems.Remove(slide);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
