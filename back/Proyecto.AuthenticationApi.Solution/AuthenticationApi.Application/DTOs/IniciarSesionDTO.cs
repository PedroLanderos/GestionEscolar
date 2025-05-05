using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.DTOs
{
    public record IniciarSesionDTO(string Email, string Password);
}
