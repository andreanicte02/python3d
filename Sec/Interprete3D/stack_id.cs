using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class stack_id: nodo3d
    {
        private nodo3d posicion;
        private nodo3d valor;

        public stack_id(nodo3d posicion, nodo3d valor)
        {
            this.posicion = posicion;
            this.valor = valor;
        }

        public override object ejecutar(entorno3d e)
        {

            double pos = (double)posicion.ejecutar(e);
            double val = (double) valor.ejecutar(e);

            e.stack[Convert.ToInt32(pos)] = val;
            
            return null;
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}