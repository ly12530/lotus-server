using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Core.Domain;
using Core.DomainServices;
using Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using RestApi.Services;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;

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
            services.AddCors();
            services.AddHttpClient();
            
            services.AddDbContext<LotusDbContext>(options => options.UseNpgsql(
                Configuration.GetConnectionString("Default")));

            services.AddScoped<IRequestRepository, RequestRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            
            services.AddSingleton<AddressService>();
            services.AddSingleton<NotificationService>();
            
            services.AddAuthorization(options =>
            {
                // Customer
                options.AddPolicy("CustomerOnly", policy =>
                {
                    policy.RequireClaim("Role", Role.Customer.GetDisplayName());
                });
                
                // Member
                options.AddPolicy("MemberOnly", policy =>
                {
                    policy.RequireClaim("Role", Role.Member.GetDisplayName());
                });
                
                // PenningMaster
                options.AddPolicy("PenningMasterOnly", policy =>
                {
                    policy.RequireClaim("Role", Role.PenningMaster.GetDisplayName());
                });
                
                // BettingCoordinator
                options.AddPolicy("BettingCoordinatorOnly", policy =>
                {
                    policy.RequireClaim("Role", Role.BettingCoordinator.GetDisplayName());
                });
                
                // Instructor
                options.AddPolicy("InstructorOnly", policy =>
                {
                    policy.RequireClaim("Role", Role.Instructor.GetDisplayName());
                }); 
                
                // Administrator
                options.AddPolicy("AdminOnly", policy =>
                {
                        policy.RequireClaim("Role", Role.Administrator.GetDisplayName());
                });
                
                // Admins & Bettingmasters
                options.AddPolicy("AdminAndBettingMasterOnly", policy =>
                {
                    policy.RequireClaim("Role", new [] {Role.Administrator.GetDisplayName(), Role.BettingCoordinator.GetDisplayName()});
                });
            });
            
            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters    
                    {    
                        ValidateIssuer = true,    
                        ValidateAudience = true,    
                        ValidateLifetime = true,    
                        ValidateIssuerSigningKey = true,    
                        ValidIssuer = Configuration["Jwt:Issuer"],    
                        ValidAudience = Configuration["Jwt:Issuer"],    
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))    
                    };
                });
            
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
            services.AddSwaggerGen(c => 
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "LOTUS RESTapi",
                    Description = "A RESTful backend API for the LOTUS 2021 project",
                    Contact = new OpenApiContact
                    {
                        Name = "Crypit",
                        Email = String.Empty,
                        Url = new Uri("https://github.com/Crypit-Coders-Inc")
                    }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var jwtSecurityScheme = new OpenApiSecurityScheme()
                {
                    Name = "Bearer",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };
                
                c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "LOTUS RESTapi v1");
                c.RoutePrefix = String.Empty;
                c.DocumentTitle = "LOTUS RESTapi";
                c.DefaultModelsExpandDepth(-1);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(c => c
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowAnyOrigin()
            );

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}