using System;
using SpaceEngineers2D.Model.Items;

namespace SpaceEngineers2D.Model.Blueprints
{
    public abstract class BlueprintComponentAddItemResult
    {
        public void Match(Action<Added> added, Action<AddedPartial> addedPartial, Action<NotAdded> notAdded)
        {
            if (this is Added addedResult)
            {
                added(addedResult);
            }

            if (this is AddedPartial addedPartialResult)
            {
                addedPartial(addedPartialResult);
            }

            if (this is NotAdded notAddedResult)
            {
                notAdded(notAddedResult);
            }

            throw new NotSupportedException();
        }

        public TReturn Match<TReturn>(Func<Added, TReturn> added, Func<AddedPartial, TReturn> addedPartial, Func<NotAdded, TReturn> notAdded)
        {
            if (this is Added addedResult)
            {
                return added(addedResult);
            }

            if (this is AddedPartial addedPartialResult)
            {
                return addedPartial(addedPartialResult);
            }

            if (this is NotAdded notAddedResult)
            {
                return notAdded(notAddedResult);
            }

            throw new NotSupportedException();
        }

        public class Added : BlueprintComponentAddItemResult
        {

        }

        public class AddedPartial : BlueprintComponentAddItemResult
        {
            public IItem Remaining { get; }

            public AddedPartial(IItem remaining)
            {
                Remaining = remaining;
            }
        }

        public class NotAdded : BlueprintComponentAddItemResult
        {

        }
    }
}