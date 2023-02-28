using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace App.Models;

public class ImagemModel
{
    [Key]
    [Display(Name = "Código")]
    public int IdImagem { get; set; }

    [Required]
    [Display(Name = "Titulo da Imagem")]
    public string Titulo { get; set; }

    public int IdGaleria { get; set; }

    [ForeignKey("IdGaleria")]
    public GaleriaModel Galeria { get; set; }

    [NotMapped, Required(ErrorMessage = "Imagem não eviada.")]
    [Display(Name = "Arquivo da Imagem")]
    public IFormFile ArquivoImagem { get; set; }

    [NotMapped]
    public string CaminhoImagem { get { return Path.Combine($"\\img\\", IdImagem.ToString("D6" + ".webp")); } }
}
