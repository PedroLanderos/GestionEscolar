using AuthenticationApi.Application.DTOs;
using Llaveremos.SharedLibrary.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUser
    {
        Task<Response> RegistrarAlumno(UsuarioDTO appUserDTO);
        Task<Response> RegistrarAdministrador(UsuarioDTO appUserDTO);
        Task<Response> Login(IniciarSesionDTO loginDTO);
        
    }
}
