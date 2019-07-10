using System;
using System.Collections.Generic;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class utilidades
    {
        public static List<nodo3d> sentenciasGlobales = new List<nodo3d>();



        public static void recogerEtiquetas(entorno3d e, List<nodo3d> sentencias, bool inMetodo, String nombreMetodo)
        {

            foreach (nodo3d v in sentencias)
            {
                v.recoger(e,inMetodo,nombreMetodo);
            }
            
        }

        public static Object ejecutarSentencias(entorno3d e, List<nodo3d> sentencias)
        {

            for (int x = 0; x < sentencias.Count; x++)
            {
                var posibleSalto = sentencias[x].ejecutar(e);

                if (posibleSalto is index i)
                {
                    x = i.inidce;
                }
            }

            return null;

        }

        public static index buscarindice(String id, List<nodo3d> sentencias, String name)
        {

            for (int x = 0; x < sentencias.Count; x++)
            {
                if (sentencias[x] is nodoLabel l)
                {
                    if (l.id.Equals(id))
                    {
                        return new index(x-1);
                    }
                }
            }
            throw new SemanticError3d("label "+ id+ " no existe en el ambito ->" + name);
        }
    }
}