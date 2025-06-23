using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventEase.Controllers
{
    public class FileController : Controller
    {
        private readonly string connectionString = "DefaultEndpointsProtocol=https;AccountName=eventstore1;AccountKey=Ppms7JVyS9bgjf8NIdizsxxv92tozF61EOaZ9ddVVSc/Hn6rKoBwCg++DULy3uOMUaDhOcGWs52G+AStBFwV1w==;EndpointSuffix=core.windows.net";
        private readonly string containerName = "images";

        public async Task<IActionResult> Index()
        {
            var imageUrls = await FetchImageUrlsAsync();
            return View(imageUrls);
        }

        private async Task<List<string>> FetchImageUrlsAsync()
        {
            var imageUrls = new List<string>();
            var containerClient = new BlobContainerClient(connectionString, containerName);

            await foreach (BlobItem blobItem in containerClient.GetBlobsAsync())
            {
                var blobClient = containerClient.GetBlobClient(blobItem.Name);
                imageUrls.Add(blobClient.Uri.ToString());
            }

            return imageUrls;
        }
        private async Task UploadFileToBlobStorageAsyns(IFormFile uploadedFile)
        {
            var containerClient = new BlobContainerClient(connectionString, containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            var blobClient = containerClient.GetBlobClient(uploadedFile.FileName);
            using (var stream = uploadedFile.OpenReadStream())
            {
                await blobClient.UploadAsync(stream, true);
            }
        }
    }
}
