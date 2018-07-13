using System.Collections.Generic;

namespace SpaceEngineers2D.Model.Blueprints
{
    public class Blueprint
    {
        public IReadOnlyList<IBlueprintComponent> Components { get; }

        public Blueprint(IReadOnlyList<IBlueprintComponent> components)
        {
            Components = components;
        }

        public BlueprintState CreateState()
        {
            return new BlueprintState(this);
        }
    }
}
