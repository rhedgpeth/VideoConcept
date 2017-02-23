using System.Threading.Tasks;
using System.Collections.Generic;

using SQLite;
using System;

namespace VideoConcept.Core.Data
{
	public class VideoItemStore
	{
		SQLiteAsyncConnection _connection;

		public SQLiteAsyncConnection Connection
		{
			get
			{
				if (_connection == null)
					_connection = GetConnection();

				return _connection;
			}
		}

		static readonly Lazy<VideoItemStore> lazy = new Lazy<VideoItemStore>(() => new VideoItemStore());

		public static VideoItemStore Instance { get { return lazy.Value; } }

		VideoItemStore()
		{ }

		SQLiteAsyncConnection GetConnection()
		{
			var connection = new SQLiteConnection(Global.VideoDatabasePath);

			// Create tables, if they don't exist
			if (!TableExists(connection, "VideoItem"))
				connection.CreateTable<VideoItem>();

			return new SQLiteAsyncConnection(Global.VideoDatabasePath);
		}

		bool TableExists(SQLiteConnection connection, string tableName)
		{
			string query = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tableName}'";
			return connection.ExecuteScalar<string>(query) != null;
		}

		public Task<int> InsertVideoItemAsync(VideoItem videoItem)
		{
			return Connection.InsertAsync(videoItem);
		}

		public Task<List<VideoItem>> GetVideoItems()
		{
			return Connection.QueryAsync<VideoItem>("SELECT * FROM VideoItem");
		}

		public Task<int> DeleteVideoItemAsync(VideoItem videoItem)
		{
			return Connection.DeleteAsync(videoItem);
		}
	}
}
