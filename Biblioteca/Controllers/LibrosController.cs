using Biblioteca.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Biblioteca.Controllers
{
    public class LibrosController : Controller
    {
        private BibliotecaContext db = new BibliotecaContext();

        // GET: Libros
        public ActionResult Index()
        {
            return View(db.Libros.ToList());
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.Genero = ObtenerGenero();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Libros.Add(libro);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexISBN"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un libro registrado con el ISBN: " + libro.ISBN;
                        ViewBag.Genero = ObtenerGenero();
                        return View(libro);
                    }
                }
            }
            ViewBag.Genero = ObtenerGenero();
            return View(libro);
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Libro libro = db.Libros.Find(id);

            if (libro != null)
            {
                ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
                return View(libro);
            }
            else
            {
                ViewBag.Info = ("Libro no encontrado...");
                ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Edit(Libro libro)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(libro).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("IndexISBN"))
                    {
                        ViewBag.ErrorMessage = "¡Error!, ya existe un libro registrado con el ISBN: " + libro.ISBN;
                        ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
                        return View(libro);
                    }
                }
            }
            ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
            return View(libro);
        }

        [HttpGet]
        public ActionResult Delete(int? id)
        {
            Libro libro = db.Libros.Find(id);
            if (libro != null)
            {
                return View(libro);
            }
            else
            {
                ViewBag.Info = ("Libro no encontrado...");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            Libro libro = db.Libros.Find(id);
            if (libro != null)
            {
                try
                {
                    db.Libros.Remove(libro);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null && ex.InnerException.InnerException != null
                        && ex.InnerException.InnerException.Message.Contains("REFERENCE"))
                    {
                        bool tieneEjemplares = db.Ejemplares.Any(e => e.LibroId == id);

                        if (tieneEjemplares)
                        {
                            ViewBag.ErrorMessage = "No se puede eliminar. Existen ejemplares asociados a este libro.";
                            ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
                            return View(libro);
                        }

                        bool tieneAutores = db.LibroAutores.Any(la => la.LibroId == id);

                        if (tieneAutores)
                        {
                            ViewBag.ErrorMessage = "No se puede eliminar. Existen autores asociados a este libro.";
                            ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
                            return View(libro);
                        }
                    }
                    else if (ex.InnerException != null)
                    {
                        ViewBag.ErrorMessage = ex.Message;
                        ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
                        return View(libro);
                    }
                }
            }
            ViewBag.Genero = new SelectList(ObtenerGenero(), "Value", "Text", libro.Genero);
            return View();
        }

        [HttpGet]
        public ActionResult Details(int? id)
        {
            Libro libro = db.Libros.Find(id);
            var libroDetailsView = new LibroDetailsView
            {
                LibroId = libro.LibroId,
                Nombre = libro.Nombre,
                Editorial = libro.Editorial,
                FechaPublicacion = libro.FechaPublicacion,
                Genero = libro.Genero,
                ISBN = libro.ISBN,
                LibroAutores = libro.LibroAutores.ToList(),
            };
            return View(libroDetailsView);
        }

        [HttpGet]
        public ActionResult AddAutor(int libroId)
        {
            //LLenar DropDownList con todos los autores
            ViewBag.AutorId = new SelectList(db.Autores, "AutorId", "NombreCompleto");

            var libroAV = new LibroAutorView
            {
                LibroId = libroId,
            };
            return View(libroAV);
        }

        [HttpPost]
        public ActionResult AddAutor(LibroAutorView libroAutorView)
        {
            //LLenar DropDownList con todos los autores
            ViewBag.AutorId = new SelectList(db.Autores, "AutorId", "NombreCompleto");

            //validar que el autor no este en el libro
            var libroAutor = db.LibroAutores.Where(
                te => te.LibroId == libroAutorView.LibroId &&
                te.AutorId == libroAutorView.AutorId).FirstOrDefault();

            if (libroAutor != null)
            {
                ViewBag.Error = "El autor ya está registrado en el libro";
            }
            else
            {
                libroAutor = new LibroAutor
                {
                    LibroId = libroAutorView.LibroId,
                    AutorId = libroAutorView.AutorId
                };
                try
                {
                    db.LibroAutores.Add(libroAutor);
                    db.SaveChanges();
                    return RedirectToAction(string.Format("Details/{0}", libroAutorView.LibroId));
                }
                catch
                {
                    return View(libroAutorView);
                }
            }
            return View(libroAutorView);
        }

        public ActionResult RetirarAutor(int id)
        {
            var aut = db.LibroAutores.Find(id);
            if (aut != null)
            {
                db.LibroAutores.Remove(aut);
                db.SaveChanges();
            }
            return RedirectToAction(string.Format("Details/{0}", aut.LibroId));
        }

        private List<SelectListItem> ObtenerGenero()
        {
            return new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text="Seleccione género",
                    Value="Seleccionar",
                    Selected=true,
                },
                new SelectListItem()
                {
                    Text="Narrativa",
                    Value="Narrativa",
                },
                new SelectListItem()
                {
                    Text="Novela",
                    Value="Novela",
                },
                new SelectListItem()
                {
                    Text="Poesía",
                    Value="Poesía",
                },
                new SelectListItem()
                {
                    Text="Comic",
                    Value="Comic",
                },
                new SelectListItem()
                {
                    Text="Biografía",
                    Value="Biografía",
                },
                new SelectListItem()
                {
                    Text="Ensayo",
                    Value="Ensayo",
                },
                new SelectListItem()
                {
                    Text="Relato",
                    Value="Relato",
                },
                new SelectListItem()
                {
                    Text="Cuento",
                    Value="Cuento",
                }
            };
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