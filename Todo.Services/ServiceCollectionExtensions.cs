using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using Todo.Data;

namespace Todo.Services
{
	public static class ServiceCollectionExtensions
	{
		public static void AddDataAccessServices(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<TodoContext>(options =>
			  options.UseSqlServer(connectionString));
		}
	}
}
