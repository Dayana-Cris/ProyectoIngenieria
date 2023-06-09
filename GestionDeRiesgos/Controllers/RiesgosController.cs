﻿using GestionDeRiesgos.Data;
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
            string probabilidad, string restaurar)
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
                if (ListaP.Count < 1)
                {
                    TempData["MensajeLista"] = "No existe ningún usuario para esta búsqueda";
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
        public async Task<IActionResult> Create([Bind("codigoRiesgo, nombre, descripcion, probabilidad, impacto," +
            "fecha, estado, observaciones")] Riesgos  riesgos)
        {
            riesgos.categoria = "Vacio";
            if (riesgos.codigoRiesgo != null && riesgos.nombre != null && riesgos.descripcion != null &&
                riesgos.probabilidad != null && riesgos.impacto != null && riesgos.fecha != null &&
                riesgos.estado != null && riesgos.observaciones != null)
            {
                
                riesgos.codigoRiesgo = riesgos.codigoRiesgo.Trim();
                
                //Toda la lista de abreviaciones
                var listaAbrev = contexto.AbrevRiesgos.ToList();
                //Se busca el codigo de riesgo que entro con el riesgo dentro de la lista de abreviaciones
                var newListA = listaAbrev.Where(x => x.abreviacion == riesgos.codigoRiesgo);
                //Trae la categoria del primer elemeneto de la abreviacion que coincida
                if (newListA.Any())
                {
                    riesgos.categoria = newListA.First().categoria;
                }
                else
                {
                    TempData["ErrorMessage"] = "La lista de categorías está vacía.";
                    // Redireccionar a otra acción o vista donde se mostrará el mensaje
                    //return RedirectToAction("Index", "Home");
                }
                //Si esta lista no esta vacia quiere decir que la abreviacion si exsite en el sistema
                if (newListA.Count() > 0)
                {
                    //Lista completa de riesgos
                    var ListaP = contexto.Riesgos.ToList();
                    //Lista de riesgos que coincidan con la abreviacion
                    var NewList = ListaP.Where(x => x.codigoRiesgo.StartsWith(riesgos.codigoRiesgo));
                    //Se devulve esta lista
                    ListaP = NewList.ToList();
                    if (ListaP.Count() > 0)
                    {
                        //Debo sacar el numero del ultimo codigo registrado
                        string[] codigo = ListaP.Last().codigoRiesgo.Split(" ");
                        //Pero como sale en string lo debo
                        //Convertie en numero, para poder sumarle uno despues
                        var numero = Convert.ToInt32(codigo[1]);
                        //Ahora que ya tengo el numero lo asigno al codigo
                        riesgos.codigoRiesgo = riesgos.codigoRiesgo + " " + (numero + 1);

                    }
                    else
                    {
                        riesgos.codigoRiesgo = riesgos.codigoRiesgo + " " + 1;
                    }

                    contexto.Add(riesgos);
                    await contexto.SaveChangesAsync();

                    TempData["MensajeConfirmacion"] = "El riesgo " + riesgos.codigoRiesgo + " se añadió  correctamente";

                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["MensajeCodigo"] = "No existe ningún código de riesgo con esa abreviatura";
                    return View(riesgos);
                }




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

                TempData["MensajeConfirmacion"] = "El riesgo " + riesgos.codigoRiesgo + " se modifico  correctamente";

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

            TempData["MensajeConfirmacionDelete"] = "El riesgo " + riesgos.codigoRiesgo + " se eliminó  correctamente";

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









        public IActionResult Abreviaciones()
        {
            return View(contexto.AbrevRiesgos.ToList());
        }

        [HttpGet]
        public IActionResult CreateAbrev()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAbrev([Bind("abreviacion,categoria")] AbrevRiesgos riesgos)
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
        //[HttpGet]
        //public async Task<IActionResult> EditAbrev(string? Id)
        //{
        //    if (Id == null)
        //    {
        //        return NotFound();
        //    }

        //    var riesgos = await contexto.AbrevRiesgos.FindAsync(Id);

        //    if (riesgos == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(riesgos);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditAbrev(string Id, [Bind("abreviacion, categoria")] AbrevRiesgos riesgos)
        //{
        //    if (Id != riesgos.abreviacion)
        //    {
        //        return NotFound();
        //    }
        //    if (ModelState.IsValid)
        //    {
        //        contexto.Update(riesgos);

        //        await contexto.SaveChangesAsync();

        //        return RedirectToAction("Index");
        //    }
        //    else
        //    {
        //        return View(riesgos);
        //    }
        //}

        [HttpGet]
        public async Task<IActionResult> DeleteAbrev(string? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var riesgos = await contexto.AbrevRiesgos.FindAsync(Id);

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
        [ValidateAntiForgeryToken]
        [ActionName("DeleteAbrev")]
        public async Task<IActionResult> DelAbrev(string Id)
        {
            var riesgos = await contexto.AbrevRiesgos.FindAsync(Id);

            var ListaP = contexto.Riesgos.ToList();
            //Lista de riesgos que coincidan con la abreviacion
            var NewList = ListaP.Where(x => x.codigoRiesgo.StartsWith(Id));

            if (NewList.Any())
            {
                TempData["MensajeDelete"] = "El codigo del riesgo no se puede eliminar por que ya existen riesgos asociados a este";
                return View(riesgos);
            }
            else{
                contexto.AbrevRiesgos.Remove(riesgos);

                await contexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            
        }
    }
}
