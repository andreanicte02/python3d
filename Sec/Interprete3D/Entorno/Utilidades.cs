using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;
using Sec.Interprete3D.Instruccion.Label;

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

                instrucciones[i].recogerEtiquetas(e);

            }


            return null;
        }

        public static Object ejecutarInstrucciones(entorno3d e, List<Instruccion3d> instrucciones)
        {

            for (int i =0; i<instrucciones.Count; i++)
            {

                var result = instrucciones[i].ejecutarSin(e);
                //si existe un call jump
                if (result is SaltoLabel)
                {
                    //se reconfugar la poscion en donde se va estar ejecutando
                    i = ((SaltoLabel) result).obetnerIndex();

                }
                

            }

            return null;
        }

 

        public static int buscarIndiceLabel(entorno3d e, String nombre)
        {

            for (int i = 0; i < lInstrucciones.Count; i++)
            {
                var result = lInstrucciones[i];
                if (result is NodoLabel)
                {
                    if (nombre.Equals(((NodoLabel) result).obtenerLabel()))
                    {
                        Console.WriteLine("match con el label " +nombre);
                        return i;
                    }

                }

            }

            return -1;
        }



    }
}