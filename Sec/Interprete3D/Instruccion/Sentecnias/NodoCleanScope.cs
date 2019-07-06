using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoCleanScope:Instruccion3d
    {
        private Instruccion3d literal1;
        private Instruccion3d literal2;


        public NodoCleanScope(Instruccion3d literal1, Instruccion3d literal2)
        {
            this.literal1 = literal1;
            this.literal2 = literal2;
        }


        public override object ejecutarSin(entorno3d e)
        {
            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}