using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion.Imprimir
{
    public class NodoPrintC:Instruccion3d
    {
        private Instruccion3d literal1;

        public NodoPrintC(Instruccion3d literal1)
        {
            this.literal1 = literal1;
        }

        public override object ejecutarSin(entorno3d e)
        {
            var algo = literal1.ejecutarSin(e);
            var val = Utilidades.desenvolver(algo);
            
            switch (val)
            {
                case Numero numero:
                {
                    var caracter = (char) numero.obtenerValor();
                    Console.WriteLine("sentencia print-> "+ caracter);
                
                    return null;
                }

                case Decmal decmal:
                {
                    var caracter = (char) decmal.obtenerValor();
                    Console.WriteLine("sentencia print-> "+ caracter);
                
                    return null;
                }

                default:
                    throw  new SemanticError3d("error al imprimir c");
            }
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}