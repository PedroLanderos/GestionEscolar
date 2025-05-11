using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Domain.Entities;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> RegistrarAlumno(UsuarioDTO appUserDTO);
        Task<Response> RegistrarAdministrador(UsuarioDTO appUserDTO);
        Task<Response> RegistrarProfesor(UsuarioDTO appUserDTO);
        Task<Response> Login(IniciarSesionDTO loginDTO);
        Task<IEnumerable<ObtenerUsuarioDTO>> GetAllUsers();
        Task<Usuario> GetByAsync(Expression<Func<Usuario, bool>> predicate);
        Task<Response> EditarUsuario(EditarUsuarioDTO dto);
        
    }
}
