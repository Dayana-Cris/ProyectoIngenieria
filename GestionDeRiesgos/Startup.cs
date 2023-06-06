using GestionDeRiesgos.Data;


using Microsoft.EntityFrameworkCore;
namespace GestionDeRiesgos
{
   
        public class Startup
        {
            public IConfiguration Configuration { get; set; }

            //Constructor con parametros, recibe el objeto de configuracion 
            public Startup(IConfiguration configuration)
            {
                this.Configuration = configuration;
            }

            //Metodos 

            public void ConfigureServices(IServiceCollection services)
            {
                //Configuracion de la autenticacion
                services.AddAuthentication("CookieAuthentication")
                    .AddCookie("CookieAuthentication", config => {
                        config.Cookie.Name = "UserLoginCookie";
                        config.LoginPath = "/Usuarios/login";
                    });

                //Se agregan los controllers con sus views al servicio 
                services.AddControllersWithViews();

                //Se agrega el  contexto, ademas el string de conexion 
                services.AddDbContext<Contexto>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("StringConexion")));

                //Permite agregar la instancia del contexto a nuestra configuracion
                services.AddScoped<Contexto>();
            }//Cierre del metodo Configure Services

            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                else
                {
                    app.UseExceptionHandler("/Home/Error");
                    app.UseHsts();
                }
                //Permite utilizar url con protocolo Https
                app.UseHttpsRedirection();

                //Permite utilizar archivos fisicos como imagenes - documentos 
                app.UseStaticFiles();

                //Permite manejar url con enrutamiento
                app.UseRouting();

                //Se indica que nuestra aplicacion maneja autenticacion 
                app.UseAuthentication();

                //Permite indicar el uso de esquema de autenticacion
                app.UseAuthorization();

                //Se indica la ruta por default
                app.UseEndpoints(endpoints => endpoints.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{Id?}"));
                //Esto permitirá que el servidor web sirva los archivos estáticos, como imágenes, desde la carpeta "wwwroot".
                app.UseStaticFiles();

        }//Cierre del metodo configure
    }
}
