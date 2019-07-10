using System;
using Sec.Analizadores;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Aritmeticas
{
    public class suma: nodo3d
    {
        private nodo3d val1;
        private nodo3d val2;

        public suma(nodo3d val1, nodo3d val2)
        {
            this.val1 = val1;
            this.val2 = val2;
        }

        public override object ejecutar(entorno3d e)
        {
            Object e1 = val1.ejecutar(e);
            Object e2 = val2.ejecutar(e);

            return (double)e1 + (double) e2;


        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}