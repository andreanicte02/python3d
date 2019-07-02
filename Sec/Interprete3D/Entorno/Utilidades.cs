using System;

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


            return null;
            //throw new RuntimeException("Estas utilizando un valor que no es ni Instancia ni Simbolo");

        }

        
        
    }
}