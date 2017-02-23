using System;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoConcept.Core.Data;

namespace VideoConcept.Shared.Services
{
	public class MediaUploadService
	{
		static readonly Lazy<MediaUploadService> lazy = new Lazy<MediaUploadService>(() => new MediaUploadService());

		public static MediaUploadService Instance { get { return lazy.Value; } }

		MediaUploadService()
		{ }

		public async Task UploadVideos()
		{
			var videos = await VideoItemStore.Instance.GetVideoItems().ConfigureAwait(false);

			foreach (var video in videos)
			{
				// Demo: Just log what's being done
				Debug.WriteLine($"Uploading {video.Title}...");

				// Pull the file data
				var file = File.Open(video.Path, FileMode.Open);

				if (file != null)
					Debug.WriteLine($"Successfully retrieved file at {video.Path}");

				// TODO: Insert Azure Media Upload services here
				// Mock upload 
				await Task.Delay(2500);

				Debug.WriteLine("Upload successful!");

				// Remove the file as a pending upload from sqlite
				await VideoItemStore.Instance.DeleteVideoItemAsync(video);

				Debug.WriteLine($"VideoItem {video.Title} removed from sqlite");
			}
		}
	}
}
