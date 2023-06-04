using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeRiesgos.Controllers
{
    public class RiesgosController : Controller
    {
        //Variable que permite manejar la referencia del contexto 
        private readonly Contexto contexto;
        public static int opcion;

        public RiesgosController(Contexto context)
        {
            contexto = context;
        }
        //Es el primer metodo que se ejecuta en el controlador
        public IActionResult Index(int id, string buscar, string idBusqueda, string nombreBusqueda,
            string probabilidad,  string restaurar)
        {
            if(id == 1)
            {
                TempData["Mensaje"] = "opcion1";
            }
            if (id == 2)
            {
                TempData["Mensaje"] = "opcion2";
            }
            if(id == 3)
            {
                TempData["Mensaje"] = "opcion3";
            }

            var ListaP = contexto.Riesgos.ToList();
            if (buscar != null)
            {
                if (idBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.codigoRiesgo.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (nombreBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.nombre.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (probabilidad != null)
                {
                    var NewList = ListaP.Where(x => x.probabilidad.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (restaurar != null)
                {
                    ListaP = contexto.Riesgos.ToList();
                }
            }

            return View(ListaP);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("codigoRiesgo, nombre, descripcion, probabilidad, impacto, categoria," +
            "fecha, estado, observaciones")] Riesgos  riesgos)
        {
            if (ModelState.IsValid)
            {
                contexto.Add(riesgos);
                await contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(riesgos);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var riesgos = await contexto.Riesgos.FindAsync(Id);

            if (riesgos == null)
            {
                return NotFound();
            }

            return View(riesgos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, [Bind("codigoRiesgo, nombre, descripcion, probabilidad, impacto, categoria," +
            "fecha, estado, observaciones")] Riesgos  riesgos)
        {
            if (Id != riesgos.codigoRiesgo)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                contexto.Update(riesgos);

                await contexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(riesgos);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var riesgos = await contexto.Riesgos.FindAsync(Id);

            if (riesgos == null)
            {
                return NotFound();
            }
            else
            {
                return View(riesgos);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EliminarRiesgo(string Id)
        {
            var riesgos = await contexto.Riesgos.FindAsync(Id);

            contexto.Riesgos.Remove(riesgos);

            await contexto.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var riesgos = await contexto.Riesgos.FindAsync(Id);

            if (riesgos == null)
            {
                return NotFound();
            }

            return View(riesgos);
        }
    }
}
