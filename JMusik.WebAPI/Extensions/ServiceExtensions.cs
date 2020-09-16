using JMusik.Data.Contratos;
using JMusik.Data.Repositorios;
using JMusik.Models;
using JMusik.WebApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMusik.WebAPI.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            //Creamos las politicas e acceso
            services.AddCors(options => {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });
        }

        public static void ConfigureJWT(this IServiceCollection services, IConfiguration configuration)
        {
            //Configurando el uso del standar JWT
            //Accedemos a la sección JWTSettings del archivo appsettings.json
            var jwtSettings = configuration.GetSection("JwtSettings");
            //Obtenemos la clave secreta
            string secretKey = jwtSettings.GetValue<string>("SecretKey");
            //Obtenemos el tiempo de vida en minutos del JWT
            int minutes = jwtSettings.GetValue<int>("MinutesToExpiration");
            //Obtenemos el valor del emiso del token en JwtSettings:Issuer
            string issuer = jwtSettings.GetValue<string>("Issuer");
            //Obtenemos el valor de la audiencia a la que esta destinado el Jwt en JwtSetting
            string audience = jwtSettings.GetValue<string>("Audience");

            var key = Encoding.ASCII.GetBytes(secretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.FromMinutes(minutes)
                };
            });
        }

        public static void ConfigureDependencies(this IServiceCollection services)
        {
            //Mapeo de las interfaces del repository
            services.AddScoped<IRepositorioGenerico<Perfil>, RepositorioPerfiles>();
            services.AddScoped<IProductosRepositorio, ProductosRepositorio>();
            services.AddScoped<IOrdenesRepositorio, RepositorioOrdenes>();
            services.AddScoped<IUsuariosRepositorio, RepositorioUsuarios>();
            services.AddScoped<IPasswordHasher<Usuario>, PasswordHasher<Usuario>>(); //Crea la encriptación de los campos

            //Solo existira una instancia de la clase TokenService
            services.AddSingleton<TokenService>();
        }
    }
}
