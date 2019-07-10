using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Irony.Parsing;
using Sec.Analizadores;

namespace Sec
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Console.WriteLine("holi");
            

            pruebaPython();
//            pruebaT();

        }

        public static void pruebaPython()
        {
            
            var entrada = @"

a = 3
b = a + 3
print(b)

";


            var ga = new GramaticaPython();
            var parser = new Parser(ga);
            
            var arbol = parser.Parse(entrada);

            var raiz = arbol.Root;
            
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                //---------------------> Hay Errores      

                foreach (var item in arbol.ParserMessages)
                {
//                    MessageBox.Show("Error->"+item.Message+" Line:"+item.Location.Line);
                    Console.WriteLine("Error->"+item.Message+" Line:"+item.Location.Line + " Column: " + item.Location.Column);

                }

                return;

            }
            
            Console.WriteLine("compilo todo bien");
            
//            Console.WriteLine(raiz.AstNode);

            var sentencias = (List<NodoPythonAst>) raiz.AstNode;

            var codigo = PUtils.generarBloqueFuncion("python_main", new List<string>(),  sentencias, "\t");
            
            Console.WriteLine(codigo.texto);
            
        }

        
        

        

        
        



    }
}