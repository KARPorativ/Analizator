using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Analizator;


namespace Analizator
{
    internal class DataTable
    {
        
        


        


        public static List<ServiceWord> GetServiceWords()
        {
            return new List<ServiceWord>()
            {
                new ("yit"),
                new("begin"),
                new("end"),
                new("var"),
                new("if"),
                new("else"),
                new("for"),
                new("to"),
                new("step"),
                new("next"),
                new("while"),
                new("readln"),
                new("writeln"),
                new("true"),
                new("false"),
                new("int"),
                new("float"),
                new("bool")
            };
        }
        //public List<string> _serviceWords 
        //{  get
        //    {
        //        return GetServiceWords();
        //    }
        //}
        //public static List<string> GetServiceWords()
        //{
        //    return new List<string>()
        //    {
        //        "begin",
        //        "end",
        //        "dim",
        //        "if",
        //        "then",
        //        "else",
        //        "for",
        //        "let",
        //        "while",
        //        "loop",
        //        "input",
        //        "output",
        //        "true",
        //        "false",
        //        "do",
        //        "end_else",
        //        "do_while",
        //        "%",
        //        "!",
        //        "$"
        //    };
        //}

        public static List<Separators> GetSeparators()
        {
            return new List<Separators>()
            {
                new("+85_"),
                new("NE"),
                new("EQ"),
                new("LT"),
                new("LE"),
                new("GT"),
                new("GE"),
                new("plus"),
                new("min"),
                new("or"),
                new("mult"),
                new("div"),
                new("and"),
                new(";"),
                new(","),
                new(":"),
                new(":="),
                new("("),
                new(")"),
                new("/n"),
                new("~"),
                new("%"),
                new(" ")
            };
        }

    }
}

//Перепиши использующей данные ключевые слова и разделители
//return new List<ServiceWord>()
//            {
//                new("begin"),
//                new("end"),
//                new("var"),
//                new("if"),
//                new("else"),
//                new("for"),
//                new("to"),
//                new("step"),
//                new("next"),
//                new("while"),
//                new("readln"),
//                new("writeln"),
//                new("true"),
//                new("false"),
//                new("int"),
//                new("float"),
//                new("bool")
//            };
//        }
//return new List<Separators>()
//            {
//                new("<>"),
//                new("="),
//                new("<"),
//                new("<="),
//                new(">"),
//                new(">="),
//                new("+"),
//                new("-"),
//                new("or"),
//                new("*"),
//                new("/"),
//                new("and"),
//                new("{"),
//                new("}"),
//                new(";"),
//                new(","),
//                new(":"),
//                new("/n"),
//                new("("),
//                new(")"),
//                new("not"),
//                new(" ")
//            };