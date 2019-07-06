using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion.Label
{
    public class NodoJumpLabel:Instruccion3d
    {
        private String nombreLabel;

        public NodoJumpLabel (String nombreLabel)
        {
            this.nombreLabel = nombreLabel;
        }

        public override object ejecutarSin(entorno3d e)
        {

           
            if (!e.tablaS.ContainsKey(nombreLabel))
            {
                throw new SemanticError3d("no se encuentra registrado el label -> " +nombreLabel);
            }

            return Utilidades.buscarIndiceLabel(e,nombreLabel, null);
            
            

        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }

        public String obtenerNombre()
        {
            return nombreLabel;
        }


    }
}