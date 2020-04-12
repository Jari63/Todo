using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Services;
using TodoApi.Controllers;
using Xunit;

namespace TodoTests
{
	public class TodoControllerTests
	{
		[Fact]
		public async Task TodoGet_ReturnsAnOkObjectResult_WithAListOfToDos()
		{
			var mockRepo = new Mock<ITodoService>();
			mockRepo.Setup(repo => repo.GetAll())
				.ReturnsAsync(GetTestItems());
			var controller = new TodoController(mockRepo.Object);

			var result = await controller.Get();

			var viewResult = Assert.IsType<OkObjectResult>(result);
			var model = Assert.IsAssignableFrom<IEnumerable<ToDo>>(
				viewResult.Value);
			Assert.Equal(2, model.ToList<ToDo>().Count);
		}

		[Fact]
		public async Task TodoPost_ReturnsBadRequestResult_WhenModelStateIsInvalid()
		{
			var mockRepo = new Mock<ITodoService>();
			mockRepo.Setup(repo => repo.GetAll())
				.ReturnsAsync(GetTestItems());
			var controller = new TodoController(mockRepo.Object);
			controller.ModelState.AddModelError("Description", "Required");
			var newToDo = new ToDo();

			var result = await controller.Post(newToDo);

			var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
			Assert.IsType<SerializableError>(badRequestResult.Value);
		}

		[Fact]
		public async Task TodoPost_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()
		{
			var mockRepo = new Mock<ITodoService>();
			mockRepo.Setup(repo => repo.Add(It.IsAny<ToDo>()))
				.Returns(Task.CompletedTask)
				.Verifiable();
			var controller = new TodoController(mockRepo.Object);
			var newToDo = new ToDo() { Description = "Test Name" };

			var result = await controller.Post(newToDo);

			var redirectToActionResult = Assert.IsType<CreatedAtActionResult>(result);
			Assert.Null(redirectToActionResult.ControllerName);
			Assert.Equal(nameof(controller.Get), redirectToActionResult.ActionName);
			mockRepo.Verify();
		}

		private List<ToDo> GetTestItems()
		{
			var sessions = new List<ToDo>();
			sessions.Add(new ToDo()
			{
				Id = 1,
				Description = "Test One"
			});
			sessions.Add(new ToDo()
			{
				Id = 2,
				Description = "Test Two"
			});
			return sessions;
		}
	}
}
