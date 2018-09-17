using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using SpaceEngineers2D.Model;
using SpaceEngineers2D.Persistence.DataModel;

namespace SpaceEngineers2D.Persistence
{
    public class PersistenceService
    {
        private string DirectoryPath { get; }

        public PersistenceService(string directoryPath)
        {
            DirectoryPath = directoryPath;
        }

        public IReadOnlyList<string> Get()
        {
            return Directory.EnumerateFiles(DirectoryPath).Select(Path.GetFileNameWithoutExtension).ToList();
        }

        public void Load(string fileName, World world)
        {
            var filePath = Path.Combine(DirectoryPath, fileName + ".json");

            if (!File.Exists(filePath))
            {
                throw new InvalidOperationException($@"Can not load save game {fileName}. File {filePath}.json not found in {DirectoryPath}.");
            }

            var json = File.ReadAllText(filePath);

            var data = JsonConvert.DeserializeObject<WorldData>(json, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            });

            var deserializer = new Deserializer(world.BlockTypes, world.ItemTypes);
            deserializer.MapWorld(world, data);
        }

        public void Save(string fileName, World world)
        {
            var filePath = Path.Combine(DirectoryPath, fileName + ".json");

            var serializer = new Serializer();
            var data = serializer.MapWorld(world);

            var json = JsonConvert.SerializeObject(data, new JsonSerializerSettings()
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.All
            });

            File.WriteAllText(filePath, json);
        }
    }
}
