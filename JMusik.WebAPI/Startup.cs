using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using AutoMapper;
using JMusik.Data;
using JMusik.Data.Contratos;
using JMusik.Data.Repositorios;
using JMusik.Models;
using JMusik.WebApi.Services;
using JMusik.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Serilog;

namespace JMusik.WebAPI
{
    public class Startup
    {
        public IConfiguration _configuration;
        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            this._configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Startup));
            //AddXmlDataContractSerializerFormatters => Nos permite aceptar y enviar datos XML
            //ReturnHttpNotAcceptable => Retorna un código de estado 406 [Formato no aceptado]
            services.AddControllers(config =>
            {
                config.ReturnHttpNotAcceptable = true;
            }).AddXmlDataContractSerializerFormatters();

            services.AddDbContext<TiendaDbContext>(options =>
            {
                options.UseSqlServer(this._configuration.GetConnectionString("TiendaDb"));
            });
            

            services.ConfigureDependencies();
            services.ConfigureJWT(this._configuration);
            services.ConfigureCors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

             
            app.UseRouting();
            app.UseCors("CorsPolicy"); //Asignamos la politica creada
            app.UseAuthentication(); //Habilitamos la validación de autenticación
            app.UseAuthorization(); //Habilitamos la validación de sesion
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World! Hola Mundo!");
                //});
            }); 
        }
    }
}
