using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Analizator
{
    internal class SemanticAnalizator
    {
        private Dictionary<string, string> _initializedVariables = new Dictionary<string, string>();
        //private Dictionary<string, string> _numberWithType = new Dictionary<string, string>();
        private List<string> operationsAssignments = new List<string>();
        private List<string> expression = new List<string>();
        private List<string> operations = new List<string> { "NE", "EQ", "LT", "LE", "GT", "GE", "plus", "min", "or", "mult", "div", "and", "~" };
        public Dictionary<string, bool> _identType = new Dictionary<string, bool>();
        public string[] vp;
        public SemanticAnalizator(Dictionary<string, bool> identType, Dictionary<string, string> initializedVariables, List<string> operationsAssignments, List<string> expression, string[] v)
        {
            _initializedVariables = initializedVariables;
            _identType = identType;
            vp = v;
            this.operationsAssignments = operationsAssignments;
            this.expression = expression;
        }

        public void StartSemanticAnalyzer()
        {
            bool ch = true;
            if (!CheckInitialized())
            {
                ch = false;
                MessageBox.Show("Не инициализированная переменная");
            }
            if (!CheckDiv())
            {
                ch = false;
                MessageBox.Show("Нельзя присвоить int переменной вещественное число");
            }

            if (!CheckBool())
            {
                ch = false;
                MessageBox.Show("Нельзя присвоить bool переменной данное значение");
            }
            if (ch) {
                MessageBox.Show("Семантический анализ завершён успешно");
            }
        }

       

        public bool CheckDiv()
        {
            foreach (var item in operationsAssignments)
            {
                string[] itemArr = item.Split(' ');
                string type = "";
                string id = itemArr[0];
                if (_initializedVariables.ContainsKey(id))
                {
                    type = _initializedVariables[id];
                }

                for (int i = 1; i < itemArr.Length; i++)
                {
                    if (itemArr[i] == "div" && (type == "int" || type == "bool"))
                    {
                        MessageBox.Show(itemArr[i]);
                        MessageBox.Show(type);
                        return false;
                    }
                }
            }
            return true;
        }
        
        public bool CheckBool()
        {
            foreach (var item in operationsAssignments)
            {
                string[] itemArr = item.Split(' ');
                string type = "";
                string id = itemArr[0];
                if (_initializedVariables.ContainsKey(id))
                {
                    type = _initializedVariables[id];
                }

                for (int i = 1; i < itemArr.Length; i++)
                {
                    if ((itemArr[i] == "true" || itemArr[i] == "false") && (type == "bool"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool CheckInitialized()
        {

            if (_identType.ContainsValue(false))
            {
                return false;
            }
            return true;
        }

    }
}

