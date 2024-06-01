using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class AutoresController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Autores
        public ActionResult Index()
        {
            return View(db.Autores.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Autor autor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Autores.Add(autor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexDocumento"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un autor con el documento: " + autor.Documento;
                        return View(autor);
                    }
                }
            }
            return View(autor);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Autor autor = db.Autores.Find(id);
            if (autor != null)
            {
                return View(autor);
            }
            else
            {
                ViewBag.Info = ("Autor no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Autor autor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(autor).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexDocumento"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un autor con el documento: " + autor.Documento;
                        return View(autor);
                    }
                }
            }
            return View(autor);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Autor autor = db.Autores.Find(id);
            if (autor != null)
            {
                return View(autor);
            }
            else
            {
                ViewBag.Info = ("Autor no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Autor autor = db.Autores.Find(id);
            if (autor != null)
            {
                try
                {
                    db.Autores.Remove(autor);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar. Existen libros asociados a este autor";
                        return View(autor);
                    }
                    else if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(autor);
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
                Autor autor = db.Autores.Find(id);
                return View(autor);
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