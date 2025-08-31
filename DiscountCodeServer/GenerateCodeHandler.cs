using Common;
using System.Net.Sockets;
using System.Text;

namespace DiscountCodeServer
{
    public class GenerateCodeHandler : IHandler
    {
        const string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
        List<string> _codes = DiscountCodeSingleton.GetInstance();
        string _latestDiscountCode = string.Empty;
        NetworkStream _stream;

        public GenerateCodeHandler(NetworkStream stream)
        {
            _stream = stream;
        }
        public async Task ExecuteAction()
        {
            string response = ActionHelper.GenerateRandomAlphanumericString(_characters, new Random().Next(7, 9));
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await _stream.WriteAsync(responseBytes, 0, responseBytes.Length);

            //save to text file
            if (!_codes.Contains(response))
            {
                if (_codes.Count() + 1 > 2000)
                    return;//limit reached

                _codes.Add(response);
                SaveCodeToTextFile(response);
            }
        }

        private void SaveCodeToTextFile(string code)
        {
            try
            {
                var filePathDB = DiscountCodeSingleton.DbPath;
                if (!File.Exists(filePathDB))
                    return;

                using (StreamWriter writer = new StreamWriter(filePathDB, true))
                {
                    writer.WriteLine(code);
                }

                DiscountCodeSingleton.LastCode = code;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
