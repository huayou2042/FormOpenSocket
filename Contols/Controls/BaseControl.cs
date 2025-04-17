using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Controls.Base;

namespace Controls.Controls
{
    /// <summary>
    ///  用户控件的基类，主要是方便统一字体和字号等
    /// </summary>
    public partial class BaseControl : UserControl, IFreshData, IRegisterForm
    {
        public BaseControl()
        {
            InitializeComponent();
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

        /// <summary>
        /// 释放集合中的每一个对象，并清空集合
        /// </summary>
        /// <param name="ctrl">需要释放子控件的控件</param>
        protected virtual void ClearSubControls(Control ctrl)
        {
            if (!InvokeRequired)
            {
                if (ctrl?.Controls.Count > 0)
                {
                    foreach (Control sub in ctrl.Controls)
                        sub.Dispose();
                    ctrl.Controls.Clear();
                }
            }
            else Invoke(new Action<Control>(o => ClearSubControls(o)), ctrl);   // 使用Invoke是因为需要同步
        }
    }
}
