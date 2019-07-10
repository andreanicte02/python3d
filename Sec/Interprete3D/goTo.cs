using System;
using System.Collections.Generic;
using System.Windows.Forms;
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

            variables lab = e.obtenerVar(label);
            

            if (lab.inMetodo)
            {
                Console.WriteLine("goto -> "+ label);
                return utilidades.buscarindice(label, e.obtenerMetodo(lab.NombreMetodo).instrucciones, lab.NombreMetodo);
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