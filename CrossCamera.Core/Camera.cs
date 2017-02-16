using System;
using System.Threading.Tasks;

namespace CrossCamera.Core
{
	public abstract class Camera
	{
		static Camera _current;

		public static Camera Current
		{
			get
			{
				if (!CrossCamera.isInitialized)
				{
					throw new CrossCameraInitializationException();
				}

				return _current;
			}
			internal set
			{
				_current = value;
			}
		}

		public abstract string DefaultVideoSaveDirectory { get; }

		public abstract Task<VideoFile> TakeVideoAsync(string path);

		public abstract void DeleteVideoFile(VideoFile videoFile);

		public abstract VideoFile OpenVideoFile(string fullPath);
	}
}
