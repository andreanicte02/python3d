using System;
using System.Collections.Generic;

namespace Sec.Interprete3D.Entorno
{
    public class entorno3d
    {

        //ya no se va usar
        public entorno3d anterior;
        public Dictionary<String, Simbolo3d> tablaS;

        //array y heap
        
        public entorno3d(entorno3d anterior)
        {

            this.anterior = anterior;
            this.tablaS = new Dictionary<string, Simbolo3d>();
        }
        


        public Simbolo3d getSimbolo(String id)
        {
            if (tablaS.ContainsKey(id))
            {
                return tablaS[id];
            }

            throw new SemanticError3d("no existe el id");
        }

        public void  setSimbol(String id, Simbolo3d valor)
        {
            if (tablaS.ContainsKey(id))
            {
                throw new SemanticError3d("ya se registro el id");
                
            }
            else
            {

                tablaS.Add(id,valor);

            }

            
        }




    }
}