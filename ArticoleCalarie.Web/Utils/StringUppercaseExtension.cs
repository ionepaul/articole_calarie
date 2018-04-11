namespace ArticoleCalarie.Web.Utils
{
    public static class StringUppercaseExtension
    {
        public static string ToFirstUppercase(this string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);

            return new string(a);
        }

        public static string ToUrlProductName(this string s)
        {
            var s1 = s.Replace(" ", "-").ToLowerInvariant();

            return s1;
        }
    }
}