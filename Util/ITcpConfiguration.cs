using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public interface ISocketConfiguration
    {
        //  TCP Client连接地址
        EndPoint RemoteEndPoint { get; }

        //  TCP Client最大连接数
        int Backlog { get; }

        //  TCP Client接口缓存大小
        int ReceiveBufferSize { get; }

        //  TCP Server绑定地址
        EndPoint LocalEndPoint { get; }
    }

    public interface IReceiveEventArgs
    {
        byte[] Data { get; }
    }
    public interface IErrorEventArgs
    {
        Exception Error { get; }
    }
    class IOEventArgs : EventArgs, IErrorEventArgs, IReceiveEventArgs
    {
        public Exception Error { get; set; }
        public EndPoint LocalEndPoint { get; set; }
        public EndPoint RemoteEndPoint { get; set; }
        public byte[] Data { get; set; }
    }

    public class Configuration : ISocketConfiguration
    {
        public EndPoint RemoteEndPoint { get; set; }

        public int Backlog { get; set; } = 10;

        public int ReceiveBufferSize { get; set; } = 2048;

        public EndPoint LocalEndPoint { get; set; }
        public int MaxConnection { get; set; } = 5;
    }
}