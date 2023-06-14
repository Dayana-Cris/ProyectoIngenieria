using Microsoft.EntityFrameworkCore;


namespace GestionDeRiesgos.Data
{
    public class Contexto : DbContext
    {

        public Contexto(DbContextOptions<Contexto> options) : base(options)
        {

        }

        //Se construye el contexto para el objeto Autor

        public DbSet<Models.Usuarios> Usuarios { get; set; }
        public DbSet<Models.Riesgos> Riesgos { get; set; }

        public DbSet<Models.Planes> Planes { get; set; }
        public DbSet<Models.AbrevRiesgos> AbrevRiesgos { get; set; }
        public DbSet<Models.AbrevPlanes> AbrevPlanes { get; set; }

    }//Cierre class
}
