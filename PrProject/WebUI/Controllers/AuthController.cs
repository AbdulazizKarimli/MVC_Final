using Business.Interfaces;
using Core.DTOs;
using Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebUI.ViewModels;
using static WebUI.Utilities.Helper;

namespace WebUI.Controllers;

public class AuthController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IMailService _mailService;

    public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager
        , RoleManager<IdentityRole> roleManager, IMailService mailService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _mailService = mailService;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel registerViewModel)
    {
        if (!ModelState.IsValid) return View(registerViewModel);

        AppUser appUser = new()
        {
            Fullname = registerViewModel.Fullname,
            UserName = registerViewModel.Username,
            Email = registerViewModel.Email,
            IsActive = true
        };

        var identityResult = await _userManager.CreateAsync(appUser, registerViewModel.Password);
        if (!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View(registerViewModel);
        }

        await _userManager.AddToRoleAsync(appUser, RoleType.Member.ToString());

        return RedirectToAction(nameof(Login));
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel loginViewModel)
    {
        if (!ModelState.IsValid) return View(loginViewModel);

        var user = await _userManager.FindByEmailAsync(loginViewModel.UsernameOrEmail);
        if (user == null)
        {
            user = await _userManager.FindByNameAsync(loginViewModel.UsernameOrEmail);
            if (user == null)
            {
                ModelState.AddModelError("", "Username/Email or password incorrect");
                return View(loginViewModel);
            }
        }

        var signInResult = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, true);
        if (signInResult.IsLockedOut)
        {
            ModelState.AddModelError("", "get sonra gelersen!!");
            return View(loginViewModel);
        }
        if (!signInResult.Succeeded)
        {
            ModelState.AddModelError("", "Username/Email or password incorrect");
            return View(loginViewModel);
        }

        if (!user.IsActive)
        {
            ModelState.AddModelError("", "not found");
            return View(loginViewModel);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Logout()
    {
        if (User.Identity.IsAuthenticated)
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        return BadRequest();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel forgotPasswordViewModel)
    {
        if (!ModelState.IsValid) return View(forgotPasswordViewModel);

        var user = await _userManager.FindByEmailAsync(forgotPasswordViewModel.Email);
        if(user is null)
        {
            ModelState.AddModelError("Email", "Email not found");
            return View(forgotPasswordViewModel);
        }

        //https://localhost:7207/Auth/ResetPassword?userId=userId&token=resetPasswordToken

        string token = await _userManager.GeneratePasswordResetTokenAsync(user);

        string? link = Url.Action("ResetPassword", "Auth", new { userId = user.Id, token = token }, HttpContext.Request.Scheme);

        //await _mailService.SendEmailAsync(new MailRequestDto { ToEmail = user.Email, Subject = "Reset password", Body = $"<a href={link}>click me</a>"});

        //TempData["Message"] = "Zehmet olmasa mailinize baxin.";

        return Json(link);
    }

    public async Task<IActionResult> ResetPassword(string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel resetPasswordViewModel, string userId, string token)
    {
        if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(token)) return BadRequest();

        if (!ModelState.IsValid) return View(resetPasswordViewModel);

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) return NotFound();

        var identityResult = await _userManager.ResetPasswordAsync(user, token, resetPasswordViewModel.NewPassword);
        if(!identityResult.Succeeded)
        {
            foreach (var error in identityResult.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
            return View();
        }

        return RedirectToAction(nameof(Login));
    }

    public async Task<IActionResult> CreateRoles()
    {
        foreach (var role in Enum.GetValues(typeof(RoleType)))
        {
            if (!await _roleManager.RoleExistsAsync(role.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }

        return Json("Ok");
    }
}