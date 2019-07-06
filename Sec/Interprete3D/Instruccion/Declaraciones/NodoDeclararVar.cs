using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoDeclararVar: Instruccion3d
    {
        private String id;

        public NodoDeclararVar(String id)
        {
            this.id = id;
        }

        public override object ejecutarSin(entorno3d e)
        
        {
            /*
           Simbolo3d s = new Simbolo3d(TipoPrimitivo.instanciaNulo);
           
           e.setSimbol(this.id,s);
           
           */
           
           return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            Simbolo3d s = new Simbolo3d(TipoPrimitivo.instanciaNulo);
           
            e.setSimbol(this.id,s);

            Console.WriteLine("se declaro variable -> "+id);
            return null;
        }
    }
}