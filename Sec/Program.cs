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


            var entrada = @"

struct General {
    int valorEntero;
    int valorFloat;
    int tipo;
    General siguiente;
    General primero;   
}

void printlnNumber(int i){
    !print(e, i);
    !print(c, 10);
}

void main(){
    printlnNumber(1993);
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