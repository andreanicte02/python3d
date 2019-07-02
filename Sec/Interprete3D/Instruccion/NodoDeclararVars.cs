using System.Collections.Generic;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoDeclararVars: Instruccion3d
    {
        private List<Instruccion3d> listaId;

        public NodoDeclararVars(List<Instruccion3d> listaId)
        {
            this.listaId = listaId;

        }

        public override object ejecutarSin(entorno3d e)
        {

            foreach (var ix in listaId)
            {
                ix.ejecutarSin(e);

            }

            return null;
        }
    }
}