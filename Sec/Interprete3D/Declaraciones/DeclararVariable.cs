using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Declaraciones
{
    public class DeclararVariable: nodo3d
    {
        private string id;
        private nodo3d literal;

        public DeclararVariable(string id, nodo3d literal)
        {
            this.id = id;
            this.literal = literal;
        }


        public override object ejecutar(entorno3d e)
        {

            double valor = (double)literal.ejecutar(e);
            variables v = e.obtenerVar(id);
            v.valor = valor;
            return null;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetodo)
        {

            if (!inMetodo)
            {
                e.registrarVar(id, new variables(id));

            }
            else
            {
                e.registrarVar(id, new variables(id,inMetodo,nameMetodo));
            }

            return null;
        }
    }
}