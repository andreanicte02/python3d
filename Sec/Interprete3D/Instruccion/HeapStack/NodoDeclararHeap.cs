using System.Collections;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoDeclararHeap:Instruccion3d
    {

        public NodoDeclararHeap()
        {
            
        }

        public override object ejecutarSin(entorno3d e)
        {
            e.heap = new ArrayList();
            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}