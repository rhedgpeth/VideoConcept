using System;

namespace VideoConcept.Messages
{
	public class VideoUploadResponseMessage
	{
		public bool HasErrors { get; set; }
		public string Message { get; set; }
	}
}
