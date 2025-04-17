using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FormOpenSocket.Base
{
    /// <summary>
    /// 用于父控件通知子空间进行数据刷新
    /// </summary>
    public interface IFreshData
    {
        public static Dictionary<string, IFreshData> DicForm = new Dictionary<string, IFreshData>();
        #region base需实现，注册和发布信息,form中调用
        string RegisterForm(string FormName, IFreshData t);
        Task<string> SendDataToForm<T>(string FormName, string FunctionName, string Key, T Value);
        #endregion
        #region Form需实现
        /// <summary>
        /// 窗体自身数据刷新
        /// </summary>
        string FreshData();
        /// <summary>
        /// 窗体接收数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Data"></param>
        /// <returns></returns>
        //string FreshData<T>(T Data);
        /// <summary>
        /// 窗体接收数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="FunctionName"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        Task<string> ReceiveData<T>(string FunctionName, string Key, T Value);
        //TODO:待实现
        //string ReceiveData<T>(string Source,string FunctionName, string Key,string TypeName, T Value);
        #endregion
    }
    public enum DataType
    {
        Order,
        OrderList,
        Step,
        StepList,
        StepData,
        StepDataList
    }
}