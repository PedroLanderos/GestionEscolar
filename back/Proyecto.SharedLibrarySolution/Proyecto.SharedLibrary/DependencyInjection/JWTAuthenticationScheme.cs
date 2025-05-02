using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llaveremos.SharedLibrary.DependencyInjection
{
    public static class JWTAuthenticationScheme
    {
        public static IServiceCollection AddJWTSchemeCollection(this IServiceCollection services, IConfiguration config)
        {
            //se agrega el servicio de autenticacion de jwt 
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("Bearer", options =>
                {
                    //clave secreta del jwt
                    var key = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
                    //emisor del token
                    string issuer = config.GetSection("Authentication:Issuer").Value!;
                    //destinatario del token
                    string audience = config.GetSection("Authentication:Audience").Value!;

                    //desactivado para desarrollo (no se usa https)
                    options.RequireHttpsMetadata = false;
                    //guardar el token para que la app pueda usarlo
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = false,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = issuer,
                        ValidAudience = audience,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    };
                });
            return services;
        }

    }
}
