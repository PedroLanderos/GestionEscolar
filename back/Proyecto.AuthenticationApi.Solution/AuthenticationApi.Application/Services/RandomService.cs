using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Services
{
    public class RandomService : IRandomService
    {
        public string GenerateHomoclave()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var rng = new System.Random();
            return new string(Enumerable.Range(0, 3).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }

        public string GenerateRandomPassword()
        {
            const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_+-=[]{}|;:,.<>?";
            var rng = new System.Random();
            return new string(Enumerable.Range(0, 10).Select(_ => chars[rng.Next(chars.Length)]).ToArray());
        }
    }
}
