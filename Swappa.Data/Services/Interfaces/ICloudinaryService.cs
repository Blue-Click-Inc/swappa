using Microsoft.AspNetCore.Http;
using Swappa.Entities.Models;
using Swappa.Shared.DTOs;

namespace Swappa.Data.Services.Interfaces
{
    public interface ICloudinaryService
    {
        Task<bool> DeleteFile(string id);
        Task RemoveVehicleImages(Guid vehicleId);
        Task RemoveVehicleImages(List<Image> images);
        Task<PhotoUploadResultDto?> UploadPhoto(IFormFile photo);
        Task UploadVehicleImages(Guid vehicleId, List<IFormFile> imageFiles, bool isForNew = false);
    }
}
