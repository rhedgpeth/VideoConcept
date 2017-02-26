using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoConcept.Core.Data;
using VideoConcept.Core.Utilities;

namespace VideoConcept.Core.Services
{
	public class MediaService
	{
		IFileService _fileService;

		static readonly Lazy<MediaService> lazy = new Lazy<MediaService>(() => new MediaService());

		public static MediaService Instance { get { return lazy.Value; } }

		MediaService()
		{
			_fileService = ServiceContainer.Resolve<IFileService>();
		}

		public async Task UploadPendingVideos()
		{
			try
			{
				// Grab the videos awaiting upload from sqlite
				var videos = await VideoItemStore.Instance.GetVideoItems().ConfigureAwait(false);

				foreach (var video in videos)
				{
					Debug.WriteLine($"Uploading {video.Title}...");

					// Pull the file data via dependency inject (IFileService is a resolved *platform* implementation of FileService)
					var file = _fileService.GetStream(video.Path);

					if (file != null)
					{
						Debug.WriteLine($"Successfully retrieved file at {video.Path}");

						// Use the Azure service to upload the file to the appropriate location
						await AzureBlobService.Instance.UploadStream("container-name-here", "blob-name-here", file);

						// Remove the file as a pending upload from sqlite
						await VideoItemStore.Instance.DeleteVideoItemAsync(video);

						Debug.WriteLine($"VideoItem {video.Title} removed from sqlite");
					}
					else
						Debug.WriteLine($"ERROR: Could not retrieve file at {video.Path}");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"ERROR: {ex.Message}");
			}
		}
	}
}
