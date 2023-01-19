using Microsoft.AspNetCore.Mvc;
using Stackaby.Interfaces;
using Stackaby.Models.Portal.User;
using Stackaby.Portal.Services;

namespace Stackaby.Portal.Controllers;

[Route("Auth/[action]")]
public class AuthenticationController : Controller
{
    private readonly IUserService _userService;
    private readonly IAuthenticationService _authenticationService;

    public AuthenticationController(IUserService userService, IAuthenticationService authenticationService)
    {
        _userService = userService;
        _authenticationService = authenticationService;
    }
    
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginModel model) 
    {
        // TODO Add validation
        if (!ModelState.IsValid) 
            return View(model);

        await _authenticationService.SignIn(model.Email, model.Password, true);

        return RedirectToAction("Privacy", "Home");
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

        return RedirectToAction("Index", "Home");
    }
    
    public async Task<IActionResult> SignOut()
    {
        await _authenticationService.SignOut();

        return RedirectToAction("Index", "Home");
    }
}