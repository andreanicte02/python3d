using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion.Imprimir
{
    public class NodoPrintF:Instruccion3d
    {
        private Instruccion3d literal;

        public NodoPrintF(Instruccion3d literal)
        {
            this.literal = literal;
        }

        public override object ejecutarSin(entorno3d e)
        {
            var algo = literal.ejecutarSin(e);
            var val = Utilidades.desenvolver(algo);
            
            Console.WriteLine("sentencia print-> "+ val.stringBonito());
            return null;


        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}