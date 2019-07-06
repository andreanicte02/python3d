using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoSoloDeclarar: Instruccion3d
    {
        private String id;
        private Instruccion3d literal;

        public NodoSoloDeclarar(String id, Instruccion3d literal)
        {
            this.id = id;
            this.literal = literal;
        }


        public override object ejecutarSin(entorno3d e)
        {
            
            Object algo = literal.ejecutarSin(e);
            Instancia_3d valor1 = Utilidades.desenvolver(algo);

            if (!(valor1 is Instancia_3d))
            {

                throw new SemanticError3d("error al declarar id");
            }

            
            Simbolo3d s = e.getSimbolo(id);
            s.definirValor(valor1);
            

            return null;

        }

        public override object recogerEtiquetas(entorno3d e)
        {
            
            e.setSimbol(id,new Simbolo3d(TipoPrimitivo.instanciaNulo));
            Console.WriteLine("se declaro una algo (var id = exp)-> " +id);
       
            return null;

        }
    }
}