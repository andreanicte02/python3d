using System;
using System.Collections.Generic;

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

        private static int contador_clases = 0;
        private static int contador_funciones = 0;

        public static int generarNumeroClase()
        {
            return contador_clases++;
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
            c.agregar($"General {c.apuntador} = {codigo.valor};");
            return c;
        }


        
        public static CodigoT generarBloqueClase(string id, List<NodoPythonAst> sentencias, string sangria)
        {


            var codigo = new CodigoT();
            codigo.agregar($"General {id}()" + "{");
            codigo.agregar($"{sangria}General ambito = crearAmbito();");
            
            
            var codigoFuncionesAnidadas = new CodigoT();
            var codigoBloque = generarBloque(sentencias, sangria, codigoFuncionesAnidadas);
            
            codigo.agregar(codigoBloque);
            codigo.agregar($"{sangria}return ambito;");
            codigo.agregar("}");
            
            
            
            codigo.agregar(codigoFuncionesAnidadas);

            return codigo;
        }
        
        public static CodigoT generarBloqueFuncion(string id, List<NodoPythonAst> sentencias, string sangria)
        {
//            var entorno = new PEntorno();

            var codigo = new CodigoT();
            codigo.agregar($"void {id}()" + "{");
            codigo.agregar($"{sangria}General ambito = crearAmbito();");
            
            
            var codigoFuncionesAnidadas = new CodigoT();
            var codigoBloque = generarBloque(sentencias, sangria, codigoFuncionesAnidadas);
            
            codigo.agregar(codigoBloque);
            codigo.agregar("}");
            
            // se generan los metodos
//            foreach (var pFuncion in entorno.obtenerPFunciones())
//            {
//                codigo.agregar(pFuncion.GenerarCodigoFuncion());
//            }
            
            codigo.agregar(codigoFuncionesAnidadas);

            return codigo;
        }
        
        public static CodigoT generarBloque(List<NodoPythonAst> sentencias, string sangria, CodigoT codigoFuncionesAnidadas)
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

            
            // se generan los metodos
            foreach (var pFuncion in entorno.obtenerPFunciones())
            {
                codigoFuncionesAnidadas.agregar(pFuncion.GenerarCodigoFuncion());
            }
            
            // se generan los metodos
            foreach (var pClase in entorno.obtenerPClasese())
            {
                codigoFuncionesAnidadas.agregar(pClase.GenerarCodigoClase());
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
        
        
        
    }


    public class PTipos
    {
        public static int tipoEntero = 1;
        public static int tipoDouble = 2;

    }
    
    
    
}