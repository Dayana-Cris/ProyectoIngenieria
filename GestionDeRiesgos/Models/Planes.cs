/*create table [Planes](
idPlan varchar(100) not null  , 
nombre varchar (100) not null,
tipo varchar (100) not null,
descripcion varchar (500) not null,
codigoRiesgo int  not null,
categoria varchar (100) not null,
fecha date not null,
estado char not null,
observaciones varchar (500) not null
)
go*/
using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class Planes
    {
        [Key]
        [Required(ErrorMessage = "Es necesario ingresar id del plan")]
        [Display(Name = "ID")]
        [StringLength(100, ErrorMessage = "El id que se ingreso es demasiado largo")]
        public string idPlan { get; set; }


        [Required(ErrorMessage = "Es necesario ingresar el nombre del plan")]
        [Display(Name = "Nombre")]
        [StringLength(100, ErrorMessage = "El nombre ingresado es demasiado largo")]
        public string nombre { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar el tipo del usuario")]
        [Display(Name = "Tipo")]
        [StringLength(100, ErrorMessage = "El tipo ingresado es demasiado largo")]
        public string tipo { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la descripción del plan")]
        [Display(Name = "Descripción")]
        [StringLength(100, ErrorMessage = "La descripción ingresada es demasiado largo")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar codigo de riesgo al que corresponde el plan")]
        [Display(Name = "Código de riesgo")]
        [StringLength(100, ErrorMessage = "El código de riesgo ingresado es demasiado largo")]
        public string codigoRiesgo { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la categoría del plan")]
        [Display(Name = "Categoría")]
        [StringLength(100, ErrorMessage = "La categoría ingresada es demasiado larga")]
        public string categoria { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar la fecha cuando se crea el plan")]
        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar el estado del plan")]
        [Display(Name = "Estado")]
        public char estado { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar las observaciones correspondientes del plan")]
        [Display(Name = "Observaciones")]
        [StringLength(500, ErrorMessage = "El nombre ingresado es demasiado largo")]
        public string observaciones { get; set; }
    }
}
