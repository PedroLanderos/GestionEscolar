using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Services
{
    public interface IRandomService
    {
        string GenerateHomoclave();
        string GenerateRandomPassword();
    }
}
