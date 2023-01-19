using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stackaby.Interfaces;
using Stackaby.Models.Portal.User;
using Stackaby.Portal.Models;
using Stackaby.Portal.Services;

namespace Stackaby.Portal.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public HomeController(ILogger<HomeController> logger, IUserService userService, IAuthenticationService authenticationService)
    {
        _logger = logger;
        _userService = userService;
        _authenticationService = authenticationService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Index(LoginModel model) 
    {
        // TODO Add validation
        if (!ModelState.IsValid) 
            return View(model);

        await _authenticationService.SignIn(model.Email, model.Password, true);

        return RedirectToAction("Privacy");
    }

    [Authorize]
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

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> SignOut()
    {
        await _authenticationService.SignOut();

        return RedirectToAction("Index");
    }
}
