using System;
using System.IO;

namespace CrossCamera.Core
{
	public abstract class VideoFile : IDisposable
	{
		public abstract Stream Stream { get; }

		public abstract string Path { get; }

		public abstract void Dispose();
	}
}
