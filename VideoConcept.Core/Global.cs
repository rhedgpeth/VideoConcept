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
		public static string Azure_Account_Key = "vSOf+2dFFsbIDD0DE8f8JTCie7m94T+GD6kYr1cIMY1LdFwh+35RtF7JymKiC8omPTrbjcBUg5uvSyXYyWVs4Q==";
		//public static string Azure_Account_Key = "BPLUlFjF6jWDQQ38Rom3QXrL5Fb/mGdfwcnfeCSkxIo=";
		public static string Azure_Blob_Videos = "videos";
	}
}
