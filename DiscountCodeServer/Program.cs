using Common;
using System.Net;
using System.Net.Sockets;
using System.Text;


const string _characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
string _filePathDB = string.Empty;
List<string> _codes = new List<string>();
Random random = new Random();

try
{
    _filePathDB = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "DB", "Codes.txt");
    var ip = IPAddress.Any;

    //load existing codes in a varialbe
    await LoadCodes(_filePathDB);

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

async Task GenerateCodeHandler(NetworkStream stream)
{
    string response = GenerateRandomAlphanumericString(random.Next(7, 9));
    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
    await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

    //save to text file
    if (!_codes.Contains(response))
    {
        _codes.Add(response);
        SaveToTextFile(response);
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

void SaveToTextFile(string response)
{
    try
    {
        if (!File.Exists(_filePathDB))
            return;

        using (StreamWriter writer = new StreamWriter(_filePathDB, true)) // 'true' for append mode
        {
            writer.WriteLine(response);
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