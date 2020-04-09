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

		public DbSet<ToDo> ToDos { get; set; }
	}
}
