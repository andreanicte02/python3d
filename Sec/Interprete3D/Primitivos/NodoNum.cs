using System;
using Sec.Analizadores;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Primitivos
{
    public class NodoNum: nodo3d
    {
        private double val;


        public NodoNum( double val )
        {
            this.val = val;
          
           
        }


        public override object ejecutar(entorno3d e)
        {
            return val;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            return null;
        }
        
    }
}