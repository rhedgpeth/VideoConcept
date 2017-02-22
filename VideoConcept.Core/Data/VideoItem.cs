using SQLite;

namespace VideoConcept.Core.Data
{
	public class VideoItem
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Title { get; set; }

		public string Path { get; set; }
	}
}
