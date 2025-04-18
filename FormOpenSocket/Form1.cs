using System.Net.Sockets;
using Controls.Tool;
using FormOpenSocket111;
using Util;

namespace FormOpenSocket
{
    public partial class Form1 : Form
    {
        OpenSocketV3Client client = new OpenSocketV3Client();
        ServerTcp tcpServer = new ServerTcp();
        Dictionary<string, Socket> dicClients = new Dictionary<string, Socket>();
        System.Windows.Forms.Timer timer;
        public Form1()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            tcpServer = new Util.ServerTcp();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            try
            {
                if (dicClients.Count == 0)
                    return;
                foreach (var client in dicClients.Values)
                {
                    byte[] data = BytesConverter.HexStringToBytes("AA 55 05 00 00 01 00 01 00 00 00 FA F5");
                    tcpServer.Write(data, 0, data.Length);
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
                    //tcpServer.ClientConnected += TcpServer_ClientConnected;
                    //tcpServer.ClientDisconnected += TcpServer_ClientDisconnected;
                    //tcpServer.DataReceived += TcpServer_DataReceived;
                    this.btnListen.Text = "停止监听";
                }
                else
                {
                    tcpServer.Stop();
                    this.btnListen.Text = "监听";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TcpServer_DataReceived(object? sender, ClientData e)
        {
            string s = $"客户端接收到信息【{e.client}】";
            ShowTest(richTextBox1, s);
        }

        private void TcpServer_ClientDisconnected(object? sender, System.Net.Sockets.TcpClient e)
        {
            string s = $"客户端断开连接【{e.Client}】";
            ShowTest(richTextBox1, s);
        }

        private void TcpServer_ClientConnected(object? sender, System.Net.Sockets.TcpClient e)
        {
            string s = $"客户端连接成功【{e.Client}】";
            ShowTest(richTextBox1, s);
        }

        public void ShowTest(RichTextBox box, string text)
        {
            if (InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { ShowTest(box, text); }));
                return;
            }
            text = DateTime.Now + "    " + text;
            if (string.IsNullOrEmpty(text))
                box.Text = text;
            else
                box.Text = text + Environment.NewLine + box.Text;
        }
    }
}