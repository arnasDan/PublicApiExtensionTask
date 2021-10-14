using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PublicApiExtension.Clients.NagerDate;
using PublicApiExtension.Clients.NagerDate.Client;
using PublicApiExtension.Services.Exceptions;
using PublicApiExtension.Services.Repositories.Events;
using PublicApiExtension.Services.Services.Events;
using PublicApiExtension.Services.Services.PublicHolidays;
using PublicApiExtension.Storage;

namespace PublicApiExtension
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
            services.AddProblemDetails(ConfigureProblemDetails);

            services.AddControllers();

            services.AddDbContext<SchedulerDatabaseContext>(options => options.UseSqlServer(Configuration.GetSection("DatabaseConnectionString").Value));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "PublicApiExtension", Version = "v1" });
            });

            services.AddSingleton<INagerDateClient, NagerDateClient>(_ => new NagerDateClient(new System.Net.Http.HttpClient(), Configuration.GetSection("NagerDateApiUrl").Value));
            services.AddSingleton<IHolidayProvider, NagerDateHolidayProvider>();
            services.AddSingleton<IPublicHolidayService, PublicHolidayService>(p => new PublicHolidayService(p.GetRequiredService<IHolidayProvider>(), Configuration.GetSection("CountryCode").Value));
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventService, EventService>();
        }

        private static void ConfigureProblemDetails(ProblemDetailsOptions options)
        {
            options.Map<DomainException>(exception => new ProblemDetails
            {
                Status = exception.ErrorCode switch
                {
                    DomainErrorCode.InvalidOperation => StatusCodes.Status400BadRequest,
                    DomainErrorCode.NotFound => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status500InternalServerError
                },
                Title = exception.ErrorCode.ToString(),
                Detail = exception.Message
            });
            options.IncludeExceptionDetails = (context, exception) => !(exception is DomainException) && context.RequestServices.GetRequiredService<IHostEnvironment>().IsDevelopment();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "PublicApiExtension v1"));
            }

            app.UseProblemDetails();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
