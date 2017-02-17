using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using CrossCamera.Core;
using System.Windows.Input;
using Plugin.Connectivity;
using System.ComponentModel;

namespace VideoConcept
{
	public class VideoConceptViewModel : INotifyPropertyChanged
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

		public event PropertyChangedEventHandler PropertyChanged;

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

		public void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(name));
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}

		async Task PerformVideoUpload(VideoFile videoFile)
		{
			IsBusy = true;

			// Placeholder for real uploading.
			await Task.Delay(1000);

			IsBusy = false;
				
			await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
		}

		public async Task RemoveVideoItemViewModel(VideoItemViewModel videoItemViewModel)
		{
			var videoItem = videoItemViewModel.VideoItem;

			var videoFile = _camera.OpenVideoFile(videoItem.Path);

			await PerformVideoUpload(videoFile);

			_camera.DeleteVideoFile(videoFile);
			await _store.DeleteAsync(videoItem);
			VideoItemViewModels.Remove(videoItemViewModel);
		}

		public void AddVideoItem(VideoItem videoItem)
		{
			VideoItemViewModels.Add(new VideoItemViewModel(videoItem, removeFromVideos: async vm =>
			{
				await RemoveVideoItemViewModel(vm);
			}));
		}

		public ICommand CaptureVideoCommand => new Command(async () =>
		{
			var name = string.Format("{0}.mp4", DateTime.Now.ToString("MMM_ddd_d_HH_mm_ss_yyyy"));
			var videoFile = await _camera.TakeVideoAsync(name);

			if (videoFile != null)
			{
				var path = videoFile.Path;

				var hasWifi = CrossConnectivity.Current.ConnectionTypes.Any(x => x == Plugin.Connectivity.Abstractions.ConnectionType.WiFi);
				var isConnected = CrossConnectivity.Current.IsConnected;
				if (hasWifi && isConnected)
				{
					await PerformVideoUpload(videoFile);
					_camera.DeleteVideoFile(videoFile);
				}
				else
				{
					videoFile.Dispose();

					var videoItem = new VideoItem
					{
						Title = path,
						Path = path
					};

					await _store.InsertAsync(videoItem);

					AddVideoItem(videoItem);
				}
			}
		});
	}
}
