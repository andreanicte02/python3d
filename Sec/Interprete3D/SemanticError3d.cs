using System;

namespace Sec.Interprete3D
{
    public class SemanticError3d: Exception
    {
        private String descripcion;

        public SemanticError3d(String descripcion) : base(descripcion)
        {
            
        }


    }
}