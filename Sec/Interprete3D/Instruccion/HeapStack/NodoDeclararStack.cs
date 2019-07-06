using System.Collections;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoDeclararStack:Instruccion3d
    
    {
        public NodoDeclararStack()
        {
        }

        public override object ejecutarSin(entorno3d e)
        {
            e.stack = new ArrayList();
            return null;

        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}