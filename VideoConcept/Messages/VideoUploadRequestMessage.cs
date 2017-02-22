using System;
using System.Collections.Generic;
using VideoConcept.Core.Data;

namespace VideoConcept.Messages
{
	public class VideoUploadRequestMessage
	{
		public List<VideoItem> Videos { get; set; } = new List<VideoItem>();

		public VideoUploadRequestMessage()
		{ }
	}
}
