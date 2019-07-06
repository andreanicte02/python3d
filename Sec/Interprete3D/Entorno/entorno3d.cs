using System;
using System.Collections;
using System.Collections.Generic;

namespace Sec.Interprete3D.Entorno
{
    public class entorno3d
    {

        //ya no se va usar
        /****************************/
        /**/
        public entorno3d anterior;
        public Dictionary<String, Simbolo3d> tablaS;
        public Dictionary<String, List<Instruccion3d>> funciones;
        public ArrayList stack = new ArrayList();
        public ArrayList heap = new ArrayList();

        //array y heap
        
        public entorno3d(entorno3d anterior)
        {

            this.heap = new ArrayList();
            this.anterior = anterior;
            this.tablaS = new Dictionary<string, Simbolo3d>();
            this.funciones = new Dictionary<string, List<Instruccion3d>>();
        }
        


        public Simbolo3d getSimbolo(String id)
        {
            if (tablaS.ContainsKey(id))
            {
                return tablaS[id];
            }

            throw new SemanticError3d("no existe el id -> "+ id);
        }

        public void  setSimbol(String id, Simbolo3d valor)
        {
            if (tablaS.ContainsKey(id))
            {
                throw new SemanticError3d("ya se registro el id -> " + id);
                
            }
            else
            {

                tablaS.Add(id,valor);

            }

            
        }

        public void setFuncion(String id, List<Instruccion3d> instrucciones)
        {
            if (funciones.ContainsKey(id))
            {
                throw new SemanticError3d("ya se registro el id de etiqueta");
            }
          
            funciones.Add(id,instrucciones); 
            

        }

        public List<Instruccion3d>  getFuncion(String id)
        {

            if (funciones.ContainsKey(id))
            {
                return funciones[id];

            }
            else
            {
                throw new SemanticError3d("ya se registro el id de etiqueta");
            }
            
            
        }





    }
}