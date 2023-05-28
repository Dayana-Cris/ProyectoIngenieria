using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class Usuarios
    {

        [Key]
        public int idUsuario { get; set; }

        [Required(ErrorMessage ="ADHd")]
        [Display(Name ="asdasd")]
        [StringLength(100, ErrorMessage ="adsdad")]
        public string  nombre { get; set; }

        [Required(ErrorMessage = "adsdad")]
        [Display(Name = "adsdad")]
        [StringLength(100, ErrorMessage = "adsdad")]
        [DataType(DataType.EmailAddress)]
        public string correo { get; set; }


        [Required(ErrorMessage = "adsdad")]
        [Display(Name = "adsdad")]
        [StringLength(100, ErrorMessage = "adsdad")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        /* idUsuario int not null primary key identity, 
 nombre varchar(100) not null,
 correo varchar(100) not null, 
 password varchar
        ANDO PROBANDO BYE*/
    }
}
