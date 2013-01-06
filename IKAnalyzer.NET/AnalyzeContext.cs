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
        // 原始分词结果集
        private SortedSet<Lexeme> _orgLexemes; 

        public AnalyzeContext()
        {
            _segmentBuffer = new char[BufferSize];
            _charTypes = new CharType[BufferSize];
            _bufferLocker = new HashSet<ISegmenter>();
            _pathMap = new Dictionary<int, LexemePath>();
            _result = new LinkedList<Lexeme>();
            _orgLexemes = new SortedSet<Lexeme>();
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

        /// <summary>
        /// 设置指定位置的字符信息，格式化字符，获取字符类型
        /// </summary>
        /// <param name="cursor"></param>
        private void SetCharInfo(int cursor)
        {
            _segmentBuffer[cursor] = CharacterUtil.Regularize(_segmentBuffer[cursor]);
            _charTypes[cursor] = CharacterUtil.GetCharType(_segmentBuffer[cursor]);
        }

        public void InitCursor()
        {
            _cursor = 0;
            SetCharInfo(_cursor);
        }

        public bool MoveCursor()
        {
            if (_cursor < _available)
            {
                _cursor++;
                SetCharInfo(_cursor);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 设置当前_segmentBuffer为锁定状态
        /// 加入正在处理的子分词器，表示此子分词器正在占用_segmentBuffer
        /// </summary>
        /// <param name="segmenter"></param>
        public void LockBuffer(ISegmenter segmenter)
        {
            _bufferLocker.Add(segmenter);
        }

        /// <summary>
        /// 释放子分词器对_segmentBuffer的占用
        /// </summary>
        /// <param name="segmenter"></param>
        public void UnlockBuffer(ISegmenter segmenter)
        {
            _bufferLocker.Remove(segmenter);
        }

        /// <summary>
        /// 判断_segmentBuffer是否被占用
        /// </summary>
        /// <returns></returns>
        public bool IsBufferLocked()
        {
            return _bufferLocker.Count > 0;
        }

        /// <summary>
        /// 判断_segmentBuffer是否已经被用完
        /// </summary>
        /// <returns></returns>
        public bool IsBufferOutOfUse()
        {
            return _cursor == _available - 1;
        }

        /// <summary>
        /// 判断缓冲区是否需要重新填充
        /// </summary>
        /// <returns></returns>
        public bool IsBufferNeedRefill()
        {
            return _available == BufferSize &&
                   _cursor < _available - 1 &&
                   _cursor > BufferSize - BufferExhaustCriticla &&
                   !IsBufferLocked();
        }

        /// <summary>
        /// 累积当前的_segmentBuffer相对于整个要处理的reader的起始位置
        /// </summary>
        public void AccumulateOffset()
        {
            _bufferOffset += _cursor;
        }

        /// <summary>
        /// 向分词结果集添加词元（尚未完成）
        /// </summary>
        /// <param name="lexeme"></param>
        public void AddLexeme(Lexeme lexeme)
        {
            
        }

        /// <summary>
        /// 添加分词结果路径（尚未完成）
        /// </summary>
        /// <param name="path"></param>
        public void AddLexemePath(LexemePath path)
        {
            
        }

        /// <summary>
        /// 对CJK字符进行单字输出，（尚未完成）
        /// </summary>
        /// <param name="index"></param>
        private void OutputSingleCJK(int index)
        {
            if (_charTypes[index] == CharType.Chinese)
            {
                
            }
            else if (_charTypes[index] == CharType.Other_CJK)
            {
                
            }
        }

        /// <summary>
        /// 处理未知的CJK字符（尚未完成）
        /// </summary>
        public void ProcessUnknownCJKChar()
        {
            int index = 0;
            while (index < _available)
            {
                if (_charTypes[index] == CharType.Other)
                {
                    index++;
                    continue;
                }
                LexemePath path = _pathMap[index];
                if (path != null)
                {
                }
            }
            _pathMap.Clear();
        }

        public bool HasNexResult()
        {
            return _result.Any();
        }

        /// <summary>
        /// 输出词元，处理合并和停止词
        /// </summary>
        /// <returns></returns>
        public Lexeme GetNextLexeme()
        {
            return new Lexeme(0, 0, 0, LexemeType.English);
        }

        /// <summary>
        /// 重置分词器状态
        /// </summary>
        public void Reset()
        {
            _result.Clear();
            _bufferLocker.Clear();
            _orgLexemes.Clear();
            _pathMap.Clear();
            _available = 0;
            _bufferOffset = 0;
            _cursor = 0;
            _charTypes = new CharType[BufferSize];
            _segmentBuffer = new char[BufferSize];
        }

        /// <summary>
        /// 合并词元
        /// </summary>
        private void Merge()
        {
            
        }
    }
}
