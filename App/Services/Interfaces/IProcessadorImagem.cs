using App.Enums;

namespace App.Services.Interfaces;

public interface IProcessadorImagem
{
    Task<bool> SalvarUploadImagemAsync(string caminhoImagem, IFormFile imagem);

    Task<bool> ExcluirImagemAsync(string caminhoImagem);

    Task<bool> AplicarEfeitoImagemAsync(string caminhoImagem, EfeitoImagem efeito);
}
