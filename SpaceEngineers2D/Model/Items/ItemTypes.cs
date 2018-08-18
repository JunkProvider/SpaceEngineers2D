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
        public readonly StandardItemType Rock;

        public readonly StandardItemType IronOre;

        public readonly StandardItemType Coal;

        public readonly StandardItemType Iron;

        public readonly List<StandardItemType> Compounds = new List<StandardItemType>();

        public ItemTypes()
        {
            Rock = new StandardItemType(
                name: "Rock",
                mass: 2.50f,
                volume: 1f,
                icon: LoadImage("Items\\Rock.png"));

            IronOre = new StandardItemType(
                name: "Iron Ore",
                mass: 7.874f,
                volume: 1f,
                icon: LoadImage("Items\\IronOre.png"));

            Coal = new StandardItemType(
                name: "Coal",
                mass: 2.26f,
                volume: 1f,
                icon: LoadImage("Items\\Coal.png"));

            Iron = new StandardItemType(
                name: "Iron",
                mass: 7.874f,
                volume: 1f,
                icon: LoadImage("Items\\Iron.png"));
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file));
        }
    }
}
