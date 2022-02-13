using AutoMapper;
using Boundaries.Database.Repositories;
using Core.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SDBlog.BusinessLayer.Interfaces;
using SDBlog.BusinessLayer.Interfaces.Files;
using SDBlog.BusinessLayer.Interfaces.Mails;
using SDBlog.BusinessLayer.Interfaces.Users;
using SDBlog.BusinessLayer.Services.Files;
using SDBlog.BusinessLayer.Services.Mails;
using SDBlog.BusinessLayer.Services.Users;
using SDBlog.BusinessLayer.Settings;
using SDBlog.DataModel.Context;
using System;
using System.Linq;
using SDBlog.Services.Interfaces.SSO;
using SDBlog.Services.Implementations.SSO;
using SDBlog.BusinessLayer.Interfaces.List;
using SDBlog.BusinessLayer.Services.Listas;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Collections.Generic;

namespace SDBlog.Api
{
    public static class StartupExtension
    {

        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<MainDbContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("MainDatabase"),
                b => b.MigrationsAssembly("MEPyDBase.Api")));

        }

        public static void ServicesImplementations(this IServiceCollection services)
        {
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IFileService, FileService>();
            services.AddTransient<FileDirectoryService>();
            //Auth
            services.AddSingleton<IAuth, Auth>();
            //Listas Genericas
            services.AddTransient<IListService, ListService>();
        }

        public static void RepositoriesImplementations(this IServiceCollection services)
        {
            services.AddTransient<IFileRepository, FileRepository>();
            services.AddTransient<IFileTypeRepository, FileTypeRepository>();
        }

        public static void ConfigureAutomapper(this IServiceCollection services)
        {
            var config = new MapperConfiguration(cfg =>
            {
                var mainAssembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(c => c.GetName().Name == "MEPyDBase.BusinessLayer");
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
                    Title = "MEPyDBase.API",
                    Version = "v3",
                    Description = "API de Comunicación del Core MEPyD",
                    Contact = new OpenApiContact
                    {
                        Name = "Ministerio de Economía Planificación y Desarrollo (MEPyD).",
                        Email = "info@economia.gob.do",
                        Url = new Uri("https://mepyd.gob.do/"),
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
            services.Configure<MailSettings>(configuration.GetSection(nameof(MailSettings)));
        }

        public static void ConfigureFile(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<FileSettings>(configuration.GetSection(nameof(FileSettings)));

            //string principalPath = Configuration["File:PrincipalPath"];
            //string principalFolderName = Configuration["File:PrincipalFolderName"];
            //string maxFilesSizeInMb = Configuration["File:MaxFilesSizeInMb"];

            //IEnumerable<string> allowedExtensions = Configuration.GetSection("File:AllowedExtensions").GetChildren().Select(extension => extension.Value).ToList();

            //return new FileConfiguration
            //{
            //    PrincipalPath = principalPath,
            //    PrincipalFolderName = principalFolderName,
            //    AllowedExtensions = allowedExtensions,
            //    MaxFileSize = Convert.ToInt64(maxFilesSizeInMb)
            //};
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
