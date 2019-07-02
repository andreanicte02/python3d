using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoSoloDeclarar: Instruccion3d
    {
        private String id;
        private Instruccion3d literal;


        public override object ejecutarSin(entorno3d e)
        {
            Object algo = literal.ejecutarSin(e);
            Instancia_3d valor1 = Utilidades.desenvolver(algo);

            e.setSimbol(id,new Simbolo3d(valor1));

            return null;

        }
    }
}