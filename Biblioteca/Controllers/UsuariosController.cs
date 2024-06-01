using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class UsuariosController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Usuarios
        public ActionResult Index()
        {
            return View(db.Usuarios.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Usuarios.Add(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexCedula"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un usuario con el documento: " + usuario.Cedula;
                        return View(usuario);
                    }
                }
            }
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario != null)
            {
                return View(usuario);
            }
            else
            {
                ViewBag.Info = ("Usuario no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(usuario).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexCedula"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un Usuario con la cedula: " + usuario.Cedula;
                        return View(usuario);
                    }
                }
            }
            return View(usuario);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario != null)
            {
                return View(usuario);
            }
            else
            {
                ViewBag.Info = ("Usuario no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Usuario usuario = db.Usuarios.Find(id);
            if (usuario != null)
            {
                try
                {
                    db.Usuarios.Remove(usuario);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar. Existen préstamos asociados a este usuario.";
                        return View(usuario);
                    }
                    else if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(usuario);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            if (id != null)
            {
                Usuario usuario = db.Usuarios.Find(id);
                return View(usuario);
            }
            return View();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}