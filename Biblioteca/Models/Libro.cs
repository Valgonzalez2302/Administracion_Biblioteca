using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Libro
    {
        [Key]
        public int LibroId { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [DisplayName("Nombre Libro")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(40, MinimumLength = 5, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        public string Editorial { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de publicación")]
        public DateTime FechaPublicacion { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(20, MinimumLength = 4, ErrorMessage = "El campo {0} debe tener entre {2} y {1} caracteres.")]
        [DisplayName("Género")]
        public string Genero { get; set; }

        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El campo {0} debe tener {1} caracteres.")]
        [Index("IndexISBN,", IsUnique = true)]
        public string ISBN { get; set; }

        // atributo virtual para la relacion con Ejemplar
        public virtual ICollection<Ejemplar> Ejemplares { get; set; }

        //Propiedad navigacional para la relación con LibroAutor
        public virtual ICollection<LibroAutor> LibroAutores { get; set; }
    }
}