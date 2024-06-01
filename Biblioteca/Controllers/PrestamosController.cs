using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class PrestamosController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Prestamos
        public ActionResult Index()
        {
            var prestamo = db.Prestamos.Include(user => user.Usuario);
            return View(db.Prestamos.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto");
            return View();
        }

        [HttpPost]
        public ActionResult Create(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Prestamos.Add(prestamo);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
                        return View(prestamo);
                    }
                }
            }
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
            return View(prestamo);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo != null)
            {
                ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
                return View(prestamo);
            }
            else
            {
                ViewBag.Info = ("Prestamo no encontrado...");
                ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(prestamo).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception e)
                {
                    if (e.InnerException != null)
                    {
                        ViewBag.ErrorMessage = e.Message;
                        ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
                        return View(prestamo);
                    }
                }
            }
            ViewBag.UsuarioId = new SelectList(db.Usuarios, "UsuarioId", "NombreCompleto", prestamo.UsuarioId);
            return View(prestamo);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            var prestamo = db.Prestamos.Find(id);
            if (prestamo != null)
            {
                return View(prestamo);
            }
            else
            {
                ViewBag.Info = ("Prestamo no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Prestamo prestamo = db.Prestamos.Find(id);
            if (prestamo != null)
            {
                try
                {
                    db.Prestamos.Remove(prestamo);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        ViewBag.ErrorMessage = "No se puede eliminar. Existen ejemplares sin devolver asociados a este préstamo";
                        return View(prestamo);
                    }
                    else if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        return View(prestamo);
                    }
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            Prestamo prestamo = db.Prestamos.Find(id);
            Usuario user = db.Usuarios.Find(prestamo.UsuarioId);
            var prestamoDetailsView = new PrestamoDetailsView
            {
                PrestamoId = prestamo.PrestamoId,
                FechaInicio = prestamo.FechaInicio,
                FechaFin = prestamo.FechaFin,
                Usuario = user.NombreCompleto,
                PrestamoEjemplares = prestamo.PrestamoEjemplares.ToList(),
            };
            return View(prestamoDetailsView);
        }

        [HttpGet]
        public ActionResult AddEjemplar(int prestamoId)
        {
            //LLenar DropDownList con todos los ejemplares
            ViewBag.EjemplarId = new SelectList(db.Ejemplares.Include("Libro"), "EjemplarId", "Libro.Nombre");

            var prestamoEV = new PrestamoEjemplarView
            {
                PrestamoId = prestamoId,
            };
            return View(prestamoEV);
        }

        [HttpPost]
        public ActionResult AddEjemplar(PrestamoEjemplarView prestamoEjemplarView)
        {
            //LLenar DropDownList con todos los ejemplares
            ViewBag.EjemplarId = new SelectList(db.Ejemplares.Include("Libro"), "EjemplarId", "Libro.Nombre");

            //validar que el autor no este en el libro
            var prestamoEjemplar = db.PrestamoEjemplares.Where(
                te => te.PrestamoId == prestamoEjemplarView.PrestamoId &&
                te.EjemplarId == prestamoEjemplarView.EjemplarId).FirstOrDefault();

            // Busca un ejemplar en especifico
            var ejemplar = db.Ejemplares.Include("Libro").SingleOrDefault(e => e.EjemplarId == prestamoEjemplarView.EjemplarId);

            if (ejemplar != null && ejemplar.NumeroCopias <= 0 )
            {
                ViewBag.Error = "No hay copias disponibles para prestar.";
            }
            else
            {
                prestamoEjemplar = new PrestamoEjemplar
                {
                    PrestamoId = prestamoEjemplarView.PrestamoId,
                    EjemplarId = prestamoEjemplarView.EjemplarId
                };
                try
                {
                    db.PrestamoEjemplares.Add(prestamoEjemplar);
                    ejemplar.NumeroCopias--;
                    db.SaveChanges();
                    return RedirectToAction(string.Format("Details/{0}", prestamoEjemplarView.PrestamoId));
                }
                catch
                {
                    ViewBag.Error = "Hubo un error al registrar el préstamo. Inténtelo de nuevo.";
                    return View(prestamoEjemplarView);
                }
            }
            return View(prestamoEjemplarView);
        }

        public ActionResult RetirarLibro(int id)
        {
            
            var ej = db.PrestamoEjemplares.Find(id);
            if (ej != null)
            {
                // Encontrar el ejemplar asociado al préstamo
                var ejemplar = db.Ejemplares.Include("Libro").SingleOrDefault(e => e.EjemplarId == ej.EjemplarId);
                if (ejemplar != null)
                {
                    
                    ejemplar.NumeroCopias++;
                    db.PrestamoEjemplares.Remove(ej);

                    try
                    {
                        // Guardar los cambios en la base de datos
                        db.SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        ViewBag.Error = "Hubo un error al devolver el libro: " + ex.Message;
                        return RedirectToAction(string.Format("Details/{0}", ej.PrestamoId));
                    }
                }
            }
            return RedirectToAction(string.Format("Details/{0}", ej?.PrestamoId ));
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
