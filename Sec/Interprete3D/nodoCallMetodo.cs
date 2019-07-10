using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class nodoCallMetodo: nodo3d
    {
        private String id;

        public nodoCallMetodo(string id)
        {
            this.id = id;
        }

        public override object ejecutar(entorno3d e)
        {

            if (!e.metos.ContainsKey(id))
            {
                throw  new SemanticError3d("error el metodo no existe->" + id);
            }

            utilidades.ejecutarSentencias(e, e.metos[id].instrucciones);
            
            return null;

        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}