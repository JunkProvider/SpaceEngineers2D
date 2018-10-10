using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Entities
{
    public abstract class EntityType<TEntity> : IEntityType
        where TEntity : IEntity
    {
        public int Id { get; }

        public string Name { get; }

        public ImageSource Image { get; }

        protected EntityType(int id, string name, ImageSource image)
        {
            Id = id;
            Name = name;
            Image = image;
        }
        
        public abstract IEntity Load(LoadContext context, DictionaryAccess data);

        public void Save(Serializer serializer, IEntity block, DictionaryAccess data)
        {
            Save(serializer, (TEntity)block, data);
        }

        public virtual void Save(Serializer serializer, TEntity entity, DictionaryAccess data)
        {

        }
    }
}
