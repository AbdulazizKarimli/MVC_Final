using Core.Entities;
using DataAccess.Contexts;
using DataAccess.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;
[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ShippingItemController : Controller
{
    private IShippingItemRepository _repository;

    public ShippingItemController(IShippingItemRepository repository)
    {
        _repository= repository;
    }

    public async Task<IActionResult> Index()
    {
        return View(await _repository.GetAllAsync());
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ShippingItem item)
    {
        if(!ModelState.IsValid) return View(item);
        await _repository.CreateAsync(item);
        await _repository.SaveAsync();
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Detail(int id)
    {
        var model=await _repository.GetAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }

    public async Task<IActionResult> Update(int id)
    {
        var model = await _repository.GetAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update(int id,ShippingItem item)
    {
        if(id!=item.Id) return BadRequest();
        if(!ModelState.IsValid) return View(item);
        var model = await _repository.GetAsync(id);
        if(model == null) return NotFound();
        model.Title = item.Title;
        model.Description = item.Description;
        model.Image=item.Image;
        _repository.Update(model);
        await _repository.SaveAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Delete(int id)
    {
        var model = await _repository.GetAsync(id);
        if (model == null) return NotFound();
        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    [ActionName("Delete")]
    public async Task<IActionResult> DeletePost(int id)
    {
        var model = await _repository.GetAsync(id);
        if (model == null) return NotFound();
        _repository.Delete(model);
        await _repository.SaveAsync();
        return RedirectToAction(nameof(Index));
    }
}
