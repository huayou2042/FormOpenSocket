using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Collections;
using System.Diagnostics;
using DH.Controls;
using System.Threading.Tasks;
using FormOpenSocket.Base;
using Microsoft.VisualBasic.Logging;

namespace FormOpenSocket.Forms
{
    public partial class BaseForm : Form, IFreshData
    {
        /// <summary>
        /// 立刻执行，并返回结果，同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskId"></param>
        /// <param name="task"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        //public virtual int DoJust<T>(int taskId, object task, ref TData<T> t) { return 0; }
        /// <summary>
        /// 异步执行，返回结果
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="taskId"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        //public virtual int DoAsyn<T>(int taskId, object task, ref TData<T> t) { return 0; }

        [DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);
        private const int WM_NCLBUTTONDOWN = 0XA1;      // 鼠标左键
        private const int HTCAPTION = 2;
        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);
        [DllImport("user32.dll")]
        private static extern int ReleaseCapture();
        public const Int32 AW_HIDE = 0x00010000;        // 隐藏窗口，缺省则显示窗口。
        public const Int32 AW_ACTIVATE = 0x00020000;    // 激活窗口。在使用了AW_HIDE标志后不要使用这个标志。
        public const Int32 AW_SLIDE = 0x00040000;       // 使用滑动类型。缺省则为滚动动画类型。当使用AW_CENTER标志时，这个标志就被忽略。
        public const Int32 AW_BLEND = 0x00080000;       // 使用淡出效果。只有当hWnd为顶层窗口的时候才可以使用此标志。
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern bool AnimateWindow(IntPtr hwnd, int dwTime, int dwFlags);
        [DllImport("kernel32.dll")]
        private static extern bool SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);

        //窗体ID号，相当于窗体的功能编号，与功能一一对应
        public long FormID { get; set; }
        //窗体编号，功能编号
        public string FormNo { get; set; }
        /// <summary>
        /// 是否左键移动窗体（默认为false，为模式窗体使用）
        /// </summary>
        public bool bMoveForm = false;
        /// <summary>
        /// 本地化标志
        /// </summary>
        public bool blocalize;//= GlobalInfo.Localize;
        /// <summary>
        /// 是否窗体缩放（默认为true）
        /// </summary>
        public bool bFitScreen = true;
        /// <summary>
        /// 是否画背景
        /// </summary>                                                    
        private bool _bDrawBackGround = true;
        private ContentAlignment _textAlign = ContentAlignment.MiddleCenter;
        // 字体使用自定义的
        private Font _textFont;// = new Font(GlobalInfo.DefaultFont.Name, 14.0F, FontStyle.Bold);
        private Color _textColor = Color.White;

        // 窗体构造函数
        public BaseForm()
        {
            // 加载控件默认图标
            base.DoubleBuffered = true;                                     // 窗体双缓冲  
            base.ShowInTaskbar = false;                                     // 窗体不在任务栏显示
        }

        [Description("确定标签中文本的位置")]
        public ContentAlignment Align
        {
            get { return _textAlign; }
            set { _textAlign = value; this.Invalidate(); }
        }
        [Description("确定标签中文本的字体")]
        public Font TextFont
        {
            get { return _textFont; }
            set { _textFont = value; this.Invalidate(); }
        }
        [Description("确定标签中文本的颜色")]
        public Color TextColor
        {
            get { return _textColor; }
            set { _textColor = value; this.Invalidate(); }
        }
        [Description("是否画背景颜色")]
        public bool DrawBackGround
        {
            get { return _bDrawBackGround; }
            set { _bDrawBackGround = value; this.Invalidate(); }
        }

        // 关闭按钮事件
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 重载关闭事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //暂时关闭
            if (base.Modal == true)
            {
                //AnimateWindow(this.Handle, 200, AW_SLIDE | AW_HIDE | AW_BLEND);
            }
        }

        /// <summary>
        /// 重载OnLoad事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            // 显示动画
            if (base.Modal)
                AnimateWindow(this.Handle, 200, AW_BLEND);
            // 适应屏幕分辨率，因影响窗体实例化时的加载速度，所以暂时注释窗体缩放功能，改成窗体自适应大小 ......zb.2019.11.28
            //FitScreenResolution();

            base.OnLoad(e);
            // 窗体句柄加入字典
            //if (!TCPClass.hWndList.ContainsKey(base.Name))
            //    TCPClass.hWndList.Add(base.Name, base.Handle);
            // Alt 键抬起
            // CommonTool.KeyUpEvent(GlobalInfo.vbKeyAlt);
            // 模式窗体判断
            if (base.Modal == true)
            {
                this.bMoveForm = true;
            }
            else
                this.bMoveForm = false;
        }

        /// <summary>
        /// 重载PaintBackground事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            base.OnPaintBackground(e);
        }

        /// <summary>
        /// 重载窗体绘制事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPaint(PaintEventArgs e)
        {
            //// 画背景 渐变 
            if (_bDrawBackGround)
            {
                Graphics graphics = e.Graphics;
                LinearGradientMode _linearGradientMode = LinearGradientMode.Vertical;
                Color _brushColorStart = ColorTranslator.FromHtml("#FFFFFF");
                Color _brushColorEnd = ColorTranslator.FromHtml("#444444");
                //if (GlobalInfo.DefaultTheme == TStyleTheme.stAshLotus)
                /*{
                    _brushColorStart = ColorTranslator.FromHtml("#353432");
                    _brushColorEnd = ColorTranslator.FromHtml("#282725");
                    this.ForeColor = Color.White;
                }*/
                Brush bush = new LinearGradientBrush(ClientRectangle, _brushColorStart, _brushColorEnd, _linearGradientMode);
                graphics.FillRectangle(bush, ClientRectangle);
            }
            base.OnPaint(e);
        }

        /// <summary>
        /// 重载窗体失效事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            // 重绘窗体自定义控件
            base.OnInvalidated(e);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            // 窗体句柄加入字典
            if (base.Visible)
                CommonTool.FormPtrAdd(base.Name, base.Handle);
            else
                CommonTool.FormPtrRemove(base.Name);
        }

        /// <summary>
        /// 重载MouseDown事件
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            if (this.bMoveForm)
            {
                ReleaseCapture();//  为应用程序释放鼠标
                SendMessage((int)this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);  //  发送消息﹐让系統以為在标题栏上按下鼠标
            }
        }

        /// <summary>
        /// Form控件语言本地化
        /// </summary>
        public void LanguageLocalize()
        {
            // 重新设置窗体中所有控件的字体
            //CommonTool.ReSetControlsFont(this);
            // 本地化Form支持多语言
            //if (blocalize)
            //;// Language.GetObj().InitFormAllControlLang(this);
        }

        /// <summary>
        /// 适应屏幕分辨率(适用于除主窗体以外的子窗体)
        /// </summary>
        public void FitScreenResolution()
        {
            try
            {
                double DEFAULT_SCREEN_WIDTH = 1600;         //默认分辨率 1600 * 1050
                double DEFAULT_SCREEN_HEIGHT = 1050;
                if (bFitScreen)
                {
                    List<Control> cListCtrl = new List<Control>();
                    double iScreenWidth = Screen.PrimaryScreen.Bounds.Width; //保存当前屏幕分辨率
                    double iScreedHeight = Screen.PrimaryScreen.Bounds.Height;
                    //将控件加入到List (反射不适用)
                    void addControlToList(Control Contrl)
                    {
                        foreach (Control con in Contrl.Controls)
                        {
                            cListCtrl.Add(con);
                            //con.Anchor = AnchorStyles.Left | AnchorStyles.Top;  // 防止DevGrid位置错误
                            con.Dock = DockStyle.None;
                            if (con is TextBox)
                            {
                                TextBox temp = con as TextBox;
                                temp.Multiline = true;
                            }
                            // if (con is ComboboX)
                            if (con.Controls.Count > 0) //递归
                                addControlToList(con);
                        }
                    }
                    //分辨率改变了，当前分辨率大于或小于默认分辨率时，进行窗体控件缩放
                    if ((iScreenWidth != DEFAULT_SCREEN_WIDTH) || (iScreedHeight != DEFAULT_SCREEN_HEIGHT))
                    {
                        double dblWidthRate = Math.Round(iScreenWidth / DEFAULT_SCREEN_WIDTH, 2); //计算比率
                        double dblHeightRate = Math.Round(iScreedHeight / DEFAULT_SCREEN_HEIGHT, 2);

                        addControlToList(this); //将控件加入到List             
                        this.Left = (int)Math.Round(this.Left * dblWidthRate, 0);
                        this.Top = (int)Math.Round(this.Top * dblHeightRate, 0);
                        this.Width = (int)Math.Round(this.Width * dblWidthRate, 0);
                        this.Height = (int)Math.Round(this.Height * dblHeightRate, 0);
                        for (int i = 0; i < cListCtrl.Count; i++)
                        {
                            Control tmpControl = cListCtrl[i] as Control;
                            //按比例缩放
                            tmpControl.Left = (int)Math.Round(cListCtrl[i].Left * dblWidthRate, 0);
                            tmpControl.Top = (int)Math.Round(cListCtrl[i].Top * dblHeightRate, 0);
                            tmpControl.Width = (int)Math.Round(cListCtrl[i].Width * dblWidthRate, 0);
                            tmpControl.Height = (int)Math.Round(cListCtrl[i].Height * dblHeightRate, 0);
                            if (cListCtrl[i] is ListView)
                            {
                                ListView tmpListView = cListCtrl[i] as ListView;
                                if (tmpListView.Columns.Count > 0)
                                {
                                    for (int j = 0; j < tmpListView.Columns.Count; j++)
                                    {
                                        tmpListView.Columns[j].Width = (int)Math.Round(tmpListView.Columns[j].Width * dblWidthRate, 0);
                                    }
                                }
                            }
                            if (cListCtrl[i] is DataGridView)
                            {
                                DataGridView tmpDataGrid = cListCtrl[i] as DataGridView;
                                if (tmpDataGrid.Columns.Count > 0)
                                {
                                    for (int j = 0; j < tmpDataGrid.Columns.Count; j++)
                                    {
                                        tmpDataGrid.Columns[j].Width = (int)Math.Round(tmpDataGrid.Columns[j].Width * dblWidthRate, 0);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }

        //winform中如果每次打开的窗体都是通过new出来的，发现几次过后就会出现提示”内存不足“问题，
        //那么在关闭窗体的时候怎么处理可以及时释放内存？dispose方法可能也无法解决这个问题。
        //我们可以每次在关闭窗体的时候刷新存储器来彻底释放内存。
        /// <summary>
        /// 刷新存储器，来彻底释放内存
        /// </summary>
        private static void FlushMemory()
        {
            GC.Collect(); // 垃圾回收
            GC.WaitForPendingFinalizers(); // 清空
            if (Environment.OSVersion.Platform == PlatformID.Win32NT)
            {
                SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
            }
        }

        #region 接口事件实现
        /// <summary>
        /// 注册窗体到base中，用于后面窗体之间的数据传输
        /// </summary>
        /// <param name="FormName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public string RegisterForm(string FormName, IFreshData t)
        {
            if (string.IsNullOrWhiteSpace(FormName))
                return "名称不能为空。";
            if (IFreshData.DicForm.ContainsKey(FormName))
                return "当前名称已注册。";
            if (t == null)
                return "注册窗体不能为null。";
            if (t is not Form)
                return "不是窗体，不能注册。";
            IFreshData.DicForm[FormName] = t;
            return null;
        }
        /// <summary>
        /// 发送消息到窗体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FormName"></param>
        /// <param name="FunctionName"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public async Task<string> SendDataToForm<T>(string FormName, string FunctionName, string Key, T Value)
        {
            if (string.IsNullOrWhiteSpace(FormName))
                return "名称不能为空。";
            if (!IFreshData.DicForm.ContainsKey(FormName))
                return "当前名称未注册。";
            var form = IFreshData.DicForm[FormName];
            return await form.ReceiveData(FunctionName, Key, Value);
        }
        public virtual string FreshData() { return null; }
        //public virtual string FreshData<T>(T Data) { return null; }
        public virtual Task<string> ReceiveData<T>(string FunctionName, string Key, T Value)
        {
            return null;
        }
        #endregion

        #region 接口事件
        /// <summary>
        /// 注册窗体到base中，用于后面窗体之间的数据传输
        /// </summary>
        /// <param name="FormName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public string RegisterForm(string FormName, IFreshData t)
        {
            if (string.IsNullOrWhiteSpace(FormName))
                return "名称不能为空。";
            if (IFreshData.DicForm.ContainsKey(FormName))
                return "当前名称已注册。";
            if (t == null)
                return "注册窗体不能为null。";
            if (t is not Form)
                return "不是窗体，不能注册。";
            IFreshData.DicForm[FormName] = t;
            return null;
        }
        #endregion

        /// <summary>
        /// 日志记录器
        /// </summary>
        protected virtual ILogger Logger { get; set; } = NLog.LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 释放集合中的每一个对象，并清空集合
        /// </summary>
        /// <param name="ctrl">需要释放子控件的控件</param>
        protected virtual void ClearSubControls(Control ctrl)
        {
            if (!InvokeRequired)
            {
                foreach (Control sub in ctrl.Controls)
                    sub.Dispose();
                ctrl.Controls.Clear();
            }
            else Invoke(new Action<Control>(o => ClearSubControls(o)), ctrl);   // 使用Invoke是因为需要同步
        }

        #region     // 展示函数
        /// <summary>
        /// 在控件中展示
        /// </summary>
        /// <param name="control">控件</param>
        public void ShowDock(Control control)
        {
            if (!InvokeRequired)
            {
                this.TopLevel = false;
                this.FormBorderStyle = FormBorderStyle.None;
                this.Dock = DockStyle.Fill;
                control.Controls.Add(this);
                this.Parent = control;
                this.Show();
            }
            else BeginInvoke(new Action<Control>(o => ShowDock(o)), control);
        }

        /// <summary>
        /// 作为独立窗体展示
        /// </summary>
        public void ShowWindow()
        {
            if (!InvokeRequired)
            {
                if (null != Parent)
                    this.Parent.Controls.Remove(this);
                this.Parent = null;
                this.Dock = DockStyle.None;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopLevel = true;
                this.Show();
            }
            else BeginInvoke(new Action(() => ShowWindow()));
        }
        #endregion  // 展示函数
    }
}