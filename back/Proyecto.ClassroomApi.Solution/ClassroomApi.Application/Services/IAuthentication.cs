﻿using ScheduleApi.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassroomApi.Application.Services
{
    public interface IAuthentication
    {
        Task<UserDto> ValidateUser(string id);
    }
}
