using System;

namespace Sec.Interprete3D.Entorno
{
    public class Instancia_3d
    {
        private String nombre;

        public Instancia_3d(String nombre)
        {
            
            this.nombre = nombre;
            
        }

        

        public bool mismaInstancia(Instancia_3d n1){
            return this.nombre.Equals(n1.nombre);
        }

        public String obtenerNombre() {
            return this.nombre;
        }
        
    
        public virtual String stringBonito() {

            return "alv";
        }






    }
}