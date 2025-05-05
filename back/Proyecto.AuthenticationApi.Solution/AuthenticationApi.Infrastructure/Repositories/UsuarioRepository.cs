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

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UsuarioRepository(AuthenticationDbContext context, IConfiguration config) : IUser
    {
        public async Task<Usuario?> GetUsuarioByCorreo(string correo)
        {
            return await context.Usuarios.FirstOrDefaultAsync(x => x.Correo == correo);
        }

        public async Task<Response> Register(UsuarioDTO dto)
        {
            var existingUser = await GetUsuarioByCorreo(dto.Correo);
            if (existingUser is not null)
                return new Response(false, "Correo ya registrado");

            var nuevoUsuario = new Usuario
            {
                NombreCompleto = dto.NombreCompleto,
                Correo = dto.Correo,
                Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                Curp = dto.Curp,
                CuentaBloqueada = dto.CuentaBloqueada,
                DadoDeBaja = dto.DadoDeBaja,
                UltimaSesion = dto.UltimaSesion,
                Rol = dto.Rol
            };

            context.Usuarios.Add(nuevoUsuario);
            await context.SaveChangesAsync();

            return new Response(true, "Usuario registrado exitosamente");
        }

        public async Task<Response> Login(IniciarSesionDTO loginDTO)
        {
            var user = await GetUsuarioByCorreo(loginDTO.Email);
            if (user is null)
                return new Response(false, "Credenciales inválidas");

            var validPassword = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.Contrasena);
            if (!validPassword)
                return new Response(false, "Credenciales inválidas");

            var token = GenerateToken(user);
            return new Response(true, token);
        }

        private string GenerateToken(Usuario user)
        {
            var key = Encoding.UTF8.GetBytes(config["Authentication:Key"]!);
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.NombreCompleto!),
                new(ClaimTypes.Email, user.Correo!),
                new("UserId", user.Id.ToString()!)
            };

            if (!string.IsNullOrEmpty(user.Rol))
                claims.Add(new(ClaimTypes.Role, user.Rol!));

            var token = new JwtSecurityToken(
                issuer: config["Authentication:Issuer"],
                audience: config["Authentication:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<ObtenerUsuarioDTO> GetUser(int usuarioID)
        {
            var user = await context.Usuarios.FindAsync(usuarioID);
            if (user is null) return null!;

            return new ObtenerUsuarioDTO
            {
                Id = user.Id!.Value,
                NombreCompleto = user.NombreCompleto!,
                Correo = user.Correo!,
                Curp = user.Curp!,
                CuentaBloqueada = user.CuentaBloqueada ?? false,
                DadoDeBaja = user.DadoDeBaja ?? true,
                UltimaSesion = user.UltimaSesion ?? DateTime.MinValue,
                Rol = user.Rol!
            };
        }

        public async Task<IEnumerable<ObtenerUsuarioDTO>> GetAllUsers()
        {
            var users = await context.Usuarios.ToListAsync();

            return users.Select(user => new ObtenerUsuarioDTO
            {
                Id = user.Id!.Value,
                NombreCompleto = user.NombreCompleto!,
                Correo = user.Correo!,
                Curp = user.Curp!,
                CuentaBloqueada = user.CuentaBloqueada ?? false,
                DadoDeBaja = user.DadoDeBaja ?? true,
                UltimaSesion = user.UltimaSesion ?? DateTime.MinValue,
                Rol = user.Rol!
            });
        }

        public async Task<Response> EditUserById(EditarUsuarioDTO dto)
        {
            var user = await context.Usuarios.FindAsync(dto.Id);
            if (user is null)
                return new Response(false, "Usuario no encontrado");

            user.NombreCompleto = dto.NombreCompleto;
            user.Correo = dto.Correo;
            user.Curp = dto.Curp;
            user.CuentaBloqueada = dto.CuentaBloqueada;
            user.DadoDeBaja = dto.DadoDeBaja;
            user.UltimaSesion = dto.UltimaSesion;
            user.Rol = dto.Rol;

            if (!string.IsNullOrWhiteSpace(dto.Contrasena))
            {
                user.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);
            }

            context.Usuarios.Update(user);
            await context.SaveChangesAsync();

            return new Response(true, "Usuario actualizado");
        }
    }
}
