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

            return this.nombreLabel;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            throw new NotImplementedException();
        }
    }
}