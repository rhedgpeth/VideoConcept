using System.IO;

namespace VideoConcept.Core.Services
{
	public interface IFileService
	{
		Stream GetFileStream(string filePath);
		Stream GetStream<T>(T data);
	}
}
