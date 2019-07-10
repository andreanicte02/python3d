using System;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class nodoLabel: nodo3d
    {
        public String id;

        public nodoLabel(String id)
        {
            this.id = id;
        }

        public override object ejecutar(entorno3d e)
        {

            //Console.WriteLine(">" +id +"<");
            return id;
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            if (!inMetodo)
            {
                e.registrarVar(id,new variables(id,"",false, true));
                
            }
            else
            {
                e.registrarVar(id, new variables(id, nameMetodo, inMetodo, true));
                
            }

            return null;
        }
    }
}