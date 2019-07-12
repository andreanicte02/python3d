using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Irony.Parsing;

namespace Sec.Analizadores
{
    public class AanalizadorAltoNivel
    {
        public AanalizadorAltoNivel()
        {
            
        }

        public void probarAnalizadorAltoNivel(String entrada)
        {
            
            GramaticaPython ga = new GramaticaPython();
            Parser parser = new Parser(ga);

            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;

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
            
            
        }

        public void graficar(String entrada)
        {
            GramaticaPython ga = new GramaticaPython();
            Parser parser = new Parser(ga);

            ParseTree arbol = parser.Parse(entrada);
            ParseTreeNode raiz = arbol.Root;

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
            
            Graficador g = new Graficador();
            g.graficar(raiz);
        }


    }
    
     public class Graficador
     {
        private int index;

        //private readonly string rutaExeDot = "C:\\Program Files (x86)\\Graphviz\\bin\\dot.exe";

   

        public void graficar(ParseTreeNode nodo)
        {
            StreamWriter archivo = new StreamWriter("ArbolSintactico.dot");
            string contenido = "graph G {";
            contenido += "node [shape = egg];";
            index = 0;
            definirNodos(nodo, ref contenido);
            index = 0;
            enlazarNodos(nodo, 0, ref contenido);
            contenido += "}";
            archivo.Write(contenido);
            archivo.Close();
            //DialogResult verImagen = MessageBox.Show("¿Desea visualizar el AST de la cadena ingresada?", "Grafica AST", MessageBoxButtons.YesNo);
         


        }

      
        public void definirNodos(ParseTreeNode nodo, ref string contenido)
        {
            if (nodo != null)
            {
                contenido += "node" + index.ToString() + "[label = \"" + nodo.ToString() + "\", style = filled, color = lightblue];";
                index++;

                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    definirNodos(hijo, ref contenido);
                }
            }
        }
        
        public void enlazarNodos(ParseTreeNode nodo, int actual, ref string contenido)
        {
            if (nodo != null)
            {
                foreach (ParseTreeNode hijo in nodo.ChildNodes)
                {
                    index++;
                    contenido += "\"node" + actual.ToString() + "\"--" + "\"node" + index.ToString() + "\"";
                    enlazarNodos(hijo, index, ref contenido);
                }
            }
        }


    }
     
     
     
}