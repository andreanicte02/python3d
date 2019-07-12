using System;
using System.Collections.Generic;
using System.IO;
using Irony.Parsing;

namespace Sec.Analizadores
{

    public class PythonSemanticException : ApplicationException
    {
        public PythonSemanticException(string message) : base(message)
        {
            
        }
    }
    
    public class PUtils
    {

        private static int contador_clases = 1;
        private static int contador_funciones = 1;

        public static int totalFunciones => contador_funciones - 1;
        
        public static int generarNumeroClase()
        {
            return contador_clases++;
        }
        public static int generarNumeroFunciones()
        {
            return contador_funciones++;
        }



        private static int contadorTemporal = 0;
        private static int contadorLabel = 0;
        public static string generarTemporal()
        {
            return $"p{contadorTemporal++}";
        }

        public static CodigoT desenvolver(CodigoT codigo)
        {
            if (!codigo.ambiguo)
            {
                return codigo;
            }
            
            var c = new CodigoT();
            c.apuntador = generarTemporal();
            c.agregar($"General {c.apuntador} = {codigo.valor}");
            return c;
        }


        
        public static CodigoT generarBloqueClase(string id, List<NodoPythonAst> sentencias, string sangria)
        {

            var entorno = new PEntorno();
            
            var codigo = new CodigoT();
            codigo.agregar($"General {id}()" + "{");
            codigo.agregar($"{sangria}General ambito = crearAmbito(ambitoAnterior);");
            
            
            var codigoFuncionesAnidadas = new CodigoT();
            var codigoBloque = generarBloque(sentencias, sangria, entorno);
            
            codigo.agregar(codigoBloque);
            codigo.agregar($"{sangria}return ambito;");
            codigo.agregar("}");
            
            
            foreach (var pFuncion in entorno.obtenerPFunciones())
            {
                codigo.agregar(pFuncion.GenerarCodigoFuncion());
            }
            foreach (var pClase in entorno.obtenerPClasese())
            {
                codigo.agregar(pClase.GenerarCodigoClase());
            }
            
//            codigo.agregar(codigoFuncionesAnidadas);

            return codigo;
        }

        public static string generarCodigoT(List<NodoPythonAst> sentencias)
        {
        
            var codigo = generarBloqueFuncion("python_main","", new List<string>(),  sentencias, "\t", "prueba");
            
            codigo.agregar("General invocar(General objetoFuncion, General[] parametros, int[] archivo, int linea, int columna){");
            
            codigo.agregar("\tif (objetoFuncion.tipo != 7){");
            codigo.agregar("\t\tthrow crearException(\"objeto no es una funcion\", archivo, linea, columna);");
            codigo.agregar("\t}");
            
            for (int i = 0; i < totalFunciones; i++)
            {
                codigo.agregar("\tif (objetoFuncion.infoFuncion.id == "+ i + "){");
//                codigo.agregar($"\tif(objetoFuncion.infoFuncion)" + "{");
//                codigo.agregar("}");
                codigo.agregar($"\t\treturn py_fun_{i}(objetoFuncion.infoFuncion.ambitoCapturado, parametros);");
                codigo.agregar("\t}");
            }
            
            codigo.agregar("}");

            return codigo.texto;
        }
        
        public static CodigoT generarBloqueFuncion(string id, string comentario, List<string> parametros, List<NodoPythonAst> sentencias, string sangria, string archivo)
        {
            var entorno = new PEntorno();

            var codigo = new CodigoT();
            codigo.agregar($"//{comentario}");
            codigo.agregar($"General {id}(General ambitoAnterior, General[] argumentos)" + "{");
            codigo.agregar(sangria +"int[] archivo = \"" + archivo + "\";");
            codigo.agregar($"{sangria}General ambito = crearAmbito(ambitoAnterior);");

            for (var i = 0 ; i < parametros.Count; i++)
            {
                codigo.agregar($"{sangria}registrar(ambito, \"{parametros[i]}\", argumentos[{i}]);");
            }
            
            var codigoBloque = generarBloque(sentencias, sangria, entorno);
            codigo.agregar(codigoBloque);
            
            codigo.agregar("\treturn crearVoidGeneral();");
            codigo.agregar("}");
            

            foreach (var pFuncion in entorno.obtenerPFunciones())
            {
                codigo.agregar(pFuncion.GenerarCodigoFuncion());
            }
            foreach (var pClase in entorno.obtenerPClasese())
            {
                codigo.agregar(pClase.GenerarCodigoClase());
            }
            

            return codigo;
        }
        
        public static CodigoT generarBloque(List<NodoPythonAst> sentencias, string sangria, PEntorno anterior)
        {
            var entorno = new PEntorno();

            var codigo = new CodigoT();
            
            // se generan todas las sentencias
            foreach (var sentencia in sentencias)
            {
                var c = sentencia.GenerarCodigo(entorno);
                c.prefix = sangria;
                codigo.agregar(c);
            }
            
            foreach (var pFuncion in entorno.obtenerPFunciones())
            {
                anterior.agregarFuncionPython(pFuncion);
            }
            foreach (var pClase in entorno.obtenerPClasese())
            {
                anterior.agregarClasePython(pClase);
            }
            
            return codigo;
        }
        
        
        /*
         *
         *
         * public static CodigoT generarBloque(string id, List<NodoPythonAst> sentencias, string sangria)
        {
            var entorno = new PEntorno();

            var codigo = new CodigoT();

            
            codigo.agregar($"void {id}()" + "{");
            codigo.agregar($"{sangria}General ambito = crearAmbito();");
            
            // se genera lo que no es funcion
            foreach (var sentencia in sentencias)
            {
                var c = sentencia.GenerarCodigo(entorno);
//                Console.WriteLine("Codigo generado s:");
//                Console.WriteLine(c.texto);
                c.prefix = sangria;
                codigo.agregar(c);
            }
            codigo.agregar("}");
            
            // se generan los metodos
            foreach (var pFuncion in entorno.obtenerPFunciones())
            {
                codigo.agregar(pFuncion.GenerarCodigoFuncion());
            }
            
            


            return codigo;
        }
        */
        
        
        public static string pruebaP()
        {
            // :v
            
            
            string entrada = File.ReadAllText(@"./../../prueba.py");
            
            var ga = new GramaticaPython();
            var parser = new Parser(ga);
                
            var arbol = parser.Parse(entrada);

            var raiz = arbol.Root;
                
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {
                foreach (var item in arbol.ParserMessages)
                {
                    Console.WriteLine("Error->"+item.Message+" Line:"+item.Location.Line + " Column: " + item.Location.Column);
                }

                return "";
            }
                
            Console.WriteLine("compilo todo bien");
            
            var sentencias = (List<NodoPythonAst>) raiz.AstNode;

//            var codigo = generarBloqueFuncion("python_main","", new List<string>(),  sentencias, "\t", "prueba");
//
//            return codigo.texto;

            return generarCodigoT(sentencias);
        }
        
        
        
    }


    public class PTipos
    {
        public static int tipoEntero = 1;
        public static int tipoDouble = 2;

    }
    
    
    
    
    
}