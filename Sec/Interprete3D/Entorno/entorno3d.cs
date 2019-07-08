using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Entorno
{
    public class entorno3d
    {


        /****************************/
        /**/
        public entorno3d anterior;
        public Dictionary<String, Simbolo3d> tablaS;
        public Dictionary<String, List<Instruccion3d>> funciones;
        public List<Instancia_3d> stack = new List<Instancia_3d>();
        public List<Instancia_3d>  heap = new List<Instancia_3d>();
        

        //array y heap
        
        public entorno3d(entorno3d anterior)
        {

            
            this.anterior = anterior;
            tablaS = new Dictionary<string, Simbolo3d>();
            funciones = new Dictionary<string, List<Instruccion3d>>();
            llenar();

        }

        public void llenar()
        {
            for (int i = 0; i < 16800000; i++)
            {
                heap.Add(new Numero(0));
                stack.Add(new Numero(0));
            }
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