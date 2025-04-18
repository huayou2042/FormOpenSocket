using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Util
{
    public class ServerTcp
    {
        Dictionary<string, Socket> dicClient = new Dictionary<string, Socket>();
        private Socket? listener;
        private Socket? clientSocket;
        public void Listen(string ip, int port, int Max = 100)
        {
            // 创建IPv4的TCP协议Socket
            listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // 绑定到本地所有IP地址的指定端口
            IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
            listener.Bind(localEP);
            listener.Listen(Max); // 最大挂起连接数
            AcceptAsync();
            LogMessage($"启动监听   {ip}:{port}");
        }

        public void Accept()
        {
            // 同步阻塞方式接受连接（返回新Socket对象）
            clientSocket = listener.Accept();
            LogMessage($"客户端{clientSocket.RemoteEndPoint}已连接");
            // 异步方式可使用BeginAccept/EndAccept
        }
        public void AcceptAsync()
        {
            listener.BeginAccept(AcceptCallbackAsync, listener);
        }
        private void AcceptCallbackAsync(IAsyncResult ar)
        {
            // 获取监听Socket
            Socket listener = (Socket)ar.AsyncState;
            try
            {
                // 完成异步连接，返回新Socket
                Socket clientSocket = listener.EndAccept(ar);
                dicClient.Add(clientSocket.RemoteEndPoint.ToString(), clientSocket);
                LogMessage($"客户端{clientSocket.RemoteEndPoint}已连接");
                // 开始处理客户端通信（如异步接收数据）
                byte[] buffer = new byte[1024];
                //int bytesReceived = await clientSocket.ReceiveAsync(buffer, SocketFlags.None);
                //if (bytesReceived == 0)Console.WriteLine("客户端已断开");

                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, clientSocket);
            }
            catch (Exception ex)
            {
                ex.ToString();
                // 处理异常（如连接中断）
            }
            finally
            {
                // 继续监听下一个连接请求
                listener.BeginAccept(new AsyncCallback(AcceptCallbackAsync), listener);
            }
        }

        public void Receive()
        {
            byte[] buffer = new byte[1024]; // 缓冲区大小建议1024或更大
            int bytesRead = clientSocket.Receive(buffer);
            LogMessage($"已接收{bytesRead}字节");
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead); // 转为字符串
        }

        public void ReceiveAsync()
        {
            byte[] buffer = new byte[1024]; // 缓冲区大小建议1024或更大
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                int bytesRead = client.EndReceive(ar);
                if (bytesRead == 0)
                {
                    LogMessage($"客户端{client.RemoteEndPoint}断开连接");
                    dicClient.Remove(client.RemoteEndPoint.ToString());
                    return;
                }
                LogMessage($"已接收{bytesRead}字节"); 
                byte[] buffer = new byte[1024]; // 缓冲区大小建议1024或更大
                client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), client);
            }
            catch (Exception ex)
            {
                LogMessage($"客户端{client.RemoteEndPoint}断开连接，异常：{ex.Message}");
                dicClient.Remove(client.RemoteEndPoint.ToString());
            }

            // 处理数据...
            //while (data.IndexOf("EOF") == -1)
            //{
            //    bytesRead += clientSocket.Receive(buffer, bytesRead, buffer.Length - bytesRead, SocketFlags.None);
            //}
        }

        public void Write(string message)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(message);
            var bytesSent = clientSocket.Send(dataBytes); // 返回实际发送字节数
            LogMessage($"已发送{bytesSent}字节");
        }
        public void Write(byte[] data, int start, int len)
        {
            clientSocket.Send(data, start, len, SocketFlags.None); // 返回实际发送字节数
        }


        public void WriteAsync(string message)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(message);
            clientSocket.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None,
    new AsyncCallback(SendCallback), clientSocket);
        }

        private void SendCallback(IAsyncResult ar)
        {
            int bytesSent = clientSocket.EndSend(ar);
            LogMessage($"已发送{bytesSent}字节");
        }

        public void Stop()
        {
            // 关闭连接顺序
            clientSocket?.Shutdown(SocketShutdown.Both); // 先禁用收发
            clientSocket?.Close(); // 再释放资源
            LogMessage("已关闭");
        }

        private void LogMessage(string message)
        {
            System.Diagnostics.Trace.WriteLine($"{DateTime.Now}     {message}");
        }
    }
}