namespace SpaceEngineers2D.Model.Items
{
    public abstract class ItemType
    {
        public string Name { get; }

        protected ItemType(string name)
        {
            Name = name;
        }
    }
}
