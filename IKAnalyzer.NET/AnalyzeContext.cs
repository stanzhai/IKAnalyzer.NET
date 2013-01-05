using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET
{
    public class AnalyzeContext
    {
        // 字符缓冲区大小
        private const int BufferSize = 4096;
        // 缓冲区耗尽临界值
        private const int BufferExhaustCriticla = 48;
        
        // 字符缓冲区
        private char[] _segmentBuffer;
        // 字符缓冲区中的字符对应的字符类型
        private CharType[] _charTypes;

        // 相对于整个要处理的字符串，已处理的字符数量
        private int _bufferOffset;
        // 当前缓冲区中的处理位置
        private int _cursor;
        // 当前缓冲区中可处理的字符串的长度
        private int _available;

        // 子分词器锁，如果列表非空，说明有分词器占用缓冲区
        private HashSet<ISegmenter> _bufferLocker;
        // LexemePath位置索引
        private IDictionary<int, LexemePath> _pathMap;
        // 分词结果集
        private LinkedList<Lexeme> _result;

        public AnalyzeContext()
        {
            _segmentBuffer = new char[BufferSize];
            _charTypes = new CharType[BufferSize];
            _bufferLocker = new HashSet<ISegmenter>();
            _pathMap = new Dictionary<int, LexemePath>();
            _result = new LinkedList<Lexeme>();
        }

        public int GetCursor()
        {
            return _cursor;
        }

        public char[] GetSegmentBuffer()
        {
            return _segmentBuffer;
        }

        public char GetCurrentChar()
        {
            return _segmentBuffer[_cursor];
        }

        public CharType GetCurrentCharType()
        {
            return _charTypes[_cursor];
        }

        public int GetBufferOffset()
        {
            return _bufferOffset;
        }

        /// <summary>
        /// 填充缓冲区
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public int FillBuffer(TextReader reader)
        {
            int readCount = 0;
            if (_bufferOffset == 0)
            {
                readCount = reader.Read(_segmentBuffer, 0, BufferSize);
            }
            else
            {
                int offset = _available - _cursor;
                if (offset > 0)
                {
                    Array.Copy(_segmentBuffer, _cursor, _segmentBuffer, 0, offset);
                    readCount = offset;
                }
                readCount += reader.Read(_segmentBuffer, offset, BufferSize - offset);
            }
            _available = readCount;
            _cursor = 0;
            return readCount;
        }
    }
}
