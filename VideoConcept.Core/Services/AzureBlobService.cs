using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace VideoConcept.Core.Services
{
	// Marked internal to prevent service from being used directly from the UI layer 
	internal class AzureBlobService
	{
		CloudBlobClient _blobClient;

		static readonly Lazy<AzureBlobService> lazy = new Lazy<AzureBlobService>(() => new AzureBlobService());

		public static AzureBlobService Instance { get { return lazy.Value; } }

		AzureBlobService()
		{ 
			// Retrieve storage account from connection string
			var storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;" +
										  $"AccountName={Global.Azure_Account_Name};" +
										  $"AccountKey={Global.Azure_Account_Key}");

			// Create the blob client
			_blobClient = storageAccount.CreateCloudBlobClient();
		}

		public async Task UploadStream(string containerName, string blobName, Stream stream)
		{
			Debug.WriteLine($"Uploading stream to container ({containerName}) and blob ({blobName})...");

			// Retrieve reference to a previously created container
			var container = _blobClient.GetContainerReference(containerName);

			// Create the container if it doesn't already exist.
			await container.CreateIfNotExistsAsync().ConfigureAwait(false);

			// Retrieve reference to a blob named "myblob".
			var blockBlob = container.GetBlockBlobReference(blobName);

			await blockBlob.UploadFromStreamAsync(stream);

			Debug.WriteLine("Upload complete!");
		}
	}

	public static class Extensions
	{
		public static async Task CreateSharedAccessPolicyAsync(this CloudBlobContainer container, string policyName)
		{
			//Create a new shared access policy and define its constraints.
			var sharedPolicy = new SharedAccessBlobPolicy()
			{
				SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
				Permissions = SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.List | SharedAccessBlobPermissions.Read
			};

			//Get the container's existing permissions.
			var permissions = await container.GetPermissionsAsync();

			//Add the new policy to the container's permissions, and set the container's permissions.
			permissions.SharedAccessPolicies.Add(policyName, sharedPolicy);

			await container.SetPermissionsAsync(permissions);
		}
	}
}
