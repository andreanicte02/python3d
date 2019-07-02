using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoSuma: Instruccion3d
    {
        private Instancia_3d l1; //literal 1
        private Instancia_3d l2; //literal 2
        
        public override object ejecutarSin(entorno3d e)
        {
            throw new System.NotImplementedException();
        }
    }
}