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

//            dynamic a = 3;
//            dynamic b = 4;
//            sumar(a, b);

            Console.WriteLine("holi");
            
            var ga = new GramaticaT();
            var parser = new Parser(ga);

            var entrada = @"

int malloc(int size){
    return #size#;
}

void printNumero(int i){
    !print(e, i);
}

void main(){
    sumar(2,3);
}
int sumar(int a, int b){
    throw 1993;
}

";
            
            
            var entornoGlobal = new ZEntorno();
            entornoGlobal.declararTipo("int", ZTipos.tipoInt);
            entornoGlobal.declararTipo("double", ZTipos.tipoDouble);
            entornoGlobal.declararTipo("void", ZTipos.tipoVoid);
            
            

            
            
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
            
            
            Console.WriteLine("Se genero el arbol");

            var sentencias = (List<NodoAst>) arbol.Root.AstNode;

            foreach (var sentencia in sentencias)
            {
                sentencia.GenerarCodigo(entornoGlobal);
            }
            
//            entornoGlobal.imprimirTipos();
            
            
            // se procesan los tipos
            foreach (var tipo in entornoGlobal.obtenerTipos())
            {
                if (tipo is ZStruct zStruct)
                {
                    zStruct.declararMiembros();
                }
            }

            var codigo = new Codigo();
            foreach (var zFuncion in entornoGlobal.obtenerFunciones())
            {
                var c = zFuncion.generarCodigoZFuncion();
                codigo.agregar(c);
            }

            // se verifica que exista un main
            entornoGlobal.obtenerZFuncionDeclarado("main");
            
            codigo.agregar("//=====================");
            codigo.comentar("========================");
            codigo.agregar($"H = {ZTipos.tipoDollar.espacioEnMemoria};");
            codigo.agregar("call main;");
            
            var codigoHayError = TUtils.desenvolver(TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError));
            codigo.agregar(codigoHayError);
            codigo.agregar(TUtils.textoComoPrints(">> Finalizado con codigo: "));
            codigo.agregar($"print(\"%e\", {codigoHayError.apuntador};");
            
            
            Console.WriteLine(codigo.texto);
            

            Console.WriteLine("fin");


//            Zide f = new Zide();
//            Application.EnableVisualStyles();
//            Application.Run(f);
        }

//        public static void sumar(int a, int b)
//        {
//            Console.WriteLine(a + b);
//        }
        
    }
}