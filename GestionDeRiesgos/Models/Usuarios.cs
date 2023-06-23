using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class Usuarios
    {

        [Key]
        public int idUsuario { get; set; }

        [Required(ErrorMessage ="Es necesario ingresar el nombre del usuario")]
        [Display(Name ="Nombre")]
        [StringLength(100, ErrorMessage ="El nombre ingresado es demasiado largo")]
        public string  nombre { get; set; }

        [Required(ErrorMessage = "Es necesario ingresar el correo del usuario")]
        [Display(Name = "Correo electronico")]
        [StringLength(100, ErrorMessage = "El correo que ingreso es demasiado largo")]
        [DataType(DataType.EmailAddress)]
        public string correo { get; set; }


        [Required(ErrorMessage = "Es necesario ingresar la contraseña del usuario")]
        [Display(Name = "Contraseña")]
        [StringLength(100, ErrorMessage = "La contraseña ingresada es demasiado larga")]
        //[DataType(DataType.Password)]
        public string password { get; set; }

        [Required(ErrorMessage = "Debe ingresar el rol")]
        [Display(Name = "Rol")]
        [StringLength(20, ErrorMessage = "El rol ingresado es demasiado largo")]
        public string rol { get; set; }

        /* idUsuario int not null primary key identity, 
 nombre varchar(100) not null,
 correo varchar(100) not null, 
 password varchar*/
    }
}
