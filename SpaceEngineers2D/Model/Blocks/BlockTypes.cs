using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media.Imaging;
using SpaceEngineers2D.Model.BlockBlueprints;

namespace SpaceEngineers2D.Model.Blocks
{
    using System.Windows.Media;

    using Items;

    public class BlockTypes
    {
        public readonly StandardBlockType Rock;

        public readonly StandardBlockType Dirt;

        public readonly StandardBlockType DirtWithGrass;

        public readonly StandardBlockType IronOreDeposit;

        public readonly StandardBlockType CoalDeposit;

        public readonly StandardBlockType Grass;

        public readonly StructuralBlockType Concrete;

        public readonly StructuralBlockType IronPlate;

        public readonly BlastFurnaceBlockType BlastFurnace;
        
        public BlockTypes(ItemTypes itemTypes)
        {
            Rock = new StandardBlockType(
                name: "Rock",
                image: LoadImage("Blocks\\Rock"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            Dirt = new StandardBlockType(
                name: "Dirt",
                image: LoadImage("Blocks\\Dirt"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            DirtWithGrass = new StandardBlockType(
                name: "DirtWithGrass",
                image: LoadImage("Blocks\\DirtWithGrass"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            IronOreDeposit = new StandardBlockType(
                name: "Iron Ore Deposit",
                image: LoadImage("Blocks\\IronOreDeposit"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.IronOre, 1 }
                });

            CoalDeposit = new StandardBlockType(
                name: "Coal Deposit",
                image: LoadImage("Blocks\\CoalDeposit"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Coal, 1 }
                });

            Grass = new StandardBlockType(
                name: "Grass",
                image: LoadImage("Blocks\\Grass"),
                droppedItems: new Dictionary<StandardItemType, int>(),
                isSolid: false);

            Concrete = new StructuralBlockType(
                name: "Concrete Block",
                image: LoadImage("Blocks\\Concrete"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Rock, 10f),
                    new BlockBlueprintComponent(1, itemTypes.Coal, 10f)
                }));

            IronPlate = new StructuralBlockType(
                name: "Iron Plate",
                image: LoadImage("Blocks\\IronPlate"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Iron, 10f)
                }));

            BlastFurnace = new BlastFurnaceBlockType(
                name: "Blast Furnace",
                image: LoadImage("Blocks\\BlastFurnace"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Rock, 10f)
                }));
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
