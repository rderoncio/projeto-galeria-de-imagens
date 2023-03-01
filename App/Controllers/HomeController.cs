using App.Context;
using App.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class HomeController : Controller
{
    private readonly DatabaseContext _database;

    public HomeController(DatabaseContext database)
    {
        _database = database;
    }
    public IActionResult Index()
    {
        IEnumerable<GaleriaModel> galerias = _database.Galerias.Include(galeria => galeria.Imagens).AsNoTracking().ToList();
        return View(galerias);
    }
}