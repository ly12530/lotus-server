using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace RestApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<LotusDbContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString("Default")));

            services.AddScoped<IRequestRepository, RequestRepository>();

            services.AddControllers();
            services.AddSwaggerGen(c => 
                { c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "LOTUS RESTapi",
                        Description = "A RESTful backend API for the LOTUS 2021 project",
                        Contact = new OpenApiContact
                        {
                            Name = "GitHub",
                            Email = String.Empty,
                            Url = new Uri("https://github.com/Crypit-Coders/Inc")
                        }
                    }); 
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LOTUS RESTapi v1");
                c.InjectStylesheet("/css/custom.css");
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}