using App.Context;
using App.Models;
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
        return View(_database.Galerias.AsNoTracking().ToList());
    }

    [HttpGet]
    public IActionResult Cadastrar()
    {
        return View();
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Cadastrar(GaleriaModel galeria)
    {
        if(!ModelState.IsValid)
            return View();
    
        _database.Galerias.Add(galeria);
        _database.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Excluir(int? id)
    {
        if(id is null)
            return NotFound();

        GaleriaModel galeria = _database.Galerias.Find(id.Value);

        if(galeria is null)
            return NotFound();

        return View(galeria);

    }

    [HttpPost, ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    public IActionResult ExcluirPost(int? id)
    {
        if(!ModelState.IsValid)
            return View();

        GaleriaModel galeria = _database.Galerias.Find(id.Value);

        if(galeria is null)
            return NotFound();
    
        _database.Galerias.Remove(galeria);
        _database.SaveChanges();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Alterar(int? id)
    {
        if(id is null)
            return NotFound();

        GaleriaModel galeria = _database.Galerias.Find(id.Value);

        if(galeria is null)
            return NotFound();

        return View(galeria);

    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Alterar(GaleriaModel galeria)
    {
        if(!ModelState.IsValid)
            return View();
    
        _database.Entry(galeria).State = EntityState.Modified;
        _database.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}