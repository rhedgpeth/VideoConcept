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

		public static string Azure_Account_Name = "bearmobile";
		public static string Azure_Account_Key = "";
	}
}
