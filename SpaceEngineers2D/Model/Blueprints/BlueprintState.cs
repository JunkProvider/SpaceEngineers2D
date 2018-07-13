using System.Collections.Generic;
using System.Linq;

namespace SpaceEngineers2D.Model.Blueprints
{
    public class BlueprintState
    {
        private readonly Blueprint _blueprint;

        public IReadOnlyList<IBlueprintComponentState> Components { get; }

        public bool IsCompleted => Components.All(c => c.IsCompleted);

        public BlueprintState(Blueprint blueprint)
        {
            _blueprint = blueprint;
            Components = blueprint.Components.Select(c => c.CreateState()).ToList();
        }
    }
}
