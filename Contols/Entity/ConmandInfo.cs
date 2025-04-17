using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.Entity
{
    /// <summary>
    /// 命令中的功能码相关信息
    /// </summary>
    public class FunctionInfo
    {
        /// <summary>
        /// 功能码
        /// </summary>
        public int FunctionId { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public required string FunctionCode { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        public required string FunctionName { get; set; }
        /// <summary>
        /// 数据长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 详细描述
        /// </summary>
        public required string Description { get; set; }
    }

    public class CommandInfo
    {
        public int Id { get; set; }
        public required string ConmandCode { get; set; }
        public required string ConmandName { get; set; }
        public CommandTypeEnum CommandType { get; set; }
        /// <summary>
        /// 字符串时有效
        /// </summary>
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public required string Description { get; set; }
    }

    public enum CommandTypeEnum
    {
        Byte,//uint8
        Short,//int16
        UShort,//uint16
        Int,//int32
        UInt,//uint32
        Float,//
        String//
    }
    public enum OperateType
    {
        Read,
        Write
    }

    /// <summary>
    /// 控件类型
    /// </summary>
    public enum ControlType
    {
        Button,
        RadioButton,
        CheckBox,
        Number,
        Text
    }
}
