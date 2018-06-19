using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceEngineers2D.Model.Chemicals
{
    public class CompoundIconProvider
    {
        private readonly Dictionary<Compound, ImageSource> _cache = new Dictionary<Compound, ImageSource>();

        public ImageSource GetIcon(Compound compound)
        {
            if (_cache.TryGetValue(compound, out var image1))
            {
                return image1;
            }

            if (TryLoadImage("Compounds\\" + compound.Name.Replace(" ", "") + ".jpg", out var image2))
            {
                _cache.Add(compound, image2);
                return image2;
            }

            if (TryLoadImage("Elements\\" + compound.Elements.First().Key.Name.Replace(" ", "") + ".jpg", out var image3))
            {
                _cache.Add(compound, image3);
                return image3;
            }

            var image4 = LoadImage("NoImage.png");
            _cache.Add(compound, image4);

            return image4;
        }

        private static bool TryLoadImage(string file, out ImageSource image)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Assets\\Images\\" + file;

            if (File.Exists(path))
            {
                image = LoadImage(file);
                return true;
            }

            image = null;
            return false;
        }

        private static ImageSource LoadImage(string file)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Assets\\Images\\" + file;
            return new BitmapImage(new Uri(path));
        }
    }
}
