using SubjectsApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.Services
{
    public interface IUser
    {
        Task<UserDTO> ObtenerDocente(int id);
    }
}
