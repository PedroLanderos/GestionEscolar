using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs
{
    public class SolicitudAltaDTO
    {
        public int Id { get; set; }
        [Required]
        public string NombreAlumno { get; set; } = null!;
        [Required]
        public string CurpAlumno { get; set; } = null!;
        [Required]
        public int Grado { get; set; }
        [Required]
        public string NombrePadre { get; set; } = null!;
        [Required]
        public string Telefono { get; set; } = null!;
        [Required, EmailAddress]
        public string CorreoPadre { get; set; } = null!;
    }
}
