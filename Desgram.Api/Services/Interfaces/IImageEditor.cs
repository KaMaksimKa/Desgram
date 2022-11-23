using SkiaSharp;

namespace Desgram.Api.Services.Interfaces
{
    public interface IImageEditor
    {
        /// <summary>
        /// Обрезает изображение до соответсвия пропорциям, оставляя центральную часть
        /// </summary>
        /// <param name="image"></param>
        /// <param name="proportions"></param>
        /// <returns></returns>
        public SKImage CropCentrally(SKImage image, ImageProportions proportions);
            
        public ImageProportions GetImageProportions(int width, int height);
    }
}
