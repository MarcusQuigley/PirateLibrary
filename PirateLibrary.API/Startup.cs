using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PirateLibrary.API.Services;

namespace PirateLibrary.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers(setupAction =>
            {
                setupAction.ReturnHttpNotAcceptable = true;
            })
                .AddXmlDataContractSerializerFormatters()
                .AddNewtonsoftJson(setup =>
                {
                    setup.SerializerSettings.ContractResolver =
                    new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
                })
                .ConfigureApiBehaviorOptions(setupAction =>
                {
                    setupAction.InvalidModelStateResponseFactory = context =>
                   {
                       var problemDetails = new ValidationProblemDetails(context.ModelState)
                       {
                           Title = "One or more model validation errors occurred,",
                           Type = "http://coursel.com/blah",
                           Status = StatusCodes.Status422UnprocessableEntity,
                           Detail = "See errors property for details",
                           Instance = context.HttpContext.Request.Path
                       };
                       problemDetails.Extensions.Add("traceId", context.HttpContext.TraceIdentifier);
                       return new UnprocessableEntityObjectResult(problemDetails)
                       {
                           ContentTypes = { "application/problem+json" }
                       };
                   };
                })
                ;
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IPirateRepository, PirateRepository>();

            services.AddDbContext<PirateLibraryContext>(options =>
            {
                  options.UseSqlite("Data Source=memory;");
               // options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=PirateLibraryDb2;Trusted_Connection=True;");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapGet("/", async context =>
                //{
                //    await context.Response.WriteAsync("Hello World!");
                //});
                endpoints.MapControllers();
            });
        }
    }
}
