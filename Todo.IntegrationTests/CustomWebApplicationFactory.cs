using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Todo.Data;
using Todo.Services;

namespace Todo.IntegrationTests
{
	public class CustomWebApplicationFactory<TStartup>
		: WebApplicationFactory<TStartup> where TStartup : class
	{
		protected ITodoService todoService;

		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			builder.ConfigureServices(services =>
			{
				// Remove the app's ApplicationDbContext registration.
				var descriptor = services.SingleOrDefault(
					d => d.ServiceType ==
						typeof(DbContextOptions<TodoContext>));

				if (descriptor != null)
				{
					services.Remove(descriptor);
				}

				services.AddEntityFrameworkInMemoryDatabase();
				// Add ApplicationDbContext using an in-memory database for testing.
				services.AddDbContext<TodoContext>(options =>
				{
					options.UseInMemoryDatabase("InMemoryDbForTesting");
				});

				// Build the service provider.
				var sp = services.BuildServiceProvider();

				// Create a scope to obtain a reference to the database
				// context (ApplicationDbContext).
				using (var scope = sp.CreateScope())
				{
					var scopedServices = scope.ServiceProvider;
					var db = scopedServices.GetRequiredService<TodoContext>();
					var logger = scopedServices.GetRequiredService<ILogger<CustomWebApplicationFactory<TStartup>>>();
					todoService = scopedServices.GetRequiredService<ITodoService>();

					// Ensure the database is created.
					db.Database.EnsureCreated();

					try
					{
						// Seed the database with test data.
						Utilities.InitializeDbForTests(todoService);
					}
					catch (Exception ex)
					{
						logger.LogError(ex, "An error occurred seeding the " +
							"database with test messages. Error: {Message}", ex.Message);
					}
				}
			});
		}
	}
}
