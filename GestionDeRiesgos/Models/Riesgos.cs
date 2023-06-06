using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class Riesgos
    {
        [Key]
        [Required(ErrorMessage = "Es necesario ingresar el codigo de riesgo")]
        [Display(Name = "Código de riesgo")]
        [StringLength(100, ErrorMessage = "El código ingresado es demasiado largo")]
        public string codigoRiesgo { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar el nombre del riesgo")]
        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "El nombre ingresado es demasiado largo")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la descripcion del riesgo")]
        [Display(Name = "Descripción")]
        [StringLength(500, ErrorMessage = "La descripcion que se ingreso es demasiado larga")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la probabilidad del riesgo")]
        [Display(Name = "Probabilidad")]
        [StringLength(100, ErrorMessage = "La probabilida que ingreso es demasiado larga")]
        public string probabilidad { get; set; }


        [Required(ErrorMessage = "Es necesario ingresar el impacto del riesgo")]
        [Display(Name = "Impacto")]
        public int impacto { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la categoría del riesgo")]
        [Display(Name = "Categoría")]
        [StringLength(100, ErrorMessage = "La categoría que se ingreso es demasiado larga")]
        public string categoria { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la fecha cuando se crea el riesgo")]
        [Display(Name = "Fecha de creación ")]
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar el estado del riesgo")]
        [Display(Name = "Estado")]
        public char estado { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar las observaciones correspondientes del riesgo")]
        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "El nombre ingresado es demasiado largo")]
        public string observaciones { get; set; }

    }
}
