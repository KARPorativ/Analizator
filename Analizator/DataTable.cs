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
        new("assign"),
        new("if"),
        new("then"),
        new("else"),
        new("for"),
        new("val"),
        new("do"),
        new("while"),
        new("next"),
        new("enter"),
        new("displ"),
        new("true"),
        new("false"),
        new("#"),
        new("@"),
        new("&")
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
        new("NEQ"),
        new("EQV"),
        new("LOWT"),
        new("LOWE"),
        new("GRT"),
        new("GRE"),
        new("add"),
        new("disa"),
        new("||"),
        new("umn"),
        new("del"),
        new("&&"),
        new(";"),
        new(":"),
        new(","),
        new("["),
        new("]"),
        new("/n"),
        new("^"),
        new("{"),
        new("}"),
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