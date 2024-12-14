using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Analizator
{
    class Data
    {
        public static List<string> GetServiceWords()
        {
            return new List<string>()
            {
                "begin",
                "end",
                "var",
                "if",
                "else",
                "for",
                "to",
                "step",
                "next",
                "while",
                "readln",
                "writeln",
                "true",
                "false",
                "int",
                "float",
                "bool"
            };
        }

        public static List<string> GetSeparators()
        {
            return new List<string>()
            {
                "<>",
                "=",
                "<",
                "<=",
                ">",
                ">=",
                "+",
                "-",
                "or",
                "*",
                "/",
                "and",
                "{",
                "}",
                ";",
                ",",
                ":",
                "/n",
                "(",
                ")",
                "not",
                " "
            };
        }

    }
}
