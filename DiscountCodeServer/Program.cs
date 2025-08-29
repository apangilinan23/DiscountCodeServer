using Common;
using System.Net;
using System.Net.Sockets;
using System.Text;


const string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
string _filePathDB = string.Empty;
string _latestDiscountCode = string.Empty;
List<string> _codes = new List<string>();
Random random = new Random();

try
{
    _filePathDB = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "DB", "Codes.txt");
    //load existing codes in a varialbe
    await LoadCodes(_filePathDB);

    var ip = IPAddress.Any;
    using (var server = new TcpListener(ip, 8082))
    {
        server.Start();
        Console.WriteLine("Server started. Waiting for connections...");
        while (true)
        {
            using (var client = server.AcceptTcpClient())
            {
                Console.WriteLine("Client connected!");

                using (var stream = client.GetStream())
                {
                    var buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    var receivedData = (ClientRequestType)Enum.Parse(typeof(ClientRequestType),
                        Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    switch (receivedData)
                    {
                        case ClientRequestType.GENERATE:
                            await GenerateCodeHandler(stream);
                            break;

                        case ClientRequestType.USE:
                            await UseCodeHandler(stream);                            
                            break;
                    }

                    Console.WriteLine("Waiting for connections...");
                }
            }
        }
    }
}
catch (Exception e)
{
    Console.WriteLine(e.Message);
}

async Task UseCodeHandler(NetworkStream stream)
{
    if (!_codes.Contains(_latestDiscountCode))
        return;
    var codeToRemove = _latestDiscountCode;
    _codes.Remove(_latestDiscountCode);
    await RefreshTextFile();

    //update latestdiscount to last generated
    _latestDiscountCode = _codes.Last();

    string response = $"{codeToRemove} successfully used and deleted from the DB!";
    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
}

async Task GenerateCodeHandler(NetworkStream stream)
{
    string response = GenerateRandomAlphanumericString(random.Next(7, 9));
    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

    //save to text file
    if (!_codes.Contains(response))
    {
        if (_codes.Count() + 1 > 2000)
            return;//limit reached

        _codes.Add(response);
        SaveCodeToTextFile(response);
    }
}


string GenerateRandomAlphanumericString(int length)
{
    if (length <= 0)
    {
        throw new ArgumentOutOfRangeException(nameof(length), "Length must be a positive integer.");
    }

    StringBuilder sb = new StringBuilder(length);
    for (int i = 0; i < length; i++)
    {
        sb.Append(_characters[random.Next(_characters.Length)]);
    }
    return sb.ToString();
}

void SaveCodeToTextFile(string code)
{
    try
    {
        if (!File.Exists(_filePathDB))
            return;

        using (StreamWriter writer = new StreamWriter(_filePathDB, true))
        {
            writer.WriteLine(code);
        }

        _latestDiscountCode = code;
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
}

async Task RefreshTextFile()
{
    try
    {
        if (!File.Exists(_filePathDB))
            return;

        using (StreamWriter writer = new StreamWriter(_filePathDB, false))
        {
            _codes.ForEach(c =>
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

async Task LoadCodes(string path)
{
    if (!File.Exists(path))
        return;
    try
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                _codes.Add(line);
            }
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}