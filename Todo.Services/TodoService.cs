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
			foreach (var todo in _db.ToDos.AsNoTracking())
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
		public Task<ToDo> GetById(int id)
		{
			Task<ToDo> todo = _db.ToDos.FirstOrDefaultAsync(t => t.Id == id);
			return todo;
		}

		/// <summary>
		/// Add new task
		/// </summary>
		public Task Add(ToDo todo)
		{
			_db.ToDos.Add(todo);
			_db.SaveChanges();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Deletes task
		/// </summary>
		/// <param name="todo"><see cref="Todo.Core.Models.ToDo"/></param>
		/// <returns></returns>
		public Task Delete(ToDo todo)
		{
			_db.Remove(todo);
			_db.SaveChanges();
			return Task.CompletedTask;
		}

		/// <summary>
		/// Toggles IsCompleted-status
		/// </summary>
		/// <param name="todo"><see cref="Todo.Core.Models.ToDo"/></param>
		/// <returns></returns>
		public Task ToggleCompleted(ToDo todo)
		{
			todo.IsCompleted = !todo.IsCompleted;
			_db.SaveChanges();
			return Task.CompletedTask;
		}
	}
}
