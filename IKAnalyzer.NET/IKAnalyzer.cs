using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace IKAnalyzer.NET
{
    /// <summary>
    /// 分词器主类
    /// </summary>
    public class IKAnalyzer
    {
        private TextReader _reader;
        private IList<ISegmenter> _segmenters;
        private AnalyzeContext _context;

        public IKAnalyzer(TextReader reader)
        {
            _reader = reader;
            _segmenters = LoadSegmenters();
            _context = new AnalyzeContext();
        }

        /// <summary>
        /// 加载子分词器
        /// </summary>
        /// <returns></returns>
        public IList<ISegmenter> LoadSegmenters()
        {
            IList<ISegmenter> segmenters = new List<ISegmenter>();

            // 通过反射加载所有实现的Segmenter子分词器
            foreach (var type in typeof(IKAnalyzer).Assembly.GetTypes())
            {
                if (type.GetInterfaces().Any(t => t == typeof(ISegmenter)))
                {
                    ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                    ISegmenter segmenter = ci.Invoke(null) as ISegmenter;
                    segmenters.Add(segmenter);
                }
            }

            return segmenters;
        }

        public Lexeme GetNextLexeme()
        {
            return null;
        }

        public void Reset(TextReader reader)
        {
            _reader = reader;
            foreach (var segmenter in _segmenters)
            {
                segmenter.Reset();
            }
        }
    }
}
