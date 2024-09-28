using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CompilerLab
{
    public static class RecursiveDescent
    {
        public class Error
        {
            public int Position;
            public string Message;
            public Error(int _position, string _message)
            {
                Position = _position;
                Message = _message;
            }
           
        }
        private static bool IsLatinUppercaseLetter(char c)
        {
            if (c >= 'a' && c <= 'z')
                return true;
            else
                return false;
        }

        private static List<Error> errors;
        private static StringBuilder result;
        private static int curentIndex;
        private static string inputText;
        public static string GetResult(string text)
        {
            errors = new List<Error>();
            curentIndex = 0;
            inputText = text+" ";
            Comand();

            if (errors.Count == 0)
                return "Ошибок не обнаружено";

            result = new StringBuilder();

            foreach (var error in errors)
            {
                error.Position = Math.Min(error.Position, text.Length);
                result.AppendLine($"Ошибка: {error.Message}. Позиция - {error.Position + 1}. \"{inputText.Substring(error.Position)}\"");
            }
            return result.ToString();
        }

        private static void SkipWhiteSpace()
        {
            while (curentIndex< inputText.Length && Char.IsWhiteSpace(inputText[curentIndex]))
                curentIndex++;
        }
        private static void SkipExtraneousCharacter()
        {
            while (curentIndex < inputText.Length 
                && !Char.IsWhiteSpace(inputText[curentIndex])
                 && inputText[curentIndex] != '*' && inputText[curentIndex] != '/'
                 && inputText[curentIndex] != '+' && inputText[curentIndex] != '-'
                 && inputText[curentIndex] != '=' && !Char.IsDigit(inputText[curentIndex]))
            {
                curentIndex++;
            }
        }
        private static int Find(string substring)
        {
           
            var tmpCurentIndex = 0;

            while (tmpCurentIndex + substring.Length <= inputText.Length)
            {
                if (inputText.Substring(tmpCurentIndex, substring.Length) == substring)
                {
                    return tmpCurentIndex;
                }
                tmpCurentIndex++;
            }
            return -1;
        }

        private static void Comand() //Comand -> if Сondition then Expression
        {
            SkipWhiteSpace();
            int index = Find("if ");
            if (index == -1)
            {
                errors.Add(new Error(curentIndex, $"Не найдено \"if\""));
                SkipExtraneousCharacter();
            }
            else
            {
                if (index != curentIndex)
                {
                    errors.Add(new Error(curentIndex, 
                        $"Встреченено значение отличное от \"if\""));
                    SkipExtraneousCharacter();
                }
                curentIndex = index+3;
            }

            SkipWhiteSpace();
            Сondition();
            SkipWhiteSpace();

            index = Find("then ");
            if (index == -1)
            {
                errors.Add(new Error(curentIndex, $"Не найдено \"then\""));
                SkipExtraneousCharacter();
            }
            else
            {
                if (index != curentIndex)
                {
                    SkipExtraneousCharacter();
                }
                curentIndex = index+5;
            }
            SkipWhiteSpace();
            Expression();
        }


        private static void Сondition() //Сondition -> Name == Number
        {
            if (!Name())
            {
                errors.Add(new Error(curentIndex, "Не верная запись имени переменной"));
                SkipExtraneousCharacter();
            }

            SkipWhiteSpace();
            int index = Find("==");
            if (index == -1)
            {
                errors.Add(new Error(curentIndex, "Не найдено \"==\""));
                //SkipExtraneousCharacter();
            }
            else
            {
                if (index != curentIndex)
                { 
                    SkipExtraneousCharacter();
                }
                curentIndex = index+2;
            }
            SkipWhiteSpace();

            if (!Number())
            {
                errors.Add(new Error(curentIndex, "Не верная запись числа"));
                SkipExtraneousCharacter();
            }
        }

        private static void Expression() //Expression -> Term | Term + Expression | Term - Expression
        {
            SkipWhiteSpace();
            Term();

            SkipWhiteSpace();
            if (curentIndex < inputText.Length && (inputText[curentIndex] == '+' 
                || inputText[curentIndex] == '-'))
            {
                curentIndex++;
                SkipWhiteSpace();
                Expression();
            }

        }

        private static void Term() // Term -> Operand | Operand * Term | Operand / Term
        {
            SkipWhiteSpace();
            Operand();
            SkipWhiteSpace();

            if (curentIndex < inputText.Length && (inputText[curentIndex] == '*' 
                || inputText[curentIndex] == '/'))
            {
                curentIndex++;
                SkipWhiteSpace();
                Term();
            }

        }

        private static void Operand() // Operand -> Name | Number | (Expression)
        {
            SkipWhiteSpace();
            if (curentIndex < inputText.Length && inputText[curentIndex] == '(')
            {
                curentIndex++;
                SkipWhiteSpace();
                Expression();
                int index = Find(")");
                if (index == -1)
                {
                    errors.Add(new Error(curentIndex, $"Не найдено \")\""));
                    SkipExtraneousCharacter();
                }
                else
                {
                    if (index != curentIndex)
                    { 
                        SkipExtraneousCharacter();
                    }
                    curentIndex = index + 1;
                }
                return;
            }


            if (Name())
            {
                return;
            }
            if (Number())
            {
                return;
            }

            errors.Add(new Error(curentIndex, "Не верная запись операнда"));
        }

        private static bool Name() // Name -> l{l|d}
        {
            SkipWhiteSpace();
            if (Find("if") == curentIndex || Find("then") == curentIndex)
                return false;
            if (curentIndex < inputText.Length && 
                IsLatinUppercaseLetter(inputText[curentIndex]))
            {
                curentIndex++;
                while (curentIndex < inputText.Length && 
                    (Char.IsDigit(inputText[curentIndex]) 
                    || IsLatinUppercaseLetter(inputText[curentIndex])))
                {
                    curentIndex++;
                }
            }
            else
            {
                return false;
            }
            if (curentIndex >= inputText.Length)
            {
                return true;
            }
            if (Char.IsWhiteSpace(inputText[curentIndex]) || inputText[curentIndex] == '=' || inputText[curentIndex] == '*'
                || inputText[curentIndex] == '/' || inputText[curentIndex] == '+' || inputText[curentIndex] == '-'
                || inputText[curentIndex] == ')')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static bool Number() // Number -> d^0{d}
        {
            SkipWhiteSpace();
            if (curentIndex < inputText.Length && Char.IsDigit(inputText[curentIndex]) 
                && inputText[curentIndex]!='0')
            {
                curentIndex++;
                while (curentIndex<inputText.Length && Char.IsDigit(inputText[curentIndex]))
                {
                    curentIndex++;
                }
            }
            else
            {
                return false;
            }
            if (curentIndex >= inputText.Length)
            {
                return true;
            }
            if (Char.IsWhiteSpace(inputText[curentIndex]) || inputText[curentIndex] == '=' || inputText[curentIndex] == '*'
                || inputText[curentIndex] == '/' || inputText[curentIndex] == '+' || inputText[curentIndex] == '-'
                || inputText[curentIndex] == ')')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
