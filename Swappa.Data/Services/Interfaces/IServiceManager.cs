namespace Swappa.Data.Services.Interfaces
{
    public interface IServiceManager
    {
        ICloudinaryService Cloudinary { get; }
        IMedia Media { get; }
        INotify Notify { get; }
        IToolService Tool { get; }
        IExportService Export { get;  }
    }
}
