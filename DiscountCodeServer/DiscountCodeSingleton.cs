namespace DiscountCodeServer
{
    public sealed class DiscountCodeSingleton
    {
        private static List<string> Instance = null;
        private static readonly object Instancelock = new object();
        public static string LastCode;
        public static string DbPath;
        public static List<string> GetInstance()
        {
            lock (Instancelock)
            {
                if (Instance == null)
                {
                    Instance = new List<string>();
                    LastCode = Instance.Count < 1 ? string.Empty :Instance[Instance.Count - 1];
                    DbPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "DB", "Codes.txt");
                }
            }

            return Instance;
        }

        public void Add(string item)
        {
            Instance.Add(item);
        }

        public void Remove(string item)
        {
            Instance.Remove(item);
        }
    }
}
