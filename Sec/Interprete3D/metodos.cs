using System;
using System.Collections.Generic;
using Sec.Analizadores;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class metodos
    {
        public String id;
        public List<nodo3d> instrucciones;

        public metodos(string id, List<nodo3d> instrucciones)
        {
            this.id = id;
            this.instrucciones = instrucciones;
        }
    }
}