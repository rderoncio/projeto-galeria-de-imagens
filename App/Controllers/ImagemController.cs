using App.Context;
using App.Enums;
using App.Models;
using App.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Controllers;

public class ImagemController : Controller
{
    private readonly DatabaseContext _database;
    private readonly IProcessadorImagem _processarImagemService;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ImagemController(DatabaseContext database, IProcessadorImagem processarImagemService, IWebHostEnvironment webHostEnvironment)
    {
        _database = database;
        _processarImagemService = processarImagemService;
        _webHostEnvironment = webHostEnvironment;
    }

    public IActionResult Index(int? id)
    {
        if (id is null)
            return NotFound();

        GaleriaModel galeria = _database.Galerias.Find(id.Value);

        if (galeria is null)
            return NotFound();

        _database.Entry(galeria).Collection(g => g.Imagens).Load();
        ViewBag.IdGaleria = id.Value;
        ViewBag.TituloGaleria = galeria.Titulo;

        return View(galeria.Imagens.ToList());
    }

    [HttpGet]
    public IActionResult Cadastrar(int? id)
    {
        if (id is null)
            return NotFound();

        GaleriaModel galeria = _database.Galerias.Find(id.Value);

        if (galeria is null)
            return NotFound();

        ImagemModel imagem = new() { IdGaleria = galeria.IdGaleria };

        return View(imagem);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Cadastrar(ImagemModel imagem)
    {
        if (!ModelState.IsValid)
            return View(imagem);

        _database.Add(imagem);

        if (_database.SaveChanges() > 0)
        {
            string caminhoArquivoImagem = ObterCaminhoImagem("\\img\\", imagem.IdImagem, "webp");
            _processarImagemService.SalvarUploadImagemAsync(caminhoArquivoImagem, imagem.ArquivoImagem).Wait();
        }

        return RedirectToAction(nameof(Index), "Imagem", new { id = imagem.IdGaleria });
    }

    [HttpGet]
    public IActionResult Alterar(int? id)
    {
        if (id is null)
            return NotFound();

        ImagemModel imagem = _database.Imagens.Find(id.Value);

        if (imagem is null)
            return NotFound();

        return View(imagem);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Alterar(ImagemModel imagem)
    {
        ModelState.Remove("ArquivoImagem");

        if (!ModelState.IsValid)
            return View(imagem);

        _database.Entry(imagem).State = EntityState.Modified;

        if (!(_database.SaveChanges() > 0))
            return NotFound();

        string caminhoArquivoImagem = ObterCaminhoImagem("\\img\\", imagem.IdImagem, "webp");
        _processarImagemService.SalvarUploadImagemAsync(caminhoArquivoImagem, imagem.ArquivoImagem).Wait();

        return RedirectToAction(nameof(Index), new { id = imagem.IdGaleria });
    }

    [HttpGet]
    public IActionResult Excluir(int? id)
    {
        if (id is null)
            return NotFound();

        ImagemModel imagem = _database.Imagens.Find(id.Value);

        if (imagem is null)
            return NotFound();

        return View(imagem);
    }

    [HttpPost, ValidateAntiForgeryToken]
    [ActionName("Excluir")]
    public IActionResult ExcluirPost(int? id)
    {
        if (!id.HasValue)
            return NotFound();

        ImagemModel imagem = _database.Imagens.Find(id.Value);

        if (imagem is null)
            return NotFound();

        _database.Remove(imagem);

        if (_database.SaveChanges() > 0)
        {
            string caminhoArquivoImagem = ObterCaminhoImagem("\\img\\", imagem.IdImagem, "webp");
            _processarImagemService.ExcluirImagemAsync(caminhoArquivoImagem).Wait();
        }

        return RedirectToAction(nameof(Index), "Imagem", new { id = imagem.IdGaleria });
    }

    [HttpGet]
    public IActionResult AplicarEfeito(int? id)
    {
        if (id is null)
            return NotFound();

        ImagemModel imagem = _database.Imagens.Find(id.Value);

        if (imagem is null)
            return NotFound();

        return View(nameof(Efeitos), imagem);
    }

    [HttpPost]
    public IActionResult AplicarEfeito(int id, string efeito)
    {
        string caminhoArquivoImagem = ObterCaminhoImagem("\\img\\", id, "webp");

        switch (efeito)
        {
            case "rdireita":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.RotacionarDireita).Wait();
                break;

            case "resquerda":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.RotacionarEsquerda).Wait();
                break;

            case "ihorizontal":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.InverterHorizontal).Wait();
                break;

            case "ivertical":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.InverterVertical).Wait();
                break;
                
            case "ecinza":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.EscalaDeCinza).Wait();
                break;

            case "sepia":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.Sepia).Wait();
                break;

            case "negativo":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.Negativo).Wait();
                break;

            case "desfoque":
                _processarImagemService.AplicarEfeitoImagemAsync(caminhoArquivoImagem, EfeitoImagem.Desfoque).Wait();
                break;
        }

        return RedirectToAction(nameof(AplicarEfeito), new { id = id });
    }

    public IActionResult Efeitos()
    {
        return View();
    }

    #region Metodos privados
    private string ObterCaminhoImagem(string pastaImagens, int IdImagem, string extensaoImagem)
    {
        string caminhoPastaImagens = _webHostEnvironment.WebRootPath + pastaImagens; // <APPDIR>/wwwroot/imagens
        string nomeArquivo = $"{IdImagem:D6}.{extensaoImagem}";

        return Path.Combine(caminhoPastaImagens, nomeArquivo); // <APPDIR>/wwwroot/imagens/000001.webp
    }
    #endregion
}
