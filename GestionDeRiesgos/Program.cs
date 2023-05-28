using GestionDeRiesgos;

public class Program
{

    //Metodo principal que se encarga de ejecutar la logica de la App
    public static void Main(String[] arg)
    {
        //Se utiliza el metodo y se realiza la ejecucion de la aplicacion
        CreateHostBuilder(arg).Build().Run();
    }

    //Metodo encargado de crear la instancia de ejecucion de nuestra App
    //Recibe los argumentos como parametros
    public static IHostBuilder CreateHostBuilder(String[] arg)
    {

        return Host.CreateDefaultBuilder(arg).ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });

    }
}