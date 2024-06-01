using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Biblioteca.Models
{
    public class LibroAutorView
    {
        public int LibroId { get; set; }

        [Required (ErrorMessage ="El campo {0} es obligatorio")]
        public int AutorId { get; set; }

        
    }
}