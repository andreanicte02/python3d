using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoBuscarId: Instruccion3d
    {
        private String id;
        public NodoBuscarId(String id)
        {
            this.id = id;

        }

        public override object ejecutarSin(entorno3d e)
        {

            return buscarId(e, id);
        }

        public static Simbolo3d buscarId(entorno3d e, String id)
        {

           return e.getSimbolo(id);
        }
    }
}