using SQLite;

namespace VideoConcept
{
	public class VideoItem
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public string Title { get; set; }

		public string Path { get; set; }
	}
}
