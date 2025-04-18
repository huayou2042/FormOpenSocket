namespace FormOpenSocket
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnConnect = new Button();
            btnListen = new Button();
            tbListenPort = new TextBox();
            label1 = new Label();
            tableLayoutPanel1 = new TableLayoutPanel();
            richTextBox2 = new RichTextBox();
            groupBox1 = new GroupBox();
            buttonTransfer1 = new Controls.Controls.ButtonTransfer();
            groupBox2 = new GroupBox();
            label3 = new Label();
            tbServerIp = new TextBox();
            tbServerPort = new Label();
            textBox2 = new TextBox();
            richTextBox1 = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // btnConnect
            // 
            btnConnect.Location = new Point(308, 78);
            btnConnect.Name = "btnConnect";
            btnConnect.Size = new Size(94, 29);
            btnConnect.TabIndex = 0;
            btnConnect.Text = "连接";
            btnConnect.UseVisualStyleBackColor = true;
            btnConnect.Click += button1_Click;
            // 
            // btnListen
            // 
            btnListen.Location = new Point(287, 78);
            btnListen.Name = "btnListen";
            btnListen.Size = new Size(94, 29);
            btnListen.TabIndex = 1;
            btnListen.Text = "监听";
            btnListen.UseVisualStyleBackColor = true;
            btnListen.Click += btnListen_Click;
            // 
            // tbListenPort
            // 
            tbListenPort.Location = new Point(119, 79);
            tbListenPort.Name = "tbListenPort";
            tbListenPort.Size = new Size(125, 27);
            tbListenPort.TabIndex = 2;
            tbListenPort.Text = "10011";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(29, 82);
            label1.Name = "label1";
            label1.Size = new Size(84, 20);
            label1.TabIndex = 3;
            label1.Text = "监听端口：";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(richTextBox2, 1, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(richTextBox1, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 331F));
            tableLayoutPanel1.Size = new Size(874, 488);
            tableLayoutPanel1.TabIndex = 4;
            // 
            // richTextBox2
            // 
            richTextBox2.Dock = DockStyle.Fill;
            richTextBox2.Location = new Point(440, 160);
            richTextBox2.Name = "richTextBox2";
            richTextBox2.Size = new Size(431, 325);
            richTextBox2.TabIndex = 3;
            richTextBox2.Text = "";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(buttonTransfer1);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(btnListen);
            groupBox1.Controls.Add(tbListenPort);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(431, 151);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "服务端";
            // 
            // buttonTransfer1
            // 
            buttonTransfer1.BackColor = SystemColors.Control;
            buttonTransfer1.BorderStyle = BorderStyle.FixedSingle;
            buttonTransfer1.FunctionId = "";
            buttonTransfer1.Hex = "";
            buttonTransfer1.Length = (ushort)0;
            buttonTransfer1.Location = new Point(119, 23);
            buttonTransfer1.Name = "buttonTransfer1";
            buttonTransfer1.Size = new Size(125, 38);
            buttonTransfer1.TabIndex = 4;
            buttonTransfer1.Text = "hello";
            buttonTransfer1.TextFont = new Font("Microsoft YaHei UI", 9F);
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(tbServerIp);
            groupBox2.Controls.Add(tbServerPort);
            groupBox2.Controls.Add(textBox2);
            groupBox2.Controls.Add(btnConnect);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(440, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(431, 151);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "客户端";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(29, 41);
            label3.Name = "label3";
            label3.Size = new Size(82, 20);
            label3.TabIndex = 7;
            label3.Text = "服务器IP：";
            // 
            // tbServerIp
            // 
            tbServerIp.Location = new Point(139, 38);
            tbServerIp.Name = "tbServerIp";
            tbServerIp.Size = new Size(125, 27);
            tbServerIp.TabIndex = 6;
            tbServerIp.Text = "10.168.1.154";
            // 
            // tbServerPort
            // 
            tbServerPort.AutoSize = true;
            tbServerPort.Location = new Point(29, 87);
            tbServerPort.Name = "tbServerPort";
            tbServerPort.Size = new Size(99, 20);
            tbServerPort.TabIndex = 5;
            tbServerPort.Text = "服务器端口：";
            // 
            // textBox2
            // 
            textBox2.Location = new Point(139, 84);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 4;
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Location = new Point(3, 160);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(431, 325);
            richTextBox1.TabIndex = 2;
            richTextBox1.Text = "";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(874, 488);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button btnConnect;
        private Button btnListen;
        private TextBox tbListenPort;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label label3;
        private TextBox tbServerIp;
        private Label tbServerPort;
        private TextBox textBox2;
        private RichTextBox richTextBox1;
        private RichTextBox richTextBox2;
        private Controls.Controls.ButtonTransfer buttonTransfer1;
    }
}
