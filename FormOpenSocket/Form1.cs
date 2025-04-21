using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using Controls.Entity;
using Controls.Tool;
using Util;
using static Util.TcpServer;

namespace FormOpenSocket
{
    public partial class Form1 : Form
    {
        OpenSocketV3Client client = new OpenSocketV3Client();
        TcpServer tcpServer = new TcpServer();
        Dictionary<string, Socket> dicClients = new Dictionary<string, Socket>();
        System.Windows.Forms.Timer timer;
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 5000;
            timer.Tick += Timer_Tick;
            timer.Start();
            tcpServer = new Util.TcpServer();
            tcpServer.ClientConnected += TcpServer_ClientConnected;
            tcpServer.ClientDisconnected += TcpServer_ClientDisconnected;
            tcpServer.DataReceived += TcpServer_DataReceived;
        }

        private void TcpServer_DataReceived(object? sender, ClientData e)
        {
            string s = $"接收信息【{e.client.RemoteEndPoint.ToString()}】<<  {BytesConverter.ByteArrayToHexString(e.Data, " ")} {Environment.NewLine + "<<<" + Encoding.ASCII.GetString(e.Data)}";
            ShowTest(richTextBox1, s);
            var transfer = Transfer.FromBytes(e.Data);
            string strTransfer = JsonSerializer.Serialize(transfer); 
            ShowTest(richTextBox1, $"数据信息  {strTransfer}");
        }

        private void TcpServer_ClientDisconnected(object? sender, Socket e)
        {
            string s = $"断开连接【{e.RemoteEndPoint}】";
            ShowTest(richTextBox1, s);
        }

        private void TcpServer_ClientConnected(object? sender, Socket e)
        {
            string s = $"连接成功【{e.RemoteEndPoint}】";
            ShowTest(richTextBox1, s);
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (tcpServer == null)
                    return;
                if (tcpServer.dicClient.Count == 0)
                    return;

                foreach (var client in tcpServer.dicClient)
                {
                    byte[] data = BytesConverter.HexStringToBytes("AA 55 05 00 00 01 00 01 00 00 00 FA F5");
                    SendData(data, true);
                }
            }
            catch { }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.Connect("10.168.1.154", 4545);
        }

        private async void btnListen_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnListen.Text == "监听")
                {
                    tcpServer.Listen(tbServerIp.Text, Convert.ToInt32(tbListenPort.Text));
                    this.btnListen.Text = "停止监听";
                    this.tbListenPort.Enabled = false;
                }
                else
                {
                    tcpServer.Stop();
                    this.btnListen.Text = "监听";
                    this.tbListenPort.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ShowTest(RichTextBox box, string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { ShowTest(box, text); }));
                return;
            }
            text = DateTime.Now + "    " + text;
            GlobalVar.logger.Info(text);
            if (string.IsNullOrEmpty(text))
                box.Text = text;
            else
                box.Text = text + Environment.NewLine + box.Text;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            byte[] data = BytesConverter.HexStringToBytes(this.tbSendHex.Text);
            SendData(data, true);
        }

        public void SendData(byte[] data, bool showText = false)
        {
            if (tcpServer == null)
                return;
            if (tcpServer.dicClient.Count == 0)
                return;
            foreach (var client in tcpServer.dicClient)
            {
                tcpServer.Write(client.Value, data, 0, data.Length);
                if (showText)
                {
                    string s = $"发送信息【{client.Value.RemoteEndPoint.ToString()}】>>  【{BytesConverter.ByteArrayToHexString(data, " ")}】";
                    ShowTest(richTextBox1, s);
                }
            }
        }

        private void numInterval_ValueChanged(object sender, EventArgs e)
        {
            timer.Enabled = this.numInterval.Value != 0;
            if (timer.Enabled)
                this.timer.Interval = (int)(this.numInterval.Value * 1000);
        }
    }
}