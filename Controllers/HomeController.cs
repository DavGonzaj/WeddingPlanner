using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Identity;
using WeddingPlanner.Models;

namespace WeddingPlanner.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    //conncection to our database "db"
    private MyContext db;

    public HomeController(ILogger<HomeController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }

    [HttpGet("")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetInt32("UUID") != null)
        {
            return RedirectToAction("Allweddings", "Wedding");
        }
        return View("Index");
    }

    [HttpPost("register")]
    public IActionResult Register(User newUser)
    {
        if (!ModelState.IsValid)
        {
            return Index();
        }

        PasswordHasher<User> hasBrowns = new PasswordHasher<User>();
        newUser.Password = hasBrowns.HashPassword(newUser, newUser.Password);

        db.Users.Add(newUser);
        db.SaveChanges();

        HttpContext.Session.SetInt32("UUID", newUser.UserId);
        HttpContext.Session.SetString("Name", newUser.Name); // Set the user's name in the session
        return RedirectToAction("AllWeddings", "Wedding");
    }

    [HttpPost("login")]
    public IActionResult Login(LoginUser loginUser)
    {
        if (!ModelState.IsValid)
        {
            return Index();
        }

        User? dbUser = db.Users.FirstOrDefault(user => user.Email == loginUser.LoginEmail);

        if (dbUser == null)
        {
            ModelState.AddModelError("Email", "not found");
            return Index();
        }

        PasswordHasher<LoginUser> hashBrowns = new PasswordHasher<LoginUser>();
        PasswordVerificationResult pwCompareResult = hashBrowns.VerifyHashedPassword(loginUser, dbUser.Password, loginUser.LoginPassword);

        if (pwCompareResult == 0)
        {
            //normally we won't be specific with errors but for demo reasons we are 
            //since malicious users can benefit from the specificity
            ModelState.AddModelError("LoginPassword", "invalid password");
        }

        HttpContext.Session.SetInt32("UUID", dbUser.UserId);
        HttpContext.Session.SetString("Name", dbUser.Name); // Set the user's name in the session
        return RedirectToAction("AllWeddings", "Wedding");
    }

    [HttpPost("logout")]

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}



