/*create table [Planes](
idPlan varchar(100) not null primary key, 
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

        public string idPlan { get; set; }

        public string nombre { get; set; }
        public string tipo { get; set; }
        public string descripcion { get; set; }

        public string codigoRiesgo { get; set; }
        public string categoria { get; set; }
        public DateTime fecha { get; set; }
        public char estado { get; set; }
        public string observaciones { get; set; }
    }
}
