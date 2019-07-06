using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoAsignarIdHeap:Instruccion3d
    {
        private String id;
        private Instruccion3d literal;

        public NodoAsignarIdHeap(string id, Instruccion3d literal)
        {
            this.id = id;
            this.literal = literal;
        }

        public override object ejecutarSin(entorno3d e)
        {
            var algo1 = literal.ejecutarSin(e);
            var valor1 = Utilidades.desenvolver(algo1);
            var simb = NodoBuscarId.buscarId(e,id);


            if (valor1 is Decmal)
            {
        
                throw new SemanticError3d("error al ahcedder a heap tiene que ser un valor int");
                
              
            }else if (valor1 is Numero)
            {
                Numero n = (Numero)valor1;
                var val = e.heap[n.obtenerValor()];
                simb.definirValor((Instancia_3d)val);

            }

            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}