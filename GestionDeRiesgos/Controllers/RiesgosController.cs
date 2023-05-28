using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeRiesgos.Controllers
{
    public class RiesgosController : Controller
    {
        //Variable que permite manejar la referencia del contexto 
        private readonly Contexto contexto;

        public RiesgosController(Contexto context)
        {
            contexto = context;
        }
        //Es el primer metodo que se ejecuta en el controlador
        public IActionResult Index()
        {
            return View(contexto.Riesgos.ToList());
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
