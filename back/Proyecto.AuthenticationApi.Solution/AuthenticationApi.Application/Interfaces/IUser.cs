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
        Task<Response> Register(UsuarioDTO appUserDTO);
        Task<Response> Login(IniciarSesionDTO loginDTO);
        Task<ObtenerUsuarioDTO> GetUser(int usuarioID);
        Task<IEnumerable<ObtenerUsuarioDTO>> GetAllUsers();
        Task<Response> EditUserById(EditarUsuarioDTO editUserDTO);
    }
}
