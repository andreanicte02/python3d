using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoAsignarHeapId:Instruccion3d
    {
        private Instruccion3d literal;
        private Instruccion3d literalValor;

        public NodoAsignarHeapId(Instruccion3d literal, Instruccion3d literalValor)
        {
            this.literal = literal;
            this.literalValor = literalValor;
        }

        public override object ejecutarSin(entorno3d e)
        {
            var instruccionIndex = literal.ejecutarSin(e);
            Instancia_3d index = Utilidades.desenvolver(instruccionIndex);

            var instruccionValor = literalValor.ejecutarSin(e);
            Instancia_3d valorAsignar = Utilidades.desenvolver(instruccionValor);
            
            if (index is Decmal)
            {
        
                throw new SemanticError3d("el acceso solo puede ser int a heap");
                
              
            }else if (index is Numero)
            {
                Numero n = (Numero)index;
                e.heap[n.obtenerValor()]=valorAsignar;
                

            }

            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}