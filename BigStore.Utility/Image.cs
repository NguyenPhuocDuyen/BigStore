using BigStore.BusinessObject;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace BigStore.Utility
{
    public static class Image
    {
        public static async Task<string> GetPathImageSaveAsync(IFormFile? ThumbnailFile, string pathFolder, string? existImagePath = null)
        {
            //save images default
            string path = "/images/noimage.jpg";

            if (ThumbnailFile != null && ThumbnailFile.Length > 0)
            {
                string fileName = CustomFile.GetUniqueFileName(ThumbnailFile.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", pathFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ThumbnailFile.CopyToAsync(stream);
                }
                //path = $"/images/{pathFolder}/{fileName}";
                path = Path.Combine("/images", pathFolder, fileName);
            }
            else if ((ThumbnailFile is null || ThumbnailFile.Length == 0) && existImagePath is not null )
            {
                path = existImagePath;
            }

            return path;
        }
    }
}
