using System.ComponentModel.DataAnnotations;

namespace App.Models;

public class GaleriaModel
{
    [Key]
    [Display(Name = "Código")]
    public int IdGaleria { get; set; }

    [Required]
    [Display(Name = "Titulo da Galeria")]
    public string Titulo { get; set; }

    public ICollection<ImagemModel> Imagens { get; set; } 
}
