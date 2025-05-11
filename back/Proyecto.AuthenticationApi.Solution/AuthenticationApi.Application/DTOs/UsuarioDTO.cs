using System;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record UsuarioDTO(
        [Required] string Id,
        [Required] string NombreCompleto,
        [Required, EmailAddress] string Correo,
        [Required] string Contrasena,
        [Required] string Curp,
        [Required] bool CuentaBloqueada,
        [Required] bool DadoDeBaja,
        [Required] DateTime UltimaSesion,
        [Required] string Rol
    );
}
