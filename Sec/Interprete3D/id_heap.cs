using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Relacionales
{
    public class id_heap: nodo3d
    
    {
        private string id;
        private nodo3d posicio;

        public id_heap(string id, nodo3d posicio)
        {
            this.id = id;
            this.posicio = posicio;
        }
        public override object ejecutar(entorno3d e)
        {
        
            //se supone que las verificaciones las hago en obtenervar

            double p =(double) posicio.ejecutar(e);
            variables v = e.obtenerVar(id);
            v.valor = e.heap[Convert.ToInt32(p)];
            
            return null;
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}