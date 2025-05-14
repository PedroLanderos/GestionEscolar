using Llaveremos.SharedLibrary.Logs;
using Microsoft.IdentityModel.Abstractions;
using SubjectsApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace SubjectsApi.Application.Services
{
    public class User : IUser
    {
        private readonly HttpClient _httpClient;
        public User(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<UserDTO> ObtenerDocente(string id)
        {
			try
			{
                var response = await _httpClient.GetAsync($"obtenerUsuarioPorId/{id}");

                if (!response.IsSuccessStatusCode)
                    return null!;

                var userDto = await response.Content.ReadFromJsonAsync<UserDTO>();

                if(userDto!.Rol != "Docente")
                    throw new Exception("El usuario no es un docente");

                return userDto;
            }
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error al obtener al docente desde el servicio en materias");
			}
        }
    }
}
