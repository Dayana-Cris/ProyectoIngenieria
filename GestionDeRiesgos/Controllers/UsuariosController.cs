using GestionDeRiesgos.Data;
using GestionDeRiesgos.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
//librerias de seguridad
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Numerics;
using System.Collections.Generic;

namespace GestionDeRiesgos.Controllers
{
    public class UsuariosController : Controller
    {

        //Variable que permite manejar la referencia del contexto 
        private readonly Contexto contexto;
        private static string MensajeContrasena = "";
        private static string pass;
        private static string correoCambio;

        public UsuariosController(Contexto context)
        {
            contexto = context;
        }
        //Es el primer metodo que se ejecuta en el controlador
        public IActionResult Index(int id, string buscar, string idBusqueda, string nombreBusqueda, string restaurar)
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
            var ListaP = contexto.Usuarios.ToList();
            if (buscar != null)
            {
                if (idBusqueda != null)
                {
                    int idBus = Convert.ToInt32(buscar);
                    var NewList = ListaP.Where(x => x.idUsuario == idBus) ;
                    ListaP = NewList.ToList();
                }

                if (nombreBusqueda != null)
                {
                    var NewList = ListaP.Where(x => x.nombre.StartsWith(buscar));
                    ListaP = NewList.ToList();
                }
                if (ListaP.Count < 1)
                {
                    TempData["MensajeLista"] = "No existe ningún usuario para esta búsqueda";
                }
                if (restaurar != null)
                {
                     ListaP = contexto.Usuarios.ToList();
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
        public async Task<IActionResult> Create([Bind("idUsuario, nombre, correo, password, rol")] Usuarios usuarios)
        {
            if (ModelState.IsValid)
            {
                if (claveSegura(usuarios.password))
                {
                    var ListaP = contexto.Usuarios.ToList();
                    var NewList = ListaP.Where(x => x.correo == usuarios.correo);
                    if (NewList.Any())
                    {
                        TempData["MensajeError"] = "El correo ingresado ya se encuentra asociado a otro usuario del sistema, por favor ingrese otro";
                        return View(usuarios);
                    }
                    else
                    {
                        TempData["MensajeConfirmacion"] = "El usuario " + usuarios.correo + " se añadió  correctamente";
                        contexto.Add(usuarios);
                        await contexto.SaveChangesAsync();
                        return RedirectToAction("Index");
                    }
                    
                }
                else
                {
                    TempData["MensajeError"] = MensajeContrasena;
                    return View(usuarios);
                }

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
            pass = usuario.password;
            correoCambio = usuario.correo;

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int Id, [Bind("idUsuario, nombre, correo, password, rol")] Usuarios usuario)
        {
            usuario.password = pass;

            if (Id != usuario.idUsuario)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                TempData["MensajeConfirmacion"] = "El usuario " + usuario.idUsuario + " se modificó  correctamente";


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

            TempData["MensajeConfirmacionDelete"] = "El usuario " + usuario.idUsuario + " se eliminó  correctamente";


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


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind] Usuarios user)
        {
            //se enviar el objeto usuario para validar su email  y pw
            var temp = this.validarUsuarios(user);

            //se pregunta si tiene datos
            if (temp != null)
            {



                //De ser verdadero, se crea identidad del usuario
                var userClaims = new List<Claim>()
                    {
                        new Claim (ClaimTypes.Name, temp.correo),
                        new Claim(ClaimTypes.Role, temp.rol)
                    };

                //permite la autenticación basada en notificaciones donde define la identidad del usuario
                //como un conjunto de notificaciones 
                var grandmaIdentity = new ClaimsIdentity(userClaims, "User Identity");
                var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity });

                //HttpContext encapsula información especifica de Http sobra la solicitud del usuario
                HttpContext.SignInAsync(userPrincipal);

                //ubicamos al usuario en la página default
                return RedirectToAction("Index", "Home");


            }
            //almacenamos una mensaje de error para mostrarlo a nivel de front-end
            TempData["Mensaje"] = "Error usuario o password incorrecto...";

            //ojo en caso que no exita información del usuario autenticado
            //enviamos el object  nuevamente al front-end para que usuario modifique los datos
            return View(user);
        }
        private Usuarios validarUsuarios(Usuarios temp)
        {
            Usuarios autorizado = null;

            var user = this.contexto.Usuarios.FirstOrDefault(u => u.correo == temp.correo);

            if (user != null)
            {
                if (user.password.Equals(temp.password))
                {

                    autorizado = user;

                }
            }

            return autorizado;
        }

        public async Task<IActionResult> Logout()
        {
            //Ojo aqui se cierra la sesion
            await HttpContext.SignOutAsync();

            //Reubicamos al usuario en la pagina default 
            return RedirectToAction("Index", "Home");
        }
        private bool claveSegura(String clave)
        {
            //se definen las varibles con los datos minimos requeridos para realizar la password correctamente
            var mayusculas = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var minusculas = "abcdefghijklmnopqrstuvwxyz";
            var numeros = "0123456789";
            var simbolos = "*$%&#+/.-,";

            int LongitudMinima = clave.Length;
            //se utiliza cuatro variables para checkear cada una de las condiciones para la clave 
            //se colocan por default en false para luego pasar a true si esta esta correcta por la condicion
            bool firstCheck = false, secondCheck = false, thirdCheck = false, FourthcCheck = false;
            //se utiliza un bucle for para hacer el recorrido caracter por caracter
            for (int i = 0; i < clave.Length; i++)
            {
                //se utiliza IndexOf para encontrar el índice y Substring para el caracter 
                //asi se puede identificar si cada tipo de dato requerido se encuentra en la password ingresada por el usuario
                if (mayusculas.IndexOf(clave.Substring(i, 1), 0) != -1)
                {
                    firstCheck = true;
                }
                if (minusculas.IndexOf(clave.Substring(i, 1)) != -1)
                {
                    secondCheck = true;
                }
                if (numeros.IndexOf(clave.Substring(i, 1)) != -1)
                {
                    thirdCheck = true;
                }
                if (simbolos.IndexOf(clave.Substring(i, 1)) != -1)
                {
                    FourthcCheck = true;
                }
            }
            //se verifica que todas las variables esten correctas y la longitud minima de la password
            if (firstCheck && secondCheck && thirdCheck && FourthcCheck && LongitudMinima >= 8)
            {
                return true;
            }
            //se crean las condiciones para mandar un mensaje al frond-end en caso de que estas no se cumplan
            if (firstCheck == false)
            {
                MensajeContrasena = "La contraseña debe contener al menos una letra mayúscula";

            }
            if (secondCheck == false)
            {
                MensajeContrasena = "La contraseña debe contener al menos una letra minúscula";

            }
            if (thirdCheck == false)
            {
                MensajeContrasena = "La contraseña debe contener al menos un número";

            }
            if (FourthcCheck == false)
            {
                MensajeContrasena = "La contraseña debe contener al menos uno de los siguientes símbolos (* $ % & # + / . - ,)";

            }
            if (LongitudMinima < 8)
            {
                MensajeContrasena = "La contraseña debe contener al menos 8 caracteres de longitud";
            }
            return false;
        }//Cierre claveSegura

        [HttpGet]
        public IActionResult Restablecer()
        {
            return View();
        }

        /// <summary>
        /// Método encargado se realizar el proceso de cambio contraseñan en la db
        /// </summary>
        /// <param name="pRestablecer"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Restablecer([Bind] SeguridadRestablecer pRestablecer)
        {
            if (pRestablecer != null)
            {
                //variable de tipo usuario temporal
                //se traen los datos del usuario a restablecer la clave
                var temp = this.contexto.Usuarios.First(u => u.correo == correoCambio);

                //Se debe verificar la contraseña actual
                if (temp.password.Equals(pRestablecer.Password))
                {
                    //verificar que la nueva clave sea igual con la confirmación
                    if (pRestablecer.NewPassword.Equals(pRestablecer.ConfirmarPassword))
                    {
                        if (claveSegura(pRestablecer.NewPassword))
                        {
                            //se debe realizar el cambio de clave
                            temp.password = pRestablecer.NewPassword;

                            //se actualiza los datos
                            this.contexto.Update(temp);

                            //se aplican los cambios
                            this.contexto.SaveChanges();

                        }
                        else
                        {
                            TempData["MensajeError"] = MensajeContrasena;
                            return View(pRestablecer);
                        }


                    }
                    else
                    {
                        //Mensaje de error
                        TempData["MensajeError"] = "La confirmación de la contraseña no coincide con la nueva";
                        return View(pRestablecer);
                    }

                }
                else
                {
                    //Vamos a mostrar un mensaje de error
                    TempData["MensajeError"] = "La contraseña actual es incorrecta.";
                    //Se ubica al usuario en el front-end de error
                    return View(pRestablecer);
                }
            }
            //Si todo salió bien, se ubica al usuario en el formulario de login
            return RedirectToAction("Index", "Usuarios");
        }

    }
}
