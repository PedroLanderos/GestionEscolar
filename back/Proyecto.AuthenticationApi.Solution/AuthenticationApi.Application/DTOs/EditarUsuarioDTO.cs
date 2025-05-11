using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public class EditarUsuarioDTO
    {
        [Required]
        public string? Id { get; set; }

        [Required]
        public string NombreCompleto { get; set; } = null!;

        [Required, EmailAddress]
        public string Correo { get; set; } = null!;
        public string? Contrasena { get; set; }

        [Required]
        public string Curp { get; set; } = null!;

        [Required]
        public bool CuentaBloqueada { get; set; }

        [Required]
        public bool DadoDeBaja { get; set; }

        [Required]
        public DateTime UltimaSesion { get; set; }

        [Required]
        public string Rol { get; set; } = null!;
    }
}
