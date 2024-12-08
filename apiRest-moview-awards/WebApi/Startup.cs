using Application;
using Application.Interfaces;
using Domain.Entities;
using LiteDB;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Persistence;
using Persistence.ReaderFile.csv;
using Persistence.ReaderFile.Interface;
using Persistence.Repository;
using System.Threading.Tasks;

namespace ApiRestMovieAwards.BFF
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
			#region Swagger
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = "ApiRestMovieAwards",
				});

			});
			#endregion
			#region Api Versioning
			// Add API Versioning to the Project
			services.AddApiVersioning(config =>
			{
				// Specify the default API Version as 1.0
				config.DefaultApiVersion = new ApiVersion(1, 0);
				// If the client hasn't specified the API version in the request, use the default API version number 
				config.AssumeDefaultVersionWhenUnspecified = true;
				// Advertise the API versions supported for the particular endpoint
				config.ReportApiVersions = true;
			});
			#endregion
			services.AddApplication();
			services.AddPersistence();
			services.AddControllers();

			services.AddTransient<IReaderCsvMoviesService, ReaderCsvMoviesService>();

			services.AddSingleton<ILiteDatabase>(provider =>
			{
				return new LiteDatabase(":memory:");  
			});

		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();
			#region Swagger
			// Enable middleware to serve generated Swagger as a JSON endpoint.
			app.UseSwagger();

			// Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
			// specifying the Swagger JSON endpoint.
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "ApiRestMovieAwards");
			});
			#endregion
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			var readerCsvMoviesService = app.ApplicationServices.GetRequiredService<IReaderCsvMoviesService>();
			lifetime.ApplicationStarted.Register(() =>
			{
				Task.Run(async () =>
				{
					await readerCsvMoviesService.RunAsync();
				});
			});
		}
	}
}
