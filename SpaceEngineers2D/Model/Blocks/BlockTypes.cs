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
        private Dictionary<int, IBlockType> _blockTypeDictionary;

        public StandardBlockType Rock { get; }

        public StandardBlockType Dirt { get; }

        public StandardBlockType DirtWithGrass { get; }

        public StandardBlockType IronOreDeposit { get; }

        public StandardBlockType CoalDeposit { get; }

        public GrassBlockType Grass { get; }

        public ReedBlockType Reed { get; }

        public StructuralBlockType Concrete { get; }

        public StructuralBlockType IronPlate { get; }

        public BlastFurnaceBlockType BlastFurnace { get; }

        public BlockTypes(ItemTypes itemTypes)
        {
            Rock = new StandardBlockType(
                id: 1,
                name: "Rock",
                image: LoadImage("Blocks\\Rock"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            Dirt = new StandardBlockType(
                id: 2,
                name: "Dirt",
                image: LoadImage("Blocks\\Dirt"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            DirtWithGrass = new StandardBlockType(
                id: 3,
                name: "DirtWithGrass",
                image: LoadImage("Blocks\\DirtWithGrass"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Rock, 1 }
                });

            IronOreDeposit = new StandardBlockType(
                id: 4,
                name: "Iron Ore Deposit",
                image: LoadImage("Blocks\\IronOreDeposit"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.IronOre, 1 }
                });

            CoalDeposit = new StandardBlockType(
                id: 5,
                name: "Coal Deposit",
                image: LoadImage("Blocks\\CoalDeposit"),
                droppedItems: new Dictionary<StandardItemType, int>
                {
                    { itemTypes.Coal, 1 }
                });

            Grass = new GrassBlockType(
                id: 6,
                name: "Grass",
                image: LoadImage("Blocks\\Grass"));

            Reed = new ReedBlockType(
                id: 7,
                name: "Reed",
                image: LoadImage("Blocks\\Reed"));

            Concrete = new StructuralBlockType(
                id: 8,
                name: "Stone Wall",
                image: LoadImage("Blocks\\Concrete"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Rock, 10f)
                }));

            IronPlate = new StructuralBlockType(
                id: 9,
                name: "Iron Plate",
                image: LoadImage("Blocks\\IronPlate"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Iron, 10f)
                }));

            BlastFurnace = new BlastFurnaceBlockType(
                id: 10,
                name: "Blast Furnace",
                image: LoadImage("Blocks\\BlastFurnace"),
                blueprint: new BlockBlueprint(new List<BlockBlueprintComponent>
                {
                    new BlockBlueprintComponent(1, itemTypes.Rock, 10f)
                }));
        }

        public IBlockType GetBlockType(int id)
        {
            if (!GetBlockTypeDictionary().TryGetValue(id, out var blockType))
            {
                throw new ArgumentException($@"Can not find block type with id {id}.", nameof(id));
            }

            return blockType;
        }

        public IReadOnlyDictionary<int, IBlockType> GetBlockTypeDictionary()
        {
            if (_blockTypeDictionary == null)
            {
                _blockTypeDictionary = new Dictionary<int, IBlockType>();

                foreach (var property in GetType().GetProperties())
                {
                    if (!(property.GetValue(this) is IBlockType blockType))
                        throw new InvalidOperationException($"All of {nameof(ItemTypes)} must be of type {nameof(ItemType)}.");

                    _blockTypeDictionary.Add(blockType.Id, blockType);
                }
            }

            return _blockTypeDictionary;
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
