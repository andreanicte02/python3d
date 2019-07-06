using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion.Label
{
    public class NodoLabel:Instruccion3d
    {
        private String label;

        public NodoLabel(string label)
        {
            this.label = label;
        }

        public override object ejecutarSin(entorno3d e)
        {

            /*
            Simbolo3d s = new Simbolo3d(TipoPrimitivo.instanciaNulo);
            
            e.setSimbol(label, s);*/
            
            
            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            Simbolo3d s = new Simbolo3d(TipoPrimitivo.instanciaNulo);
            
            e.setSimbol(label, s);
            return null;
        }

        public String obtenerLabel()
        {
            return this.label;
        }
    }
}