using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Data;
using Todo.Services;
using TodoApi;
using TodoApi.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Todo.IntegrationTests
{
	public class IntegrationTests : IClassFixture<CustomWebApplicationFactory<TodoApi.Startup>>
	{
		private readonly CustomWebApplicationFactory<TodoApi.Startup> _factory;

		public IntegrationTests(CustomWebApplicationFactory<TodoApi.Startup> factory)
		{
			_factory = factory;
		}

		[Fact]
		public async Task GetTodosReturnsListOfTodos()
		{
			var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
			// Arrange
			var httpResponse = await client.GetAsync("/api/Todo");

			// MUST be successful.
			httpResponse.EnsureSuccessStatusCode();

			// Act
			var stringResponse = await httpResponse.Content.ReadAsStringAsync();
			var todos = JsonConvert.DeserializeObject<List<ToDo>>(stringResponse);

			// Assert
			Assert.NotNull(todos);
			Assert.True(todos.Count > 0);
		}

		[Fact]
		public async Task PostTodoAsyncReturnsSuccess()
		{
			var client = _factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });

			// Arrange
			var request = new
			{
				Url = "/api/Todo",
				Body = new
				{
					Description = "Test 1",
					IsCompleted = false
				}
			};

			// Act
			var response = await client.PostAsync(request.Url, Utilities.GetStringContent(request.Body));
			var value = await response.Content.ReadAsStringAsync();

			// Assert
			response.EnsureSuccessStatusCode();
		}
	}
}
