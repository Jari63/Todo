using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Todo.Core.Models;
using Todo.Data;
using Todo.Services;

namespace Todo.IntegrationTests
{
	public static class Utilities
	{

		public static List<ToDo> GetSeedingMessages()
		{
			return new List<ToDo>()
			{
				new ToDo(){ Description = "TEST 1", IsCompleted=false },
				new ToDo(){ Description = "TEST 2", IsCompleted=false },
				new ToDo(){ Description = "TEST 3", IsCompleted=false }
			};
		}

		public static StringContent GetStringContent(object obj) => new StringContent(JsonConvert.SerializeObject(obj), Encoding.Default, "application/json");

		internal static void InitializeDbForTests(ITodoService todoService)
		{
			Utilities.GetSeedingMessages().ForEach(message => todoService.Add(message));
		}
	}
}
