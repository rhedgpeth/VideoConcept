using System.IO;

namespace CrossCamera.Core
{
	internal class InternalVideoFile : VideoFile
	{
		readonly FileStream _fileStream;
		readonly string _path;

		bool _isDisposed = false;

		public static VideoFile Open(string path)
		{
			var fileStream = File.OpenRead(path);
			return new InternalVideoFile(path, fileStream);
		}

		public static void Delete(string path)
		{
			File.Delete(path);
		}

		InternalVideoFile(string path, FileStream fileStream)
		{
			_path = path;
			_fileStream = fileStream;
		}

		public override string Path
		{
			get
			{
				return _path;
			}
		}

		public override Stream Stream
		{
			get
			{
				return _fileStream;
			}
		}

		public override void Dispose()
		{
			if (!_isDisposed)
			{
				_fileStream.Dispose();
			}
		}
	}
}
