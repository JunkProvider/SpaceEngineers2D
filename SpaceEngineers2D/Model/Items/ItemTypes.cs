using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace SpaceEngineers2D.Model.Items
{
    public class ItemTypes
    {
        public readonly StandardItemType Ore;

        public readonly StandardItemType Rock;

        public readonly List<StandardItemType> Compounds = new List<StandardItemType>();

        public ItemTypes()
        {
            Rock = new StandardItemType(
                name: "Rock",
                mass: 2.50f,
                volume: 1f,
                icon: LoadImage("coal.png"));

            Ore = new StandardItemType(
                name: "Iron Ore",
                mass: 7.874f,
                volume: 1f,
                icon: LoadImage("diamond.png"));
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file));
        }
    }
}
