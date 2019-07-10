using System;
using System.Collections.Generic;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D.Declaraciones
{
    public class DeclararMetodo: nodo3d
    {
        private string id;
        private List<nodo3d> sentencias;

        public DeclararMetodo(string id, List<nodo3d> sentencias)
        {
            this.id = id;
            this.sentencias = sentencias;
        }

        public override object ejecutar(entorno3d e)
        {
            
            return null;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            e.registrarMetodo(id, new metodos(id,sentencias));
            registrarLocales(e, true);
            return null;
        }

        public void registrarLocales(entorno3d e,  bool inMetodo)
        {
            foreach (nodo3d n in sentencias)
            {
                n.recoger(e, inMetodo, id); //nombre del metodo
                
            }
        }
    }
}