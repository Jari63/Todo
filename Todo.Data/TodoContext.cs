using Microsoft.EntityFrameworkCore;
using System;
using Todo.Core.Models;

namespace Todo.Data
{
	public class TodoContext : DbContext
	{
		public TodoContext(DbContextOptions<TodoContext> options) : base(options)
		{
		}

		public TodoContext() : base(GetOptions())
		{
		}

		private static DbContextOptions GetOptions()
		{
			var optionsBuilder = new DbContextOptionsBuilder<TodoContext>();
			optionsBuilder.UseSqlServer(@"Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Todo.Data.TodoContext;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True;");
			//return new TodoContext(optionsBuilder.Options);
			return optionsBuilder.Options;
		}

		public DbSet<ToDo> ToDos { get; set; }
	}
}
