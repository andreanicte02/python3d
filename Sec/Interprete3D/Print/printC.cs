using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Print
{
    public class printC:nodo3d
    {
        private nodo3d litera;

        public printC(nodo3d litera)
        {
            this.litera = litera;
        }

        public override object ejecutar(entorno3d e)
        {
            double val = (double)litera.ejecutar(e);

            char f = (char) val;
            Zide.console.Text += f;
            Console.WriteLine("%c "+ f);


            return null;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}