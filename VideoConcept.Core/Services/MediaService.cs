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
					Debug.WriteLine($"Uploading {video.FileName}...");

					// Pull the file data via dependency inject (IFileService is a resolved *platform* implementation of FileService)
					var file = _fileService.GetFileStream(video.FilePath);

					if (file != null)
					{
						Debug.WriteLine($"Successfully retrieved file at {video.FilePath}");

						// Use the Azure service to upload the file to the appropriate location
						await AzureBlobService.Instance.UploadStream(Global.Azure_Blob_Videos, video.FileName, file);

						// Upon successfully upload of the video send corresponding meta data contained within a json file (stream)
						var metaData = new MetaDataSample { ID = video.Name, Description = "Video description test" };
						var metaDataStream = _fileService.GetStream<MetaDataSample>(metaData);

						// Upload the metadata json stream
						await AzureBlobService.Instance.UploadStream(Global.Azure_Blob_Videos, video.Name + ".json", metaDataStream);

						// Remove the file as a pending upload from sqlite 
						await VideoItemStore.Instance.DeleteVideoItemAsync(video);

						Debug.WriteLine($"VideoItem {video.FileName} removed from sqlite");
					}
					else
						Debug.WriteLine($"ERROR: Could not retrieve file at {video.FilePath}");
				}
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"ERROR: {ex.Message}");
			}
		}
	}

	public class MetaDataSample
	{
		public string ID { get; set; }
		public string Description { get; set; }
	}
}
