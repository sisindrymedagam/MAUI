using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using YTShorts.Web.Data;
using YTShorts.Web.Entities;

namespace YTShorts.Web.Controllers;

public class AccountController(ApplicationDbContext context) : Controller
{
    public IActionResult Index()
    {
        return RedirectToActionPermanent("Login");
    }

    [HttpGet]
    public IActionResult Login()
    {
        ViewData["Title"] = "Login";
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(string email, string password)
    {
        ViewData["Title"] = "Login";
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
        {
            ModelState.AddModelError("", "Email and password are required.");
            return View();
        }
        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
        {
            ModelState.AddModelError("", "Invalid email or password.");
            return View();
        }
        List<Claim> claims = new()
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Email, user.Email)
        };
        ClaimsIdentity claimsIdentity = new(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        AuthenticationProperties authProperties = new()
        {
            IsPersistent = true
        };
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        return RedirectToAction("Index", "Shorts");
    }

    //[HttpGet]
    //public IActionResult Register()
    //{
    //    ViewData["Title"] = "Register";
    //    return View();
    //}

    //[HttpPost]
    //[ValidateAntiForgeryToken]
    //public async Task<IActionResult> Register(string name, string email, string password, string confirmPassword)
    //{
    //    ViewData["Title"] = "Register";
    //    if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
    //    {
    //        ModelState.AddModelError("", "All fields are required.");
    //        return View();
    //    }
    //    if (password != confirmPassword)
    //    {
    //        ModelState.AddModelError("", "Passwords do not match.");
    //        return View();
    //    }
    //    if (await context.Users.AnyAsync(u => u.Email == email))
    //    {
    //        ModelState.AddModelError("", "Email already exists.");
    //        return View();
    //    }
    //    var user = new User
    //    {
    //        Name = name,
    //        Email = email,
    //        Password = BCrypt.Net.BCrypt.HashPassword(password),
    //        CreatedOn = DateTime.UtcNow
    //    };
    //    context.Users.Add(user);
    //    await context.SaveChangesAsync();
    //    return RedirectToAction("Login");
    //}

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login");
    }
}
