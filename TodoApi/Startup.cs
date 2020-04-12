using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Todo.Data;
using Todo.Services;
using Microsoft.OpenApi.Models;
using System.IO;
using System.Reflection;

namespace TodoApi
{
	public class Startup
	{
		readonly string MyAllowSpecificOrigins = "MyAllowSpecificOrigins";
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddCors(options =>
			{
				options.AddPolicy(MyAllowSpecificOrigins,
				builder =>
				{
					builder.WithOrigins("http://localhost:3000");
					builder.WithHeaders("Access-Control-Allow-Origin, Content-Type", "content-type", "Access-Control-Allow-Headers");
					builder.WithMethods("GET, POST, PUT, DELETE, OPTIONS");
				});
			});

			AddSwaggerDocs(services);

			services.AddDataAccessServices(Configuration.GetConnectionString("Default"));
			services.AddTransient<ITodoService, TodoService>();
		}

		private void AddSwaggerDocs(IServiceCollection services)
		{
			// Register the Swagger generator, defining 1 or more Swagger documents
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "ToDo API",
					Description = "A simple example ASP.NET Core Web API",
					TermsOfService = new Uri("https://example.com/terms"),
					Contact = new OpenApiContact
					{
						Name = "Jari Kotro",
						Email = "jari.kotro@gmail.com",
						//Url = new Uri("https://twitter.com/spboyer"),
					},
					License = new OpenApiLicense
					{
						Name = "Use under LICX",
						Url = new Uri("https://example.com/license"),
					}
				});

				// Set the comments path for the Swagger JSON and UI.
				var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
				var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
				c.IncludeXmlComments(xmlPath);
			});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				//app.UseDeveloperExceptionPage();
				app.UseExceptionHandler("/error-local-development");
			}
			else
			{
				app.UseExceptionHandler("/error");
			}

			#region Swagger
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
				// to show Swagger UI at the app's root
				c.RoutePrefix = string.Empty;
			});
			#endregion

			app.UseRouting();

			app.UseCors(builder =>
			{
				//builder.AllowAnyOrigin();
				builder.WithOrigins(MyAllowSpecificOrigins);
				builder.AllowAnyHeader();
				builder.AllowAnyMethod();
			});


			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}
