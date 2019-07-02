using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D
{
    public abstract class Instruccion3d: nodoAst3
    {

        abstract public Object ejecutarSin(entorno3d e);



    }
}