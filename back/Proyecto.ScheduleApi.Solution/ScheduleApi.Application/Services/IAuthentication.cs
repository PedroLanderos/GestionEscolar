using ScheduleApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.Services
{
    public interface IAuthentication
    {
        Task<UserDto> ValidateUser(int id); //metodo que valida si un usuario existe a traves de authenticaion api
    }
}
