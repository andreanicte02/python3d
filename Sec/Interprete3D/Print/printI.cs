using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Print
{
    public class printI:nodo3d
    {
        private nodo3d literal;

        public printI(nodo3d literal)
        {
            this.literal = literal;
        }

        public override object ejecutar(entorno3d e)
        {
            double val = (double)literal.ejecutar(e);

            int f = (int) val;
            Zide.console.Text += f;
            Console.WriteLine("%i "+ f);


            return null;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}