using App.Context;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class GaleriaController : Controller
{
    private readonly DatabaseContext _database;
    private readonly IProcessadorImagem _processarImagemService;

    public GaleriaController(DatabaseContext database, IProcessadorImagem processarImagemService)
    {
        _database = database;
        _processarImagemService = processarImagemService;
    }
    public IActionResult Index()
    {
        return View(_database.Galeria.AsNoTracking().ToList());
    }
}