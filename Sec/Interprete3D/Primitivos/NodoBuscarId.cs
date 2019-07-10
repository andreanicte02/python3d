using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Primitivos
{
    public class NodoBuscarId:nodo3d
    {
        private String id;

        public NodoBuscarId(String id)
        {
            this.id = id;
        }

        public override object ejecutar(entorno3d e)
        {

            //obtengo solo el valor
            return e.obtenerVar(id).valor;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
    }
}