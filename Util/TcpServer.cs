using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public class TcpServer
    {
        Socket _socket;
        ISocketConfiguration _tcpConfig;

        List<SocketAsyncEventArgs> queueEvtArg = new List<SocketAsyncEventArgs>();
        //控制接受最大连接数
        Semaphore semaphore;

        public TcpServer(ISocketConfiguration tcpConfig)
        {
            _tcpConfig = tcpConfig;
            Init();
        }

        //发送消息
        public bool Send(byte[] buffer)
        {
            if (queueEvtArg.Count(a => a.AcceptSocket.Connected == true) == 0)
            {
                return false;
            }

            foreach (SocketAsyncEventArgs item in queueEvtArg.Where(a => a.AcceptSocket.Connected == true))
            {
                item.AcceptSocket.Send(buffer);
            }
            return true;
        }

        //关闭服务
        public void CloseSelf()
        {
            Console.WriteLine($"Server:服务端关闭！");

            foreach (var item in queueEvtArg)
            {
                item.Completed -= AcceptEvent_Completed;

                if (item.AcceptSocket != null)
                {
                    Console.WriteLine($"ServerReceive:{item.AcceptSocket.RemoteEndPoint}客户端关闭！");
                    item.AcceptSocket.Close();
                }

                item.Dispose();
            }
            queueEvtArg.Clear();
            _socket.Close();
            _socket = null;
        }

        //重启服务
        public void StartSelf()
        {
            if (_socket != null)
            {
                return;
            }
            Console.WriteLine("Waiting for a client");
            Init();

        }

        #region 监听事件
        private void AcceptEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessAccept(e);
            Console.WriteLine("AcceptEvent_Completed");
        }
        private void ReceiveEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive(e);
            Console.WriteLine("ReceiveEvent_Completed");
        }

        #endregion

        #region 私有方法
        private void Init()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            semaphore = new Semaphore(_tcpConfig.Backlog, _tcpConfig.Backlog);
            Bind();
            Listen();
            Accept();
        }
        private void Bind()
        {
            _socket.Bind(_tcpConfig.LocalEndPoint);
        }

        private void Listen()
        {
            _socket.Listen(_tcpConfig.Backlog);
        }
        byte[] data = new byte[1024];
        private void Accept()
        {
            var _event = new SocketAsyncEventArgs();
            _event.SetBuffer(new byte[_tcpConfig.ReceiveBufferSize], 0, _tcpConfig.ReceiveBufferSize);
            _event.Completed += AcceptEvent_Completed;

            semaphore.WaitOne();
            var isPending = _socket.AcceptAsync(_event);

            if (!isPending)
            {
                ProcessAccept(_event);
            }
        }

        private void ProcessAccept(SocketAsyncEventArgs _event)
        {
            if (_event.SocketError == SocketError.OperationAborted)
            {
                return;
            }
            //有客户端连接上了
            Console.WriteLine($"ServerReceive:{_event.AcceptSocket.RemoteEndPoint}客户端连接成功！");

            _event.UserToken = Guid.NewGuid();
            queueEvtArg.Add(_event);

            var tmp = new SocketAsyncEventArgs();
            tmp.SetBuffer(new byte[_tcpConfig.ReceiveBufferSize], 0, _tcpConfig.ReceiveBufferSize);
            tmp.Completed += ReceiveEvent_Completed;
            tmp.AcceptSocket = _event.AcceptSocket;
            tmp.UserToken = _event.UserToken;

            Receive(tmp);
            Accept();
        }
        private void Receive(SocketAsyncEventArgs tmp)
        {
            var isPending = tmp.AcceptSocket.ReceiveAsync(tmp);
            if (!isPending)
            {
                ProcessReceive(tmp);
            }
        }
        private void ProcessReceive(SocketAsyncEventArgs tmp)
        {
            if (tmp.SocketError == SocketError.Success && tmp.BytesTransferred > 0)
            {
                var data = new byte[tmp.BytesTransferred];
                Array.Copy(tmp.Buffer, 0, data, 0, data.Length);
                Console.WriteLine($"SReceive:{Encoding.UTF8.GetString(data)}_{tmp.AcceptSocket.RemoteEndPoint}");

                Receive(tmp);
            }
            else
            {
                Close(tmp);
            }
        }
        private void Close(SocketAsyncEventArgs tmp)
        {
            if (tmp.SocketError == SocketError.OperationAborted)
            {
                return;
            }
            queueEvtArg.Remove(queueEvtArg.FirstOrDefault(a => a.UserToken == tmp.UserToken));

            tmp.Completed -= AcceptEvent_Completed;

            if (tmp.AcceptSocket != null)
            {
                Console.WriteLine($"ServerReceive:{tmp.AcceptSocket.RemoteEndPoint}客户端关闭！");
                tmp.AcceptSocket.Close();
            }
            tmp.Dispose();

            semaphore.Release();
        }
        #endregion

    }
}
