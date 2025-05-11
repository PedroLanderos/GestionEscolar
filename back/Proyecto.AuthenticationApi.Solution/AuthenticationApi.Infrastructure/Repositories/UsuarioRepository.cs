using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Llaveremos.SharedLibrary.Logs;
using AuthenticationApi.Application.Services;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UsuarioRepository(AuthenticationDbContext context, IConfiguration config, IRandomService randomService) : IUser
    {
        public async Task<Usuario?> GetUsuarioByCorreo(string correo)
        {
            return await context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo);
        }

        public async Task<Response> RegistrarAlumno(UsuarioDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Curp) || dto.Curp.Length < 10)
                    return new Response(false, "CURP inválida");

                string baseCurp = dto.Curp.Substring(0, 10).ToUpper();

                string alumnoId = $"C{baseCurp}";
                string homoclave = randomService.GenerateHomoclave();
                string padreId = $"{baseCurp}{homoclave}";

                string alumnoPass = randomService.GenerateRandomPassword();
                string padrePass = randomService.GenerateRandomPassword();

                var alumno = new Usuario
                {
                    Id = alumnoId,
                    NombreCompleto = dto.NombreCompleto,
                    Correo = dto.Correo,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(alumnoPass),
                    Curp = dto.Curp.ToUpper(),
                    CuentaBloqueada = false,
                    DadoDeBaja = false,
                    UltimaSesion = null,
                    Rol = "Alumno"
                };

                var padre = new Usuario
                {
                    Id = padreId,
                    NombreCompleto = $"Padre de {dto.NombreCompleto}",
                    Correo = $"padre.{baseCurp.ToLower()}@mail.com", // puedes ajustar esto si lo pasas por el DTO
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(padrePass),
                    Curp = dto.Curp.ToUpper(),
                    CuentaBloqueada = false,
                    DadoDeBaja = false,
                    UltimaSesion = null,
                    Rol = "Padre"
                };

                context.Usuarios.AddRange(alumno, padre);
                await context.SaveChangesAsync();


                return new Response(true, "Usuarios registrados exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error mientras se creaba el usuario en el repositorio");
            }
        }
    }
}
