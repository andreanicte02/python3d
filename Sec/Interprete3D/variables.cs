using System;

namespace Sec.Interprete3D
{
    public class variables
    {
       
        public String id;
        public String NombreMetodo;
        public  Double valor;
        public bool inMetodo;
        public bool esEtiqueta;

        public variables(string id, string nombreMetodo, bool inMetodo, bool esEtiqueta)
        {
            this.id = id;
            NombreMetodo = nombreMetodo;
            this.inMetodo = inMetodo;
            this.esEtiqueta = esEtiqueta;
            valor = 0;
        }

        public variables(String id)
        {
            this.id = id;
            NombreMetodo = "";
            inMetodo = false;
            esEtiqueta = false;
            valor = 0;


        }
        
        
        public variables(String id, bool inMetodo, String nombreMetodo)
        {
            this.id = id;
            NombreMetodo = nombreMetodo;
            this.inMetodo = inMetodo;
            esEtiqueta = false;
            valor = 0;


        }
        



    }
    
    
}