using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoCrearDecimal: Instruccion3d
    {
        private double valor;

        public NodoCrearDecimal(double valor)
        {
            this.valor = valor;
        }

        public override object ejecutarSin(entorno3d e)
        {

            return new Decmal(valor);
        }
    }
}