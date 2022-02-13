using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SDBlog.BusinessLayer.Interfaces.Files;
using SDBlog.BusinessLayer.Services.Files;
using SDBlog.BusinessLayer.Validators.Base;
using SDBlog.BusinessLayer.Validators.Users;
using SDBlog.DataModel.Entities.Users;

namespace SDBlog.Api
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
            services.ConfigureCors();
            services.AddControllers().AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<UserValidator>());
            services.ConfigureDbContext(Configuration);
            services.ServicesImplementations();
            services.RepositoriesImplementations();
            services.ConfigureAutomapper();
            
            services.ConfigureAddControllers();
            services.ConfigureMail(Configuration);
            services.ConfigureFile(Configuration);

            services.ConfigureAuthentication(Configuration);
            services.ConfigureSwagger(Configuration);

            //Esto va en un metodo aparte [TODO]
            services.AddScoped<IValidator<User>, UserValidator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            app.ConfigureSwaggerMiddleWare();
        }
    }
}
