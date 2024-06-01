using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Biblioteca.Controllers
{
    public class EjemplaresController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Ejemplares
        public ActionResult Index()
        {
            var ejemplar = db.Ejemplares.Include(lib => lib.Libro);
            return View(db.Ejemplares.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Ejemplar ejemplar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Ejemplares.Add(ejemplar);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
                        return View(ejemplar);
                    }
                }
            }
            ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
            return View(ejemplar);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Ejemplar ejemplar = db.Ejemplares.Find(id);
            if (ejemplar != null)
            {
                ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
                return View(ejemplar);
            }
            else
            {
                ViewBag.Info = ("Torneo no encontrado...");
                ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Ejemplar ejemplar)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(ejemplar).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        ViewBag.ErrorMessage = e.Message;
                        ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
                        return View(ejemplar);
                    }
                }
            }
            ViewBag.LibroId = new SelectList(db.Libros, "LibroId", "Nombre", ejemplar.LibroId);
            return View(ejemplar);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var ejemplar = db.Ejemplares.Find(id);
            if (ejemplar != null)
            {
                return View(ejemplar);
            }
            else
            {
                ViewBag.Info = ("Ejemplar no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Ejemplar ejemplar = db.Ejemplares.Find(id);
            if (ejemplar != null)
            {
                try
                {
                    db.Ejemplares.Remove(ejemplar);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar. Existen prestamos asociados a este ejemplar";
                        return View(ejemplar);
                    }
                    else if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(ejemplar);
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
                Ejemplar ejemplar = db.Ejemplares.Find(id);
                return View(ejemplar);
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
