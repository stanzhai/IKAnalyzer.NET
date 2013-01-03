using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET
{
    /// <summary>
    /// 词元
    /// </summary>
    public class Lexeme : IComparable<Lexeme>
    {
        private string _text;
        /// <summary>
        /// 词元文本
        /// </summary>
        public string Text 
        {
            get { return _text ?? ""; }
            set 
            {
                if (value == null)
                {
                    _text = "";
                    Length = 0;
                }
                else
                {
                    _text = value;
                    Length = _text.Length;
                }
            }
        }
        /// <summary>
        /// 词元类型
        /// </summary>
        public LexemeType Type { get; set; }
        /// <summary>
        /// 词元的起始位置
        /// </summary>
        public int Offset { get; set; }
        /// <summary>
        /// 词元的相对起始位置
        /// </summary>
        public int Begin { get; set; }
        /// <summary>
        /// 词元的长度
        /// </summary>
        public int Length { get; set; }

        public Lexeme(int offset, int begin, int length, LexemeType type)
        {
            this.Offset = offset;
            this.Begin = begin;
            if (length < 0)
            {
                throw new Exception("Lexeme Length < 0");
            }
            this.Length = length;
            this.Type = type;
        }

        /// <summary>
        /// 获取词元在文本中的起始位置
        /// </summary>
        /// <returns></returns>
        public int GetBeginPos()
        {
            return Offset + Begin;
        }

        /// <summary>
        /// 获取词元的在文本中的结束位置
        /// </summary>
        /// <returns></returns>
        public int GetEndPos()
        {
            return GetBeginPos() + Length;
        }

        /// <summary>
        /// 合并两个相邻的词元
        /// </summary>
        /// <param name="neighborLexeme"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Append(Lexeme neighborLexeme, LexemeType type)
        {
            if (neighborLexeme != null && this.GetEndPos() == neighborLexeme.GetBeginPos())
            {
                Length += neighborLexeme.Length;
                Type = type;
                return true;
            }
            return false;
        }

        /// <summary>
        /// 词元排序比较规则
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Lexeme other)
        {
            // 起始位置优先
            if (this.Begin < other.Begin)
            {
                return -1;
            }
            if (this.Begin == other.Begin)
            {
                // 词元长度优先
                if (this.Length > other.Length)
                {
                    return -1;
                }
                if (this.Length == other.Length)
                {
                    return 0;
                }
                return 1;
            }
            return 1;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (this == obj)
            {
                return true;
            }

            if (obj is Lexeme)
            {
                Lexeme other = obj as Lexeme;
                if (this.Offset == other.Offset &&
                    this.Begin == other.Begin &&
                    this.Length == other.Length)
                {
                    return true;
                }
                return false;
            }

            return false;
        }

        public override int GetHashCode()
        {
            int beginPos = GetBeginPos();
            int endPos = GetEndPos();
            return (beginPos * 37) + (endPos * 31) + ((beginPos * endPos) % Length) * 11;
        }

        public override string ToString()
        {
            return String.Format("[{0}:{1}-{2},{3}]", Text, GetBeginPos(), GetEndPos(), Type);
        }

    }
}
