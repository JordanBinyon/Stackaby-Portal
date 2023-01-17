using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Stackaby.Interfaces;
using Stackaby.Models.Portal.User;
using Stackaby.Portal.Models;

namespace Stackaby.Portal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;

    public HomeController(ILogger<HomeController> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public IActionResult Index()
    {
        return View();
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

    public IActionResult Register()
    {
        return View(new RegisterModel());
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterModel model)
    {
        // TODO Add validation
        if (!ModelState.IsValid) 
            return View(model);
        
        await _userService.Register(model.FirstName, model.LastName, model.Email, model.Password);

        return View("Index");
    }
}
