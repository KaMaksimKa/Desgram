using Desgram.Api.Services.Interfaces;
using SkiaSharp;

namespace Desgram.Api.Services
{
    public class ImageEditor:IImageEditor
    {
        public SKImage CropCentrally(SKImage image, ImageProportions proportions)
        {
            var lengthPart = Math.Min(image.Width / proportions.CountPartsWidth,
                image.Height / proportions.CountPartsHeight);

            var newWidth = lengthPart * proportions.CountPartsWidth;
            var newHeight = lengthPart * proportions.CountPartsHeight;

            if (newWidth == image.Width && newHeight == image.Height)
            {
                return SKImage.FromEncodedData(image.EncodedData);
            }

            var offsetWidth = (image.Width - newWidth) / 2;
            var offsetHeight = (image.Height - newHeight) / 2;

            return image.Subset(SKRectI.Create(offsetWidth, offsetHeight, newWidth, newHeight));
        }

        public ImageProportions GetImageProportions(int width, int height)
        {
            throw new NotImplementedException();
        }
    }
}
