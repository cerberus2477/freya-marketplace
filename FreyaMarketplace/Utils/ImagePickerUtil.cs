using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreyaMarketplace.Utils
{
    public static class ImagePickerUtil
    {
        public static async Task<List<FileResult>> PickImagesAsync(int currentCount, int maxCount = 10)
        {
            var toPick = maxCount - currentCount;
            var options = new PickOptions
            {
                PickerTitle = "Válaszd ki a képeket",
                FileTypes = FilePickerFileType.Images
            };

            var files = await FilePicker.PickMultipleAsync(options);
            return files?.Take(toPick).ToList() ?? new List<FileResult>();
        }
    }

}
