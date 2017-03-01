using System;
using System.IO;
using Newtonsoft.Json;
using VideoConcept.Core.Services;

namespace VideoConcept.Shared
{
	public class FileService : IFileService
	{
		public Stream GetStream<T>(T data)
		{
			string json = JsonConvert.SerializeObject(data);

			var ms = new MemoryStream();
			var sw = new StreamWriter(ms);
		
			sw.Write(json);
			sw.Flush();
			ms.Seek(0, SeekOrigin.Begin);

			return ms;
		}

		public Stream GetFileStream(string filePath)
		{
			return File.OpenRead(filePath);
		}
	}
}
