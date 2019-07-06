using System;
using System.Collections.Generic;
using Microsoft.CSharp.RuntimeBinder;

namespace Sec.Interprete3D.Entorno
{
    public class Utilidades
    {
        
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

        public static Object ejecutarInstrucciones(entorno3d e, List<Instruccion3d> instrucciones)
        {

            for (int i =0; i<instrucciones.Count; i++)
            {

                instrucciones[i].ejecutarSin(e);

                Console.WriteLine("hola");
                
            }

            return null;
        }

        public static Object recogerEtiquetas(entorno3d e, List<Instruccion3d> instrucciones)
        {
            
            for (int i =0; i<instrucciones.Count; i++)
            {

                instrucciones[i].ejecutarSin(e);

            }


            return null;
        }



    }
}