using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Primitivos
{
    public class NodoID: nodo3d
    {
        private string id;

        public NodoID(String id)
        {
            this.id = id;
        }

        public override object ejecutar(entorno3d e)
        {
            return id;
        }

        public override object recoger(entorno3d e ,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}