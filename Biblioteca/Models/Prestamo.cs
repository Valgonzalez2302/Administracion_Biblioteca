using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Foolproof;

namespace Biblioteca.Models
{
    public class Prestamo
    {
        [Key]
        public int PrestamoId { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [DisplayName("Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El Campo {0} es Obligatorio")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [GreaterThan("FechaInicio", ErrorMessage = "El campo {0} debe ser mayor que {1}")]
        [DisplayName("Fecha Fin")]
        public DateTime FechaFin { get; set; }

        // Lleva Foranea para la relacion con Usuario
        public int UsuarioId { get; set; }
        public virtual Usuario Usuario { get; set; }

        //Propiedad navigacional para la relación con PrestamoEjemplar
        public virtual ICollection<PrestamoEjemplar> PrestamoEjemplares { get; set; }
    }
}