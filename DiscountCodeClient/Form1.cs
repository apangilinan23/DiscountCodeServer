using Common;
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
            ConnectToServer(ClientRequestType.GENERATE);
        }

        private void UseCodeBtn_Click(object sender, EventArgs e)
        {
            ConnectToServer(ClientRequestType.USE);
        }

        private async Task ConnectToServer(ClientRequestType type)
        {
            try
            {
                using (var client = new TcpClient("127.0.0.1", 8082))
                {
                    Console.WriteLine("Connected to server.");

                    using (var stream = client.GetStream())
                    {
                        byte[] messageBytes = Encoding.UTF8.GetBytes(Enum.GetName(typeof(ClientRequestType), type));
                        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);

                        // Example: Receive data
                        byte[] buffer = new byte[1024];
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        DiscountLbl.Text = receivedData;
                        DiscountLbl.Visible = true;
                        UseCodeBtn.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}
