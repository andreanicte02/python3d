using System;

namespace Sec.Analizadores
{
    public class SyntaxException: ApplicationException
    {
        public int Line { get; }
        public int Column { get; }

        public SyntaxException(string message, int line, int column): base(message)
        {
            Line = line;
            Column = column;
        }
    }
}