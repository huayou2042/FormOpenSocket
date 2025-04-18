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
    public class TcpServer
    {
        public event EventHandler<Socket> ClientConnected;
        public event EventHandler<Socket> ClientDisconnected;
        public event EventHandler<ClientData> DataReceived;
        public event EventHandler<ClientData> SendCompleted;
        public Dictionary<string, Socket> dicClient = new Dictionary<string, Socket>();
        private Socket? listener;
        private Socket? clientSocket;
        byte[] buffer = new byte[1024];
        public void Listen(string ip, int port, int Max = 100)
        {
            try
            {
                // 创建IPv4的TCP协议Socket
                if (listener == null)
                    listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                // 绑定到本地所有IP地址的指定端口
                IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(ip), port);
                listener.Bind(localEP);
                listener.Listen(Max); // 最大挂起连接数
                AcceptAsync();
                LogMessage($"启动监听   {ip}:{port}");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Stop()
        {
            if (dicClient != null && dicClient.Count > 0&&false)
                foreach (var item in dicClient)
                {
                    try
                    {
                        item.Value.Shutdown(SocketShutdown.Both);// 先禁用收发
                        dicClient.Remove(item.Value.RemoteEndPoint.ToString());
                        item.Value.Close();// 再释放资源
                        item.Value.Dispose();
                        LogMessage($"客户端{item.Value.RemoteEndPoint}已关闭");
                    }
                    catch { }
                }
            try
            {
                listener.Shutdown(SocketShutdown.Both);
                listener.Disconnect(true);
            }
            catch { }
            try
            {
                LogMessage($"服务器{listener.LocalEndPoint}关闭中");
                listener.Close();
                listener.Dispose();
                listener = null;
            }
            catch { }
            LogMessage("服务已关闭");
        }

        #region 同步
        public void Accept()
        {
            clientSocket = listener.Accept();
            LogMessage($"客户端{clientSocket.RemoteEndPoint}已连接");
        }
        public void Receive(Socket clientSocket)
        {
            int bytesRead = clientSocket.Receive(buffer);
            LogMessage($"已接收{bytesRead}字节");
            string data = Encoding.ASCII.GetString(buffer, 0, bytesRead); // 转为字符串
        }
        public void Write(Socket client, string message)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(message);
            var bytesSent = client.Send(dataBytes); // 返回实际发送字节数
            LogMessage($"已发送{bytesSent}字节");
        }
        public void Write(Socket client, byte[] data, int start, int len)
        {
            client.Send(data, start, len, SocketFlags.None); // 返回实际发送字节数
        }
        #endregion

        #region 异步
        public void AcceptAsync()
        {
            try
            {
                listener.BeginAccept(AcceptCallbackAsync, listener);
            }
            catch { }
        }
        private void AcceptCallbackAsync(IAsyncResult ar)
        {
            // 获取监听Socket
            Socket listener = (Socket)ar.AsyncState;
            try
            {
                // 完成异步连接，返回新Socket
                Socket clientSocket = listener.EndAccept(ar);
                if(ClientConnected!=null)
                    ClientConnected(this,clientSocket);
                dicClient.Add(clientSocket.RemoteEndPoint.ToString(), clientSocket);
                LogMessage($"客户端{clientSocket.RemoteEndPoint}已连接");
                ReceiveAsync(clientSocket);
            }
            catch (Exception ex)
            {
                ex.ToString();
                // 处理异常（如连接中断）
            }
            finally
            {
                AcceptAsync();
            }
        }


        public void ReceiveAsync(Socket clientSocket)
        {
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveCallback), clientSocket);
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            try
            {
                int BytesToRead = client.EndReceive(ar);
                if (BytesToRead == 0)
                {
                    LogMessage($"客户端{client.RemoteEndPoint}断开连接");
                    dicClient.Remove(client.RemoteEndPoint.ToString());
                    if (ClientDisconnected != null)
                        ClientDisconnected(this, client);
                    return;
                }
                LogMessage($"已接收{BytesToRead}字节");
                ClientData clientData = new ClientData();
                clientData.client = client;
                clientData.DataLenth = BytesToRead;
                byte[] data = new byte[BytesToRead];
                Array.Copy(buffer, 0, data, 0, BytesToRead);
                clientData.Data = data;
                if (DataReceived != null)
                    DataReceived(this, clientData);
                ReceiveAsync(client);
            }
            catch (Exception ex)
            {
                if (!client.Connected)
                    return;
                LogMessage($"客户端{client.RemoteEndPoint}断开连接，异常：{ex.Message}");
                dicClient.Remove(client.RemoteEndPoint.ToString());
                if (ClientDisconnected != null)
                    ClientDisconnected(this, client);
            }
            // 处理数据...
            //while (data.IndexOf("EOF") == -1)
            //{
            //    bytesRead += clientSocket.Receive(buffer, bytesRead, buffer.Length - bytesRead, SocketFlags.None);
            //}
        }

        public void WriteAsync(Socket client, string message)
        {
            byte[] dataBytes = Encoding.ASCII.GetBytes(message);
            client.BeginSend(dataBytes, 0, dataBytes.Length, SocketFlags.None,
    new AsyncCallback(WriteCallback), client);
        }
        public void WriteAsync(Socket client, byte[] data, int start, int len)
        {
            client.BeginSend(data, start, len, SocketFlags.None,
    new AsyncCallback(WriteCallback), client);
        }
        private void WriteCallback(IAsyncResult ar)
        {
            Socket client = (Socket)ar.AsyncState;
            int bytesSent = client.EndSend(ar);
            ClientData clientData = new ClientData();
            clientData.client = client;
            clientData.DataLenth = bytesSent;

            if (SendCompleted != null)
                SendCompleted(this, clientData);
            LogMessage($"已给{client.RemoteEndPoint}发送{bytesSent}字节");
        }
        #endregion

        private void LogMessage(string message)
        {
            System.Diagnostics.Trace.WriteLine($"{DateTime.Now}     {message}");
        }
        public class ClientData
        {
            public Socket client { get; set; }
            public int DataLenth { get; set; }
            public byte[] Data { get; set; }
        }
    }
}