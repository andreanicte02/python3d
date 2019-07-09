using System;
using System.Collections.Generic;
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
            

//            pruebaPython();
            pruebaT();

        }

        public static void pruebaPython()
        {
            
            var entrada = @"
hola = 2

def hola():
    a = 3

otro = hola

otro(1, 2)

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

            var codigo = PUtils.generarBloqueFuncion("python_main", sentencias, "\t");
            
            Console.WriteLine(codigo.texto);
            
        }

        
        

        public static void pruebaT()
        {
            var entrada = @"
                public int 

                int[] generar(){
	                int[] n = (int[]) #21#;
	                n[0] = 1993;
	                return n;
                }

                void printlnNumber(int i){
                    !print(e, i);
                    !print(c, 10);
                }

                void main(){
	                int[] numeros = generar();
	                printlnNumber(numeros[0]);
                }


";
            try
            {
                var codigo3d = TUtils.generarCodigo3D(entrada);
                Console.WriteLine(codigo3d);
            }
            catch (SemanticException semanticException)
            {
                Console.WriteLine("Error semantico en T:");
                Console.WriteLine(semanticException.Message);
            }
        }

        
        



    }
}