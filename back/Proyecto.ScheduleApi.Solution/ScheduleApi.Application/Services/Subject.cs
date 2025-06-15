using Llaveremos.SharedLibrary.Logs;
using Microsoft.IdentityModel.Abstractions;
using ScheduleApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.Services
{
    public class Subject : ISubject
    {
		private readonly HttpClient _httpClient;

        public Subject(HttpClient s)
        {
            _httpClient = s;
        }
        public async Task<SubjectDTO> ObtenerMateriaPorCodigo(string codigo)
        {
			try
			{
                var response = await _httpClient.GetAsync($"obtenerPorCodigo/{codigo}");
                if (!response.IsSuccessStatusCode)
                    return null!;

                var subject = await response.Content.ReadFromJsonAsync<SubjectDTO>();
                return subject!;

            }
			catch (Exception ex)
			{
				LogException.LogExceptions(ex);
				throw new Exception("Error en servicio para obtener la materia");
			}
        }
    }
}
