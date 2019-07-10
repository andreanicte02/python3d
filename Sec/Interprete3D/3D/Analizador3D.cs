using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Irony.Parsing;
using Sec.Interprete3D;
using Sec.Interprete3D.intefaces;


namespace Sec.Analizadores
{
    public class Analizador3D
    {
        public Analizador3D()
        {
            
        }
        
        public void ejecutarAnalizador(String entrada)
        {
            //llamada a los cosos de irony
            Gramatica3D ga = new Gramatica3D();
            Parser parser = new Parser(ga);
            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;
            
            //verificar si el arbol no sta nulo
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                
                if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
                {
                    //---------------------> Hay Errores      

                    foreach (var item in arbol.ParserMessages)
                    {
                        MessageBox.Show("Error->"+item.Message+" Line:"+item.Location.Line);

                    }
                    return;
                }
            }

            utilidades.sentenciasGlobales = new List<nodo3d>();
             
             
             var instrucciones = (List<nodo3d>) arbol.Root.AstNode;
             utilidades.sentenciasGlobales = instrucciones;
            
             entorno3d e = new entorno3d();
             
             utilidades.recogerEtiquetas(e,utilidades.sentenciasGlobales, false,"");
             
             
             Console.WriteLine("se analizo el archivo");

             utilidades.ejecutarSentencias(e, utilidades.sentenciasGlobales);
             
             Console.WriteLine("reciguebdi etuqyetas");
            
             
        }

    }
}