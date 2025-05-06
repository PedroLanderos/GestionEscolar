using Llaveremos.SharedLibrary.Logs;
using ScheduleApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace ScheduleApi.Application.Services
{
    public class Authentication : IAuthentication
    {
        private readonly HttpClient _httpClient;
        public Authentication(HttpClient client)
        {
            _httpClient = client;
        }
        public async Task<UserDto> ValidateUser(int id)
        {
            try
            {
                var getuser = await _httpClient.GetAsync($"/api/authentication/{id}");

                if (!getuser.IsSuccessStatusCode)
                    return null!;

                var user = await getuser.Content.ReadFromJsonAsync<UserDto>();
                return user!;

            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }
    }

}
