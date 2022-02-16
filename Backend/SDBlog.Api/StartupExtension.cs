using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Services;
using SDBlog.DataModel.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SDBlog.Api
{
    public static class StartupExtension
    {

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MainDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("MainDatabase"),
                b => b.MigrationsAssembly("SDBlog.Api")));

        }

        public static void InternalServicesImplementations(this IServiceCollection services)
        {
            services.AddTransient<ICategoryService, CategoryService>();
            services.AddTransient<IPostService, PostService>();
            services.AddTransient<IPostTagService, PostTagService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IUserService, UserService>();
        }

        public static void RepositoriesImplementations(this IServiceCollection services)
        {
            
        }

        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var mainAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(c => c.GetName().Name == "SDBlog.BusinessLayer");
                cfg.AddMaps(mainAssembly);
                cfg.AllowNullCollections = true;
            });
            var mapper = config.CreateMapper();
            services.AddSingleton(mapper);
        }

        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(build =>
            {
                build.AddPolicy(nameof(Startup), _ => _.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            });
        }

        public static void ConfigureAddControllers(this IServiceCollection services)
        {
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

        public static void ConfigureSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v3", new OpenApiInfo
                {
                    Title = "SDBlog.API",
                    Version = "v1",
                    Description = "API de Comunicación del Core SDBlog",
                    Contact = new OpenApiContact
                    {
                        Name = "Blog de Shift Dominicana.",
                        Email = "info@shiftdo.com.do",
                        Url = new Uri("https://shiftdo.com.do/"),
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement() {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });
                c.AddServer(new OpenApiServer()
                {
                    Url = configuration["Swagger:ServerUrl"]
                });
            });

        }

        public static void ConfigureSwaggerMiddleWare(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v3/swagger.json", "MEPyDBase API");
                opt.RoutePrefix = "swagger";
            });
        }

        public static void ConfigureMail(this IServiceCollection services, IConfiguration configuration)
        {
            
        }

        public static void ConfigureFile(this IServiceCollection services, IConfiguration configuration)
        {


            
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new
                    SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes
                    (configuration["Jwt:Key"]))
                };
            });
            services.AddAuthorization();
        }
    }
}
