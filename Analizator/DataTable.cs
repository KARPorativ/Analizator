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
                new("begin"),//1
                new("end"),//2
                new("var"),//3
                new("if"),//4
                new("else"),//5
                new("for"),//6
                new("to"),//7
                new("step"),//8
                new("next"),//9
                new("while"),//10
                new("readln"),//11
                new("writeln"),//12
                new("true"),//13
                new("false"),//14
                new("int"),//15
                new("float"),//16
                new("bool")//17
            };
        }
       

        public static List<Separators> GetSeparators()
        {
            return new List<Separators>()
            {
                new("+85_"),
                new("NE"),//1
                new("EQ"),//2
                new("LT"),//3
                new("LE"),//4
                new("GT"),//5
                new("GE"),//6
                new("plus"),//7
                new("min"),//8
                new("or"),//9
                new("mult"),//10
                new("div"),//11
                new("and"),//12
                new(";"),//13
                new(","),//14
                new(":"),//15
                new(":="),//16
                new("("),//17
                new(")"),//18
                new("/n"),//19
                new("~"),//20
                new("%"),//21
                new(" ")//22
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