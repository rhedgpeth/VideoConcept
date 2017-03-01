using SQLite;

namespace VideoConcept.Core.Data
{
	public class VideoItem
	{
		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }
		public string Name { get; set; }
		public string FileName { get; set; }
		public string FilePath { get; set; }
	}
}
