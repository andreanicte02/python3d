using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Relacionales
{
    public class nodoIf: nodo3d
    {
        private nodo3d condicion;
        private goTo go;


        public nodoIf(nodo3d condicion, goTo go)
        {
            this.condicion = condicion;
            this.go = go;
        }

        public override object ejecutar(entorno3d e)
        {
            bool con = (bool) condicion.ejecutar(e);

            if (con)
            {
                //Console.Write("nodo if-->");
                return go.ejecutar(e);
                
            }

            return null;
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}