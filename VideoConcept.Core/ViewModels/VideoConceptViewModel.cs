using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Linq;
using CrossCamera.Core;
using System.Windows.Input;
using Plugin.Connectivity;
using System.ComponentModel;
using VideoConcept.Core.Data;

namespace VideoConcept.Core.ViewModels
{
	public class VideoConceptViewModel : INotifyPropertyChanged
	{
		readonly VideoItemStore _store = VideoItemStore.Create();
		readonly Func<string, string, string, Task> _displayAlert;
		readonly ObservableCollection<VideoItemViewModel> _videoItemViewModels = new ObservableCollection<VideoItemViewModel>();

		Camera Camera
		{
			get
			{
				return Camera.Current;
			}
		}

		public ObservableCollection<VideoItemViewModel> VideoItemViewModels
		{
			get
			{
				return _videoItemViewModels;
			}
		}

		public VideoConceptViewModel(Func<string, string, string, Task> displayAlert)
		{
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

		async Task PerformVideoUpload(VideoFile videoFile)
		{
			IsBusy = true;

			// Placeholder for real uploading.
			await Task.Delay(1000);

			IsBusy = false;

			if (_displayAlert != null)
				await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
		}

		public async Task RemoveVideoItemViewModel(VideoItemViewModel videoItemViewModel)
		{
			var videoItem = videoItemViewModel.VideoItem;

			var videoFile = Camera.OpenVideoFile(videoItem.Path);

			await PerformVideoUpload(videoFile).ConfigureAwait(false);

			Camera.DeleteVideoFile(videoFile);

			await _store.DeleteAsync(videoItem);

			VideoItemViewModels.Remove(videoItemViewModel);
		}

		public void AddVideoItem(VideoItem videoItem)
		{
			VideoItemViewModels.Add(new VideoItemViewModel(videoItem, async vm =>
			{
				await RemoveVideoItemViewModel(vm);
			}));
		}

		public ICommand CaptureVideoCommand => new Command(async () =>
		{
			// Give the video a unique name
			var name = string.Format("{0}.mp4", DateTime.Now.ToString("MMM_ddd_d_HH_mm_ss_yyyy"));

			// This will initiate the camera activity in order to take the video
			var videoFile = await Camera.TakeVideoAsync(name);

			if (videoFile != null)
			{
				var path = videoFile.Path;

				// Check to see if the user is currently on Wifi
				var hasWifi = CrossConnectivity.Current.ConnectionTypes.Any(x => x == Plugin.Connectivity.Abstractions.ConnectionType.WiFi);

				var isConnected = CrossConnectivity.Current.IsConnected;

				// If the user is on Wifi then go ahead with the upload
				if (hasWifi && isConnected)
				{
					await PerformVideoUpload(videoFile);
					Camera.DeleteVideoFile(videoFile);
				}
				// If the user is not on Wifi then store a record of the video file locally 
				// to be queued up when the user gains Wifi acess 
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

		// ViewModel property update stuff
		public void OnPropertyChanged(string name)
		{
			if (PropertyChanged != null)
			{
				PropertyChanged(this,
					new PropertyChangedEventArgs(name));
			}
		}

		bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				OnPropertyChanged("IsBusy");
			}
		}
	}
}
