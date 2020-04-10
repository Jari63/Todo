namespace Todo.Core.Models
{
	public class ToDo
	{
		/// <summary>
		/// Unique identifier of the task
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Description of the task
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Is task completed
		/// </summary>
		public bool IsCompleted { get; set; }
	}
}
