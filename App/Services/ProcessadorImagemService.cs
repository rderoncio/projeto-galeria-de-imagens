using App.Enums;
using App.Services.Interfaces;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace App.Services;

public class ProcessadorImagemService : IProcessadorImagem
{
    public async Task<bool> AplicarEfeitoImagemAsync(string caminhoImagem, EfeitoImagem efeito)
    {
        FileStream fileStream = new FileStream(caminhoImagem, FileMode.Open, FileAccess.Read);
        Image imagem = await Image.LoadAsync(fileStream);
        fileStream.Close();

        switch (efeito)
        {
            case EfeitoImagem.RotacionarDireita:
                RotacionarDireita(imagem);
                break;

            case EfeitoImagem.RotacionarEsquerda:
                RotacionarEsquerda(imagem);
                break;

            case EfeitoImagem.InverterHorizontal:
                InverterHorizontal(imagem);
                break;

            case EfeitoImagem.InverterVertical:
                InverterVertical(imagem);
                break;

            case EfeitoImagem.Sepia:
                AplicarSepia(imagem);
                break;

            case EfeitoImagem.EscalaDeCinza:
                AplicarEscalaDeCiza(imagem);
                break;

            case EfeitoImagem.Desfoque:
                AplicarDesfoque(imagem);
                break;

            case EfeitoImagem.Negativo:
                AplicarNegativo(imagem);
                break;
        }

        await imagem.SaveAsync(caminhoImagem);
        return true;
    }

    public async Task<bool> ExcluirImagemAsync(string caminhoImagem)
    {
        if (File.Exists(caminhoImagem))
        {
            try
            {
                File.Delete(caminhoImagem);
                return true;
            }
            catch (IOException)
            {
                return await Task.FromResult(false);
            }
        };

        return await Task.FromResult(false);
    }

    public async Task<bool> SalvarUploadImagemAsync(string caminhoImagem, IFormFile imagem)
    {
        if (imagem is null)
            return false;

        MemoryStream memoryStream = new();
        await imagem.CopyToAsync(memoryStream);
        memoryStream.Position = 0;

        return await SalvarImagemComoWebpAsync(caminhoImagem, memoryStream, true);
    }

    #region Métodos privados
    private static async Task<bool> SalvarImagemComoWebpAsync(string caminhoImagem, MemoryStream memoryStream, bool quadrada = true)
    {
        Image imagem = await Image.LoadAsync(memoryStream);
        string extensao = GetExtension(caminhoImagem);

        // Redimenciona tamanho da imagem
        if (quadrada)
        {
            Size tamanho = imagem.Size();
            int ladoMenor = tamanho.Height < tamanho.Width ? tamanho.Height : tamanho.Width;

            imagem.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(ladoMenor, ladoMenor),
                Mode = ResizeMode.Crop
            }).BackgroundColor(new Rgba32(255, 255, 255, 0)));
        }

        caminhoImagem = caminhoImagem.Replace(extensao, ".webp");
        await imagem.SaveAsWebpAsync(caminhoImagem);

        return true;
    }

    private static string GetExtension(string caminhoImagem)
    {
        return caminhoImagem.Substring(caminhoImagem.LastIndexOf('.')).ToLower();
    }

    private void RotacionarDireita(Image image)
    {
        image.Mutate(img => img.Rotate(90));
    }

    private void RotacionarEsquerda(Image image)
    {
        image.Mutate(img => img.Rotate(-90));
    }

    private void InverterVertical(Image image)
    {
        image.Mutate(img => img.Flip(FlipMode.Vertical));
    }

    private void InverterHorizontal(Image image)
    {
        image.Mutate(img => img.Flip(FlipMode.Horizontal));
    }

    private void AplicarSepia(Image image)
    {
        image.Mutate(img => img.Sepia());
    }

    private void AplicarEscalaDeCiza(Image image)
    {
        image.Mutate(img => img.Grayscale());
    }

    private void AplicarDesfoque(Image image)
    {
        image.Mutate(img => img.GaussianBlur());
    }

    private void AplicarNegativo(Image image)
    {
        image.Mutate(img => img.Invert());
    }
    #endregion
}