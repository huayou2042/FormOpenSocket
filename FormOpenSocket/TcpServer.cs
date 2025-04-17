using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FormOpenSocket
{
    public class ClientData
    {
        public TcpClient client { get; set; }
        public int DataLenth {  get; set; }
        public byte[] Data {  get; set; }
    }
    public class TcpServer
    {
        public event EventHandler<TcpClient> ClientConnected;
        public event EventHandler<TcpClient> ClientDisconnected;
        public event EventHandler<ClientData> DataReceived;
        public async Task listen(string ip, int port = 4545)
        {
            try
            {
                IPAddress ipAddress = IPAddress.Parse(ip); // 设置监听的IP地址

                using (TcpListener listener = new TcpListener(ipAddress, port))
                {

                    listener.Start();
                    Console.WriteLine("Server is listening...");

                    while (true)
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        Console.WriteLine("Client connected!");

                        _ = HandleClientAsync(client);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            using (client)
            {
                if (ClientConnected != null)
                    ClientConnected(this, client);
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
                {
                    if (DataReceived != null)
                        DataReceived(this, new ClientData() { client = client, DataLenth = bytesRead,Data = buffer});
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");
                }
            }

            if (ClientDisconnected != null)
                ClientDisconnected(this, client);
            Console.WriteLine("Client disconnected.");
        }

        public async Task Write(TcpClient client, byte[] buffer, int offset, int count)
        {
            // 反馈信息给客户端
            byte[] responseBuffer = Encoding.UTF8.GetBytes("Message received.");
            await client.GetStream().WriteAsync(buffer, offset, count);
        }
    }
}