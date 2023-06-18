using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
                if (ListaP.Count < 1)
                {
                    TempData["MensajeLista"] = "No existe ningún usuario para esta búsqueda";
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
        public async Task<IActionResult> Create([Bind("idPlan, nombre,tipo, descripcion,codigoRiesgo," +
            "fecha, estado, observaciones")] Planes  planes)
        {
            planes.categoria = "Vacio";
            if (planes.idPlan != null && planes.nombre != null && planes.tipo != null && planes.descripcion != null && planes.codigoRiesgo != null  
                && planes.fecha != null && planes.estado != null && planes.observaciones != null )
            {
                var listaAbrev = contexto.AbrevPlanes.ToList();

                var newList = listaAbrev.Where(x => x.abreviacion == planes.idPlan);

                if (newList.Any())
                {
                    planes.categoria = newList.First().categoria;
                }
                else
                {
                    TempData["ErrorMessage"] = "La lista de categorías está vacía.";
                }

                if (newList.Count()>0)
                {
                    var listaP = contexto.Planes.ToList();

                    var NewList = listaP.Where(x => x.idPlan.StartsWith(planes.idPlan));

                    listaP = NewList.ToList();


                    if (listaP.Count()>0)
                    {
                        string[] id = listaP.Last().idPlan.Split(" ");

                        var numero = Convert.ToInt32(id[1]);

                        planes.idPlan = planes.idPlan + " "+ (numero+1);
                    }
                    else
                    {
                        planes.idPlan = planes.idPlan + " " + 1;
                    }
                    contexto.Add(planes);
                    await contexto.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensajeCodigo"] = "No existe esa abreviacion de plan";
                    return View(planes);
                }


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

        public IActionResult Abreviaciones()
        {
            return View(contexto.AbrevPlanes.ToList());
        }

        public async Task<IActionResult> CreateAbrev([Bind("abreviacion,categoria")] AbrevPlanes abrevPlanes)
        {
            if (ModelState.IsValid)
            {
                contexto.Add(abrevPlanes);
                await contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(abrevPlanes);
            }
        }

        //[HttpGet]
        //public async Task<IActionResult> EditAbrev(string ? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    var planes = await contexto.AbrevPlanes.FindAsync(Id);

        //    if (planes == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(planes);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditAbrev(string Id, [Bind("abreviacion, categoria")] AbrevPlanes planes)
        //{
        //    if (Id != planes.abreviacion)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        contexto.Update(planes);

        //        await contexto.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View(planes);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> DeleteAbrev(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var planes = await contexto.AbrevPlanes.FindAsync(Id);

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
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAbrev")]
        public async Task<IActionResult> DelAbrev(string Id)
        {
            var planes = await contexto.AbrevPlanes.FindAsync(Id);


            var ListaP = contexto.Planes.ToList();
            //Lista de riesgos que coincidan con la abreviacion
            var NewList = ListaP.Where(x => x.idPlan.StartsWith(Id));

            if (NewList.Any())
            {
                TempData["MensajeDelete"] = "El codigo del riesgo no se puede eliminar por que ya existen riesgos asociados a este";
                return View(planes);
            }
            else
            {
                contexto.AbrevPlanes.Remove(planes);

                await contexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }


            
        }
    }
}
