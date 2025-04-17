using System;
using System.Runtime.InteropServices;
using Controls.Entity;
using Controls.Tool;

namespace Controls.Entity
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Transfer
    {
        private bool IsResponse;
        static ushort franme = 0;
        [ByteOrder(OrderTypeEnum.BigFirst)]
        public readonly UInt16 Head { get; } = 0xAA55;

        [ByteOrder(OrderTypeEnum.BigFirst)]
        public UInt16 FunctionId { get; set; } = 0x0500;

        [ByteOrder(OrderTypeEnum.BigFirst)]
        public UInt16 FrameId { get; set; } = 1;

        [ByteOrder(OrderTypeEnum.BigFirst)]
        public UInt16 Length { get; set; }

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 50)]
        public byte[] data;

        public byte[] Data { get { return data; } set { data = value; } }
        public UInt16 Sum { get; set; }

        [ByteOrder(OrderTypeEnum.BigFirst)]
        public readonly UInt16 Tail { get; } = 0xFAF5;

        public Transfer(int Len)
        {
            int _len = 50;
            _len = Len;//使用Marshal进行序列化的时候，屏蔽此项
            data = new byte[_len];
            Data = new byte[_len];
            Length = (ushort)Len;
        }
        /// <summary>
        /// 响应时使用
        /// </summary>
        /// <param name="Len"></param>
        /// <param name="Frame"></param>
        public Transfer(ushort Len, ushort Frame)
        {
            IsResponse = true;
            int _len = 50;
            _len = Len;//使用Marshal进行序列化的时候，屏蔽此项
            data = new byte[_len];
            Data = new byte[_len];
            Length = Len;
            this.FrameId = Frame;
        }
        void UpdateFrameId()
        {
            FrameId = ++Transfer.franme;
        }
        [Obsolete]
        public byte[] ToBytesBack()
        {
            byte[] bytesT = BytesConverter.StructToBytes(this);
            BytesConverter.ReverseBytesByOrderFields(ref bytesT, this);
            byte[] datas = new byte[Length + 12];
            Array.Copy(bytesT, 0, datas, 0, this.Length + 8);
            Array.Copy(bytesT, bytesT.Length - 2, datas, this.Length + 10, 2);
            int sum = 0;
            for (int i = 2; i < datas.Length - 2; i++)
                sum += datas[i];
            datas[datas.Length - 3] = Convert.ToByte(sum >> 8 % 256);
            datas[datas.Length - 4] = Convert.ToByte(sum % 256);
            return datas;
        }
        public byte[] ToBytes()
        {
            if(!IsResponse)
                UpdateFrameId();
            byte[] bytesResult = new byte[Length + 12];
            byte[] tempBytes = BitConverter.GetBytes(this.Head);
            BitConverter.GetBytes(this.Head).Reverse().ToArray().CopyTo(bytesResult, 0);
            BitConverter.GetBytes(this.FunctionId).CopyTo(bytesResult, 2);
            BitConverter.GetBytes(this.FrameId).Reverse().ToArray().CopyTo(bytesResult, 4);
            BitConverter.GetBytes(this.Length).Reverse().ToArray().CopyTo(bytesResult, 6);
            if (Length > 0)
                Data.CopyTo(bytesResult, 8);
            BitConverter.GetBytes(Tail).Reverse().ToArray().CopyTo(bytesResult, data.Length + 10);

            ushort sum = 0;
            for (int i = 2; i < bytesResult.Length - 4; i++)
                sum += bytesResult[i];
            this.Sum = sum;
            BitConverter.GetBytes(Sum).CopyTo(bytesResult, Data.Length + 8);

            return bytesResult;
        }
        public string ToHex(string split = "")
        {
            byte[] data = ToBytes();
            string Hex = BytesConverter.ByteArrayToHexString(data, split);
            return Hex;
        }
        public static string? ValidateFormat(byte[] bytes)
        {
            if (bytes.Length < 12) return "数据长度不足";
            if (bytes[0] != 0xAA || bytes[1] != 0X55) return "协议头不匹配";
            int lenBytes = bytes.Length;
            if (bytes[lenBytes - 2] != 0xFA || bytes[lenBytes - 1] != 0xF5) return "协议尾不匹配";
            int lenCompute = (bytes[6] << 8) + bytes[7];
            if (lenBytes - 12 != lenCompute)
                return "长度不匹配";
            return null;
        }
        public static bool ValidateSum(byte[] bytes)
        {
            int sum = 0;
            for (int i = 2; i <= bytes.Length - 5; i++)
                sum += bytes[i];
            int sumCompute = (bytes[bytes.Length - 3] << 8) + bytes[bytes.Length - 4];
            if (sum != sumCompute)
                return false;
            return true;
        }
        public static Transfer? FromBytes(byte[] bytes, bool checkSum = false)
        {
            if (!string.IsNullOrEmpty(ValidateFormat(bytes))) return null;
            if (checkSum) if (!ValidateSum(bytes)) return null;
            var transResult = new Transfer(bytes.Length - 12);
            Array.Copy(bytes, 8, transResult.data, 0, bytes.Length - 12);

            transResult.FunctionId = BitConverter.ToUInt16(bytes, 2);
            transResult.FrameId = Convert.ToUInt16((bytes[4] << 8) + bytes[5]);

            transResult.Sum = BitConverter.ToUInt16(bytes, bytes.Length - 4);
            //bool re = BytesConverter.BytesToStruct(bytes, ref result);
            return transResult;
        }
    }

    public class ByteOrderAttribute : Attribute
    {
        OrderTypeEnum _type = OrderTypeEnum.LittleFirst;
        public OrderTypeEnum orderType { get { return _type; } }
        public ByteOrderAttribute(OrderTypeEnum type)
        {
            this._type = type;
        }
    }

    public enum OrderTypeEnum
    {
        BigFirst,
        LittleFirst
    }
}
