using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Infrastructure.Data;
using Llaveremos.SharedLibrary.Responses;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Serilog;
using Llaveremos.SharedLibrary.Logs;
using AuthenticationApi.Application.Services;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UsuarioRepository(AuthenticationDbContext context, IEmail emailService, IConfiguration config, IRandomService randomService, ISolicitudRepository solicitudes) : IUser
    {
        public async Task<IEnumerable<ObtenerUsuarioDTO>> GetAllUsers()
        {
            try
            {
                var users = await context.Usuarios.ToListAsync();
                return users.Select(user => new ObtenerUsuarioDTO
                {
                    Id = user.Id,
                    NombreCompleto = user.NombreCompleto!,
                    Correo = user.Correo!,
                    Curp = user.Curp!,
                    CuentaBloqueada = user.CuentaBloqueada ?? false,
                    DadoDeBaja = user.DadoDeBaja ?? false,
                    UltimaSesion = user.UltimaSesion ?? DateTime.MinValue,
                    Rol = user.Rol!
                });

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener todos los usuarios en el repositorio");
            }
        }

        public async Task<Usuario> GetUser(string identificador)
        {
            try
            {
                var user = await context.Usuarios
                    .FirstOrDefaultAsync(u => u.Id == identificador || u.Correo == identificador);

                if (user is null) return null!;
                return user;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al buscar usuario en el repository");
            }
        }

        public async Task<Response> Login(IniciarSesionDTO loginDTO)
        {
            try
            {
                var usuario = await GetUser(loginDTO.Identificador);
                if (usuario is null) return new Response(false, "Identificador invalido");

                var contrasenaUsuario = usuario.Contrasena;
                bool isValid = false;

                if (contrasenaUsuario!.StartsWith("$2"))
                    isValid = BCrypt.Net.BCrypt.Verify(loginDTO.Password, contrasenaUsuario);
                else
                {
                    isValid = contrasenaUsuario == loginDTO.Password;
                    if (isValid)
                    {
                        usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(contrasenaUsuario);
                        context.Update(usuario);
                        await context.SaveChangesAsync();
                    }
                }

                if (!isValid) return new Response(false, "Credenciales invalidas");
                if (usuario.CuentaBloqueada == true) return new Response(false, "Usuario bloqueado");

                string token = GenerateToken(usuario);
                return new Response(true, token);
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al iniciar sesion desde repository");
            }
        }

        private string GenerateToken(Usuario user)
        {
            var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(key);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.NombreCompleto!),
                new(ClaimTypes.Email, user.Correo!),
                new("UserId", user.Id!)
            };

            if (!string.IsNullOrEmpty(user.Rol) && !Equals("string", user.Rol))
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

        public async Task<Response> RegistrarAlumno(UsuarioDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Curp) || dto.Curp.Length < 10)
                    return new Response(false, "CURP inválida");

                string baseCurp = dto.Curp.Substring(0, 10).ToUpper();
                string alumnoId = $"C{baseCurp}";
                string homoclave = randomService.GenerateHomoclave();
                string padreId = $"C{baseCurp}{homoclave}";

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
                    Correo = dto.Correo,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(padrePass),
                    Curp = dto.Curp.ToUpper(),
                    CuentaBloqueada = false,
                    DadoDeBaja = false,
                    UltimaSesion = null,
                    Rol = "Padre"
                };

                context.Usuarios.AddRange(alumno, padre);
                await context.SaveChangesAsync();

                string subject = "Datos de acceso - Sistema Escolar";
                string body = $@"
                <p>Hola <strong>{padre.NombreCompleto}</strong>,</p>
                <p>Tu solicitud ha sido procesada exitosamente. A continuación, se encuentran los datos de acceso:</p>
                <hr/>
                <p><strong>Alumno:</strong><br/>
                Usuario: {alumno.Id}<br/>
                Contraseña: {alumnoPass}</p>

                <p><strong>Padre/Tutor:</strong><br/>
                Usuario: {padre.Id}<br/>
                Contraseña: {padrePass}</p>

                <p>⚠️ Por seguridad, te recomendamos iniciar sesión y cambiar la contraseña en cuanto sea posible.</p>
                <br/>
                <p>Atentamente,<br/>Sistema Escolar</p>";

                try
                {
                    await emailService.EnviarCorreoAsync(padre.Correo!, subject, body);
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                    return new Response(true, "Usuarios registrados, pero no se pudo enviar el correo.");
                }

                try
                {
                    await solicitudes.MarcarSolicitudComoProcesada(dto.Curp);
                }
                catch (Exception ex)
                {
                    LogException.LogExceptions(ex);
                }

                return new Response(true, "Usuarios registrados exitosamente y correo enviado.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error mientras se creaba el usuario en el repositorio");
            }
        }

        public async Task<Response> RegistrarAdministrador(UsuarioDTO dto)
        {
            try
            {
                var result = context.Usuarios.Add(new Usuario()
                {
                    Id = dto.Id,
                    NombreCompleto = dto.NombreCompleto,
                    Correo = dto.Correo,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                    Curp = dto.Curp.ToUpper(),
                    CuentaBloqueada = false,
                    DadoDeBaja = false,
                    UltimaSesion = null,
                    Rol = "Administrador"
                });

                await context.SaveChangesAsync();

                if (result is null) return new Response(false, "Error al registrar adminstrador");
                return new Response(true, "Administrador registrado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al registrar adminstrador");
            }
        }

        public async Task<IEnumerable<UsuarioDTO>> FiltrarPorGrado(int grado)
        {
            try
            {
                var alumnos = await GetByAsync(x => x.Rol == "Alumno");
                var curpsAlumnos = alumnos.Select(a => a.Curp!.ToUpper()).ToList();

                var solicitudesFiltradas = await context.SolicitudesAltas
                   .Where(s => curpsAlumnos.Contains(s.CurpAlumno.ToUpper()) && s.Grado == grado)
                   .ToListAsync();

                var curpsFiltrados = solicitudesFiltradas.Select(s => s.CurpAlumno.ToUpper()).ToHashSet();

                var alumnosFiltrados = alumnos
                .Where(a => curpsFiltrados.Contains(a.Curp!.ToUpper()))
                .Select(a => new UsuarioDTO(
                    a.Id!,
                    a.NombreCompleto!,
                    a.Correo!,
                    a.Contrasena!,
                    a.Curp!,
                    a.CuentaBloqueada ?? false,
                    a.DadoDeBaja ?? false,
                    a.UltimaSesion ?? DateTime.MinValue,
                    a.Rol!
                ));

                return alumnosFiltrados;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        public async Task<IEnumerable<Usuario>> GetByAsync(Expression<Func<Usuario, bool>> predicate)
        {
            try
            {
                var usuario = await context.Usuarios.Where(predicate).ToListAsync()!;
                return usuario is not null ? usuario : null!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en metodo GetBy en repository");
            }
        }

        public async Task<Response> EditarUsuario(EditarUsuarioDTO dto)
        {
            try
            {
                var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == dto.Id);
                if (usuario is null)
                    return new Response(false, "Usuario no encontrado");

                usuario.NombreCompleto = dto.NombreCompleto;
                usuario.Correo = dto.Correo;
                usuario.Curp = dto.Curp;
                usuario.CuentaBloqueada = dto.CuentaBloqueada;
                usuario.DadoDeBaja = dto.DadoDeBaja;
                usuario.UltimaSesion = dto.UltimaSesion;
                usuario.Rol = dto.Rol;

                if (!string.IsNullOrWhiteSpace(dto.Contrasena))
                    usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena);

                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();

                return new Response(true, "Usuario actualizado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al actualizar el usuario");
            }
        }

        public async Task<Response> RegistrarProfesor(UsuarioDTO dto)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(dto.Id) || (dto.Id != "BA" && dto.Id != "BB"))
                    return new Response(false, "El prefijo del ID debe ser 'BA' o 'BB'.");

                var random = new Random();
                var randomDigits = random.Next(100000, 999999);

                string idGenerado = $"{dto.Id}{randomDigits}";

                var result = context.Usuarios.Add(new Usuario()
                {
                    Id = idGenerado,
                    NombreCompleto = dto.NombreCompleto,
                    Correo = dto.Correo,
                    Contrasena = BCrypt.Net.BCrypt.HashPassword(dto.Contrasena),
                    Curp = dto.Curp.ToUpper(),
                    CuentaBloqueada = false,
                    DadoDeBaja = false,
                    UltimaSesion = null,
                    Rol = "Docente"
                });

                await context.SaveChangesAsync();

                if (result is null)
                    return new Response(false, "Error al registrar el profesor");

                return new Response(true, $"Profesor registrado exitosamente con ID: {idGenerado}");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al registrar un profesor en el repositorio");
            }
        }

        public async Task<ObtenerUsuarioDTO> ObtenerUsuarioPorId(string id)
        {
            try
            {
                var usuario = await context.Usuarios.FindAsync(id);
                if (usuario is null)
                    return null!;

                return new ObtenerUsuarioDTO
                {
                    Id = usuario.Id,
                    NombreCompleto = usuario.NombreCompleto!,
                    Correo = usuario.Correo!,
                    Curp = usuario.Curp!,
                    CuentaBloqueada = usuario.CuentaBloqueada ?? false,
                    DadoDeBaja = usuario.DadoDeBaja ?? false,
                    UltimaSesion = usuario.UltimaSesion ?? DateTime.MinValue,
                    Rol = usuario.Rol!
                };
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error al obtener el usuario desde el repositorio");
            }
        }

        public async Task<Response> EliminarUsuario(string id)
        {
            try
            {
                var usuario = await context.Usuarios.FindAsync(id);

                if (usuario is null)
                    return new Response(false, "Usuario no encontrado");

                context.Usuarios.Remove(usuario);
                await context.SaveChangesAsync();

                return new Response(true, "Usuario eliminado exitosamente");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }

        public async Task<Response> RecuperarContrasenaPorId(string id)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(id) || id.Length < 11 || !id.StartsWith("C"))
                    return new Response(false, "ID inválido para recuperación de contraseña.");

                // Buscar el usuario por ID
                var usuario = await context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
                if (usuario is null)
                    return new Response(false, "No se encontró un usuario con ese ID.");

                // Solo se permite recuperar contraseña a tutores/padres
                if (usuario.Rol?.ToLower() != "padre")
                    return new Response(false, "Solo los padres o tutores pueden recuperar su contraseña por este medio.");

                // Extraer base CURP del ID
                string curpBase = id.Substring(1, 10); // Sin la "C"

                // Buscar solicitud asociada a esa CURP
                var solicitud = (await solicitudes.GetBy(s => s.CurpAlumno.ToUpper().StartsWith(curpBase))).FirstOrDefault();
                if (solicitud is null)
                    return new Response(false, "No se encontró una solicitud asociada al usuario para enviar el correo.");

                // Generar nueva contraseña
                string nuevaContrasena = randomService.GenerateRandomPassword();
                usuario.Contrasena = BCrypt.Net.BCrypt.HashPassword(nuevaContrasena);

                context.Usuarios.Update(usuario);
                await context.SaveChangesAsync();

                // Enviar email al padre/tutor
                string subject = "Recuperación de contraseña - Sistema Escolar";
                string body = $@"
            <p>Hola <strong>{solicitud.NombrePadre}</strong>,</p>
            <p>Se ha generado una nueva contraseña para la cuenta asociada a tu hijo(a) <strong>{solicitud.NombreAlumno}</strong>.</p>
            <p><strong>ID de usuario:</strong> {usuario.Id}</p>
            <p><strong>Nueva contraseña:</strong> {nuevaContrasena}</p>
            <p>⚠️ Te recomendamos cambiar esta contraseña al iniciar sesión.</p>
            <br/>
            <p>Atentamente,<br/>Sistema Escolar</p>";

                await emailService.EnviarCorreoAsync(solicitud.CorreoPadre, subject, body);

                return new Response(true, "Contraseña actualizada y enviada correctamente al correo del padre/tutor.");
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                return new Response(false, "Error al recuperar la contraseña.");
            }
        }



    }
}
