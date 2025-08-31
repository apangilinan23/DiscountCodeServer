using System.IO;
using System.Net.Sockets;
using System.Text;

namespace DiscountCodeServer
{
    public class UseCodeHandler : IHandler
    {
        string _latestDiscountCode;
        NetworkStream _stream;
        public UseCodeHandler(string latestDiscountCode, NetworkStream stream) 
        {
            _latestDiscountCode = latestDiscountCode;
            _stream = stream;
        }

        public async Task ExecuteAction()
        {

            if (!DiscountCodeSingleton.GetInstance().Contains(_latestDiscountCode))
                return;
            var codeToRemove = _latestDiscountCode;            
            await RefreshTextFile();

            //update latestdiscount to last generated
            _latestDiscountCode = DiscountCodeSingleton.GetInstance() == null ||
                !DiscountCodeSingleton.GetInstance().Any() ? string.Empty : DiscountCodeSingleton.GetInstance().Last();

            string response = $"{codeToRemove} successfully used and deleted from the DB!";
            byte[] responseBytes = Encoding.UTF8.GetBytes(response);
            await _stream.WriteAsync(responseBytes, 0, responseBytes.Length);
        }

        private async Task RefreshTextFile()
        {
            try
            {

                //read for manual manipulation
                var filePathDb = DiscountCodeSingleton.DbPath;
                var codes = new List<string>();
                if (!File.Exists(filePathDb))
                    return;
                try
                {
                    using (StreamReader reader = new StreamReader(filePathDb))
                    {
                        string line;
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            codes.Add(line);
                        }
                        codes.Remove(_latestDiscountCode);
                        DiscountCodeSingleton.GetInstance().AddRange(codes);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                //will update the text file even if manipulation takes place
                using (StreamWriter writer = new StreamWriter(filePathDb, false))
                {
                    codes.ForEach(c =>
                    {
                        writer.WriteLineAsync(c);
                    });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
