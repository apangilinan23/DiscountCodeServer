using Common;
using DiscountCodeServer;
using System.Net;
using System.Net.Sockets;
using System.Text;
try
{
    var filePathDB = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName, "DB", "Codes.txt");
    //load existing codes in a varialbe
    await LoadCodes(filePathDB);

    var ip = IPAddress.Any;
    using (var server = new TcpListener(ip, 8082))
    {
        server.Start();
        Console.WriteLine("Server started. Waiting for connections...");
        while (true)
        {
            using (var client = server.AcceptTcpClient())
            {
                using (var stream = client.GetStream())
                {
                    var buffer = new byte[1024];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);

                    var receivedData = (ClientRequestType)Enum.Parse(typeof(ClientRequestType),
                        Encoding.UTF8.GetString(buffer, 0, bytesRead));

                    Console.WriteLine($"{Enum.GetName(typeof(ClientRequestType),receivedData)} action requested!");

                    switch (receivedData)
                    {
                        case ClientRequestType.GENERATE:
                            var generateHandler = new GenerateCodeHandler(stream);
                            await UseHandler(generateHandler);
                            break;

                        case ClientRequestType.USE:
                            var useCodeHandler = new UseCodeHandler(DiscountCodeSingleton.LastCode, stream);
                            await UseHandler(useCodeHandler);
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

async Task UseHandler(IHandler handler)
{
    await handler.ExecuteAction();
}

async Task LoadCodes(string path)
{
    var codes = new List<string>();
    if (!File.Exists(path))
        return;
    try
    {
        using (StreamReader reader = new StreamReader(path))
        {
            string line;
            while ((line = await reader.ReadLineAsync()) != null)
            {
                codes.Add(line);
            }
            DiscountCodeSingleton.GetInstance().AddRange(codes);
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message);
    }
}