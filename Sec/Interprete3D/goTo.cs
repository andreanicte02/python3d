using System;
using System.Collections.Generic;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Relacionales
{
    public class goTo: nodo3d
    {
        private string label;

        public goTo(string labe)
        {
            this.label = labe;
        }

        public override object ejecutar(entorno3d e)
        {
            
            //----buscar indice

            if (!e.vars.ContainsKey(label))
            {
             
                throw  new SemanticError3d(" no se encuentra el label -> " + label);
                
            }

            if (e.vars[label].inMetodo)
            {
                Console.WriteLine("goto -> "+ label);
                variables v = e.vars[label];
                return utilidades.buscarindice(label, e.metos[v.NombreMetodo].instrucciones, v.NombreMetodo);
            }
            else
            {
                Console.WriteLine("goto -> "+ label);
                return utilidades.buscarindice(label, utilidades.sentenciasGlobales, "");
            }

            
        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}