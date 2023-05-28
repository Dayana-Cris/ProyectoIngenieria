using System.ComponentModel.DataAnnotations;

namespace GestionDeRiesgos.Models
{
    public class Riesgos
    {
        [Key]

        public string codigoRiesgo { get; set; }

        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string probabilidad { get; set; }

        public int impacto { get; set; }
        public string categoria { get; set; }
        public DateTime fecha { get; set; }
        public char estado { get; set; }
        public string observaciones { get; set; }
    }
}
