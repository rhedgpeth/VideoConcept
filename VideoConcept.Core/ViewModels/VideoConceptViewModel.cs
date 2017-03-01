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

		public async void Initialize()
		{
			var videoItems = await VideoItemStore.Instance.GetVideoItems();

			foreach (var videoItem in videoItems)
			{
				AddVideoItem(videoItem);
			}
		}

		async Task PerformVideoUpload(VideoFile videoFile)
		{
			IsBusy = true;

			// Placeholder for Azure Media Services uploading
			// This can also be combine with what is already existing in VideoConcept.Shared.MediaUploadService
			await Task.Delay(1000);

			IsBusy = false;

			if (_displayAlert != null)
				await _displayAlert("Video Uploaded!", "Congratulations! You have uploaded a video!", "Ok");
		}

		public async Task RemoveVideoItemViewModel(VideoItemViewModel videoItemViewModel)
		{
			var videoItem = videoItemViewModel.VideoItem;

			var videoFile = Camera.OpenVideoFile(videoItem.FilePath);

			await PerformVideoUpload(videoFile).ConfigureAwait(false);

			Camera.DeleteVideoFile(videoFile);

			await VideoItemStore.Instance.DeleteVideoItemAsync(videoItem);

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
			var name = Guid.NewGuid().ToString();
			var fileName = string.Format("{0}.mp4", name);

			// This will initiate the camera activity in order to take the video
			var videoFile = await Camera.TakeVideoAsync(fileName);

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
					//Camera.DeleteVideoFile(videoFile);
				}
				// If the user is not on Wifi then store a record of the video file locally 
				// to be queued up when the user gains Wifi acess 
				else
				{
					videoFile.Dispose();

					var videoItem = new VideoItem
					{
						Name = name,
						FileName = fileName,
						FilePath = path
					};

					await VideoItemStore.Instance.InsertVideoItemAsync(videoItem);

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
