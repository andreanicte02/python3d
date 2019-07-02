using System;

namespace Sec.Interprete3D.Entorno
{
    public class Instancia_3d:entorno3d
    {
        private String nombre;
        private Instancia_3d instanciaOrigen;


        public Instancia_3d(String nombre, entorno3d entornoPadre, Instancia_3d instanciaOrigne):base(entornoPadre)
        {
            
            this.nombre = nombre;
            this.instanciaOrigen = instanciaOrigne;
            
        }

        public Instancia_3d obtenerInstanciaOrigen()
        {
            return this.instanciaOrigen;
        }
        
        public bool mismaInstanciaOrigen(Instancia_3d n1) {
            if (this.instanciaOrigen == null) {
                return false;
            }
            if (n1.instanciaOrigen == null) {
                return false;
            }
            return this.instanciaOrigen.nombre.Equals(n1.instanciaOrigen.nombre);
        }

        public bool es(Instancia_3d n1) {

            return this.instanciaOrigen.nombre.Equals(n1.nombre);
        }

        public bool mismaInstancia(Instancia_3d n1){
            return this.nombre.Equals(n1.nombre);
        }

        public String obtenerNombre() {
            return this.nombre;
        }

        public String obtenerNombreTipo() {
            return this.instanciaOrigen.nombre;
        }

    
        public virtual String stringBonito() {

            return "alv";
        }






    }
}