using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class AbrevRiesgos
    {
        [Key]
        [Required(ErrorMessage = "Es necesario ingresar ID del riesgo")]
        [Display(Name = "ID")]
        public string abreviacion { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la categoría del riesgo")]
        [Display(Name = "Categoría")]
        [StringLength(100, ErrorMessage = "La categoría que se ingreso es demasiado larga")]
        public string categoria { get; set; }
    }
}
