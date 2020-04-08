using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models;

namespace Todo.Services
{
	public interface ITodoService
	{
		/// <summary>
		/// Gets list of all the tasks
		/// </summary>
		/// <returns></returns>
		Task<List<ToDo>> GetAll();

		/// <summary>
		/// Gets task by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task<ToDo> GetById(int id);

		/// <summary>
		/// Add new task
		/// </summary>
		/// <param name="todo"></param>
		/// <returns></returns>
		Task Add(ToDo todo);

		/// <summary>
		/// Delete task
		/// </summary>
		/// <param name="id">Id of the task</param>
		/// <returns></returns>
		Task Delete(int id);

		/// <summary>
		/// Toggle task completed
		/// </summary>
		/// <param name="id">If of the task</param>
		/// <returns></returns>
		Task ToggleCompleted(int id);
	}
}
