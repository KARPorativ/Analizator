using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Analizator;
using Microsoft.VisualBasic;
using static Analizator.DataTable;

namespace Analizator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public class ServiceWord
    {
        private string _word;
        public string word { get
            {
                return _word;
            } set; 
        }

        public ServiceWord(string word)
        {
            _word = word;
        }

        
    }

    public class Separators
    {
        private string _separator;
        // Свойство для хранения ключевого слова
        public string separator
        {
            get
            {
                return _separator;
            }
            set;
        }

        // Конструктор для инициализации ключевого слова
        public Separators(string separator)
        {
            _separator = separator;
        }

        // Переопределение метода ToString для удобного отображения

    }

    public class Identificator
    {
        private int _id;
        private string _value;
        public int idIdentificator
        {
            get
            {
                return _id;
            }
            set;
        }
        public string valueIdentificator
        {
            get
            {
                return _value;
            }
            set;
        }

        public Identificator(int idIdentificatorr, string valueIdentificatorr)
        {
            _id = idIdentificatorr;
            _value = valueIdentificatorr;
        }
    }

    public class Konstanta
    {
        private int _id;
        private string _value;
        public int idKonstanta
        {
            get
            {
                return _id;
            }
            set;
        }
        public string valueKonstanta
        {
            get
            {
                return _value;
            }
            set;
        }



        public Konstanta(int id, string value)
        {
            _id = id;
            _value = value;
        }
    }

    public partial class MainWindow : Window
    {
        //public string ServiceWord { get; set; }
        public enum TypeTable
        {
            ServiceWord,
            Separators,
            Number,
            Identifier,
            Non
        }
        

        public Dictionary<string, string> numberWithType = new Dictionary<string, string>();

        private List<ServiceWord> _serviceWords;
        private List<Separators> _separators;
        private List<Identificator> _identificators;
        private List<Konstanta> _konstants;
        private List<string> _arrResult;


        //List<ServiceWord> serviceWords, List<Separators> separators
        public MainWindow()
        {
            InitializeComponent();
            _serviceWords = GetServiceWords();
            _separators = GetSeparators();
            _arrResult = new List<string>();
            _konstants = new List<Konstanta>();
            _identificators = new List<Identificator>();

        }



        public string StartLexicalAnalyzer(string programStr)
        {
            string result = "";
            bool hasEnd = false;
            while (programStr.Contains("\n")) { programStr = programStr.Replace("\n", " "); }
            while (programStr.Contains("\r")) { programStr = programStr.Replace("\r", " "); }
            string[] programStrArr = programStr.Split(' ');
            
            string y = "";
            foreach (string t in programStrArr)
            {
                y += t + "|";
            }
            LargeTextBo.Text = y;
            for (int i = 0; i < programStrArr.Length; i++)
            {
                if (programStrArr[i] == "end")
                {
                    _arrResult.Add($"[0, {CheckServiceWord(programStrArr[i])}]");
                    hasEnd = true;
                    break;
                }
                int serviceWordId = CheckServiceWord(programStrArr[i]);
                int separatorId = CheckSeparator(programStrArr[i]);
                int identificatorId = CheckIdentificator(programStrArr[i]);
                int numberId = CheckNumber(programStrArr[i]);



                if (serviceWordId > -1)
                {
                    _arrResult.Add($"[0, {serviceWordId}]");
                }
                else if (separatorId > -1)
                {
                    _arrResult.Add($"[1, {separatorId}]");
                }
                else if (identificatorId > -1)
                {
                    _arrResult.Add($"[3, {identificatorId}]");
                }
                else if (numberId > -1)
                {
                    _arrResult.Add($"[2, {numberId}]");
                }
                else
                {
                    //_form.CatchError(programStrArr[i]);
                }
            }

            if (!hasEnd)
            {
                MessageBox.Show(programStrArr[programStrArr.Length-1]); 
                MessageBox.Show("Программа должна заканчиваться на end!");
            }
            int po = 0;
            foreach (var item in _arrResult)
            {
                
                if (po != 0)
                {
                    result += $"{item},";
                }
                else {
                    result += $"{item}\n";
                }
                po++;
                if (po == 10)
                { po = 0; }
                
            }

            //_form.SetTableIdentificators(_identificators);
            //_form.SetTableKonstants(_konstants);

            return result;
        }

        /// <summary>
        /// Проверка на ключевые слова
        /// </summary>

        public int CheckServiceWord(string str)
        {
            return _serviceWords.FindIndex(x => x.word == str);
        }
        public int CheckSeparator(string str)
        {
            return _separators.FindIndex(x => x.separator == str);
        }
        public int CheckIdentificator(string str)
        {
            if (String.IsNullOrEmpty(str))
            {
                return -1;
            }

            int identificatorId = _identificators.FindIndex(x => x.valueIdentificator == str);
            if (identificatorId > -1)
            {
                return identificatorId;
            }
            else
            {
                if (CheckWord(str) && _separators.FindIndex(x => x.separator == str) < 0 && _serviceWords.FindIndex(x => x.word == str) < 0)
                {
                    //_form.identificators1.Add(str);
                    if (_identificators.Count == 0)
                    {
                        _identificators.Add(new Identificator(0, str));
                        return _identificators.Last().idIdentificator;
                    }
                    else
                    {
                        int lastIndex = _identificators.Last().idIdentificator;
                        _identificators.Add(new Identificator(lastIndex + 1, str));
                        return _identificators.Last().idIdentificator;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }


        /// Проверяет, является ли строка словом

        public bool CheckWord(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if ((str[i] < 'A' || str[i] > 'Z') && (str[i] < 'a' || str[i] > 'z'))
                {
                    return false;
                }
            }
            return true;
        }


        /// Проверяет является ли строка числом.

        public int CheckNumber(string str)
        {
            if (CheckNumeral(str))
            {
                char NumCC = ValidateNum(str);
                if (NumCC != '-')
                {
                    //_form.number.Add(str);
                    string num = str.Remove(str.Length - 1);
                    string binaryNum = ConvertNum(num, NumCC);
                    int numberId = _konstants.FindIndex(x => x.valueKonstanta == binaryNum);
                    if (numberId == -1)
                    {
                        if (NumCC == 'r' || NumCC == 'e') // создаем словарь с переменными и их типами.
                            numberWithType.Add(str, "int");
                        else
                            numberWithType.Add(str, "float");

                        if (_konstants.Count == 0)
                        {
                            _konstants.Add(new Konstanta(0, binaryNum));
                            return _konstants.Last().idKonstanta;
                        }
                        else
                        {
                            int lastIndex = _konstants.Last().idKonstanta;
                            _konstants.Add(new Konstanta(lastIndex + 1, binaryNum));
                            return _konstants.Last().idKonstanta;
                        }
                    }
                    else
                    {
                        return numberId;
                    }
                }
                //_form.CatchError($"Неверный тип переменной");
            }
            return -1;
        }


        /// перевод чисел в машинный код
        public string ConvertNum(string num, char numCC)
        {
            switch (numCC)
            {
                case 'b':
                    return num;
                case 'd':
                    return ConvertBinary.ConvertDecimal(num);
                case 'o':
                    return ConvertBinary.ConvertOctal(num);
                case 'h':
                    return ConvertBinary.ConvertHEX(num);
                case 'r':
                    return ConvertBinary.ConvertReal(num);
                case 'e':
                    return ConvertBinary.ConvertExponential(num);
                default:
                    return "";
            }
        }


        /// Есть ли число в строке.

        public bool CheckNumeral(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] >= '0' && str[i] <= '9')
                {
                    return true;
                }
            }
            return false;
        }


        /// Проверка валидности числа.

        public char ValidateNum(string str)
        {
            if (str.Last() == 'b' || str.Last() == 'B')
            {
                return CheckBinatyNumber(str, str.Last()) ? 'b' : '-';
            }
            else if (str.Last() == 'o' || str.Last() == 'O')
            {
                return CheckOctalNumber(str, str.Last()) ? 'o' : '-';
            }
            else if (str.Last() == 'd' || str.Last() == 'D')
            {
                return CheckDecimalNumber(str, str.Last()) ? 'd' : '-';
            }
            else if (str.Last() == 'h' || str.Last() == 'H')
            {
                return CheckHEXNumber(str, str.Last()) ? 'h' : '-';
            }
            else if (str.Last() >= '0' && str.Last() <= '9')
            {
                if (CheckRealNumber(str))
                {
                    return 'r';
                }
                else if (CheckExponentialFormNumber(str))
                {
                    return 'e';
                }
                else
                {
                    return '-';
                }
            }
            else
            {
                return '-';
            }
        }


        /// Проверка Exp формы.

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

        /// Проверка на вещественное число

        public bool CheckRealNumber(string str)
        {
            bool point = false;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '.')
                {
                    point = true;
                    continue;
                }

                if (((str[i] < '0' || str[i] > '9') && str[i] != ',') || (point && str[i] == '.'))
                {
                    return false;
                }
            }
            return point;
        }

        /// Проверка на двоичное число

        public bool CheckBinatyNumber(string str, char lastChar)
        {
            int i = 0;
            while (str[i] != lastChar)
            {
                if (str[i] != '0' && str[i] != '1')
                {
                    return false;
                }
                i++;
            }
            return true;
        }


        /// Проверка на восьмеричное число

        public bool CheckOctalNumber(string str, char lastChar)
        {
            int i = 0;
            while (str[i] != lastChar)
            {
                if (str[i] < '0' && str[i] > '7')
                {
                    return false;
                }
                i++;
            }
            return true;
        }

        /// Проверка на десятичное число

        public bool CheckDecimalNumber(string str, char lastChar)
        {
            int i = 0;
            while (str[i] != lastChar)
            {
                if (str[i] < '0' || str[i] > '9')
                {
                    return false;
                }
                i++;
            }
            return true;
        }


        /// Проверка на шестнадцатеричное число
        public bool CheckHEXNumber(string str, char lastChar)
        {
            if (str[0] < '0' || str[0] > '9')
            {
                return false;
            }
            int i = 1;
            while (str[i] != lastChar)
            {
                if ((str[i] < '0' || str[i] > '9') && (str[i] < 'A' || str[i] > 'F') && (str[i] < 'a' || str[i] > 'f'))
                {
                    return false;
                }
                i++;
            }
            return true;
        }

    

    private void Button_Click(object sender, RoutedEventArgs e)
        {
            //string enteredText = LargeTextBox.Text;
            string enteredText = "begin var startIndex , endIndex , i : int ; var isFound : float ; var Atrive : bool ; startIndex := 100d ; endIndex := 2.31d ; isFound := 12.123E+12 ; isFound := 140d div 70d ; % true if index found % for i := 0d to i LT endIndex step i := i plus 1d writeln i next if ( i NE startIndex ) writeln i ; else readln i ; while ( endIndex LE startIndex ) writeln endIndex or 1d ; end";
            //string enteredText = "begin dim startIndex , 135d endIndex, i %;dim isFound !;dim Atrive $;startIndex = 100d ; let endIndex = 23.1; isFound = 12.123E+12; /* true if index found */ for ( i ; i < endIndex; i = i + 1d) output (i); if i <> startIndex; then output(i); else input (i) end_else; do while endIndex <= startIndex output (endIndex or 1d) loop; 245 end";
            string v = StartLexicalAnalyzer(enteredText);
            LargeTextBox.Text = v;
            SyntacticAnalizator syntactic = new SyntacticAnalizator(_identificators, _konstants,enteredText);
            syntactic.CheckProgram(enteredText);
            //LargeTextBo.Text = enter.Text;
            //MessageBox.Show(v);
        }

        
    }

    
}



//--------------------------------------------------------------------------
//public Dictionary<string, string> numberWithType = new Dictionary<string, string>();

//private List<ServiceWord> _serviceWords;
//private List<Separators> _separators;
//private List<Identificator> _identificators;
//private List<Konstanta> _konstants;
//private List<string> _arrResult;



//public MainWindow(List<ServiceWord> serviceWords, List<Separators> separators)
//{
//    InitializeComponent();
//    _serviceWords = serviceWords;
//    _separators = separators;
//    _arrResult = new List<string>();
//    _konstants = new List<Konstanta>();
//    _identificators = new List<Identificator>();

//}
//public int CheckServiceWord(string str)
//{
//    return _serviceWords.FindIndex(x => x.word == str);
//}
//public static List<string> GetServiceWords()
//{
//    return new List<string>()
//            {
//                "begin",
//                "end",
//                "dim",
//                "if",
//                "then",
//                "else",
//                "for",
//                "let",
//                "while",
//                "loop",
//                "input",
//                "output",
//                "true",
//                "false",
//                "do",
//                "end_else",
//                "do_while",
//                "%",
//                "!",
//                "$"
//            };
//}
//Даны фрагменты кода. На основании их напиши класс ServiceWord