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
        public SyntacticAnalizator(List<Identificator> identificators, List<Konstanta> numbers,string enterText)
        {
            //_form = form;
            _identificators = identificators;
            _numbers = numbers;
            ident = _identificators.Select(tuple => tuple.valueIdentificator).ToList();
            numb = _numbers.Select(tuple => tuple.valueKonstanta).ToList();
            _enterText = enterText;
            //foreach (var ident in identificators)
            //{
            //    identType.Add(ident.valueIdentificator, false);
            //}
        }

        public void CheckProgram(string programStr)
        {
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
            string[] programStrArr = programStr.Split(' ');
            int p = 0;
            for (int i = 0; i < programStructure.Length; i++)
            {

                if (programStructure[i] == "{description}")
                {
                    
                    p = Description(programStrArr, p);
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
                    Body(programStrArr, p);
                }

                if (programStrArr[i] == "begin")
                {
                    p++;
                }

            }
        }

        /// Инициализация переменных.
        public int Description(string[] str, int p)
        {
            string[] descriptionStructure = ReferenceStrings.Description.Split(' ');
            List<string> tempInd = new List<string>();
            for (int i = 0; i < descriptionStructure.Length; i++)
            {
                
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
                    
                    if (str[p] == "int" || str[p] == "float" || str[p] == "bool")
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
                if (str[p] == "var")
                {
                    p = Description(str, p);
                }
            }
            return p;
        }

        /// <summary>
        /// Разбор тела
        /// </summary>
        public void Body(string[] str, int p)
        {
            string[] bodyStructure = ReferenceStrings.Body.Split(' ');
            int pn = p;
            while (str[p] != "end")
            {
                
                if (str[p] == "/n")
                {
                    
                    p++;
                    pn++;
                }
                while (str[p] == ";" || str[p] == "%") // если мы встретили : или скобки комментария
                {
                    if (str[p] == "%")
                    {
                        
                        p++;
                        pn++;
                        //MessageBox.Show($"Найдено начало комментария");
                        while (str[p] != "%")
                        {  
                            p++;
                            pn++;
                            
                        }
                        //_form.CatchError($"Найдено начало комментария");
                    }
                    if (str[p] == "%")
                    {
                        //MessageBox.Show("Найден конец комментария");
                        //_form.CatchError($"Найден конец комментария");
                    }
                    p++;
                    pn++;
                }
                string rt = "";
                //foreach (string y in ident)
                //{
                //    rt += " | " + y;
                //}
                //MessageBox.Show(rt);
                //if (ident.Contains(str[p]))
                //{
                //    identType.Add(str[p], false);
                //}
                
                if (str[p] == "end")
                {
                    MessageBox.Show("Найден конец программы");
                    SemanticAnalizator semantic = new SemanticAnalizator(identType,_initializedVariables,operationsAssignments,expression);
                    semantic.StartSemanticAnalyzer();
                    MessageBox.Show("Синтактический анализ завершён успешно");

                    //foreach (var s in operationsAssignments)
                    //{
                    //    MessageBox.Show(s.ToString());
                    //}
                    //foreach (var s in identType)
                    //{
                    //    MessageBox.Show(s.ToString());
                    //}
                    break;
                }

                if (CheckOperator(str, ref pn))
                {
                    p = pn;
                }
                else
                {
                    MessageBox.Show($"Неверный синтаксис оператора {str[pn]}, {pn}");
                    //_form.CatchError($"Неверный синтаксис оператора {str[pn]}, {pn}");
                    pn++;
                    p = pn;
                    if (p == str.Length)
                    {
                        MessageBox.Show("Не найден end - выход за пределы массива, анализ завершится.");
                        //_form.CatchError("Не найден end - выход за пределы массива, анализ завершится.");
                        break;
                    }
                }
            }
        }
        public bool CheckOperator(string[] str, ref int p)
        {
            return isAssignment(str, ref p) || isFor(str, ref p) || isIf(str, ref p) || isWhile(str, ref p) || isWrite(str, ref p) || isReadLn(str, ref p);
        }

        //public bool isIf(string[] str, ref int p)
        //{
        //    if (str[p] == "if")
        //    {
        //        p++;

        //        if (isExpression(str, ref p, true))
        //        {
        //            p++;
        //            if (str[p] == "then")
        //            {
        //                p++;
        //                if (CheckOperator(str, ref p))
        //                {
        //                    p++;
        //                    if (str[p] == "else")
        //                    {
        //                        p++;
        //                        if (CheckOperator(str, ref p))
        //                        {
        //                            if (str[p] == "end_else")
        //                            {
        //                                p++;
        //                                return true;
        //                            }
        //                            else
        //                            {
        //                                MessageBox.Show("Не найден оператор end_else.");
        //                                //_form.CatchError($"Не найден оператор end_else.");
        //                            }
        //                        }
        //                    }

        //                    else if (str[p] == "end_else")
        //                    {
        //                        return true;
        //                    }

        //                    else
        //                    {
        //                        MessageBox.Show("Не найден оператор else.");
        //                        //_form.CatchError($"Не найден оператор else.");
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                MessageBox.Show("Не найден оператор then.");
        //                //_form.CatchError($"Не найден оператор then.");
        //                return false;
        //            }
        //        }
        //    }
        //    return false;
        //}
        public bool isIf(string[] str, ref int p)
        {
            if (str[p] == "if")
            {
                
                p++;
                if (str[p] == "(")
                {
                    p++;
                    if (isExpression(str, ref p, true))
                    {
                        
                        if (str[p] == ")")
                        {
                            p++;
                            if (CheckOperator(str, ref p))
                            {
                                p++;
                                
                                if (str[p] == "else")
                                {
                                    p++;
                                    if (CheckOperator(str, ref p))
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



        //private bool isReadLn(string[] str, ref int p)
        //{
        //    if (str[p] == "readln")
        //    {
        //        p++;

        //            while (str[p] != ";")
        //            {

        //                if (ident.Contains(str[p]))
        //                {
        //                    p++;
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }

        //            if (str[p] == ";")
        //            {
        //                p++;
        //                return true;
        //            }

        //    }
        //    return false;
        //}
        private bool isReadLn(string[] str, ref int p)
        {
            if (str[p] == "readln")
            {
                p++;
                if (isExpression(str, ref p, true))
                {
                    if (str[p] == ",")
                    {
                        p++;
                        if (isExpression(str, ref p, true))
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
        //private bool isWrite(string[] str, ref int p)
        //{
        //    if (str[p] == "writeIn")
        //    {
        //        p++;
        //            while (str[p] != ";")
        //            {
        //                if (isExpression(str, ref p, true))
        //                {
        //                    if (str[p] == ";")
        //                    {
        //                        p++;
        //                        return true;
        //                    }
        //                }
        //                else
        //                {
        //                    return false;
        //                }
        //            }

        //    }
        //    return false;
        //}

        private bool isWrite(string[] str, ref int p)
        {
            if (str[p] == "writeln")
            {
                p++;
                if (isExpression(str, ref p, true))
                { 
                    if (str[p] == ",")
                    {
                        p++;
                        if (isExpression(str, ref p, true))
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

        //private bool isWhile(string[] str, ref int p)
        //{
        //    if (str[p] == "do")
        //    {
        //        p++;
        //        if (str[p] == "while")
        //        {
        //            p++;
        //            if (isExpression(str, ref p, true))
        //            {
        //                p++;
        //                if (CheckOperator(str, ref p))
        //                {
        //                    if (str[p] == "loop")
        //                    {
        //                        p++;
        //                        return true;
        //                    }

        //                }
        //            }
        //        }
        //        else
        //        {
        //            MessageBox.Show("Не найден оператор while");
        //            //_form.CatchError($"Не найден оператор while");
        //        }
        //    }
        //    return false;
        //}
        private bool isWhile(string[] str, ref int p)
        {
            if (str[p] == "while")
            {
                
                p++;
                if (str[p] == "(")
                {
                    p++;
                    if (isExpression(str, ref p, true))
                    {
                        if (str[p] == ")")
                        {
                            p++;
                            if (CheckOperator(str, ref p))
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
        private bool isFor(string[] str, ref int p)
        {
            if (str[p] == "for")
            {
                
                p++;
                if (isAssignment(str, ref p))
                {
                    //MessageBox.Show(str[p]);
                    //p++;
                    if (str[p] == "to")
                    {
                        
                        p++;
                        if (isExpression(str, ref p, true))
                        {
                            
                            //p++;
                            if (str[p] == "step")
                            {
                                
                                p++;
                                
                                if (isAssignment(str, ref p))
                                {
                                    
                                    p++;
                                    if (CheckOperator(str, ref p))
                                    {
                                        p++;
                                        
                                        if (str[p] == "next")
                                        {
                                            return true;
                                        }
                                    }
                                }
                                else
                                {
                                    if (CheckOperator(str, ref p))
                                    {
                                        p++;
                                        if (str[p] == "next")
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

        public bool isAssignment(string[] str, ref int p)
        {
            if (ident.Contains(str[p]))
            {
                
                int startIndex = p;
                p++;
                if (str[p] == ":=")
                {
                    //MessageBox.Show(str[p],"3");
                    p++;
                    
                    if (isExpression(str, ref p))
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
                    MessageBox.Show("Не найдена операция присваиванияуeddd", str[p]);
                    //_form.CatchError($"Не найдена операция присваивания");
                    return false;
                }
            }
            else if (str[p] == "let")
            {
                p++;
                if (ident.Contains(str[p]))
                {
                    int startIndex = p;
                    p++;
                    if (str[p] == ":=")
                    {
                        p++;
                        if (isExpression(str, ref p))
                        {
                            operationsAssignments.Add(string.Join(" ", str, startIndex, p - startIndex));
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не найдена операция присваиванияdggdfgggg");
                        //_form.CatchError($"Не найдена операция присваивания");
                        return false;
                    }
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
                }
                return true;
            }
            else
            {
                return false;
            }
        }
        //Проверка числа
        public bool isDigit(string[] str, ref int p)
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
                    //MessageBox.Show(num);
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
        public bool isExpression(string[] str, ref int p, bool addExpression = false)
        {
            if (ident.Contains(str[p]) || isDigit(str, ref p) || str[p] == "true" || str[p] == "false")
            {
                int temp = 0;
                int startIndex = p;
                p++;
                bool operation = false;
                while (
                    str[p] != ")" &&
                    str[p] != ";" &&
                    str[p] != "step" &&
                    str[p] != "then" &&
                    str[p] != "to" &&
                    str[p] != "else" &&
                    str[p] != "next" &&
                    str[p] != ":=" &&
                    str[p] != "loop")
                {
                    if ((ident.Contains(str[p]) || isDigit(str, ref p)) && operation)
                    {
                        operation = false;
                        p++;
                    }
                    else if (operations.Contains(str[p]) && !operation)
                    {
                        operation = true;
                        p++;
                    }
                    else if ((ident.Contains(str[p]) || isDigit(str, ref p)) && !operation)
                    {
                        break;
                    }
                    else if (str[p] == "writeln" ||
                    str[p] == "if" ||
                    str[p] == "for" ||
                    str[p] == "readln")
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

        //    static bool CommentCheck(string s)
        //    {
        //        string t = "/**/";
        //        Stack<char> st = new Stack<char>();
        //        foreach (var x in s)
        //        {
        //            int f = t.IndexOf(x);

        //            if (f >= 0 && f <= 2)
        //                st.Push(x); ;

        //            if (f > 2)
        //            {
        //                if (st.Count == 0 || st.Pop() != t[f - 3])
        //                    return false;
        //            }
        //        }

        //        if (st.Count != 4)
        //            return false;


        //        return true;
        //    }
        //}
        //static bool CommentCheck(string s)
        //{
        //    string t = "%%";
        //    Stack<char> st = new Stack<char>();
        //    foreach (var x in s)
        //    {
        //        int f = t.IndexOf(x);

        //        if (f >= 0 && f <= 2)
        //            st.Push(x); ;

        //        if (f > 2)
        //        {
        //            if (st.Count == 0 || st.Pop() != t[f - 3])
        //                return false;
        //        }
        //    }

        //    if (st.Count != 4)
        //        return false;


        //    return true;
        //}
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

