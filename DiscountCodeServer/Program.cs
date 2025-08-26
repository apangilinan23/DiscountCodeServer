using System.Net;
using System.Net.Sockets;
using System.Text;

var ip = IPAddress.Any;
const string characters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
Random random = new Random();
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
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"{receivedData} connected");

                string response = GenerateRandomAlphanumericString(random.Next(7, 9));
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                Console.WriteLine("Waiting for connections...");
            }
        }
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
        sb.Append(characters[random.Next(characters.Length)]);
    }
    return sb.ToString();
}
