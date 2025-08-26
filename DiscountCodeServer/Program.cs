using System.Net;
using System.Net.Sockets;
using System.Text;

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
                string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Received data: {receivedData}");

                string response = "Hello from server!";
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                stream.Write(responseBytes, 0, responseBytes.Length);

                Console.WriteLine("Waiting for connections...");
            }
        }
    }
    
}
