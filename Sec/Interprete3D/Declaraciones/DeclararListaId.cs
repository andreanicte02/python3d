using System;
using System.Collections.Generic;
using Sec.Interprete3D.intefaces;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Declaraciones
{
    public class DeclararListaId: nodo3d
    {
        public List<NodoID> listaID;

        public DeclararListaId(List<NodoID> listaId)
        {
            listaID = listaId;
        }

        public override object ejecutar(entorno3d e)
        {
            return null;
        }

        public override object recoger(entorno3d e,  bool inMetodo, String nameMetod)
        {
            foreach (NodoID varId in listaID)
            {
                string id = (string)varId.ejecutar(e);
                
                e.registrarVar(id, new variables(id,inMetodo,nameMetod));
                

            }

            return null;
        }
    }
}