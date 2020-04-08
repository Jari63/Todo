using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Models;
using Todo.Services;

namespace TodoApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	//[Microsoft.AspNetCore.Cors.EnableCors]
	public class TodoController : ControllerBase
	{
		private readonly ITodoService _todoService;

		public TodoController(ITodoService todoService)
		{
			_todoService = todoService;
		}

		// GET: api/Todo
		/// <summary>
		/// Returns all tasks
		/// </summary>
		/// <returns>List of Todos</returns List of <see cref="Todo.Core.Models.ToDo"/>>
		[HttpGet]
		public async Task<IEnumerable<ToDo>> Get()
		{
			List<ToDo> todos = await _todoService.GetAll();
			return Ok(todos).Value as IEnumerable<ToDo>;
		}

		// GET: api/Todo/5
		/// <summary>
		/// Returns task by id
		/// </summary>
		/// <param name="id">Id of the task</param>
		/// <returns></returns>
		[HttpGet("{id}", Name = "Get")]
		public ActionResult<ToDo> GetById(int id)
		{
			var todo = _todoService.GetById(id);
			if (todo == null)
			{
				return NotFound(todo);
			}
			return Ok(todo);
		}

		// POST: api/Todo
		/// <summary>
		/// Adds new task
		/// </summary>
		/// <param name="todo"><see cref="Todo.Core.Models.ToDo"/></param>
		[HttpPost]
		public ActionResult Post([FromBody] ToDo todo)
		{
			_todoService.Add(todo);
			return RedirectToAction(nameof(Get));
		}

		/// <summary>
		/// PUT: api/Todo/5
		/// Toggles IsCompleted-state
		/// </summary>
		/// <param name="id">Id of the task</param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public ActionResult Put(int id)
		{
			_todoService.ToggleCompleted(id);
			return RedirectToAction(nameof(Get));
		}

		/// <summary>
		/// DELETE: api/ApiWithActions/5
		/// Deletes task
		/// </summary>
		/// <param name="id">Id of the task</param>
		[HttpDelete("{id}")]
		public ActionResult Delete(int id)
		{
			_todoService.Delete(id);
			return RedirectToAction(nameof(Get));
		}
	}
}
