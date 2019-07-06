using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using Sec.Interprete3D.Instruccion.Label;
using Sec.Interprete3D.Instruccion.Metodos;

namespace Sec.Interprete3D.Entorno
{
    public class Utilidades
    {
        //instrucciones
        public static List<Instruccion3d> lInstrucciones;
        
        public static Instancia_3d desenvolver(Object valor) {

            if (valor is Simbolo3d) {
                Simbolo3d sim = (Simbolo3d) valor;
                return sim.obtenerValor();
            }
            
            if (valor is Instancia_3d) {
                return (Instancia_3d) valor;
            }


            throw new RuntimeBinderException("Estas utilizando un valor que no es ni Instancia ni Simbolo");

        }
        //priemra pasada recoger etiquetas
        public static Object recogerEtiquetas(entorno3d e, List<Instruccion3d> instrucciones)
        {
            
            for (int i =0; i<instrucciones.Count; i++)
            {


                if (instrucciones[i] is NodoDeclararMetodo)
                {
                    instrucciones[i].recogerEtiquetas(e);
                    recogerEtiquetaMetodo(e, ((NodoDeclararMetodo) instrucciones[i]).obenterInstruccioes());
                }
                else
                {
                    
                    instrucciones[i].recogerEtiquetas(e);
                }

            }


            return null;
        }

        /// <summary>
        /// recoge los temporales, etiquetas declaradas en en un metodo
        /// </summary>
        /// <param name="e"></param>
        /// <param name="instrucciones"></param>
        /// <returns></returns>
        public static Object recogerEtiquetaMetodo(entorno3d e, List<Instruccion3d> instrucciones)
        {

            for (int i = 0; i < instrucciones.Count; i++)
            {
                instrucciones[i].recogerEtiquetas(e);
            }

            return null;
        }
        
        /// <summary>
        /// ejecutar las intrucciones
        /// </summary>
        /// <param name="e"></param>
        /// <param name="instrucciones"></param>
        /// <returns></returns>

        public static Object ejecutarInstrucciones(entorno3d e, List<Instruccion3d> instrucciones)
        {

            for (int i =0; i<instrucciones.Count; i++)
            {
                
                var result = instrucciones[i].ejecutarSin(e);

                if (result is SaltoLabel)
                {
                    i = ((SaltoLabel) result).obetnerIndex();
                }



            }

            return null;
        }

     


        public static SaltoLabel buscarIndiceLabel(entorno3d e, String nombre, List<Instruccion3d> ins)
        {

            if (ins == null)
            {
                ins = lInstrucciones;
            }

            for (int x = 0; x < ins.Count; x++)
            {
                if (ins[x] is NodoLabel)
                {
                    if (nombre.Equals(((NodoLabel) ins[x]).obtenerLabel()))
                    {
                        return  new SaltoLabel(x);
                        
                    }
                }else if (ins[x] is NodoCallMetodo)
                {
                    //buscar salots de label en los temporales
                     SaltoLabel result = buscarIndiceLabel(e, nombre, e.funciones[((NodoCallMetodo) ins[x]).obenterNombre()]);
                     if (result.obetnerIndex() == -1)
                     {
                         //no pasa nada
                     }
                     else
                     {
                         return result;
                     }
                }
            }


            return null;
            // return new SaltoLabel(-1);
        }
        
      


    }
}