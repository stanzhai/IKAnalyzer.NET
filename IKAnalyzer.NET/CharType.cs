using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET
{
    /// <summary>
    /// 字符类型，如阿拉伯数字，中文，英文等
    /// </summary>
    public enum CharType
    {
        /// <summary>
        /// 其他类型
        /// </summary>
        Other = 0,
        /// <summary>
        /// 阿拉伯数字
        /// </summary>
        Arabic = 0x00000001,
        /// <summary>
        /// 英文字符
        /// </summary>
        English = 0x00000002,
        /// <summary>
        /// 中文字符
        /// </summary>
        Chinese = 0x00000004,
        /// <summary>
        /// 全角半角，日文，朝鲜语字符
        /// </summary>
        Other_CJK = 0x00000008
    }
}
