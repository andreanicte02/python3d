using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Print
{
    public class printF: nodo3d
    {
        private nodo3d litreal;

        public printF(nodo3d litreal)
        {
            this.litreal = litreal;
        }

        public override object ejecutar(entorno3d e)
        {
            double val = (double)litreal.ejecutar(e);

            
            Zide.console.Text += val;
            Console.WriteLine("%f "+ val);


            return null;
            
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}