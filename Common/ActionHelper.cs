using System.Text;

namespace Common
{
    public static class ActionHelper
    {
        public static string GenerateRandomAlphanumericString(string characters, int length)
        {
            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
            }

            StringBuilder sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                sb.Append(characters[new Random().Next(characters.Length)]);
            }
            return sb.ToString();
        }
    }
}
