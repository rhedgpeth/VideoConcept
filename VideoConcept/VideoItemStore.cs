using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;

namespace VideoConcept
{
	public class VideoItemStore
	{
		readonly string _path;

		public static VideoItemStore Create(string path)
		{
			var connection = new SQLiteAsyncConnection(path);
			connection.CreateTableAsync<VideoItem>();
			return new VideoItemStore(path);
		}

		public VideoItemStore(string path)
		{
			_path = path;
		}

		public Task<int> InsertAsync(VideoItem videoItem)
		{
			var connection = new SQLiteAsyncConnection(_path);
			return connection.InsertAsync(videoItem);
		}

		public Task<List<VideoItem>> QueryAsync()
		{
			var connection = new SQLiteAsyncConnection(_path);
			return connection.QueryAsync<VideoItem>("SELECT * FROM VideoItem");
		}

		public Task<int> DeleteAsync(VideoItem videoItem)
		{
			var connection = new SQLiteAsyncConnection(_path);
			return connection.DeleteAsync(videoItem);
		}
	}
}
