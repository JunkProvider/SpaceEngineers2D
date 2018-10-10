using System.Windows.Media;
using SpaceEngineers2D.Persistence;

namespace SpaceEngineers2D.Model.Entities
{
    public class FrogType : EntityType<Frog>
    {
        public FrogType(int id, string name, ImageSource image)
            : base(id, name, image)
        {
        }

        public Frog Instantiate()
        {
            return new Frog(this);
        }

        public override IEntity Load(LoadContext context, DictionaryAccess data)
        {
            return new Frog(this);
        }

        public override void Save(Serializer serializer, Frog entity, DictionaryAccess data)
        {
            base.Save(serializer, entity, data);
        }
    }
}
