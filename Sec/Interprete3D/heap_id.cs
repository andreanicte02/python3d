using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Relacionales
{
    public class heap_id:nodo3d
    {
        
        private nodo3d posicion;
        private nodo3d valor;


        public heap_id(nodo3d posicion, nodo3d valor)
        {
            this.posicion = posicion;
            this.valor = valor;
        }

        public override object ejecutar(entorno3d e)
        {
            double pos = (double)posicion.ejecutar(e);
            double val = (double) valor.ejecutar(e);


            try
            {
                e.heap[Convert.ToInt32(pos)] = val;

            }
            catch (Exception exception)
            {
                Console.WriteLine("se lleno el heap alv :(");
                throw  new SemanticError3d("se lleno el heap");
            }
            return null;
          
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}