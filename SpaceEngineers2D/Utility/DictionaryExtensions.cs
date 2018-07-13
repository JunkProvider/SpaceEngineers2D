using System.Collections.Generic;

namespace SpaceEngineers2D.Utility
{
    public static class DictionaryExtensions
    {
        public static bool TryGetValues<TKey, TValue>(this IDictionary<TKey, TValue> pairs, TKey key1, out TValue value1, TKey key2, out TValue value2)
        {
            value1 = default(TValue);
            value2 = default(TValue);

            if (pairs.TryGetValue(key1, out value1))
            {
                if (pairs.TryGetValue(key2, out value2))
                {
                    return true;
                }
                else
                {
                    value1 = default(TValue);
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
