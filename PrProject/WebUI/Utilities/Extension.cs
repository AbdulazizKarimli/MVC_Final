namespace WebUI.Utilities;

public static class Extension
{
    public static bool CheckFileSize(this IFormFile file,int kByte)
    {
        return file.Length / 1024 <= kByte;
    }

    public static bool CheckFileFormat(this IFormFile file, string fileFormat)
    {
        return file.ContentType.Contains(fileFormat);
    }

    public static async Task<string> CopyFileAsync(this IFormFile file,string wwwroot,params string[] folders)
    {
        try
        {
            var filename = Guid.NewGuid().ToString() + file.FileName;
            var resultPath = wwwroot;
            foreach (string folder in folders)
            {
                resultPath = Path.Combine(resultPath,folder);
            }
            resultPath = Path.Combine(resultPath, filename);
            using (FileStream stream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return filename;
        }
        catch (Exception)
        {

            throw;
        }
    }
}
