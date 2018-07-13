namespace SpaceEngineers2DCodeGeneration
{
    public static class StringExtensions
    {
        public static string Capitalize(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Substring(0, 1).ToUpper() + s.Substring(1);
        }

        public static string UnCapitalize(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            return s.Substring(0, 1).ToLower() + s.Substring(1);
        }
    }
}
