using Microsoft.AspNetCore.Components.Forms;
using Swappa.Entities.Enums;

namespace Swappa.Shared.Extensions
{
    public static class FileExtensions
    {
        private const long MAX_IMAGE_SIZE = 2097152;
        private const long MAX_SHEET_SIZE = 5242880;
        private const int MAX_ALLOWED_FILES = 5;
        private const int ONE_MB = 1024 * 1024;
        private static List<string> SUPPORTED_IMAGE_FILES = new List<string>
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        public static List<string> SUPPORTED_SHEET_FILES = new List<string>
        {
            ".xlsx",
            ".xls"
        };

        public static bool IsValid(this IBrowserFile? file, FileTypes type)
        {
            var fileExt = Path.GetExtension(file.Name);
            if (file.IsNotNull() && fileExt.IsNotNullOrEmpty())
            {
                switch (type)
                {
                    case FileTypes.Image:
                        return SUPPORTED_IMAGE_FILES.Contains(fileExt ?? string.Empty)
                            && file.Size <= MAX_IMAGE_SIZE;
                    case FileTypes.Sheet:
                        return SUPPORTED_SHEET_FILES.Contains(fileExt ?? string.Empty)
                            && file.Size <= MAX_SHEET_SIZE;
                    default:
                        break;
                }
            }
            return false;
        }

        public static long MaxSize(this FileTypes type)
        {
            long maxSize = 0;
            switch (type)
            {
                case FileTypes.Image:
                    maxSize = MAX_IMAGE_SIZE / ONE_MB;
                        break;
                case FileTypes.Sheet:
                    maxSize = MAX_SHEET_SIZE / ONE_MB;
                    break;
            }

            return maxSize;
        }

        public static string AllowedFileType(this FileTypes type)
        {
            string str = string.Empty;
            switch (type)
            {
                case FileTypes.Image:
                    str = string.Join(", ",SUPPORTED_IMAGE_FILES);
                    break;
                case FileTypes.Sheet:
                    str = string.Join(", ", SUPPORTED_SHEET_FILES);
                    break;
            }

            return str;
        }
    }
}
