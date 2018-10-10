using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Entities
{
    public interface IEntityType
    {
        int Id { get; }

        string Name { get; }

        ImageSource Image { get; }

        IEntity Load(LoadContext context, DictionaryAccess data);

        void Save(Serializer serializer, IEntity entity, DictionaryAccess data);
    }
}
