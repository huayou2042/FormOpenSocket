using System;
using System.IO;
using System.Windows.Forms;
using System.Collections;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using System.CodeDom.Compiler;
using System.Data;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using Microsoft.Win32;
using Microsoft.CSharp;
//using NPOI.HSSF.UserModel;
using System.Threading;
using System.Net.NetworkInformation;
using System.ComponentModel;
//using YiSha.Util;

namespace Controls.Tool
{
    public class CommonTool
    {
        #region dll API接口函数
        ///   <summary>   
        ///   API发送消息函数
        ///   </summary> 
        ///   <param name="hWnd">其窗口程序接收消息的窗口的句柄</param>   
        ///   <param name="msg">指定被寄送的消息</param>
        ///   <param name="wParam">指定附加的消息特定的信息</param>
        ///   <param name="lParam">指定附加的消息特定的信息</param>
        [DllImport("user32.dll")]
        private static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        ///   <summary>   
        ///   API设置自动休眠挂起和屏幕的自动关闭
        ///   </summary>
        [DllImport("kernel32.dll")]
        static extern uint SetThreadExecutionState(ExecutionFlag flags);
        [Flags]
        enum ExecutionFlag : uint
        {
            System = 0x00000001,
            Display = 0x00000002,
            Continus = 0x80000000,
        }

        ///   <summary>   
        ///   等待某一线程完成了再继续做其他事情 （非主线程）
        ///   </summary>
        ///   <param name="hHandle">对象句柄</param> 
        ///   <param name="dwMilliseconds">定时时间间隔（毫秒）</param> 
        [DllImport("Kernel32.dll")]
        private static extern int WaitForSingleObject(IntPtr hHandle, int dwMilliseconds);

        ///   <summary>   
        ///   创建或打开一个命名的或无名的事件对象
        ///   </summary>
        ///   <param name="lpEventAttributes">可被子进程继承的结构的句柄</param> 
        ///   <param name="bManualReset">指定将事件对象创建成手动复原还是自动复原</param> 
        ///   <param name="bInitialState">指定事件对象的初始状态</param> 
        ///   <param name="lpName">指定事件的对象的名称</param> 
        [DllImport("kernel32.dll")]
        private static extern IntPtr CreateEvent(IntPtr lpEventAttributes, bool bManualReset, bool bInitialState, string lpName);

        ///   <summary>   
        ///   创建一个可等待的计时器对象
        ///   </summary>
        ///   <param name="lpEventAttributes">指定一个结构，用于设置对象的安全特性</param> 
        ///   <param name="bManualReset">创建一个手工重置计时器或自动重置计时器</param> 
        ///   <param name="lpTimerName">指定可等待计时器对象的名称</param> 
        //[DllImport("kernel32.dll")]
        //static extern IntPtr CreateWaitableTimer(IntPtr lpTimerAttributes, bool bManualReset, string lpTimerName);

        ///   <summary>   
        ///   关闭一个内核对象
        ///   </summary>
        ///   <param name="hObject">欲关闭的一个对象的句柄</param> 
        //[DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi, CharSet = CharSet.Auto)]
        //[return: MarshalAs(UnmanagedType.Bool)]
        //static extern bool CloseHandle(IntPtr hObject);

        ///   <summary>   
        ///   等待一个线程时仍可以响应消息
        ///   </summary>
        ///   <param name="nCount">指定列表中的句柄数量</param> 
        ///   <param name="handle">指定对象句柄组合中的第一个元素</param> 
        ///   <param name="fWaitAll">如果为TRUE，表示除非对象同时发出信号，否则就等待下去。如果为FALSE，表示任何对象发出信号即可。</param> 
        ///   <param name="dwMilliseconds">指定要等待的毫秒数</param> 
        ///   <param name="dwWakeMask">欲观察的用户输入消息类型</param> 
        //[DllImport("User32.dll")]
        //static extern int MsgWaitForMultipleObjects(int nCount, ref IntPtr handle, bool fWaitAll, int dwMilliseconds, int dwWakeMask);

        ///   <summary>   
        ///   运行一个外部程序
        ///   </summary>
        ///   <param name="hwnd">指定父窗口句柄</param>
        ///   <param name="lpOperation">指定动作, 譬如: open、runas、print、edit、explore、find</param>
        ///   <param name="lpFile">指定要打开的文件或程序</param>
        ///   <param name="lpParameters">给要打开的程序指定参数</param>
        ///   <param name="lpDirectory">缺省目录</param>
        ///   <param name="nShowCmd">打开选项</param>
        //[DllImport("shell32.dll")]
        //static extern IntPtr ShellExecute(IntPtr hwnd, string lpOperation, string lpFile, string lpParameters, string lpDirectory, TShowCommands nShowCmd);

        /// <summary>
        /// 获得一个顶层窗口的句柄，不查找子窗口
        /// </summary>
        /// <param name="lpClassName">指向一个指定了类名的空结束字符串</param>
        /// <param name="lpWindowName">指向一个指定了窗口名</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        public extern static IntPtr FindWindow(string lpClassName, string lpWindowName);

        /// <summary>
        /// 模拟按键
        /// </summary>
        /// <param name="bVk"></param>
        /// <param name="bScan"></param>
        /// <param name="dwFlags"></param>
        /// <param name="dwExtraInfo"></param>
        /// <returns></returns>
        [DllImport("user32.dll")]
        public static extern IntPtr keybd_event(
           byte bVk,        // 虚拟键值
           byte bScan,      // 一般为0
           int dwFlags,     // 这里是整数类型  0 为按下，2为释放
           int dwExtraInfo  // 这里是整数类型 一般情况下设成为 0
        );
        #endregion
        ///   <summary>   
        ///   阻止系统休眠，直到线程结束恢复休眠策略
        ///   </summary> 
        ///   <param name="includeDisplay">是否阻止关闭显示器</param>
        ///   <returns>无</returns>
        public static void PreventSleep(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display | ExecutionFlag.Continus);
            else
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Continus);
        }

        ///   <summary>   
        ///   恢复系统休眠策略
        ///   </summary> 
        ///   <returns>无</returns>
        public static void ResotreSleep()
        {
            SetThreadExecutionState(ExecutionFlag.Continus);
        }

        ///   <summary>   
        ///   重置系统休眠计时器
        ///   </summary> 
        ///   <param name="includeDisplay">是否阻止关闭显示器</param>
        ///   <returns>无</returns>
        public static void ResetSleepTimer(bool includeDisplay = false)
        {
            if (includeDisplay)
                SetThreadExecutionState(ExecutionFlag.System | ExecutionFlag.Display);
            else
                SetThreadExecutionState(ExecutionFlag.System);
        }

        ///   <summary>   
        ///   应用程序路径GetAppPath(不带"\")
        ///   </summary> 
        ///   <returns>返回应用程序目录</returns>
        public static string GetAppPath()
        {
            string sPath;
            sPath = Application.StartupPath;
            return sPath;
        }

        ///   <summary>   
        ///   字符串使用UTF8编码 
        ///   </summary> 
        ///   <returns>UTF8编码字符串</returns>
        public static string Get_UTF8(string unicodeString)
        {
            UTF8Encoding utf8 = new UTF8Encoding();
            Byte[] encodedBytes = utf8.GetBytes(unicodeString);
            String decodedString = utf8.GetString(encodedBytes);
            return decodedString;
        }


        ///   <summary>   
        ///   按特定条件分隔字符串，并读取分隔内容  
        ///   </summary> 
        ///   <param name="sText">需要分隔的源字符串</param>
        ///   <param name="sMask">分隔字符</param>
        ///   <param name="iPos">分隔字符位置</param>
        ///   <returns>返回按分隔字符位置分隔出来的字符串内容</returns>
        public static string GetMaskString(string sText, string sMask, int iPos)
        {
            string sReturn = "";
            int Len = 0;
            for (int i = 0; i <= iPos - 1; i++)
            {
                if (sText.IndexOf(sMask) < 0)
                {
                    sReturn = sText;
                    break;
                }
                sReturn = sText.Substring(0, sText.IndexOf(sMask));
                Len = sReturn.Length;
                sText = sText.Substring(Len + 1, sText.Length - Len - 1);
            }
            return sReturn;
        }

        #region==助记符==
        /// <summary>
        /// 定义拼音区编码数组
        /// </summary>
        private static int[] getValue = new int[]
        {
                -20319,-20317,-20304,-20295,-20292,-20283,-20265,-20257,-20242,-20230,-20051,-20036,
                -20032,-20026,-20002,-19990,-19986,-19982,-19976,-19805,-19784,-19775,-19774,-19763,
                -19756,-19751,-19746,-19741,-19739,-19728,-19725,-19715,-19540,-19531,-19525,-19515,
                -19500,-19484,-19479,-19467,-19289,-19288,-19281,-19275,-19270,-19263,-19261,-19249,
                -19243,-19242,-19238,-19235,-19227,-19224,-19218,-19212,-19038,-19023,-19018,-19006,
                -19003,-18996,-18977,-18961,-18952,-18783,-18774,-18773,-18763,-18756,-18741,-18735,
                -18731,-18722,-18710,-18697,-18696,-18526,-18518,-18501,-18490,-18478,-18463,-18448,
                -18447,-18446,-18239,-18237,-18231,-18220,-18211,-18201,-18184,-18183, -18181,-18012,
                -17997,-17988,-17970,-17964,-17961,-17950,-17947,-17931,-17928,-17922,-17759,-17752,
                -17733,-17730,-17721,-17703,-17701,-17697,-17692,-17683,-17676,-17496,-17487,-17482,
                -17468,-17454,-17433,-17427,-17417,-17202,-17185,-16983,-16970,-16942,-16915,-16733,
                -16708,-16706,-16689,-16664,-16657,-16647,-16474,-16470,-16465,-16459,-16452,-16448,
                -16433,-16429,-16427,-16423,-16419,-16412,-16407,-16403,-16401,-16393,-16220,-16216,
                -16212,-16205,-16202,-16187,-16180,-16171,-16169,-16158,-16155,-15959,-15958,-15944,
                -15933,-15920,-15915,-15903,-15889,-15878,-15707,-15701,-15681,-15667,-15661,-15659,
                -15652,-15640,-15631,-15625,-15454,-15448,-15436,-15435,-15419,-15416,-15408,-15394,
                -15385,-15377,-15375,-15369,-15363,-15362,-15183,-15180,-15165,-15158,-15153,-15150,
                -15149,-15144,-15143,-15141,-15140,-15139,-15128,-15121,-15119,-15117,-15110,-15109,
                -14941,-14937,-14933,-14930,-14929,-14928,-14926,-14922,-14921,-14914,-14908,-14902,
                -14894,-14889,-14882,-14873,-14871,-14857,-14678,-14674,-14670,-14668,-14663,-14654,
                -14645,-14630,-14594,-14429,-14407,-14399,-14384,-14379,-14368,-14355,-14353,-14345,
                -14170,-14159,-14151,-14149,-14145,-14140,-14137,-14135,-14125,-14123,-14122,-14112,
                -14109,-14099,-14097,-14094,-14092,-14090,-14087,-14083,-13917,-13914,-13910,-13907,
                -13906,-13905,-13896,-13894,-13878,-13870,-13859,-13847,-13831,-13658,-13611,-13601,
                -13406,-13404,-13400,-13398,-13395,-13391,-13387,-13383,-13367,-13359,-13356,-13343,
                -13340,-13329,-13326,-13318,-13147,-13138,-13120,-13107,-13096,-13095,-13091,-13076,
                -13068,-13063,-13060,-12888,-12875,-12871,-12860,-12858,-12852,-12849,-12838,-12831,
                -12829,-12812,-12802,-12607,-12597,-12594,-12585,-12556,-12359,-12346,-12320,-12300,
                -12120,-12099,-12089,-12074,-12067,-12058,-12039,-11867,-11861,-11847,-11831,-11798,
                -11781,-11604,-11589,-11536,-11358,-11340,-11339,-11324,-11303,-11097,-11077,-11067,
                -11055,-11052,-11045,-11041,-11038,-11024,-11020,-11019,-11018,-11014,-10838,-10832,
                -10815,-10800,-10790,-10780,-10764,-10587,-10544,-10533,-10519,-10331,-10329,-10328,
                -10322,-10315,-10309,-10307,-10296,-10281,-10274,-10270,-10262,-10260,-10256,-10254
        };

        /// <summary>
        /// 定义拼音数组
        /// </summary>
        private static string[] getName = new string[]
        {
                "A","Ai","An","Ang","Ao","Ba","Bai","Ban","Bang","Bao","Bei","Ben",
                "Beng","Bi","Bian","Biao","Bie","Bin","Bing","Bo","Bu","Ba","Cai","Can",
                "Cang","Cao","Ce","Ceng","Cha","Chai","Chan","Chang","Chao","Che","Chen","Cheng",
                "Chi","Chong","Chou","Chu","Chuai","Chuan","Chuang","Chui","Chun","Chuo","Ci","Cong",
                "Cou","Cu","Cuan","Cui","Cun","Cuo","Da","Dai","Dan","Dang","Dao","De",
                "Deng","Di","Dian","Diao","Die","Ding","Diu","Dong","Dou","Du","Duan","Dui",
                "Dun","Duo","E","En","Er","Fa","Fan","Fang","Fei","Fen","Feng","Fo",
                "Fou","Fu","Ga","Gai","Gan","Gang","Gao","Ge","Gei","Gen","Geng","Gong",
                "Gou","Gu","Gua","Guai","Guan","Guang","Gui","Gun","Guo","Ha","Hai","Han",
                "Hang","Hao","He","Hei","Hen","Heng","Hong","Hou","Hu","Hua","Huai","Huan",
                "Huang","Hui","Hun","Huo","Ji","Jia","Jian","Jiang","Jiao","Jie","Jin","Jing",
                "Jiong","Jiu","Ju","Juan","Jue","Jun","Ka","Kai","Kan","Kang","Kao","Ke",
                "Ken","Keng","Kong","Kou","Ku","Kua","Kuai","Kuan","Kuang","Kui","Kun","Kuo",
                "La","Lai","Lan","Lang","Lao","Le","Lei","Leng","Li","Lia","Lian","Liang",
                "Liao","Lie","Lin","Ling","Liu","Long","Lou","Lu","Lv","Luan","Lue","Lun",
                "Luo","Ma","Mai","Man","Mang","Mao","Me","Mei","Men","Meng","Mi","Mian",
                "Miao","Mie","Min","Ming","Miu","Mo","Mou","Mu","Na","Nai","Nan","Nang",
                "Nao","Ne","Nei","Nen","Neng","Ni","Nian","Niang","Niao","Nie","Nin","Ning",
                "Niu","Nong","Nu","Nv","Nuan","Nue","Nuo","O","Ou","Pa","Pai","Pan",
                "Pang","Pao","Pei","Pen","Peng","Pi","Pian","Piao","Pie","Pin","Ping","Po",
                "Pu","Qi","Qia","Qian","Qiang","Qiao","Qie","Qin","Qing","Qiong","Qiu","Qu",
                "Quan","Que","Qun","Ran","Rang","Rao","Re","Ren","Reng","Ri","Rong","Rou",
                "Ru","Ruan","Rui","Run","Ruo","Sa","Sai","San","Sang","Sao","Se","Sen",
                "Seng","Sha","Shai","Shan","Shang","Shao","She","Shen","Sheng","Shi","Shou","Shu",
                "Shua","Shuai","Shuan","Shuang","Shui","Shun","Shuo","Si","Song","Sou","Su","Suan",
                "Sui","Sun","Suo","Ta","Tai","Tan","Tang","Tao","Te","Teng","Ti","Tian",
                "Tiao","Tie","Ting","Tong","Tou","Tu","Tuan","Tui","Tun","Tuo","Wa","Wai",
                "Wan","Wang","Wei","Wen","Weng","Wo","Wu","Xi","Xia","Xian","Xiang","Xiao",
                "Xie","Xin","Xing","Xiong","Xiu","Xu","Xuan","Xue","Xun","Ya","Yan","Yang",
                "Yao","Ye","Yi","Yin","Ying","Yo","Yong","You","Yu","Yuan","Yue","Yun",
                "Za", "Zai","Zan","Zang","Zao","Ze","Zei","Zen","Zeng","Zha","Zhai","Zhan",
                "Zhang","Zhao","Zhe","Zhen","Zheng","Zhi","Zhong","Zhou","Zhu","Zhua","Zhuai","Zhuan",
                "Zhuang","Zhui","Zhun","Zhuo","Zi","Zong","Zou","Zu","Zuan","Zui","Zun","Zuo"
        };

        /// <summary>
        /// 获取助记符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMnemonics(string str)
        {
            Regex reg = new Regex("^[\u4e00-\u9fa5]$");//验证是否输入汉字
            byte[] arr = new byte[2];
            string pystr = "";
            int asc = 0, M1 = 0, M2 = 0;
            char[] mChar = str.ToCharArray();//获取汉字对应的字符数组
            for (int j = 0; j < mChar.Length; j++)
            {
                //如果输入的是汉字
                if (reg.IsMatch(mChar[j].ToString()))
                {
                    arr = System.Text.Encoding.Default.GetBytes(mChar[j].ToString());
                    M1 = (short)(arr[0]);
                    M2 = (short)(arr[1]);
                    asc = M1 * 256 + M2 - 65536;
                    if (asc > 0 && asc < 160)
                    {
                        pystr += mChar[j];
                    }
                    else
                    {
                        switch (asc)
                        {
                            case -9254:
                                pystr += "Zhen"; break;
                            case -8985:
                                pystr += "Qian"; break;
                            case -5463:
                                pystr += "Jia"; break;
                            case -8274:
                                pystr += "Ge"; break;
                            case -5448:
                                pystr += "Ga"; break;
                            case -5447:
                                pystr += "La"; break;
                            case -4649:
                                pystr += "Chen"; break;
                            case -5436:
                                pystr += "Mao"; break;
                            case -5213:
                                pystr += "Mao"; break;
                            case -3597:
                                pystr += "Die"; break;
                            case -5659:
                                pystr += "Tian"; break;
                            default:
                                for (int i = (getValue.Length - 1); i >= 0; i--)
                                {
                                    if (getValue[i] <= asc)//判断汉字的拼音区编码是否在指定范围内
                                    {
                                        pystr += getName[i].First();//如果不超出范围则获取对应的拼音
                                        break;
                                    }
                                }
                                break;
                        }
                    }
                }
                else//如果不是汉字
                {
                    pystr += mChar[j].ToString();//如果不是汉字则返回
                }
            }
            return pystr.ToUpper();//返回获取到的汉字拼音
        }
        #endregion

        /// <summary>
        /// list集合转datatable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entitys"></param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                return new DataTable();
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();
            //生成DataTable的structure
            DataTable dt = new DataTable("dt");
            for (int i = 0; i < entityProperties.Length; i++)
            {
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);

                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }

        private static System.Threading.Mutex mutex;
        /// <summary>   
        /// 判断软件是否是运行一个实例
        /// </summary> 
        /// <returns>判断结果</returns>
        public static bool IsOneInstance()
        {
            bool bResult = true;
            string sAppName;
            sAppName = Path.GetFileNameWithoutExtension(Application.ExecutablePath);
            mutex = new System.Threading.Mutex(true, sAppName, out bResult);
            if (!bResult)
            {
                bResult = false;
            }
            return bResult;
        }

        /// <summary>
        /// 重启软件
        /// </summary>
        public static void ExecuteResetMe()
        {
            try
            {
                //获取当前程序集的EXE的名称
                string fileName = Path.GetFileName(Application.ExecutablePath);
                //获取当前程序集的完整路径
                string currentPath = AppDomain.CurrentDomain.BaseDirectory + @"ResetMe.bat";
                List<string> resetMeList = new List<string>();
                resetMeList.Add("@each off");
                resetMeList.Add(":loop");
                resetMeList.Add("tasklist|findstr " + "\"" + fileName + " " + fileName + "\"");
                resetMeList.Add("if %errorlevel%==0 goto loop");
                resetMeList.Add(@"start .\" + "\"" + fileName + "\"");
                resetMeList.Add("del " + "\"" + @".\ResetMe.bat" + "\"");
                resetMeList.Add("exit");
                File.WriteAllLines(currentPath, resetMeList.ToArray());

                ProcessStartInfo startInfo = new ProcessStartInfo();
                startInfo.CreateNoWindow = true;
                startInfo.FileName = currentPath;
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;
                Process.Start(startInfo);  //直接调用打开文件

                // 刷新主界面
                //if (TCPClass.hWndList.ContainsKey("MainForm"))
                //    PostMessage(TCPClass.hWndList["MainForm"], GlobalInfo.WM_SHUTDOWNAPP, 0, 0);
            }
            catch
            {
            }

        }

        /// <summary>   
        /// 线程延时
        /// </summary> 
        /// <param name="AMilliSeconds">延时毫秒数</param>
        /// <returns>无</returns>
        public static void Delay(int AMilliSeconds)
        {
            IntPtr IHandle;
            IHandle = CreateEvent(IntPtr.Zero, true, false, "");
            WaitForSingleObject(IHandle, AMilliSeconds);
        }

        /// <summary>   
        /// 判断架号是否有效
        /// </summary> 
        /// <param name="ARackNo">样品架号</param>
        /// <returns>架号是否有效</returns>
        public static bool IsValidRack(ushort ARackNo)
        {
            string[] StrArr = { "1", "2", "3", "4", "5", "6" };
            bool bResult = ((IList)StrArr).Contains(Convert.ToString(Math.Floor((decimal)ARackNo / 1000)));
            bResult = bResult && ((ARackNo % 1000) != 0);
            return bResult;
        }

        /// <summary>
        /// 文本对其方式转换
        /// </summary>
        /// <param name="ContentAlign"></param>
        /// <param name="StringAlign"></param>
        public static void ContentAlignmentToStringFormat(ContentAlignment ContentAlign, ref StringFormat StringAlign)
        {
            switch (ContentAlign)
            {
                case ContentAlignment.TopLeft:
                    { StringAlign.LineAlignment = StringAlignment.Near; StringAlign.Alignment = StringAlignment.Near; break; }
                case ContentAlignment.TopCenter:
                    { StringAlign.LineAlignment = StringAlignment.Near; StringAlign.Alignment = StringAlignment.Center; break; }
                case ContentAlignment.TopRight:
                    { StringAlign.LineAlignment = StringAlignment.Near; StringAlign.Alignment = StringAlignment.Far; break; }
                case ContentAlignment.MiddleLeft:
                    { StringAlign.LineAlignment = StringAlignment.Center; StringAlign.Alignment = StringAlignment.Near; break; }
                case ContentAlignment.MiddleCenter:
                    { StringAlign.LineAlignment = StringAlignment.Center; StringAlign.Alignment = StringAlignment.Center; break; }
                case ContentAlignment.MiddleRight:
                    { StringAlign.LineAlignment = StringAlignment.Center; StringAlign.Alignment = StringAlignment.Far; break; }
                case ContentAlignment.BottomLeft:
                    { StringAlign.LineAlignment = StringAlignment.Far; StringAlign.Alignment = StringAlignment.Near; break; }
                case ContentAlignment.BottomCenter:
                    { StringAlign.LineAlignment = StringAlignment.Far; StringAlign.Alignment = StringAlignment.Center; break; }
                case ContentAlignment.BottomRight:
                    { StringAlign.LineAlignment = StringAlignment.Far; StringAlign.Alignment = StringAlignment.Far; break; }
            }
        }

        /// <summary>
        /// 截取字符串，不限制字符串长度
        /// </summary>
        /// <param name="str">待截取的字符串</param>
        /// <param name="len">每行的长度，多于这个长度自动换行</param>
        /// <returns></returns>
        public static string CutStr2(string str, int len)
        {
            //汉字和数字都当做一个个字节
            string result = string.Empty;
            int strLenth = System.Text.Encoding.Default.GetByteCount(str);
            for (int i = 0; i < strLenth; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {
                    if (r == 0) //换行
                    {
                        result += str.Substring(i - len, len) + "\r\n";
                    }
                }
                else if (i > last)
                {
                    result += str.Substring(i - 1);
                    break;
                }
            }
            return result;
        }

        /// <summary>   
        /// 字符转ASCII码。(长度为1)
        /// </summary> 
        /// <param name="Character">需要转换的字符串，长度为1</param>
        ///   <returns>返回值</returns>
        public static int StrToAscii(string Character)
        {
            if (Character.Length == 1)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                int intAsciiCode = (int)asciiEncoding.GetBytes(Character)[0];
                return (intAsciiCode);
            }
            else
                return 0;
            //throw new Exception("Character is not valid.");
        }

        /// <summary>   
        /// ASCII码转字符。(长度为1)
        /// </summary> 
        /// <param name="AsciiCode">需要转换的ASCII码</param>
        ///   <returns>返回值</returns>
        public static string AsciiToStr(int AsciiCode)
        {
            if (AsciiCode >= 0 && AsciiCode <= 255)
            {
                System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
                byte[] byteArray = new byte[] { (byte)AsciiCode };
                string strCharacter = asciiEncoding.GetString(byteArray);
                return (strCharacter);
            }
            else
                return "";
            //throw new Exception("ASCII Code is not valid.");
        }

        /// <summary>   
        /// 字符串AES加密函数(每次生成结果都不同)   ...需修改
        /// </summary> 
        /// <param name="plainText">要加密的字符串</param>
        /// <param name="AESKey">加密密钥</param>
        ///   <returns>返回加密后的字符串</returns>
        public static string AESEncryptStr(string plainText, string AESKey)
        {
            if (AESKey.Length == 0)
                AESKey = "NBMedicalSystem"; //...加密密钥默认值
            if (plainText.Length == 0)
                return "";
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(AESKey.PadRight(bKey.Length)), bKey, bKey.Length);
            try
            {
                var rijndaelCipher = Aes.Create();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(plainText);
                rijndaelCipher.Key = bKey;
                rijndaelCipher.GenerateIV();
                byte[] keyIv = rijndaelCipher.IV;
                byte[] cipherBytes = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, rijndaelCipher.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        cipherBytes = ms.ToArray();
                        cs.Close();
                        ms.Close();
                    }
                }
                var allEncrypt = new byte[keyIv.Length + cipherBytes.Length];
                Buffer.BlockCopy(keyIv, 0, allEncrypt, 0, keyIv.Length);
                Buffer.BlockCopy(cipherBytes, 0, allEncrypt, keyIv.Length * sizeof(byte), cipherBytes.Length);
                return Convert.ToBase64String(allEncrypt);
            }
            catch
            {
                return "";
            }
        }

        /// <summary>   
        /// 字符串AES解密函数。   
        /// </summary> 
        /// <param name="showText">要解密的字符串</param>
        /// <param name="AESKey">解密密钥</param>
        ///   <returns>返回解密后的字符串</returns>
        public static string AESDecryptStr(string showText, string AESKey)
        {
            string result = string.Empty;
            if (AESKey.Length == 0)
                AESKey = "NBMedicalSystem"; //...解密密钥默认值
            if (showText.Length == 0)
                return result;
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(AESKey.PadRight(bKey.Length)), bKey, bKey.Length);
            try
            {
                byte[] cipherText = Convert.FromBase64String(showText);
                int length = cipherText.Length;
                var rijndaelCipher = Aes.Create();
                rijndaelCipher.Key = bKey;
                byte[] iv = new byte[16];
                Buffer.BlockCopy(cipherText, 0, iv, 0, 16);
                rijndaelCipher.IV = iv;
                byte[] decryptBytes = new byte[length - 16];
                byte[] passwdText = new byte[length - 16];
                Buffer.BlockCopy(cipherText, 16, passwdText, 0, length - 16);
                using (MemoryStream ms = new MemoryStream(passwdText))
                {
                    using (CryptoStream cs = new CryptoStream(ms, rijndaelCipher.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        cs.ReadExactly(decryptBytes, 0, decryptBytes.Length);
                        cs.Close();
                        ms.Close();
                    }
                }
                result = Encoding.UTF8.GetString(decryptBytes).Replace("\0", "");  //将字符串后尾的'\0'去掉
                return result;
            }
            catch
            {
                return "";
            }
        }

        ///   <summary>   
        ///   字符串RSA加密函数。   ...需修改
        ///   </summary> 
        ///   <param name="ASource">要加密的字符串</param>
        ///   <param name="AEncryptkey">加密密钥</param>
        ///   <returns>返回加密字符串</returns>
        public static string RSAEncryptStr(string ASource, string AEncryptkey)
        {
            CspParameters Param = new CspParameters();
            if (AEncryptkey == "")
                AEncryptkey = "meikang";
            Param.KeyContainerName = AEncryptkey;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(Param))
            {
                byte[] plaindata = Encoding.Default.GetBytes(ASource);
                byte[] encryptdata = RSA.Encrypt(plaindata, false);
                return Convert.ToBase64String(encryptdata);
            }
        }

        ///   <summary>   
        ///   字符串RSA解密函数。   ...需修改
        ///   </summary> 
        ///   <param name="ASource">要加密的字符串</param>
        ///   <param name="AEncryptkey">加密密钥</param>
        ///   <returns>返回解密字符串</returns>
        public static string RSADecryptStr(string ASource, string AEncryptkey)
        {
            CspParameters Param = new CspParameters();
            if (AEncryptkey == "")
                AEncryptkey = "meikang"; // ...需修改
            Param.KeyContainerName = AEncryptkey;
            using (RSACryptoServiceProvider RSA = new RSACryptoServiceProvider(Param))
            {
                byte[] encryptdata = Convert.FromBase64String(ASource);
                byte[] decryptdata = RSA.Decrypt(encryptdata, false);
                return Encoding.Default.GetString(decryptdata);
            }
        }
        ///   <summary>   
        ///   根据试剂量取得试剂针多吸的量   ...需修改
        ///   </summary> 
        ///   <param name="R1Vol">R1试剂量</param>
        ///   <param name="R3Vol">R3试剂量</param>
        ///   <param name="Num">R1或R3</param>
        ///   <param name="Type">两种模式 0 or 1</param>
        ///   <returns>返回计算出的试剂量</returns>
        public static int GetReagentDummyVol(ushort R1Vol, ushort R3Vol, byte Num, byte Type)
        {
            float fResult = 0;
            float k;
            if (Type == 0)
                fResult = 0;
            else
            {
                if (Num == 1)
                {
                    if (R3Vol == 0)
                        fResult = 0;
                    else
                    {
                        if (R1Vol >= R3Vol)
                        {
                            k = R1Vol / R3Vol;
                            if (k >= 5)
                                fResult = 40;
                            else if (k >= 4)
                            {
                                if (R3Vol > 40)
                                    fResult = k * (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = (k * 8);
                            }
                            else if (k >= 3)
                            {
                                if (R3Vol > 50)
                                    fResult = k * (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = k * (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = (k * 8);
                            }
                            else if (k >= 2)
                            {
                                if (R3Vol > 67)
                                    fResult = k * (13 + (R3Vol - 67) / 5);
                                else if (R3Vol > 50)
                                    fResult = k * (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = k * (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = (k * 8);
                            }
                            else if (k >= 1)
                            {
                                if (R3Vol > 100)
                                    fResult = k * (20 + (R3Vol - 100) / 10);
                                else if (R3Vol > 67)
                                    fResult = k * (13 + (R3Vol - 67) / 5);
                                else if (R3Vol > 50)
                                    fResult = k * (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = k * (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = (k * 8);
                            }
                        }
                        else
                            fResult = 0;
                    }
                }
                else if (Num == 3)
                {
                    if (R1Vol == 0)
                        fResult = 0;
                    else
                    {
                        if (R3Vol <= R1Vol)
                        {
                            k = R1Vol / R3Vol;
                            if (k >= 5)
                                fResult = 8;
                            else if (k >= 4)
                            {
                                if (R3Vol > 40)
                                    fResult = (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = 8;
                            }
                            else if (k >= 3)
                            {
                                if (R3Vol > 50)
                                    fResult = (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = 8;
                            }
                            else if (k >= 2)
                            {
                                if (R3Vol > 67)
                                    fResult = (13 + (R3Vol - 67) / 5);
                                else if (R3Vol > 50)
                                    fResult = (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = 8;
                            }
                            else if (k >= 1)
                            {
                                if (R3Vol > 100)
                                    fResult = (20 + (R3Vol - 100) / 10);
                                else if (R3Vol > 67)
                                    fResult = (13 + (R3Vol - 67) / 5);
                                else if (R3Vol > 50)
                                    fResult = (10 + (R3Vol - 50) / 5);
                                else if (R3Vol > 40)
                                    fResult = (8 + (R3Vol - 40) / 5);
                                else
                                    fResult = 8;
                            }
                        }
                        else
                            fResult = 0;
                    }
                }
            }
            return (int)Math.Round(fResult, 0);
        }

        ///   <summary>   
        ///   读取EXE文件版本信息   
        ///   </summary>
        ///   <param name="AFileName">EXE文件路径</param>
        ///   <returns>返回EXE文件的版本信息</returns>
        public static string GetFileVersionInformation(string AFileName)
        {
            string sResult = "";
            try
            {
                if (File.Exists(AFileName))
                {
                    System.Diagnostics.FileVersionInfo info = System.Diagnostics.FileVersionInfo.GetVersionInfo(AFileName);
                    sResult = info.FileVersion;  //文件版本
                    //return info.ProductVersion;  //产品版本
                }
            }
            catch
            {
                sResult = "";
            }
            return sResult;
        }

        /// <summary>
        /// 返回指示文件是否已被其它程序使用
        /// </summary>
        /// <param name="fileFullName">文件的完全限定名</param>
        /// <returns>如果文件已被其它程序使用，则为 true；否则为 false。</returns>
        public static bool IsFileInUse(string fileName)
        {
            bool inUse = true;
            if (!System.IO.File.Exists(fileName))
                return false;
            FileStream? fs = null;
            try
            {
                fs = new FileStream(fileName, FileMode.Open, FileAccess.Read,
                FileShare.None);
                inUse = false;
            }
            catch (Exception e)
            {
                Console.WriteLine(String.Format("IsFileInUse - Error : \r\n{0}", e.Message));
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }
            return inUse;//true表示正在使用,false没有使用  
        }

        /// <summary>
        /// 验证IP地址是否合法(使用正则表达式)
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>
        public static bool IsIP(string ip)
        {
            if (string.IsNullOrEmpty(ip))
                return false;
            ip = ip.Trim(); //清除空格
            //正则表达式
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";
            //验证
            return Regex.IsMatch(ip, pattern);
        }

        /// <summary>
        /// 通过枚举获取枚举描述
        /// </summary>
        /// <param name="en"></param>
        /// <returns></returns>
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] info = type.GetMember(en.ToString());
            if (info != null && info.Length > 0)
            {
                DescriptionAttribute[] attrs = info[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description;
                }
            }
            return en.ToString();
        }
        /// <summary>
        /// 通过枚举名  获取枚举值GetEnumIdByStr(db.GetStringByName("name"), new name() );
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mobj"></param>
        /// <returns></returns>
        public static int GetEnumValueByStr(string name, Enum mobj)
        {
            int idval = -1;
            foreach (int value in Enum.GetValues(mobj.GetType()))
            {
                string strtemp = Enum.GetName(mobj.GetType(), value);
                if (strtemp.CompareTo(name) != 0)
                    continue;
                idval = value;
                break;
            }
            return idval;
        }

        /// <summary>
        /// 将窗体句柄加入全局字典,只有Application.Run(form)或者form.Show()之后才有句柄
        /// </summary>
        /// <param name="sFromName"></param>
        public static void FormPtrAdd(string sFromName, IntPtr iFormHandle)
        {
            try
            {
                // 窗体句柄加入字典
                lock (GlobalVar.Instance.dicFormPtr)
                {
                    if (!GlobalVar.Instance.dicFormPtr.ContainsKey(sFromName))
                        GlobalVar.Instance.dicFormPtr.Add(sFromName, iFormHandle);
                }
            }
            catch (Exception)
            {
                //LogHelper.Error(ex);
            }
        }
        /// <summary>
        /// 从全局字典中删除窗体句柄
        /// </summary>
        /// <param name="sFromName"></param>
        public static void FormPtrRemove(string sFromName)
        {
            try
            {
                // 删除窗体句柄
                lock (GlobalVar.Instance.dicFormPtr)
                {
                    if (GlobalVar.Instance.dicFormPtr.ContainsKey(sFromName))
                        GlobalVar.Instance.dicFormPtr.Remove(sFromName);
                }
            }
            catch
            {
            }
        }
    }
}