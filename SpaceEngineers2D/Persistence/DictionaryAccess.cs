using System.Collections.Generic;

namespace SpaceEngineers2D.Persistence
{
    public class DictionaryAccess
    {
        private readonly Dictionary<string, object> _dictionary;

        public DictionaryAccess()
            : this(new Dictionary<string, object>())
        {
        }

        public DictionaryAccess(Dictionary<string, object> dictionary)
        {
            _dictionary = dictionary;
        }

        public double GetOrDefault(string key, double defaultValue = 0)
        {
            if (TryGet(key, out double value))
            {
                return value;
            }

            return defaultValue;
        }

        public bool TryGet(string key, out double value)
        {
            value = default(double);

            if (!_dictionary.TryGetValue(key, out var objValue))
                return false;

            value = (double)objValue;

            return true;
        }

        public bool TryGetClass<T>(string key, out T value)
            where T : class, IDataModel
        {
            value = null;

            if (!_dictionary.TryGetValue(key, out var objValue))
                return false;

            // Value is set but null
            if (objValue == null)
            {
                return true;
            }

            value = objValue as T;

            // Value is of the wrong type
            if (value == null)
            {
                return false;
            }

            return true;
        }

        public bool TryGetClassNotNull<T>(string key, out T value)
            where T : class, IDataModel
        {
            value = null;

            if (!_dictionary.TryGetValue(key, out var objValue))
                return false;

            // Value is set but null
            if (objValue == null)
            {
                return false;
            }

            value = objValue as T;

            // Value is of the wrong type
            if (value == null)
            {
                return false;
            }

            return true;
        }

        public void Set(string key, double value)
        {
            _dictionary[key] = value;
        }

        public void Set(string key, IDataModel value)
        {
            _dictionary[key] = value;
        }
    }
}
