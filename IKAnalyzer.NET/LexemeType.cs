using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET
{
    /// <summary>
    /// 词元类型
    /// </summary>
    public enum LexemeType
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown,
        /// <summary>
        /// 英文
        /// </summary>
        English,
        /// <summary>
        /// 数字
        /// </summary>
        Arablic,
        /// <summary>
        /// 英文数字混合
        /// </summary>
        Letter,
        /// <summary>
        /// 中文词元，单词
        /// </summary>
        CNWord,
        /// <summary>
        /// 中文单字
        /// </summary>
        CNChar,
        /// <summary>
        /// 中文数词
        /// </summary>
        CNNum,
        /// <summary>
        /// 中文数量词
        /// </summary>
        CNQuan,
        /// <summary>
        /// 中文量词
        /// </summary>
        CNCount,
        /// <summary>
        /// 日韩文字
        /// </summary>
        OtherCJK
    }
}
