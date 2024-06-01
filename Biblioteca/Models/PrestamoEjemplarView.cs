using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class PrestamoEjemplarView
    {
        public int PrestamoId { get; set; }

        [Required (ErrorMessage = "El campo {0} es obligatorio")]
        public int EjemplarId { get; set; }
        public string NombreLibro { get; set; }
    }
}