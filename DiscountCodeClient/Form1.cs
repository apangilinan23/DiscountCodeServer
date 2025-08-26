using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DiscountCodeClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void GenerateCodeBtn_Click(object sender, EventArgs e)
        {
            var ip = GetIp();
            using (var client = new TcpClient("127.0.0.1", 8082))
            {
                Console.WriteLine("Connected to server.");

                using (var stream = client.GetStream())
                {
                    byte[] messageBytes = Encoding.UTF8.GetBytes(ip);
                    stream.Write(messageBytes, 0, messageBytes.Length);

                    // Example: Receive data
                    byte[] buffer = new byte[256];
                    int bytesRead = stream.Read(buffer, 0, buffer.Length);
                    string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    DiscountLbl.Text = receivedData;
                    DiscountLbl.Visible = true;
                }
            }
        }

        private string GetIp()
        {
            string hostName = Dns.GetHostName();
            string result = string.Empty;
            Console.WriteLine($"Local Machine's Host Name: {hostName}");

            IPHostEntry ipEntry = Dns.GetHostEntry(hostName);
            IPAddress[] addresses = ipEntry.AddressList;

            Console.WriteLine("IP Addresses:");
            foreach (IPAddress ip in addresses)
            {
                // Filter for IPv4 addresses if needed, otherwise list all
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    result = ip.ToString();
                    break;
                }   
            }
            return result;
        }
    }
}
