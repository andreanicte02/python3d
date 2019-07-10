using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Relacionales
{
    public class igualIgual:nodo3d
    {
        private nodo3d val1;
        private nodo3d val2;

        public igualIgual(nodo3d val1, nodo3d val2)
        {
            this.val1 = val1;
            this.val2 = val2;
        }

        public override object ejecutar(entorno3d e)
        {
            Object e1 = val1.ejecutar(e);
            Object e2 = val2.ejecutar(e);

            return (double)e1 == (double) e2;


        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}