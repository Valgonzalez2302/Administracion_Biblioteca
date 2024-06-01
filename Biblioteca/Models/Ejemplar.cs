using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Ejemplar
    {
        [Key]
        public int EjemplarId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [DisplayName("Número de copias")]
        public int NumeroCopias { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        public bool Estado {  get; set; }

        // Lleva Foranea para la relacion con Libro
        [DisplayName("Libro")]
        public int LibroId { get; set; }
        public virtual Libro Libro { get; set; }

        //Propiedad navigacional para la relación con PrestamoEjemplar
        public virtual ICollection<PrestamoEjemplar> PrestamoEjemplares { get; set; }

    }
}