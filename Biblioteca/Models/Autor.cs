using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Autor
    {
        [Key]
        public int AutorId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        public string Apellidos { get; set; }

        [DisplayName("Nombre de autor")]
        public string NombreCompleto { get { return (this.Nombres + " " + this.Apellidos); } }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        [Index("IndexDocumento,", IsUnique = true)]
        public string Documento { get; set; }

        //Propiedad navigacional para la relación con LibroAutor
        public virtual ICollection<LibroAutor> LibroAutores { get; set; }
    }
}