using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class PrestamoDetailsView
    {
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

        public string Usuario { get; set; }

        public List<PrestamoEjemplar> PrestamoEjemplares { get; set; }
    }
}