using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoIf:Instruccion3d
    {
        private Instruccion3d op;
        private Instruccion3d jump;

        public NodoIf(Instruccion3d op, Instruccion3d jump)
        {
            this.op = op;
            this.jump = jump;
        }

        public override object ejecutarSin(entorno3d e)
        {
            
            var algo1 = op.ejecutarSin(e);
            var valor1 = Utilidades.desenvolver(algo1);

            if (valor1 is Bol)
            {
                if (((Bol) valor1).obtenerValor())
                {
                    var nuevaEtiqueta = jump.ejecutarSin(e);
                    return nuevaEtiqueta;
                }

                return null;
            }
            
            return null;



        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}