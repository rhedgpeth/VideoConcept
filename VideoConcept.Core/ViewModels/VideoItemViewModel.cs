using System;
using System.Windows.Input;
using VideoConcept.Core.Data;

namespace VideoConcept.Core.ViewModels
{
	public class VideoItemViewModel
	{
		readonly Action<VideoItemViewModel> _removeFromVideos;

		public VideoItemViewModel(VideoItem videoItem, Action<VideoItemViewModel> removeFromVideos)
		{
			VideoItem = videoItem;
			_removeFromVideos = removeFromVideos;
		}

		VideoItem _videoItem;

		public VideoItem VideoItem
		{
			get
			{
				return _videoItem;
			}
			set
			{
				_videoItem = value;

				Title = value.Title;
				Path = value.Path;
			}
		}

		public string Title { get; private set; }

		public string Path { get; private set; }

		public ICommand UploadVideoCommand => new Command(() =>
		{
			_removeFromVideos(this);
		});
	}
}
