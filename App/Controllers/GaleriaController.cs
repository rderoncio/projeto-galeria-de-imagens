using Microsoft.AspNetCore.Mvc;

namespace App.Controllers;

public class GaleriaController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}