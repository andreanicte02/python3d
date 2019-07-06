using System;
using System.Collections.Generic;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion.Metodos
{
    public class NodoDeclararMetodo: Instruccion3d
    {
 
        private String id;
        private List<Instruccion3d> instrucciones;

        public NodoDeclararMetodo(string id, List<Instruccion3d> instrucciones)
        {
            this.id = id;
            this.instrucciones = instrucciones;
        }

        public override object ejecutarSin(entorno3d e)
        {

            Utilidades.ejecutarInstrucciones(e, instrucciones);


            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            e.setFuncion(id,instrucciones);
            

            return null;
        }
    }
}