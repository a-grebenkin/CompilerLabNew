using Antlr4.Runtime.Tree;
using Antlr4.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace CompilerLab
{
    public static class AntlerHelper
    {
        public static string GetResult(string text)
        {
            ICharStream stream = CharStreams.fromString(text);
            BASICLexer lexer = new BASICLexer(stream);

            var errorListener = new CustomErrorListener();

            lexer.RemoveErrorListeners();
            lexer.AddErrorListener(errorListener);

            ITokenStream tokens = new CommonTokenStream(lexer);
            BASICParser parser = new BASICParser(tokens);

            parser.RemoveErrorListeners();
            parser.AddErrorListener(errorListener);


            parser.program(); 

            var output = errorListener.GetErrors();
            if (output == string.Empty)
            {
                return "Ошибок не обнаружено";
            }
            return output;
        }
    }

    class CustomErrorListener : IAntlrErrorListener<int>, IAntlrErrorListener<IToken>
    {
        private readonly StringBuilder _errors = new StringBuilder();

        public void SyntaxError(TextWriter output, IRecognizer recognizer, int offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            _errors.AppendLine($"[Lexer Error] ({line}:{charPositionInLine+1}): {msg}");
        }

        public void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            _errors.AppendLine($"[Parser Error] ({line}:{charPositionInLine+1}): {msg}");
        }

        public string GetErrors()
        {
            return _errors.ToString();
        }
    }
}
