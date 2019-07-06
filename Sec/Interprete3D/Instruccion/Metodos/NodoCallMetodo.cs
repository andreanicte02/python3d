using System;
using System.Collections.Generic;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion.Metodos
{
    public class NodoCallMetodo:Instruccion3d
    {
        private String id;

        public NodoCallMetodo(String id)
        {
            this.id = id;
        }

        public override object ejecutarSin(entorno3d e)
        {
            if (!e.funciones.ContainsKey(id))
            {
                throw new SemanticError3d("el metodo no existe");
            }

            List<Instruccion3d> ins = e.funciones[id];

            Utilidades.ejecutarInstrucciones(e, ins);
            
          
            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }

        public String obenterNombre()
        {
            return this.id;
        }
    }
}