namespace Controls
{
    /// <summary>
    /// 全局变量
    /// </summary>
    public class GlobalVar
    {
        static GlobalVar obj;
        GlobalVar() { }
        public static GlobalVar Instance
        {
            get
            {
                if (obj == null)
                    obj = new GlobalVar();
                return obj;
            }
        }
        /// <summary>
        /// 存储所有窗体指针，用来发送post信息
        /// </summary>
        public Dictionary<string, IntPtr> dicFormPtr { get; set; } = new Dictionary<string, IntPtr>();
        /// <summary>
        /// 加载信息
        /// </summary>
        public ConfigInfo configInfo = new ConfigInfo();
    }

    /// <summary>
    /// 用于通讯的消息ID
    /// </summary>
    enum MessageId : int
    {
        WM_USER = 0x400 + 100,// 1124开始
        /// <summary>自定义消息，用于测试交互</summary>
        WM_TESTDATA = WM_USER + 1
    }

    /// <summary>
    /// 记录在配置文件中的配置信息
    /// </summary>
    public class ConfigInfo
    {
        public ConfigInfo() { }
        /// <summary>
        /// 是否左键移动窗体（默认为true：为模态窗体使用，false：为模式窗体使用）
        /// </summary>
        public bool bMoveMainForm { get; set; } = true;
        /// <summary>
        /// WebAPI 地址
        /// </summary>
        public string SrvUrl { get; set; } = "http://localhost:5001";
        //public string SrvUrl { get; set; } = "http://127.0.0.1:4523/mock/785024"; // Mock模拟器地址，前期开发用
        /// <summary>
        /// 服务地址
        /// </summary>
        public string SrvUrl1 { get; set; } = "http://localhost:9001";
        /// <summary>
        /// 工位编号
        /// </summary>
        public string StationCode = "ZZ7";
        /// <summary>
        /// 工位ID
        /// </summary>
        public long? StationId = 317791888360869888;
        /// <summary>
        /// 工位ID
        /// </summary>
        public long? LineId = 266916128184340480;
        /// <summary>
        /// Andon通讯方式
        /// </summary>
        public int AndonTransType = 0;
    }
}
