using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Analizator
{
    internal class Parser
    {
        public static string StartParser(string programStr)
        {
            while (programStr.Contains("\n")) { programStr = programStr.Replace("\n", " "); }
            while (programStr.Contains("\r")) { programStr = programStr.Replace("\r", " "); }
            for (int i = 0; i < programStr.Length; i++)
            {
                if (programStr[i] == ':')
                {
                    programStr = programStr.Insert(i, " ");
                    i++;
                }
                else if (programStr[i] == ',' || programStr[i] == '{' || programStr[i] == '}' || programStr[i] == '(' || programStr[i] == ')' || programStr[i] == ';')
                {
                    programStr = programStr.Insert(i, " ");
                    programStr = programStr.Insert(i + 2, " ");
                    i += 2;
                }
            }
            while (programStr.Contains("  ")) { programStr = programStr.Replace("  ", " "); }
            int count = 0;
            string tempStr = "";
            bool SymbolComment = false;
            for (int i = 0; i < programStr.Count(); i++)
            {

                if (programStr[i] == '/' && programStr[i + 1] == '*')
                {

                    SymbolComment = true;

                    tempStr += programStr[i];
                    tempStr += programStr[i + 1];
                    tempStr += " ";


                }
                if (programStr[i] == '*' && programStr[i + 1] == '/')
                {
                    SymbolComment = false;
                }

                if (!SymbolComment)
                {
                    tempStr += programStr[i];

                }
            }
            return tempStr;
        }

    }
}
