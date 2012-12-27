using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IKAnalyzer.NET.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            foreach (var s in "0aB在ス☆")
            {
                Console.WriteLine(s + " -> " + CharacterUtil.GetCharType(s));
            }
            Console.ReadLine();
        }
    }
}
