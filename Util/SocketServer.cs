using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    internal class SocketServer
    {
        void Listen(int port)
        {
            // 设置本地 IP 和端口
            IPAddress ipAddress = IPAddress.Any; // 监听所有网络接口

            // 创建 TcpClient 并绑定到本地端口
            using (Socket listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
            {
                listenerSocket.Bind(new IPEndPoint(ipAddress, port));
                listenerSocket.Listen(5); // 最大挂起连接数为 5
                Console.WriteLine($"Listening on {ipAddress}:{port}");

                while (true)
                {
                    // 接受客户端连接
                    Socket clientSocket = listenerSocket.Accept();
                    Console.WriteLine("Client connected!");

                    // 接收数据
                    byte[] buffer = new byte[1024];
                    int bytesRead = clientSocket.Receive(buffer);
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");

                    // 发送响应
                    string response = "Hello from server!";
                    byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                    clientSocket.Send(responseBytes);

                    // 关闭客户端连接
                    clientSocket.Shutdown(SocketShutdown.Both);
                    clientSocket.Close();
                }
            }
        }
    }
}
