using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Controls.Entity;

namespace Controls.Tool
{
    public class BytesConverter
    {
        public static bool BytesToStruct<T>(byte[] bytes, ref T? structure)
        {
            if (structure == null)
                return false;
            //int size = Marshal.SizeOf(structure.GetType());
            int size = bytes.Length;
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, 0, buffer, size);
                //object? obj = Marshal.PtrToStructure(buffer, structure.GetType());
                //object? obj = 
                    Marshal.PtrToStructure(buffer, structure);
                //if (obj == null)
                //    return false;
                //structure = (T)obj;
                return true;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        public static byte[] StructToBytes<T>(T data)
        {
            int size = Marshal.SizeOf(data);
            byte[] bytes = new byte[size];
            IntPtr ptr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(data, ptr, false);
                Marshal.Copy(ptr, bytes, 0, size);
            }
            finally
            {
                Marshal.FreeHGlobal(ptr);
            }
            return bytes;
        }
        public static void ReverseBytesByOrderFields<T>(ref byte[] bytes, T? structType)
        {
            if (structType == null)
                return;
            int offset = 0;
            var s = structType.GetType().GetProperties();
            foreach (var field in structType.GetType().GetProperties())
            {
                var endianAttr = field.GetCustomAttribute<ByteOrderAttribute>();
                if (endianAttr?.orderType == OrderTypeEnum.BigFirst)
                {
                    int fieldSize = Marshal.SizeOf(field.PropertyType);
                    Array.Reverse(bytes, offset, fieldSize);
                }
                if (field.PropertyType.Name.Contains("Byte[]"))
                {
                    PropertyInfo lengthProperty = structType.GetType().GetProperty("Length");
                    int length = Convert.ToInt32(lengthProperty.GetValue(structType));
                    length = 50;
                    offset += length;
                }
                else
                    offset += Marshal.SizeOf(field.PropertyType);
                //System.Diagnostics.Trace.WriteLine(offset);
            }
        }
        public static Byte[] HexStringToBytes(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex))
                return new byte[0];
            string tempHex = hex.Replace(" ", "");
            if (tempHex.Length % 2 != 0)
                throw new Exception("Hex长度不符");
            string pattern = @"^^[0-9A-Fa-f]*$";
            bool isHex = Regex.IsMatch("1A3F567890ABCDEF", pattern); // 返回 True
            if (!isHex)
                throw new Exception($"{hex} is not a hex");
            int length = tempHex.Length;
            byte[] bytes = new byte[length / 2];

            for (int i = 0; i < bytes.Length; i ++)
            {
                bytes[i] = Convert.ToByte(tempHex.Substring(i*2, 2), 16);
            }

            return bytes;
        }
        public static string ByteArrayToHexString(byte[] bytes, string split = "")
        {
            if (bytes == null)
                throw new ArgumentNullException(nameof(bytes));
            return BitConverter.ToString(bytes).Replace("-", split).ToUpper();
        }
    }
}