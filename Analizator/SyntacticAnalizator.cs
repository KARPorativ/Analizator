using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
//using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Analizator
{
    //v5
    internal class SyntacticAnalizator
    {
        public Dictionary<string, string> _initializedVariables = new Dictionary<string, string>();
        public List<string> operationsAssignments = new List<string>();
        public List<string> expression = new List<string>();
        public Dictionary<string, bool> identType = new Dictionary<string, bool>();
        public Dictionary<string, string> typePer = new Dictionary<string, string>();
        //private Form1 _form;
        private List<Identificator> _identificators;
        List<string> ident;
        private List<Konstanta> _numbers;
        List<string> numb;
        string _enterText;
        

        private List<string> operations = new List<string> { "NE", "EQ", "LT", "LE", "GT", "GE", "plus", "min", "or", "mult", "div", "and", "~" };
        public SyntacticAnalizator(List<Identificator> identificators, List<Konstanta> numbers,List<string> enterText)
        {
            //_form = form;
            _identificators = identificators;
            _numbers = numbers;
            ident = _identificators.Select(tuple => tuple.valueIdentificator).ToList();
            numb = _numbers.Select(tuple => tuple.valueKonstanta).ToList();
            //_enterText = enterText;
            //foreach (var ident in identificators)
            //{
            //    identType.Add(ident.valueIdentificator, false);
            //}
        }

        public void CheckProgram(string programStr, List<string> ps, List<string> v)
        {
            string[] _ps = ps.ToArray();
            string[] _v = v.ToArray();

            if (!BracketCheck(programStr))// проверяем парность скобок
            {
                MessageBox.Show("Нарушена парность скобок");
                //_form.CatchError($"Нарушена парность скобок");
            }
            if (!CommentCheck(programStr))// проверяем парность коментария
            {
                MessageBox.Show("Нарушена парность комментария");
                //_form.CatchError($"Нарушена парность комментария");
            }
            
            string[] programStructure = ReferenceStrings.Program.Split(' ');

            //string[] programStrArr = programStr.Split(' ');
            int p = 0;
            for (int i = 0; i < programStructure.Length; i++)
            {

                if (programStructure[i] == "{description}")
                {
                    
                    p = Description(_ps,_v, p);
                    if (p == -1)
                    {                      
                        MessageBox.Show("Неверное описание переменных в программе.");
                        MessageBox.Show("Синтаксический тест завершиться с ошибкой.");
                        //_form.CatchError($"Неверное описание переменных в программе.");
                        //_form.CatchError($"Синтаксический тест завершиться с ошибкой.");
                        break;

                    }
                }

                if (programStructure[i] == "{body}")
                {
                    Body(_ps,_v, p);
                }

                if (v[i] == "[0, 1]")//begin
                {
                    p++;
                }
                

            }
        }

        /// Инициализация переменных.
        public int Description(string[] str,string[] vp, int p)
        {
            string[] descriptionStructure = ReferenceStrings.Description.Split(' ');
            List<string> tempInd = new List<string>();
            for (int i = 0; i < descriptionStructure.Length; i++)
            {
                
                //MessageBox.Show(str[p]+"2","2");
                //    MessageBox.Show(descriptionStructure[i]+"y", "3");
                if (descriptionStructure[i] == str[p])
                {
                    p++;
                    continue;
                }
                //else if (descriptionStructure[i] == "{identifier}" && _identificators.Contains(str[p]))
                //else if (descriptionStructure[i] == "{identifier}" && _identificators.FirstOrDefault(item=>item.valueIdentificator.Contains(str[p]))
                else if (descriptionStructure[i] == "{identifier}" && _identificators.Any(item => item.valueIdentificator == str[p]))
                {
                    identType.Add(str[p], false);
                    tempInd.Add(str[p]);
                    typePer.Add(str[p],"null");
                    p++;
                    
                    continue;
                }
                else if (descriptionStructure[i] == "{type/,}")
                {
                    
                    if (str[p] == "," && _identificators.Any(item => item.valueIdentificator == str[p + 1]))
                    {
                        
                        p++;
                        i -= 2;
                        continue;
                    }
                    if (str[p] == ":")
                    {
                        p++;
                    }
                    else return -1;
                    
                    if (vp[p] == "[0, 15]" || vp[p] == "[0, 16]" || vp[p] == "[0, 17]")//int float bool
                    {
                        
                        string CheckSameVar = "";
                        for (int j = 0; j < tempInd.Count; j++)
                        {
                            CheckSameVar = tempInd[j];
                            for (int x = j + 1; x < tempInd.Count; x++)
                            {
                                if (CheckSameVar == tempInd[x])
                                {
                                    MessageBox.Show($"Повторно введена перменная {CheckSameVar}");
                                    //_form.CatchError($"Повторно введена перменная {CheckSameVar}");
                                    return -1;
                                }
                            }

                        }
                        while (tempInd.Count > 0)
                        {
                            _initializedVariables.Add(tempInd[tempInd.Count - 1], str[p]);
                            tempInd.RemoveAt(tempInd.Count - 1);
                        }
                        p++;
                    }
                    else
                    {
                        MessageBox.Show("Типа данных " + str[p] + " нет");
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
                p++;
                if (vp[p] == "[0, 3]")//var
                {
                    p = Description(str,vp, p);
                }
            }
            return p;
        }

        /// <summary>
        /// Разбор тела
        /// </summary>
        public void Body(string[] str,string[] vp, int p)
        {
            string[] bodyStructure = ReferenceStrings.Body.Split(' ');
            int pn = p;
            //while (str[p] != "end")
            while (vp[p] != "[0,2]")
            {
                
                if (vp[p] == "[1, 19]")///n
                {
                    
                    p++;
                    pn++;
                }
                while (vp[p] == "[1, 13]" || vp[p] == "[1, 21]") //; %
                {
                    if (vp[p] == "[1, 21]")//%
                    {
                        
                        p++;
                        pn++;
                        while (vp[p] != "[1, 21]")//%
                        {  
                            p++;
                            pn++;
                            
                        }
                    }
                    if (vp[p] == "[1, 21]")//%
                    {
                        
                    }
                    p++;
                    pn++;
                }
                
                //if (str[p] == "end")
                if (vp[p] == "[0, 2]")
                {
                    MessageBox.Show("Найден конец программы");
                    MessageBox.Show("Синтактический анализ завершён успешно");
                    SemanticAnalizator semantic = new SemanticAnalizator(identType,_initializedVariables,operationsAssignments,expression, vp);
                    semantic.StartSemanticAnalyzer();
                    break;
                }

                if (CheckOperator(str, vp, ref pn))
                {
                    p = pn;
                }
                else
                {
                    MessageBox.Show($"Неверный синтаксис оператора {str[pn]}, {pn}");
                    pn++;
                    p = pn;
                    if (p == str.Length)
                    {
                        MessageBox.Show("Не найден end - выход за пределы массива, анализ завершится.");
                        break;
                    }
                }
            }
        }
        public bool CheckOperator(string[] str, string[] vp, ref int p)
        {
            return isAssignment(str, vp, ref p) || isFor(str, vp, ref p) || isIf(str, vp, ref p) || isWhile(str, vp, ref p) || isWrite(str, vp, ref p) || isReadLn(str, vp, ref p);
        }

       
        public bool isIf(string[] str, string[] vp, ref int p)
        {
            if (vp[p] == "[0, 4]")//if
            {
                
                p++;
                if (vp[p] == "[1, 17]")//(
                {
                    p++;
                    if (isExpression(str, vp, ref p, true))
                    {
                        
                        if (vp[p] == "[1, 18]")//)
                        {
                            p++;
                            if (CheckOperator(str, vp, ref p))
                            {
                                p++;
                                
                                if (vp[p] == "[0, 5]")//else
                                {
                                    p++;
                                    if (CheckOperator(str, vp, ref p))
                                    {
                                        p++;
                                        return true;
                                    }
                                }
                                else
                                {
                                    return true;
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("В if нехватает выражения");
                    }
                }
                else
                {
                    MessageBox.Show("В if нет (");
                }
            }
            return false;
        }



       
        private bool isReadLn(string[] str, string[] vp, ref int p)
        {
            if (vp[p] == "[0, 11]")//readln
            {
                p++;
                if (isExpression(str, vp, ref p, true))
                {
                    if (vp[p] == "[1, 14]")//,
                    {
                        p++;
                        if (isExpression(str, vp, ref p, true))
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    return false;
                }

            }
            return false;
        }
        

        private bool isWrite(string[] str, string[] vp, ref int p)
        {
            if (vp[p] == "[0, 12]")//writeln
            {
                p++;
                if (isExpression(str, vp, ref p, true))
                { 
                    if (vp[p] == "[1, 14]")//,
                    {
                        p++;
                        if (isExpression(str, vp, ref p, true))
                        {
                            return true ;
                        }
                        else
                        {
                            return false ;
                        }
                    }
                    else
                    {
                        return true ;
                    }
                }
                else
                {
                    return false;
                }
                
            }
            else return false;
        }

        
        private bool isWhile(string[] str, string[] vp, ref int p)
        {
            if (vp[p] == "[0, 10]")//while
            {
                
                p++;
                if (vp[p] == "[1, 17]")//(
                {
                    p++;
                    if (isExpression(str, vp, ref p, true))
                    {
                        if (vp[p] == "[1, 18]")//)
                        {
                            p++;
                            if (CheckOperator(str, vp, ref p))
                            {
                                return true ;
                            }
                        }
                    }
                }
            }
            return false;
        }

        //private bool isFor(string[] str, ref int p)
        //{
        //    if (str[p] == "for")
        //    {
        //        p++;
        //        if (str[p] == "(")
        //        {
        //            p++;
        //            while (str[p] != ")")
        //            {
        //                if (isExpression(str, ref p, true))
        //                {
        //                    if (str[p] == ";")
        //                    {
        //                        p++;
        //                        continue;
        //                    }
        //                    else if (str[p] == ")")
        //                    {
        //                        p++;
        //                        return CheckOperator(str, ref p);
        //                    }
        //                    else
        //                    {
        //                        return false;
        //                    }
        //                }
        //            }
        //            if (str[p] != ")")
        //            {
        //                p++;
        //                return CheckOperator(str, ref p);
        //            }
        //        }
        //    }
        //    return false;
        //}
        private bool isFor(string[] str, string[] vp, ref int p)
        {
            if (vp[p] == "[0, 6]")//for
            {
                
                p++;
                if (isAssignment(str, vp, ref p))
                {
                    if (vp[p] == "[0, 7]")//to
                    {
                        
                        p++;
                        if (isExpression(str, vp, ref p, true))
                        {
                            if (vp[p] == "[0, 8]")//step
                            {
                                
                                p++;
                                
                                if (isAssignment(str, vp, ref p))
                                {
                                    
                                    p++;
                                    if (CheckOperator(str, vp, ref p))
                                    {
                                        p++;
                                        
                                        if (vp[p] == "[0, 9]")//next
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (CheckOperator(str, vp, ref p))
                                    {
                                        p++;
                                        if (vp[p] == "[0, 9]")//next
                                        {
                                            return true;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Отсутствует step");
                            }

                        }
                    }
                }
            }
            return false;
        }

        /// Проверка оператора присваивания
        public bool isAssignment(string[] str, string[] vp, ref int p)
        {
            if (ident.Contains(str[p]))
            {
                
                int startIndex = p;
                p++;
                if (vp[p] == "[1, 16]")//:=
                {
                    
                    p++;
                    
                    if (isExpression(str, vp, ref p))
                    {
                        operationsAssignments.Add(string.Join(" ", str, startIndex, p - startIndex));
                        identType[str[startIndex]] = true;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show("Не найдена операция присваивания ", str[p]);
                    
                    return false;
                }
            }
            
            return false;
        }
        public bool CheckExponentialFormNumber(string str)
        {
            if (((str[0] >= '0' && str[0] <= '9') || str[0] == '.') && (str.Contains('e') || str.Contains('E')))
            {
                char firstSymbol = str[0];
                bool indexE = false;
                bool indexpPoin = false;
                bool indexPlusOrMin = false;
                if (firstSymbol == '.' && (str[1] == 'e' || str[1] == 'E'))
                {
                    return false;
                }

                for (int i = 1; i < str.Length; i++)
                {
                    if ((str[i] < '0' || str[i] > '9') && str[i] != 'e' && str[i] != 'E' && str[i] != '+' && str[i] != '-' && str[i] != '.')
                    {
                        return false;
                    }

                    if (str[i] == '.')
                    {
                        if (indexE || indexpPoin || indexPlusOrMin)
                        {
                            return false;
                        }
                        else
                        {
                            indexpPoin = true;
                        }
                    }

                    if (str[i] == 'e' || str[i] == 'E')
                    {
                        if (indexE || indexPlusOrMin)
                        {
                            return false;
                        }
                        else
                        {
                            indexE = true;
                        }
                    }

                    if (str[i] == '+' || str[i] == '-')
                    {
                        if (str[i - 1] != 'e' && str[i - 1] != 'E')
                        {
                            return false;
                        }
                        else
                        {
                            indexPlusOrMin = true;
                        }
                    }
                    if (str[i] == '+' || str[i] == '-')
                    {
                        if ( !Char.IsDigit(str[i + 1]))
                        {
                            return false;
                        }
                        
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //Проверка числа
        public bool isDigit(string[] str, string[] vp, ref int p)
        {
            if (CheckExponentialFormNumber(str[p]))
            {
                return true;
            }
            string num = str[p];
            if (num[0] == '0' || num[0] == '1' || num[0] == '2' || num[0] == '3' || num[0] == '4' || num[0] == '5' || num[0] == '6' || num[0] == '7' || num[0] == '8' || num[0] == '9')
            {
                if (num[num.Length - 1] == 'b' || num[num.Length - 1] == 'd' || num[num.Length - 1] == 'o' || num[num.Length - 1] == 'h' || num[num.Length - 1] == 'r' || num[num.Length - 1] == 'e')
                    {
                    
                    return true;
                }
                else return false;
            }
            return false;
        }
        /// Проверка выражения.
        //public bool isExpression(string[] str, ref int p, bool addExpression = false)
        //{
        //    if (isDigit(str,ref p) || numb.Contains(str[p]) || str[p] == "true" || str[p] == "false")
        //    {
        //        int temp = 0;
        //        int startIndex = p;
        //        p++;
        //        bool operation = false;
        //        while (
        //            str[p] != ")" &&
        //            str[p] != ";" &&
        //            str[p] != "step" &&
        //            str[p] != "then" &&
        //            str[p] != "to" &&
        //            str[p] != "else" &&
        //            str[p] != "next" &&
        //            str[p] != "loop")
        //        {
        //            if ((ident.Contains(str[p]) || numb.Contains(str[p])) && operation)
        //            {
        //                operation = false;
        //                p++;
        //            }
        //            else if (operations.Contains(str[p]) && !operation)
        //            {
        //                operation = true;
        //                p++;
        //            }
        //            else if ((ident.Contains(str[p]) || numb.Contains(str[p])) && !operation)
        //            {
        //                break;
        //            }
        //            else if (str[p] == "output" ||
        //            str[p] == "if" ||
        //            str[p] == "for" ||
        //            str[p] == "input")
        //            {
        //                p--;
        //                temp = 1;
        //                break;
        //            }
        //            else
        //            {
        //                return false;
        //            }
        //        }
        //        if (addExpression)
        //        {
        //            expression.Add(string.Join(" ", str, startIndex, p - startIndex + temp));
        //        }

        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }

        //}
        public bool isExpression(string[] str, string[] vp, ref int p, bool addExpression = false)
        {
            if (ident.Contains(str[p]) || isDigit(str, vp, ref p) || vp[p] == "[0, 13]" || vp[p] == "[0, 14]")
            {
                int temp = 0;
                int startIndex = p;
                p++;
                bool operation = false;
                while (
                    vp[p] != "[1, 18]" && //)
                    vp[p] != "[1, 13]" && //;
                    vp[p] != "[0, 8]" && //step
                    vp[p] != "[0, 7]" && //to
                    vp[p] != "[0, 5]" && //else
                    vp[p] != "[0, 9]" && //next
                    vp[p] != "[1, 16]") //:=
                {
                    if ((ident.Contains(str[p]) || isDigit(str, vp, ref p)) && operation)
                    {
                        operation = false;
                        p++;
                    }
                    else if (operations.Contains(str[p]) && !operation)
                    {
                        operation = true;
                        p++;
                    }
                    else if ((ident.Contains(str[p]) || isDigit(str, vp, ref p)) && !operation)
                    {
                        break;
                    }
                    else if (str[p] == "[0, 12]" || //writeln
                    str[p] == "[0, 4]" || //if
                    str[p] == "[0, 6]" || //for
                    str[p] == "[0, 11]") //readln
                    {
                        p--;
                        temp = 1;
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (addExpression)
                {
                    
                    expression.Add(string.Join(" ", str, startIndex, p - startIndex + temp));
                }

                return true;
            }
            else
            {
                return false;
            }

        }
        /// Проверка на парность скобок.
        static bool BracketCheck(string s)
        {
            string t = "[{(]})";
            Stack<char> st = new Stack<char>();
            foreach (var x in s)
            {
                int f = t.IndexOf(x);

                if (f >= 0 && f <= 2)
                    st.Push(x); ;

                if (f > 2)
                {
                    if (st.Count == 0 || st.Pop() != t[f - 3])
                        return false;
                }
            }

            if (st.Count != 0)
                return false;

            return true;
        }
        
        static bool CommentCheck(string s)
        {
            int com = 0;
            foreach (var x in s)
            {
                if (x == '%')
                {
                    com++;
                }
            }
            if (com % 2 == 0)
            {
                return true;
            }
            return false;
        }
    }
    public static class ReferenceStrings
    {
        public static string Program = "begin {description} {body} end";
        public static string Description = "var {identifier} {type/,}";
        public static string Body = "{operator} : end/{operator}";
    }

}

