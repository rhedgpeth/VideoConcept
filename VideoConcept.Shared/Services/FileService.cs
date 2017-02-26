using System;
using System.IO;
using VideoConcept.Core.Services;

namespace VideoConcept.Shared
{
	public class FileService : IFileService
	{
		public Stream GetStream(string filePath)
		{
			return File.OpenRead(filePath);
		}
	}
}
