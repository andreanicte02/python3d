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

otro()

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
int ackermann(long m,long n){
    if(m == 0){
        return (n + 1);
    }else if(m > 0 && n == 0){
        return ackermann(m - 1, 1);
    }else{
        return ackermann(m - 1, ackermann(m, n - 1));
    }
}


void main(){
    Vector v1 = (Vector) malloc(@Vector);
    v1.x = 9;
    v1.y = 8;
    v1.z = 7;
    Vector v2 = (Vector) malloc(@Vector);
    v2.z = 6;
    v2.y = 5;
    v2.x = 4;
    printNumero(v1.x + v2.z);
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