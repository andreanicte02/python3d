using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoCrearNumero:Instruccion3d
    {

        public readonly int valor;

        public NodoCrearNumero(int valor)
        {
            this.valor = valor;
        }


        public  override object ejecutarSin(entorno3d e)
        {
            return null;

        }
        



    }
}