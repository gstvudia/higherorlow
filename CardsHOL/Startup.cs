using CardsHOL.Api.Data;
using CardsHOL.Api.Services;
using CardsHOL.Api.Services.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using AutoMapper;
using CardsHOL.Api.Mapping;

namespace CardsHOL.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(MapperProfiles).Assembly);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(Configuration["Api:Version"],
                    new OpenApiInfo
                    {
                        Title = Configuration["Api:Name"],
                        Description = Configuration["Api:Description"],
                        Version = Configuration["Api:Version"]
                    });


                // Set the comments path for the Swagger JSON and UI.
                var xmlPath = Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                c.IncludeXmlComments(xmlPath);
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IgnoreNullValues = true;
            })
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );
            services.AddTransient<IGameService, GameService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger(c => 
            {
                c.RouteTemplate = "api-docs/{documentName}/swagger.json";
            });

            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = Configuration["Api:Name"];
                c.SwaggerEndpoint($"/api-docs/{Configuration["Api:Version"]}/swagger.json", $"{Configuration["Api:Name"]} {Configuration["Api:Version"]}");
                c.RoutePrefix = String.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
