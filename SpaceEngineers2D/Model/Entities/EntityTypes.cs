using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Entities
{
    public class EntityTypes
    {
        private Dictionary<int, IEntityType> _entityTypeDictionary;

        public PlayerType Player { get; }

        public FrogType Frog { get; }

        public EntityTypes()
        {
            Player = new PlayerType(1, "Player", LoadImage("Player"));
            Frog = new FrogType(2, "Frog", LoadImage("Entities\\SillyCreeperHead"));
        }

        public IEntityType GetEntityType(int id)
        {
            if (!GetEntityTypeDictionary().TryGetValue(id, out var blockType))
            {
                throw new ArgumentException($@"Can not find entity type with id {id}.", nameof(id));
            }

            return blockType;
        }

        public IReadOnlyDictionary<int, IEntityType> GetEntityTypeDictionary()
        {
            if (_entityTypeDictionary == null)
            {
                _entityTypeDictionary = new Dictionary<int, IEntityType>();

                foreach (var property in GetType().GetProperties())
                {
                    if (!(property.GetValue(this) is IEntityType blockType))
                        throw new InvalidOperationException($"All of {nameof(ItemTypes)} must be of type {nameof(ItemType)}.");

                    _entityTypeDictionary.Add(blockType.Id, blockType);
                }
            }

            return _entityTypeDictionary;
        }

        private static ImageSource LoadImage(string file)
        {
            var x = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            return new BitmapImage(new Uri(x + "\\Assets\\Images\\" + file + ".png"));
        }
    }
}
