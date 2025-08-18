using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using YTShorts.Web.Data;
using YTShorts.Web.Entities;
using YTShorts.Web.Models;

namespace YTShorts.Web.Controllers;

public class AccountController(ApplicationDbContext context, IConfiguration configuration) : Controller
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
    public async Task<IActionResult> Login(LoginDto model)
    {
        ViewData["Title"] = "Login";
        if (!ModelState.IsValid)
        {
            return View();
        }

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
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

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Token([FromBody] LoginDto model)
    {
        if (!ModelState.IsValid)
        {
            return Unauthorized("Email and password are required.");
        }

        User? user = await context.Users.FirstOrDefaultAsync(u => u.Email == model.Email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
        {
            return Unauthorized("Invalid email or password.");
        }
        List<Claim> claims = new()
        {
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.Name),
            new (ClaimTypes.Email, user.Email)
        };
        var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: configuration["Jwt:Issuer"],
            audience: configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddYears(1),
            signingCredentials: creds);

        return Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
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
