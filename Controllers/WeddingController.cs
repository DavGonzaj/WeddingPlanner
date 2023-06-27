using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WeddingPlanner.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace WeddingPlanner.Controllers;

[SessionCheck]
public class WeddingController : Controller
{
    private readonly ILogger<WeddingController> _logger;
    //conncection to our database "db"
    private MyContext db;

    public WeddingController(ILogger<WeddingController> logger, MyContext context)
    {
        _logger = logger;
        db = context;
    }
    [HttpGet("weddings")]
    public IActionResult AllWeddings()
    {
        List<Wedding> allWeddings = db.Weddings.Include(wedding => wedding.Planner).Include(wedding => wedding.Signups).ToList();
        return View("AllWeddings", allWeddings);
    }

    [HttpGet("weddings/new")]
    public IActionResult NewWedding()
    {
        return View("New");
    }

    [HttpPost("weddings/create")]
    public IActionResult CreateWedding(Wedding newWedding)
    {
        if (!ModelState.IsValid)
        {
            //send user back to form so they can see and fix erros
            return View("New");
        }

        newWedding.UserId = (int)HttpContext.Session.GetInt32("UUID");

        db.Weddings.Add(newWedding);
        //db doesn't update until we run save changes
        //after SaveChanges, our newPost object now has it's PostID updated from db auto generated id
        db.SaveChanges();

        return RedirectToAction("AllWeddings");
    }

    // view one
    [HttpGet("weddings/{weddingId}")]
    public IActionResult ViewOne(int weddingId)
    {
        Wedding? oneWedding = db.Weddings.Include(wedding => wedding.Planner).Include(wedding => wedding.Signups).ThenInclude(signup => signup.User).FirstOrDefault(wedding => wedding.WeddingId == weddingId);
        if (oneWedding == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View("Details", oneWedding);
    }
    //edit one
    [HttpGet("weddings/{weddingId}/edit")]
    public IActionResult EditWedding(int weddingId)
    {
        Wedding? oneWedding = db.Weddings.FirstOrDefault(wedding => wedding.WeddingId == weddingId);
        if (oneWedding == null)
        {
            return RedirectToAction("AllWeddings");
        }
        return View("Edit", oneWedding);
    }

    //update one
    [HttpPost("/weddings/{weddingId}/update")]
    public IActionResult UpdateWedding(int weddingId, Wedding editedWedding)
    {
        if (!ModelState.IsValid)
        {
            return EditWedding(weddingId);
        }
        Wedding? dbWedding = db.Weddings.FirstOrDefault(wedding => wedding.WeddingId == weddingId);

        if (dbWedding == null)
        {
            return RedirectToAction("AllWeddings");
        }

        dbWedding.WedOne = editedWedding.WedOne;
        dbWedding.WedTwo = editedWedding.WedTwo;
        dbWedding.WedDate = editedWedding.WedDate;
        dbWedding.WedAddress = editedWedding.WedAddress;
        dbWedding.Updated_at = DateTime.Now;

        db.SaveChanges();

        return RedirectToAction("ViewOne", new { weddingId = weddingId });

    }
    // delete
    [HttpPost("weddings/{weddingId}/delete")]
    public IActionResult DeleteWedding(int weddingId)
    {
        Wedding? wedding = db.Weddings.FirstOrDefault(wedding => wedding.WeddingId == weddingId);
        if (wedding != null)
        {
            db.Weddings.Remove(wedding);
            db.SaveChanges();
        }
        return RedirectToAction("AllWeddings");
    }

    //counts
    [HttpPost("weddings/{weddingId}/signup")]
    public IActionResult Signup(int weddingId)
    {
        UserWeddingSignup? existingSignup = db.UserWeddingSignups.FirstOrDefault(signup => signup.UserId == HttpContext.Session.GetInt32("UUID") && signup.WeddingId == weddingId);
        if (existingSignup == null)
        {
            UserWeddingSignup newSignup = new UserWeddingSignup()
            {
                WeddingId = weddingId,
                UserId = (int)HttpContext.Session.GetInt32("UUID")
            };

            db.UserWeddingSignups.Add(newSignup);
        }
        else
        {
            db.UserWeddingSignups.Remove(existingSignup);
        }
        db.SaveChanges();
        return RedirectToAction("AllWeddings");
    }
}

// Name this anything you want with the word "Attribute" at the end
public class SessionCheckAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        // Find the session, but remember it may be null so we need int?
        int? userId = context.HttpContext.Session.GetInt32("UUID");
        // Check to see if we got back null
        if (userId == null)
        {
            // Redirect to the Index page if there was nothing in session
            // "Home" here is referring to "HomeController", you can use any controller that is appropriate here
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
