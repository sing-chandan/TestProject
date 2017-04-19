using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiveMonitoring
{
    internal class GetKeyValue
    {
        internal static string GetPressValue(string strKey, bool iscaplock)
        {
            switch (strKey)
            {
                //letters
                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                case "J":
                case "K":
                case "L":
                case "M":
                case "N":
                case "O":
                case "P":
                case "Q":
                case "R":
                case "S":
                case "T":
                case "U":
                case "V":
                case "W":
                case "X":
                case "Y":
                case "Z":

                    if (iscaplock == true)
                    {
                        return strKey;
                    }
                    return strKey.ToLower();

                //digits
                case "D0":
                case "NumPad0":
                    return "0";

                case "D1":
                case "NumPad1":
                    return "1";

                case "D2":
                case "NumPad2":
                    return "2";

                case "D3":
                case "NumPad3":
                    return "3";

                case "D4":
                case "NumPad4":
                    return "4";

                case "D5":
                case "NumPad5":
                    return "5";

                case "D6":
                case "NumPad6":
                    return "6";

                case "D7":
                case "NumPad7":
                    return "7";

                case "D8":
                case "NumPad8":
                    return "8";

                case "D9":
                case "NumPad9":
                    return "9";

                //punctuation
                case "Add":
                    return "+";
                case "Subtract":
                    return "-";
                case "Divide":
                    return "/";
                case "Multiply":
                    return "*";
                case "Space":
                    return " ";
                case "Decimal":
                    return ".";

                case "Back":
                    return "";

                case "Enter":
                case "Return":
                    return " \n ";

                case "CapsLock":
                    return "CapsLock";

                //oem keys 
                case "Oem1":
                    return ";";
                case "OemSemicolon": //oem1
                    return ";";
                case "OemQuestion":
                case "Oem2":         //oem2
                    return "/";
                case "Oemtilde":     //oem3
                    return "`";
                case "OemOpenBrackets":
                case "Oem4":        //oem4
                    return "[";
                case "OemPipe":  //oem5
                    return "|";
                case "OemCloseBrackets":
                case "Oem6":         //oem6
                    return "]";
                case "OemQuotes":        //oem7
                    return "'";
                case "Oemplus":
                    return "=";
                case "OemMinus":
                    return "-";
                case "Oemcomma":
                    return ",";
                case "OemPeriod":
                    return ".";
                case "Oem7":
                    return "\'";
                case "Oem5":
                    return "\\";


                default:

                    return string.Empty;
            }
        }

        internal static string GetShiftValue(string strKey, bool iscaplock)
        {
            switch (strKey)
            {
                //oem keys 
                case "Oem1":
                    return ":";
                case "Oem5":
                    return "|";
                case "OemSemicolon": //oem1
                    return ";";
                case "OemQuestion":
                case "Oem2":         //oem2
                    return "?";
                case "Oemtilde":     //oem3
                    return "~";
                case "OemOpenBrackets":
                case "Oem4":        //oem4
                    return "{";
                case "OemPipe":  //oem5
                    return "|";
                case "OemCloseBrackets":
                case "Oem6":         //oem6
                    return "}";
                case "OemQuotes":        //oem7
                    return "'";
                case "Oemplus":
                    return "+";
                case "OemMinus":
                    return "_";
                case "Oemcomma":
                    return "<";
                case "OemPeriod":
                    return ">";

                case "Oem7":
                    return "\"";


                //digits
                case "D1":
                    return "!";
                case "D2":
                    return "@";
                case "D3":
                    return "#";
                case "D4":
                    return "$";
                case "D5":
                    return "%";
                case "D6":
                    return "^";
                case "D7":
                    return "&";
                case "D8":
                    return "*";
                case "D9":
                    return "(";
                case "D0":
                    return ")";


                case "A":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                case "G":
                case "H":
                case "I":
                case "J":
                case "K":
                case "L":
                case "M":
                case "N":
                case "O":
                case "P":
                case "Q":
                case "R":
                case "S":
                case "T":
                case "U":
                case "V":
                case "W":
                case "X":
                case "Y":
                case "Z":

                    if (iscaplock == true)
                    {
                        return strKey.ToLower();
                    }
                    return strKey;

                //punctuation
                case "Add":
                    return "+";
                case "Subtract":
                    return "-";
                case "Divide":
                    return "/";
                case "Multiply":
                    return "*";
                case "Space":
                    return " ";


                case "Back":
                    return "";

                case "Enter":
                case "Return":
                    return " \n ";

                case "CapsLock":
                    return "CapsLock";

                default:
                    return string.Empty;
            }
        }

        internal static string GetcontrolValue(string strKey)
        {
            switch (strKey)
            {
                case "Back":
                    return "";

                case "Enter":
                case "Return":
                    return " \n ";

                case "CapsLock":
                    return "CapsLock";

                default:
                    return string.Empty;
            }
        }
    }
}
