namespace UrlShortener.API.Core.Tests.Extensions
{
    public static class Base62Extensions
    {
        private const string Alphanumeric = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        public static string EncodeToBase62(this long number)
        {
            if (number == 0) return Alphanumeric[0].ToString();

            var result = new Stack<char>();
            while (number > 0)
            {
                result.Push(Alphanumeric[(int)(number % 62)]);
                number /= 62;
            }

            return new string([.. result]);
        }
    }
}
