using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FormOpenSocket
{
    public class OpenSocketV3Client
    {
        private TcpClient _client=new TcpClient();
        private NetworkStream _stream;

        // 连接设备
        public void Connect(string ip, int port = 4545)
        {
            try
            {
                _client = new TcpClient();
                _client.Connect(ip, port);
                _stream = _client.GetStream();
                System.Diagnostics.Trace.WriteLine($"Connected to {ip}:{port}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLine($"Connecte Error: {ex.ToString()}");
            }
        }

        // 发送指令
        public void SendCommand(byte[] payload)
        {
            byte[] header = new byte[12];
            // 填充Header（示例代码需补全CRC计算和长度处理）
            _stream.Write(header, 0, header.Length);
            _stream.Write(payload, 0, payload.Length);
        }

        // 接收响应
        public async Task<byte[]> ReceiveData()
        {
            byte[] headerBuffer = new byte[12];
            await _stream.ReadExactlyAsync(headerBuffer, 0, 12);
            // 解析Header获取数据长度
            int dataLength = BitConverter.ToInt32(headerBuffer, 4);
            byte[] payload = new byte[dataLength];
            await _stream.ReadExactlyAsync(payload, 0, dataLength);
            return payload;
        }

        // 关闭连接
        public void Disconnect()
        {
            _stream?.Close();
            _client?.Close();
        }
    }
}