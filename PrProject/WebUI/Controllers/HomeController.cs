using DataAccess.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.ViewModels;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private AppDbContext _context;
    public HomeController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        //HttpContext.Session.SetString("name", "Metin");
        //Response.Cookies.Append("surname", "Iskenderov",new CookieOptions { MaxAge=TimeSpan.FromSeconds(30)});
        //CookieOptions opt=new CookieOptions()
        //{

        //};
        HomeViewModel homeVM = new()
        {
            SlideItems= _context.SlideItems.AsNoTracking(),
            ShippingItems= _context.ShippingItems.AsNoTracking(),
            Products = _context.Products.Take(8).AsNoTracking(),
        };
        return View(homeVM);
        #region ViewBag,Tempdata,Redirect
        //ViewBag.Name = "Metin";
        //ViewData["Surname"] = "Iskenderov";
        //TempData["Age"] = 5;
        //return RedirectToAction(nameof(Test));
        //return RedirectToAction("Test");
        //return Test();
        //return View(nameof(Test));
        #endregion
    }

    //public IActionResult Test()
    //{
    //    var s=HttpContext.Session.GetString("name");
    //    var c = Request.Cookies["surname"];
    //    return Json(s+" "+c);
    //}
}
