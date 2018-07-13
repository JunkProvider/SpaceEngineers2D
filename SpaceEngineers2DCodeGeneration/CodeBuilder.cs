using System;
using System.Text;

namespace SpaceEngineers2DCodeGeneration
{
    public class CodeBuilder
    {
        private int _indent;
        private readonly StringBuilder _stringBuilder = new StringBuilder();

        public void Indent()
        {
            _indent++;
        }

        public void UnIndent()
        {
            _indent--;
        }

        public void OpenBrackets()
        {
            WriteLine("{");
            Indent();
        }

        public void CloseBrackets()
        {
            UnIndent();
            WriteLine("}");
        }

        public void WriteLine(string text = "")
        {
            WriteIndent();
            _stringBuilder.Append(text);
            _stringBuilder.Append(Environment.NewLine);
        }

        public override string ToString()
        {
            return _stringBuilder.ToString();
        }

        private void WriteIndent()
        {
            for (var i = 0; i < _indent; i++)
            {
                _stringBuilder.Append("\t");
            }
        }
    }
}