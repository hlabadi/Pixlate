using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nokia.Graphics.Imaging;

namespace Pixlate
{
    public class PixlateEffect : CustomEffectBase
    {
        public PixlateEffect(IImageProvider provider)
            : base(provider)
        {

        }
        private uint ResultPixel(uint sourcePixel)
        {
            var pixel = sourcePixel;
            uint blue = pixel & 0x000000ff; // blue color component
            uint green = (pixel & 0x0000ff00) >> 8; // green color component
            uint red = (pixel & 0x00ff0000) >> 16; // red color component
            return 0xdd000000 | blue | (green << 8) | (red << 16); // we return the original pixel, only slightly darker

        }
        protected override void OnProcess(PixelRegion sourcePixelRegion, PixelRegion targetPixelRegion)
        {
            var sourcePixels = sourcePixelRegion.ImagePixels;
            var targetPixels = targetPixelRegion.ImagePixels;
            var row = 0;
            sourcePixelRegion.ForEachRow((index, width, position) =>
            {
                if (row % 2 == 0)
                {
                    for (int y = 0; y < width; ++y, ++index)
                    {
                        targetPixels[index] = ResultPixel(sourcePixels[index]);
                    }
                }
                else
                {
                    for (int x = 0; x < width; ++x, ++index)
                    {
                        if (x % 2 == 0)
                            targetPixels[index] = ResultPixel(sourcePixels[index]);
                        else
                            targetPixels[index] = sourcePixels[index];
                    }
                }
                row++;

            });
        }
    }
}
