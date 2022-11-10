namespace FlowerSite.Areas.admin.Data
{
    public static class FileExtensions
    {
        public static bool IsImage(this IFormFile file)
        {
            if (!file.ContentType.Contains("image"))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool ImageAllowed(this IFormFile file,int mb)
        {
            if (file.Length > 1024 * 1024 * mb)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async static Task<string> GenerateFile(this IFormFile file,string rootPath)
        {
            var unicalName = $"{Guid.NewGuid}-{file.FileName}";
            var path = Path.Combine(rootPath, "img", unicalName);

            var fs = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fs);

            fs.Close();

            return unicalName;
        }
    }
}
