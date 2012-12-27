using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET
{
    public class CharacterUtil
    {
        /// <summary>
        /// 获取字符类型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static CharType GetCharType(char input)
        {
            // 字符编码参考百度百科，unicode
            // http://baike.baidu.com/view/40801.htm

            if (input >= '0' && input <= '9')
            {
                return CharType.Arabic;
            }
            if ((input >= 'a' && input <= 'z') || (input >= 'A' && input <= 'Z'))
            {
                return CharType.English;
            }
            if (input >= 0x4e00 && input <= 0x9fa5)    // 汉字范围在0x4e00 - 0x9fa5
            {
                return CharType.Chinese;
            }
            if ((input >= 0xff00 && input <= 0xffef) ||         // 半角全角
                (input >= 0x3040 && input <= 0x309f) ||         // 日文平假名
                (input >= 0x30a0 && input <= 0x30ff) ||         // 日文片假名
                (input >= 0x1100 && input <= 0x11FF) ||         // 朝鲜文
                (input >= 0x3130 && input <= 0x318f))           // 朝鲜文兼容字母
            {
                return CharType.Other_CJK;
            }
            return CharType.Other;
        }

        /// <summary>
        /// 规范化字符，将全角转换为半角，大写转化为小写
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static char Regularize(char input)
        {
            if (input == 12288)
            {
                input = (char)32;

            }
            else if (input > 65280 && input < 65375)
            {
                input = (char)(input - 65248);

            }
            else if (input >= 'A' && input <= 'Z')
            {
                input = (char)(input + 32);
            }
            return input;
        }
    }
}
