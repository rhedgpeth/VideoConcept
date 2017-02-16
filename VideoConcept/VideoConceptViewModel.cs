using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using CrossCamera.Core;
using System.Windows.Input;

namespace VideoConcept
{
	public class VideoConceptViewModel
	{
		readonly VideoItemStore _store = VideoItemStore.Create(System.IO.Path.Combine(App.DocumentsPath, "VideoItem.db"));
		readonly Camera _camera;
		readonly Func<string, string, string, Task> _displayAlert;
		readonly ObservableCollection<VideoItemViewModel> _videoItemViewModels = new ObservableCollection<VideoItemViewModel>();

		public VideoConceptViewModel(Camera camera, Func<string, string, string, Task> displayAlert)
		{
			_camera = camera;
			_displayAlert = displayAlert;
		}

		public async Task Initialize()
		{
			var videoItems = await _store.QueryAsync();

			foreach (var videoItem in videoItems)
			{
				AddVideoItem(videoItem);
			}
		}

		public ObservableCollection<VideoItemViewModel> VideoItemViewModels
		{
			get
			{
				return _videoItemViewModels;
			}
		}

		public void AddVideoItem(VideoItem videoItem)
		{
			VideoItemViewModels.Add(new VideoItemViewModel(videoItem, removeFromVideos: async vm =>
			{
				var videoFile = _camera.OpenVideoFile(vm.Path);

				_camera.DeleteVideoFile(videoFile);
				await _store.DeleteAsync(vm.VideoItem);
				VideoItemViewModels.Remove(vm);

				await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
			}));
		}

		public ICommand CaptureVideoCommand => new Command(async () =>
		{
			var name = string.Format("{0}.mp4", DateTime.Now.ToString("MMM_ddd_d_HH_mm_ss_yyyy"));
			var videoFile = await _camera.TakeVideoAsync(name);

			if (videoFile != null)
			{
				var path = videoFile.Path;
				videoFile.Dispose();

				var videoItem = new VideoItem
				{
					Title = path,
					Path = path
				};

				await _store.InsertAsync(videoItem);

				AddVideoItem(videoItem);
			}
		});
	}
}
