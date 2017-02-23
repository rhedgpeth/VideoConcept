using System;
using System.Diagnostics;
using System.Threading.Tasks;
using VideoConcept.Core.Data;

namespace VideoConcept.Core.Services
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
				// Upload the videos
				// Success = remove from sqlite table
				// Failure = handle some way?

				// Demo: Just log what's being done
				Debug.WriteLine($"Uploading {video.Title}...");

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
