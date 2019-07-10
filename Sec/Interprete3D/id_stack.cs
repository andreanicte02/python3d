using System;
using Sec.Interprete3D.intefaces;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Relacionales
{
    public class id_stack: nodo3d
    {
        private string id;
        private nodo3d posicio;


        public id_stack(string id, nodo3d posicio)
        {
            this.id = id;
            this.posicio = posicio;
        }

        public override object ejecutar(entorno3d e)
        {
          
            //se supoen que las verificaiones las hago en obtenre var
            double p =(double) posicio.ejecutar(e);
            variables v = e.obtenerVar(id);
            v.valor = e.stack[Convert.ToInt32(p)];
            return null;
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}