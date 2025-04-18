using System.Runtime.InteropServices;
using System.Text;
using Controls.Entity;
using Controls.Tool;
using NLog;

namespace FormOpenSocket
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            GlobalVar.logger.Info("程序启动");
            #region  测试代码
            /*
            string h = "AA 55 05 00 00 01 00 02 05 05 12 00 FA F5";
            byte[] b = BytesConverter.HexStringToBytes(h);
            var transfer2 = Transfer.FromBytes(b, true);
            if (transfer2 == null) return;
            string p = transfer2.Value.ToHex(" ");
            System.Diagnostics.Trace.WriteLine("h   " + h);
            System.Diagnostics.Trace.WriteLine("p   " + p);

            System.Diagnostics.Trace.WriteLine("                ：" + "AA 55 05 00 00 01 00 01 00 00 00 FA F5");
            Transfer transfer = new Transfer(1);
            byte[] bytesT = BytesConverter.StructToBytes(transfer);
            string Hex = BytesConverter.ByteArrayToHexString(bytesT, " ");
            System.Diagnostics.Trace.WriteLine("未考虑大小端结果：" + Hex);

            BytesConverter.ReverseBytesByOrderFields(ref bytesT, transfer);
            Hex = BytesConverter.ByteArrayToHexString(bytesT, " ");
            System.Diagnostics.Trace.WriteLine("考虑到大小端结果：" + Hex);

            var arrayOut = transfer.ToBytes();
            Hex = BytesConverter.ByteArrayToHexString(arrayOut, " ");
            System.Diagnostics.Trace.WriteLine("带校验和的数据  ：" + Hex);

            Transfer transfer1 = new Transfer(0);
            transfer1.FunctionId = 0x0810;
            System.Diagnostics.Trace.WriteLine("带校验和的数据= ：" + transfer1.ToHex());
            */
            #endregion
            AppDomain.CurrentDomain.UnhandledException += (sender, e) => { Console.WriteLine("Unhandled Exception: " + e.ExceptionObject.ToString()); };
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}