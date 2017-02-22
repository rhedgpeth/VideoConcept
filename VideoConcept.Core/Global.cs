namespace VideoConcept.Core
{
	public static class Global
	{
		public static string EnvironmentalDocumentsPath { get; set; }

		public static string VideoDatabaseName = "VideoItem.db";

		public static string VideoDatabasePath
		{
			get
			{
				return System.IO.Path.Combine(EnvironmentalDocumentsPath, VideoDatabaseName);
			}
		}
	}
}
