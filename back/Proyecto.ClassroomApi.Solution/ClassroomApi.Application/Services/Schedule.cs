using ClassroomApi.Application.DTOs;
using Llaveremos.SharedLibrary.Logs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Services
{
    public class Schedule : ISchedule
    {
        private readonly HttpClient _httpClient;
        public Schedule(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ScheduleDTO> GetScheduleByUserId(string userId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"schedule/horarioPorUsuario/{userId}");
                if (!response.IsSuccessStatusCode)
                    return null!;

                var schedule = await response.Content.ReadFromJsonAsync<ScheduleDTO>();
                return schedule!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw new Exception("Error en el servicio para obtener el horario de un alumno desde classroom api");
            }
        }
    }
}
