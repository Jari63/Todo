using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Models;
using Todo.Services;

namespace TodoApi.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[EnableCors("MyAllowSpecificOrigins")]
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
		/// <returns>List of ToDos <see cref="Todo.Core.Models.ToDo" /> </returns>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			List<ToDo> todos = await _todoService.GetAll();
			return Ok(todos) as ActionResult;
		}

		// GET: api/Todo/5
		/// <summary>
		/// Returns task by id
		/// </summary>
		/// <param name="id">Id of the task</param>
		/// <returns></returns>
		[HttpGet("{id}", Name = "Get")]
		public async Task<IActionResult> GetById(int id)
		{
			var todo = await _todoService.GetById(id);
			if (todo == null)
			{
				return NotFound();
			}
			return Ok(todo);
		}

		// POST: api/Todo
		/// <summary>
		/// Adds new task
		/// </summary>
		/// <param name="todo"><see cref="Todo.Core.Models.ToDo"/></param>
		[HttpPost]
		public async Task<IActionResult> Post([FromBody] ToDo todo)
		{
			await _todoService.Add(todo);
			return Ok(todo);
		}

		/// <summary>
		/// PUT: api/Todo/5
		/// Toggles IsCompleted-state
		/// </summary>
		/// <param name="id">Id of the task</param>
		/// <returns></returns>
		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id)
		{
			var todo = _todoService.GetById(id);
			if (todo == null)
			{
				return NotFound();
			}
			await _todoService.ToggleCompleted(todo.Result);
			return NoContent();
		}

		/// <summary>
		/// DELETE: api/ApiWithActions/5
		/// Deletes task
		/// </summary>
		/// <param name="id">Id of the task</param>
		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			var todo = _todoService.GetById(id);
			if (todo == null)
			{
				return NotFound();
			}
			await _todoService.Delete(todo.Result);
			return NoContent();
		}
	}
}
