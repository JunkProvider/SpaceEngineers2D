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
        private Dictionary<int, ItemType> _itemTypeDictionary;

        public StandardItemType Rock { get; }

        public StandardItemType IronOre { get; }

        public StandardItemType Coal { get; }

        public StandardItemType Iron { get; }

        public ItemTypes()
        {
            Rock = new StandardItemType(
                1,
                name: "Rock",
                mass: 2.50f,
                volume: 1f,
                icon: LoadImage("Items\\Rock.png"));

            IronOre = new StandardItemType(
                2,
                name: "Iron Ore",
                mass: 7.874f,
                volume: 1f,
                icon: LoadImage("Items\\IronOre.png"));

            Coal = new StandardItemType(
                3,
                name: "Coal",
                mass: 2.26f,
                volume: 1f,
                icon: LoadImage("Items\\Coal.png"));

            Iron = new StandardItemType(
                4,
                name: "Iron",
                mass: 7.874f,
                volume: 1f,
                icon: LoadImage("Items\\Iron.png"));
        }

        public ItemType GetItemType(int id)
        {
            return GetItemTypeDictionary()[id];
        }

        public IReadOnlyDictionary<int, ItemType> GetItemTypeDictionary()
        {
            if (_itemTypeDictionary == null)
            {
                _itemTypeDictionary = new Dictionary<int, ItemType>();

                foreach (var property in GetType().GetProperties())
                {
                    if (!(property.GetValue(this) is ItemType itemType))
                        throw new InvalidOperationException($"All of {nameof(ItemTypes)} must be of type {nameof(ItemType)}.");

                    _itemTypeDictionary.Add(itemType.Id, itemType);
                }
            }

            return _itemTypeDictionary;
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file));
        }
    }
}
