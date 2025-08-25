using Loop.Web.Data;
using Loop.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loop.Web.Controllers;

public class UserController(ApplicationDbContext context) : Controller
{
    public async Task<IActionResult> Index()
    {
        var users = await context.Users.Select(s => new UserListDto
        {
            Id = s.Id,
            Email = s.Email,
            Name = s.Name
        }).ToListAsync();
        return View(users);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RegisterDto register)
    {
        if (!ModelState.IsValid)
        {
            return View(register);
        }

        register.Email = register.Email.ToLower();

        var user = context.Users.FirstOrDefault(u => u.Email == register.Email);

        if (user != null)
        {
            ModelState.AddModelError("Email", "Email is already registered.");
            return View(register);
        }

        user = new Entities.User
        {
            Name = register.Name,
            Email = register.Email,
            Password = BCrypt.Net.BCrypt.HashPassword(register.Password),
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System"
        };

        context.Users.Add(user);
        await context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
}
