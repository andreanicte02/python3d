using System;
using System.Collections.Generic;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class utilidades
    {
        /// <summary>
        /// sentencias globales
        /// </summary>
        public static List<nodo3d> sentenciasGlobales = new List<nodo3d>();


        /// <summary>
        /// recogerEtiquetas: primera pasada, se utiliza para recoger las etiquetas
        /// </summary>
        /// <param name="e"> entorno </param>
        /// <param name="sentencias"> listado de sentencias </param>
        /// <param name="inMetodo"> bandera que demuestra si esta dentro de un metodo </param>
        /// <param name="nombreMetodo"> nombre demetodo si este existe</param>

        public static void recogerEtiquetas(entorno3d e, List<nodo3d> sentencias, bool inMetodo, String nombreMetodo)
        {

            foreach (nodo3d v in sentencias)
            {
                v.recoger(e,inMetodo,nombreMetodo);
            }
            
        }

        /// <summary>
        /// ejecutarSentencias: ejecuta un listado de sentencias
        /// </summary>
        /// <param name="e"> entorno</param>
        /// <param name="sentencias"> listado de sentencias </param>
        /// <returns> solo puede retornar un posible salto </returns>
        public static Object ejecutarSentencias(entorno3d e, List<nodo3d> sentencias)
        {
          
            //Console.WriteLine("probando");
            for (int x = 0; x < sentencias.Count; x++)
            {

                if (Zide.inDebug3d==1 || Zide.inDebug3d ==3)
                {
                    utilidadesParaDegg.enciclar(sentencias[x],e);
                }


                if (Zide.inDebug3d != -1)
                {
                    var posibleSalto = sentencias[x].ejecutar(e);

                    if (posibleSalto is index i)
                    {
                        x = i.inidce;
                    }
                }
            }

            return null;

        }

        /// <summary>
        /// metodo utilizado en los goTo, para buscar el indice donde este se encuentra
        /// </summary>
        /// <param name="id">nombre del label </param>
        /// <param name="sentencias">listado de sentencias </param>
        /// <param name="name">ambito, puede ser global o dentor de un metodo </param>
        /// <returns></returns>
        /// <exception cref="SemanticError3d"></exception>
        public static index buscarindice(String id, List<nodo3d> sentencias, String name)
        {

            for (int x = 0; x < sentencias.Count; x++)
            {
                if (sentencias[x] is nodoLabel l)
                {
                    if (l.id.Equals(id))
                    {
                        return new index(x);
                    }
                }
            }
            throw new SemanticError3d("label "+ id+ " no existe en el ambito ->" + name);
        }
    }
}