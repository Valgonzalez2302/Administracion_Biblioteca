using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class LibroAutor
    {
        [Key]
        public int LibroAutorId { get; set; }

        //Llave foranea para Libro
        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        public int LibroId { get; set; }

        //Llave foranea para Autor
        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        public int AutorId { get; set; }

        //Propiedad navigacional para la relación con Libro
        public virtual Libro Libro { get; set; }

        //Propiedad navigacional para la relación con Autor
        public virtual Autor Autor { get; set; }
    }
}