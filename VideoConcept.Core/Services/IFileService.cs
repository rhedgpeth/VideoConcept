using System.IO;

namespace VideoConcept.Core.Services
{
	public interface IFileService
	{
		Stream GetStream(string filePath);
	}
}
