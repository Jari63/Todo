using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Data;

namespace Todo.Services
{
	public class TodoService : ITodoService
	{
		private TodoContext _db;

		public TodoService(TodoContext db)
		{
			_db = db;
		}

		/// <summary>
		/// Get all tasks
		/// </summary>
		/// <returns></returns>
		public Task<List<ToDo>> GetAll()
		{
			List<ToDo> todos = new List<ToDo>();
			foreach (var todo in _db.ToDos)
			{
				todos.Add(todo);
			}
			return Task.Run(() => todos);
		}

		/// <summary>
		/// Get task by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public async Task<ToDo> GetById(int id)
		{
			var todo = await _db.ToDos.FirstOrDefaultAsync(t => t.Id == id);
			return todo;
		}

		/// <summary>
		/// Add new task
		/// </summary>
		public async Task Add(ToDo todo)
		{
			_db.ToDos.Add(todo);
			await _db.SaveChangesAsync();
		}

		public Task Delete(int id)
		{
			ToDo todo = _db.ToDos.FirstOrDefault(t => t.Id == id);
			_db.Remove(todo);
			_db.SaveChanges();
			return Task.CompletedTask;

		}

		public Task ToggleCompleted(int id)
		{
			ToDo todo = _db.ToDos.FirstOrDefault(t => t.Id == id);
			todo.IsCompleted = !todo.IsCompleted;
			_db.SaveChanges();
			return Task.CompletedTask;
		}
	}
}
