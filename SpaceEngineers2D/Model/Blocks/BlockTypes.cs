using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Windows.Media;

    using Items;

    public class BlockTypes
    {
        public readonly StandardBlockType Rock;

        public readonly StandardBlockType Ore;

        public readonly ConcreteBlockType Concrete;

        public readonly GrassBlockType Grass;

        public BlockTypes(ItemTypes itemTypes)
        {
            Rock = new StandardBlockType(
                image: LoadImage("rock"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            Ore = new StandardBlockType(
                image: LoadImage("ore"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Compounds[0], 1 },
                    { itemTypes.Compounds[1], 1 },
                    { itemTypes.Compounds[2], 1 },
                    { itemTypes.Compounds[3], 1 },
                    { itemTypes.Compounds[4], 1 }
                });

            Concrete = new ConcreteBlockType(
                image: LoadImage("stone_slab_side"),
                itemTypes: itemTypes);

            Grass = new GrassBlockType();
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
