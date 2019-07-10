using System;
using Irony.Parsing;

namespace Sec.Interprete3D.intefaces
{
    public abstract class nodo3d
    {
        public abstract object ejecutar(entorno3d e);
        public abstract object recoger(entorno3d e, bool inMetodo, String nameMetodo);

        public SourceLocation location;
    }
}