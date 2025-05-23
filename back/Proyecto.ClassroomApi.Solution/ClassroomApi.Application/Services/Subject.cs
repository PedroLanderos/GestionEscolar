﻿using Llaveremos.SharedLibrary.Logs;
using ScheduleApi.Application.DTOs;
using SubjectsApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Services
{
    public class Subject : ISubject
    {
        private readonly HttpClient _httpClient;

        public Subject(HttpClient http)
        {
            _httpClient = http;
        }
        public async Task<SubjectDTO> GetSubject(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"subject/{id}");
                if (!response.IsSuccessStatusCode)
                    return null!;

                var subject = await response.Content.ReadFromJsonAsync<SubjectDTO>();
                return subject!;
            }
            catch (Exception ex)
            {
                LogException.LogExceptions(ex);
                throw;
            }
        }
    }
}
