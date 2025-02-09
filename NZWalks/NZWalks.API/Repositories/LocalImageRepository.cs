using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository : IImageRepository
    {
        public LocalImageRepository(IWebHostEnvironment webHostEnvironment, IHttpContextAccessor httpContextAccessor, NZWalksDBContext dBContext)
        {
            WebHostEnvironment = webHostEnvironment;
            HttpContextAccessor = httpContextAccessor;
            DBContext = dBContext;
        }

        public IWebHostEnvironment WebHostEnvironment { get; }
        public IHttpContextAccessor HttpContextAccessor { get; }
        public NZWalksDBContext DBContext { get; }

        public async Task<Image> upload(Image image)
        {
            var localFilePath = Path.Combine(WebHostEnvironment.ContentRootPath, "Images", $"{image.FileName}{image.FileExtension}");
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilePath = $"{HttpContextAccessor.HttpContext.Request.Scheme}://{HttpContextAccessor.HttpContext.Request.Host}{HttpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilePath;

            await DBContext.Images.AddAsync(image);
            await DBContext.SaveChangesAsync();

            return image;

        }
    }
}
