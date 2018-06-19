using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpaceEngineers2D.Model.Chemicals;

namespace SpaceEngineers2D.Model.Items
{
    public class ItemTypes
    {
        public readonly StandardItemType Ore;

        public readonly StandardItemType Rock;

        public readonly List<StandardItemType> Compounds = new List<StandardItemType>();

        public ItemTypes(CompoundList compounds)
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

            foreach (var compound in compounds.GetAll())
            {
                Compounds.Add(new StandardItemType(
                    name: compound.Name,
                    mass: 1f,
                    volume: 1f,
                    icon: LoadImage("Compounds\\" + compound.Name.Replace(" ", "") + ".jpg")));
            }
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file));
        }
    }
}
