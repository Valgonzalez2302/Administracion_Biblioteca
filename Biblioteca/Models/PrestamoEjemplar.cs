using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class PrestamoEjemplar
    {
        [Key]
        public int PrestamoEjemplarId { get; set; }

        //Llave foranea para Prestamo
        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        public int PrestamoId { get; set; }

        //Llave foranea para Ejemplar
        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        public int EjemplarId { get; set; }

        //Propiedad navigacional para la relación con Prestamo
        public virtual Prestamo Prestamos { get; set; }

        //Propiedad navigacional para la relación con Ejemplar
        public virtual Ejemplar Ejemplares { get; set; }
    }
}