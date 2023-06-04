using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeRiesgos.Controllers
{
    public class PlanesController : Controller
    {  //Variable que permite manejar la referencia del contexto 
        private readonly Contexto contexto;

        public PlanesController(Contexto context)
        {
            contexto = context;
        }
        //Es el primer metodo que se ejecuta en el controlador
        public IActionResult Index(int id, string buscar, string idBusqueda, string nombreBusqueda, 
            string tipoBusqueda, string riesgoAsociado, string restaurar)
        {
            if (id == 1)
            {
                TempData["Mensaje"] = "opcion1";
            }
            if (id == 2)
            {
                TempData["Mensaje"] = "opcion2";
            }
            if (id == 3)
            {
                TempData["Mensaje"] = "opcion3";
            }
            var ListaP = contexto.Planes.ToList();
            if (buscar != null)
            {
                if (idBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.idPlan.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (nombreBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.nombre.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (tipoBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.tipo.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (riesgoAsociado != null)
                {
                    var NewList = ListaP.Where(x => x.codigoRiesgo.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }

                if (restaurar != null)
                {
                    ListaP = contexto.Planes.ToList();
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
        public async Task<IActionResult> Create([Bind("idPlan, nombre,tipo, descripcion,codigoRiesgo, categoria," +
            "fecha, estado, observaciones")] Planes  planes)
        {
            if (ModelState.IsValid)
            {
                contexto.Add(planes);
                await contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(planes);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var planes = await contexto.Planes.FindAsync(Id);

            if (planes == null)
            {
                return NotFound();
            }

            return View(planes);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string Id, [Bind("idPlan, nombre,tipo, descripcion,codigoRiesgo, categoria," +
            "fecha, estado, observaciones")] Planes  planes)
        {
            if (Id != planes.idPlan)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                contexto.Update(planes);

                await contexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(planes);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(string Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var planes = await contexto.Planes.FindAsync(Id);

            if (planes == null)
            {
                return NotFound();
            }
            else
            {
                return View(planes);
            }
        }

        [HttpPost]
        [ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> EliminarPlan(string Id)
        {
            var planes = await contexto.Planes.FindAsync(Id);

            contexto.Planes.Remove(planes);

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

            var planes = await contexto.Planes.FindAsync(Id);

            if (planes == null)
            {
                return NotFound();
            }

            return View(planes);
        }
    }
}
