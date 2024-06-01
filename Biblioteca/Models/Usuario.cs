using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class Usuario
    {
        [Key]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        public string Nombres { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(40, MinimumLength = 3, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        public string Apellidos { get; set; }

        [DisplayName("Nombre de usuario")]
        public string NombreCompleto { get{ return (this.Nombres + " " + this.Apellidos); } }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [Range(3000000000, 3999999999, ErrorMessage = "En el campo {0} debe ingresar valores entre {1} y {2}")]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [DisplayName("Dirección")]
        public string Direccion {  get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [StringLength(15, MinimumLength = 6, ErrorMessage = "El Campo {0} debe tener entre {2} y {1} caracteres ")]
        [Index("IndexCedula,", IsUnique = true)]
        [DisplayName("Cédula")]
        public string Cedula { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [EmailAddress(ErrorMessage = "Favor ingresar un correo electrónico válido")]
        [DisplayName("Correo electrónico")]
        public string Correo { get; set; }

        // atributo virtual para la relacion con Prestamo
        public virtual ICollection<Prestamo> Prestamos { get; set; }
    }
}