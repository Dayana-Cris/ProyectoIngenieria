using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class SeguridadRestablecer
    {
        public string Correo { get; set; }

        [Required(ErrorMessage = "Debe ingresar la password actual")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Debe ingresar la nueva password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Debe ingresar la Confirmacion de la nueva password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "La nueva clave y la confirmar no coinciden")]
        public string ConfirmarPassword { get; set; }
    }
}
