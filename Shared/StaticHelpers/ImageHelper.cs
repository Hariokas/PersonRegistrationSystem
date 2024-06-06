using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace Shared.StaticHelpers;

public static class ImageHelper
{
    public static async Task<string> SaveImage(IFormFile imageFile)
    {
        if (imageFile == null)
            throw new ArgumentNullException(nameof(imageFile), "Image file cannot be null");

        var uploadsFolderPath = "ProfilePictures";
        if (!Directory.Exists(uploadsFolderPath)) Directory.CreateDirectory(uploadsFolderPath);

        var uniqueFileName = $"{Guid.NewGuid()}.jpeg";
        var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

        await SaveImageAsync(filePath, imageFile, 200, 200);

        return filePath;
    }

    private static async Task SaveImageAsync(string path, IFormFile image, int width, int height)
    {
        if (height <= 0)
            throw new ArgumentOutOfRangeException(nameof(height), "Height must be greater than 0");

        if (width <= 0)
            throw new ArgumentOutOfRangeException(nameof(width), "Width must be greater than 0");

        using var img = await Image.LoadAsync(image.OpenReadStream());
        img.Mutate(x => x.Resize(width, height));
        await img.SaveAsJpegAsync(path);
    }

    public static async Task<MemoryStream> LoadImageAsync(string path)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException("File not found", path);

        using var img = await Image.LoadAsync(path);

        var ms = new MemoryStream();
        await img.SaveAsync(ms, new JpegEncoder());
        ms.Position = 0;

        return ms;
    }

}