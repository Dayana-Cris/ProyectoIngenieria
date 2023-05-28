using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeRiesgos.Controllers
{
    public class UsuariosController : Controller
    {

        //Variable que permite manejar la referencia del contexto 
        private readonly Contexto contexto;

        public UsuariosController(Contexto context)
        {
            contexto = context;
        }
        //Es el primer metodo que se ejecuta en el controlador
        public IActionResult Index()
        {
            return View(contexto.Usuarios.ToList());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("idUsuario, nombre, correo, password")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                contexto.Add(usuarios);
                await contexto.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            else
            {
                return View(usuarios);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var usuario = await contexto.Usuarios.FindAsync(Id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("idUsuario, nombre, correo, password")] Usuarios usuario)
        {
            if (Id != usuario.idUsuario)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                contexto.Update(usuario);

                await contexto.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var usuario = await contexto.Usuarios.FindAsync(Id);

            if (usuario == null)
            {
                return NotFound();
            }
            else
            {
                return View(usuario);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Delete(int Id)
        {
            var usuario = await contexto.Usuarios.FindAsync(Id);

            contexto.Usuarios.Remove(usuario);

            await contexto.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var usuario = await contexto.Usuarios.FindAsync(Id);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }
    }
}
