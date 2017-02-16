using System;
using Xamarin.Forms;
using CrossCamera.Core;

namespace VideoConcept
{
	public class VideoItemViewModel
	{
		readonly Action<string> _upload;
		readonly Action<VideoItemViewModel> _removeFromVideos;

		public VideoItemViewModel(string title, string path, Action<string> upload, Action<VideoItemViewModel> removeFromVideos)
		{
			_upload = upload;
			_removeFromVideos = removeFromVideos;
			Title = title;
			Path = path;
		}

		public string Title { get; private set; }
		public string Path { get; private set; }

		public Command UploadVideoCommand => new Command(() =>
		{
			_upload(Path);
			_removeFromVideos(this);
		});
	}
}
