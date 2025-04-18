using System.Net.Sockets;
using System.Text;

namespace Util
{
    public class TcpClient
    {

        Socket _socket;
        SocketAsyncEventArgs _asyncEvent;

        ISocketConfiguration _tcpConfig;

        public TcpClient(ISocketConfiguration tcpConfig)
        {
            _tcpConfig = tcpConfig;
            _asyncEvent = new SocketAsyncEventArgs();
            _asyncEvent.SetBuffer(new byte[_tcpConfig.ReceiveBufferSize], 0, _tcpConfig.ReceiveBufferSize);
            _asyncEvent.Completed += ReceiveEvent_Completed;
            Init();
            Connect();
        }

        //发送消息
        public bool Send(byte[] buffer)
        {
            bool isC = true;
            if (!_socket.Connected)
            {
                isC = ReConnect();
            }
            if (isC)
            {
                var count = _socket.Send(buffer);

                return true;
            }
            else
            {
                return false;
            }
        }

        //关闭客户端
        public void CloseSelf()
        {
            _asyncEvent.Completed -= ReceiveEvent_Completed;
            _asyncEvent.Dispose();

            Console.WriteLine($"Client:客户端关闭！");
            _socket.Close();
        }

        //重连服务端
        public bool ReConnect()
        {
            if (_socket.Connected)
            {
                return true;
            }
            if (_asyncEvent.SocketError != SocketError.Success)
            {
                _asyncEvent = new SocketAsyncEventArgs();
                _asyncEvent.SetBuffer(new byte[_tcpConfig.ReceiveBufferSize], 0, _tcpConfig.ReceiveBufferSize);
                _asyncEvent.Completed += ReceiveEvent_Completed;
                Init();
            }
            return Connect();
        }

        #region 监听事件
        private void ReceiveEvent_Completed(object sender, SocketAsyncEventArgs e)
        {
            ProcessReceive();
        }
        #endregion

        #region 私有方法
        private void Init()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        bool _isConnecting = false;
        private bool Connect()
        {
            if (_isConnecting)
            {
                return true;
            }

            try
            {
                _isConnecting = true;
                if (_socket.RemoteEndPoint == null)
                {
                    _socket.Connect(_tcpConfig.RemoteEndPoint);
                }
                Console.WriteLine($"Client:我是{_socket.LocalEndPoint}成功连接{_socket.RemoteEndPoint}服务端！");
                _socket.Send(Encoding.UTF8.GetBytes("connect"));
                Receive();
                return true;
            }
            catch (Exception error)
            {
                Console.WriteLine($"Client：服务端未开启， see inner:{error.Message}");
                return false;
            }
            finally
            {
                _isConnecting = false;
            }

        }
        byte[] data = new byte[1024];
        private void Receive()
        {
            var isPending = _socket.ReceiveAsync(_asyncEvent);
            if (!isPending)
            {
                ProcessReceive();
            }
        }
        private void ProcessReceive()
        {
            if (_asyncEvent.SocketError == SocketError.Success && _asyncEvent.LastOperation == SocketAsyncOperation.Receive && _asyncEvent.BytesTransferred > 0)
            {
                var data = new byte[_asyncEvent.BytesTransferred];
                Array.Copy(_asyncEvent.Buffer, 0, data, 0, data.Length);
                Console.WriteLine("ClientReceive:" + Encoding.UTF8.GetString(data));
                Receive();
            }
            else
            {
                Close();
            }
        }

        private void Close()
        {
            _asyncEvent.Completed -= ReceiveEvent_Completed;
            _asyncEvent.Dispose();

            Console.WriteLine($"ClientReceive:{_socket.RemoteEndPoint}服务端关闭！");
            _socket.Close();
        }
        #endregion
    }
}
