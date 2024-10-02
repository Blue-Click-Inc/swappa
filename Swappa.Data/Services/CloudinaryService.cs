using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Swappa.Data.Contracts;
using Swappa.Data.Services.Interfaces;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;
using Swappa.Shared.Extensions;

namespace Swappa.Data.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly Cloudinary _cloud;
        private readonly IRepositoryManager repository;
        private const long MAX_FILE_SIZE = 2097152;

        public CloudinaryService(IConfiguration configuration, IRepositoryManager repository)
        {
            Account cloudAccount = new Account
            {
                ApiKey = configuration.GetSection("CloudinarySettings").GetSection("Key").Value,
                ApiSecret = configuration.GetSection("CloudinarySettings").GetSection("Secret").Value,
                Cloud = configuration.GetSection("CloudinarySettings").GetSection("CloudName").Value,
            };
            _cloud = new Cloudinary(cloudAccount);
            this.repository = repository;
        }

        public async Task<bool> DeleteFile(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return true;

            var deletionParams = new DeletionParams(id)
            {
                ResourceType = ResourceType.Image
            };
            var delRes = await _cloud.DestroyAsync(deletionParams);
            return delRes.StatusCode == System.Net.HttpStatusCode.OK && delRes.Result.ToLower() == "ok";
        }

        public async Task<PhotoUploadResultDto?> UploadPhoto(IFormFile photo)
        {
            var imageUploadParams = new ImageUploadParams
            {
                File = new FileDescription(photo.Name, photo.OpenReadStream()),
                Transformation = new Transformation().Width(344).Height(258)
            };
            var res = await _cloud.UploadAsync(imageUploadParams);
            if (res.StatusCode != System.Net.HttpStatusCode.OK)
                return null;

            return new PhotoUploadResultDto { PublicId = res.PublicId, Url = res.Url.ToString() };
        }

        public async Task UploadVehicleImages(Guid vehicleId, List<IFormFile> imageFiles, bool isForNew = false)
        {
            var imagesToCreate = new List<Image>();

            if (imageFiles.IsNotNullOrEmpty())
            {
                for (var i = 0; i < imageFiles.Count; i++)
                {
                    var uploadResult = await this.UploadPhoto(imageFiles[i]);
                    if (uploadResult == null) continue;
                    var image = new Image
                    {
                        Url = uploadResult.Url,
                        PublicId = uploadResult.PublicId,
                        VehicleId = vehicleId
                    };
                    if (isForNew)
                    {
                        image.IsMain = i == 0;
                    }
                    imagesToCreate.Add(image);
                }

                await repository.Image.AddAsync(imagesToCreate);
            }
        }

        public async Task RemoveVehicleImages(Guid vehicleId)
        {
            var images = await repository.Image.FindManyAsync(i => i.VehicleId.Equals(vehicleId));
            if (!images.IsNotNullOrEmpty())
            {
                return;
            }

            var publicIds = images.Select(i => i.PublicId).ToList();
            foreach (var publicId in publicIds)
            {
                await this.DeleteFile(publicId);
            }
        }

        public async Task RemoveVehicleImages(List<Image> images)
        {
            if (!images.IsNotNullOrEmpty())
            {
                return;
            }

            var publicIds = images.Select(i => i.PublicId).ToList();
            foreach (var publicId in publicIds)
            {
                await this.DeleteFile(publicId);
            }
        }
    }
}
