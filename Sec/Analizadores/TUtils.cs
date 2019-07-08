using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Sec.Analizadores
{


    public class GestorTemp
    {
        private static readonly List<string> _temporales = new List<string>();
        public static int Cantidad => _temporales.Count;

        public static void agregarTemp(string temp)
        {
            _temporales.Add(temp);
        }

        public static void limpiar()
        {
            _temporales.Clear();
        }

        public static void usarTemp(string temp)
        {
            _temporales.Remove(temp);
        }

        public static string obtenerTemporal(int i)
        {
            return _temporales[i];
        }
    }

    
    
    public class TUtils
    {

        public static string generarDeclaracionesTemporales()
        {
            var result = "var t0";
            for (var i = 1; i < contadorTemporal; i++)
            {
                if (i % 20 == 0)
                {
                    result += "\n";
                }
                result += $", t{i}";
            }
            return result + ";";
        }
        
        private static int contadorTemporal = 0;
        private static int contadorLabel = 0;
        public static string generarTemporal()
        {
            return $"t{contadorTemporal++}";
        }

        public static string generarLabel()
        {
            return $"L{contadorLabel++}";
        }


        public static Codigo generarCodigoBloque(List<NodoAst> sentencias, ZEntorno entorno)
        {
            var codigo = new Codigo();
            foreach (var sentencia in sentencias)
            {
                GestorTemp.limpiar();
                var codigoSentencia = sentencia.GenerarCodigo(entorno);
                codigo.agregar(codigoSentencia);
            }

            return codigo;
        }


        public static int espacioAlOrigen(ZEntorno entorno)
        {
            var total = 0;
            for (var temporal = entorno.entornoPadre; temporal != null; temporal = temporal.entornoPadre)
            {
                total += temporal.espacioEnMemoria;
            }

            return total;
        }

        public static bool esNumerico(ZEntorno e1)
        {
            return e1 == ZTipos.tipoInt || e1 == ZTipos.tipoDouble;
        }

        public static ZEntorno expandirTipoNumerico(ZEntorno e1, ZEntorno e2)
        {
            if (e1 == ZTipos.tipoInt && e2 == ZTipos.tipoInt)
            {
                return ZTipos.tipoInt;
            }
            if (e1 == ZTipos.tipoInt && e2 == ZTipos.tipoDouble)
            {
                return ZTipos.tipoDouble;
            }
            if (e1 == ZTipos.tipoDouble && e2 == ZTipos.tipoDouble)
            {
                return ZTipos.tipoDouble;
            }
            if (e1 == ZTipos.tipoDouble && e2 == ZTipos.tipoInt)
            {
                return ZTipos.tipoDouble;
            }

            return null;
        }

//        public static Codigo generarYDesenvolver(NodoAst nodoAst, ZEntorno entorno)
//        {
//            var codigo = nodoAst.GenerarCodigo(entorno);
//            return desenvolver(codigo);
//        }


        public static Codigo invocar(ZEntorno entorno, string id, List<Codigo> codigosArgumento)
        {

            var zFuncion = entorno.obtenerZFuncionDeclarado(id);
            
            if (zFuncion.NombresParametro.Count != codigosArgumento.Count)
            {
                throw new SemanticException($"Funcion toma {zFuncion.NombresParametro.Count} parametros no {codigosArgumento.Count}");
            }
            
            // TODO validacion de tipos?


            var codigo = new Codigo();
            codigo.tipo = zFuncion.TipoRetorno;
            codigo.extra = $"{id}({string.Join(", ", codigosArgumento.Select(c => c.extra))})";

            foreach (var codigoArgumento in codigosArgumento)
            {
                GestorTemp.usarTemp(codigoArgumento.apuntador);
                codigo.agregar(codigoArgumento);
            }

            var temporalesNoUtilizados = GestorTemp.Cantidad;

            // Backup de temporales no utilizados
            for (var i = 0; i < temporalesNoUtilizados; i++)
            {
                var temporal = TUtils.generarTemporal();
                codigo.agregar($"{temporal} = P + {espacioAlOrigen(entorno) + entorno.espacioEnMemoria + i};");
                codigo.comentar($"& Backup {GestorTemp.obtenerTemporal(i)}");
                
                codigo.agregar($"stack[{temporal}] = {GestorTemp.obtenerTemporal(i)};");
                codigo.comentar($"Backup {GestorTemp.obtenerTemporal(i)}");
            }
            
            // cambio de ambito
            
            codigo.agregar($"P = P + {espacioAlOrigen(entorno) + entorno.espacioEnMemoria + temporalesNoUtilizados};");
            codigo.comentar($"Cambio de ambito");

            // paso de parametros
            if (codigosArgumento.Count > 0)
            {
                for (int i = 0; i < zFuncion.NombresParametro.Count; i++)
                {
                    var codigoDestino = obtenerPropiedad(zFuncion.Entorno, zFuncion.NombresParametro[i]);
                    var codigoValor = codigosArgumento[i].copiaApuntador();
                    
                    var codigoAsignacion = asignar(codigoDestino, codigoValor);

                    codigo.agregar(codigoAsignacion);
                    codigo.agregar("//");
                    codigo.comentar($"{id}:{zFuncion.NombresParametro[i]} = {codigosArgumento[i].extra}");
                }
            }

            // invocacion
            codigo.agregar($"call {zFuncion.Id};");
            codigo.comentar($"call {zFuncion.Id}");
            
            // captura de retorno
            if (zFuncion.TipoRetorno != ZTipos.tipoVoid)
            {
                var codigoRetorno = desenvolver(obtenerPropiedad(zFuncion.Entorno, ZFuncion.RETURN));
                codigo.agregar(codigoRetorno);
                codigo.apuntador = codigoRetorno.apuntador;
            }
            
            
            // restauracion de ambito
            codigo.agregar($"P = P - {espacioAlOrigen(entorno) + entorno.espacioEnMemoria + temporalesNoUtilizados};");
            codigo.comentar($"Ambito restaurado");
            
            // se verifica si hay una exception
            var codigoHayError = desenvolver(TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError));
            codigo.agregar(codigoHayError);
            codigo.agregar($"if ({codigoHayError.apuntador} != 0) goto {entorno.labelFin};");
            codigo.comentar("chequeo de exception");
            GestorTemp.usarTemp(codigoHayError.apuntador);


            // recupero temporales
            // Backup de temporales no utilizados
            for (var i = 0; i < temporalesNoUtilizados; i++)
            {

                codigo.agregar($"{GestorTemp.obtenerTemporal(i)} = P + {espacioAlOrigen(entorno) + entorno.espacioEnMemoria + i};");
                codigo.comentar($"& Backup {GestorTemp.obtenerTemporal(i)}");
                
                codigo.agregar($"{GestorTemp.obtenerTemporal(i)} = stack[{GestorTemp.obtenerTemporal(i)}];");
                codigo.comentar($" {GestorTemp.obtenerTemporal(i)} = Backup {GestorTemp.obtenerTemporal(i)}");
            }

            return codigo;
        }

        public static Codigo desenvolver(Codigo codigo)
        {
            if (codigo.tipo is ZReferencia zReferencia)
            {
                var c = new Codigo();
                c.agregar(codigo);
                c.tipo = zReferencia.tipo;
                
                c.apuntador = TUtils.generarTemporal();
                GestorTemp.agregarTemp(c.apuntador);
                GestorTemp.usarTemp(codigo.apuntador);
                c.agregar($"{c.apuntador} = {codigo.textoValor};");
                c.comentar($"{codigo.extra}");
                c.extra = codigo.extra;

                return c;                
            }


            return codigo;
        }

        public static Codigo asignar(Codigo codigoDestino, Codigo codigoValor)
        {
            codigoValor = desenvolver(codigoValor);

            if (codigoDestino.tipo is ZReferencia zReferencia)
            {
                if (zReferencia.tipo == codigoValor.tipo)
                {
                    var codigo = new Codigo();
                    
                    codigo.agregar(codigoDestino);
                    GestorTemp.usarTemp(codigoDestino.apuntador);

                    codigo.agregar(codigoValor);
                    GestorTemp.usarTemp(codigoValor.apuntador);

                    codigo.agregar($"{codigoDestino.textoValor} = {codigoValor.apuntador};");
                    codigo.comentar($"{codigoDestino.extra} = {codigoValor.extra}");
                    
                    return codigo;
                }
                else
                {
                    throw new SemanticException($"No se puede guardar de tipo {codigoValor.tipo.Nombre} en variable de tipo {zReferencia.tipo.Nombre} ");
                }
            }
            else
            {
                throw new SemanticException($"lado izquierdo no es una referencia");
            }
            
        }

        public static Codigo accesoPunto(Codigo codigoValor, string id)
        {

            var (propiedad, direccion) = codigoValor.tipo.obtenerPropiedadConDireccionRelativa(id);
            

            var codigo = new Codigo();
            
            codigo.tipo = propiedad;
            codigo.apuntador = generarTemporal();
            codigo.extra = $"{codigoValor.extra}.{id}";
            
            codigo.agregar(codigoValor);
            GestorTemp.usarTemp(codigoValor.apuntador);
            GestorTemp.agregarTemp(codigo.apuntador);
            
            codigo.agregar($"{codigo.apuntador} = {codigoValor.apuntador} + {direccion};");
            codigo.comentar($"*({codigo.extra})");
            codigo.textoValor = $"heap[{codigo.apuntador}]";

            return codigo;
        }
        
        
        public static Codigo obtenerPropiedad(ZEntorno entorno, string id)
        {
            for (var temporal = entorno; temporal != null; temporal = temporal.entornoPadre)
            {
                if (!temporal.tienePropiedad(id)) continue;
                
                var espacioUsado = espacioAlOrigen(temporal);

                var (propiedad, direccionRelativa) = temporal.obtenerPropiedadConDireccionRelativa(id);
                
//                Console.WriteLine($" {id} en {direccionRelativa} ");
//                Console.WriteLine($" espacioUsado: {espacioUsado} ");

                var codigo = new Codigo();
                codigo.apuntador = TUtils.generarTemporal();
                    
                GestorTemp.agregarTemp(codigo.apuntador);

                var direccion = espacioUsado + direccionRelativa;
                    
                codigo.agregar($"{codigo.apuntador} = P + {direccion};");
                codigo.comentar($"&{id}");

                codigo.textoValor = $"stack[{codigo.apuntador}]";
                codigo.extra = $"{id}";
                codigo.tipo = propiedad;

                return codigo;
            }
            
            throw new SemanticException($"var {id} no declarado");
        }

        public static Codigo textoComoPrints(string texto)
        {
            var codigo = new Codigo();

            for (int i = 0; i < texto.Length; i++)
            {
                codigo.agregar($"print(\"%c\", {(int) texto[i]});");
            }

            return codigo;
        }

        public static string generarCodigo3D(string entrada)
        {
            var ga = new GramaticaT();
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

                return null;
            }

            return generarCodigo3D((List<NodoAst>) raiz.AstNode);

        }
        
        public static string generarCodigo3D(List<NodoAst> sentencias){
            
            
            var entornoGlobal = new ZEntorno();
            entornoGlobal.declararTipo("int", ZTipos.tipoInt);
            entornoGlobal.declararTipo("double", ZTipos.tipoDouble);
            entornoGlobal.declararTipo("void", ZTipos.tipoVoid);

            foreach (var sentencia in sentencias)
            {
                sentencia.GenerarCodigo(entornoGlobal);
            }

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
            
            codigo.agregar("//=======================================");
//            codigo.comentar("========================");
            codigo.agregar($"H = {ZTipos.tipoDollar.espacioEnMemoria};");
            codigo.agregar("call main;");
            
            var codigoHayError = TUtils.desenvolver(TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError));
            codigo.agregar(codigoHayError);
            codigo.agregar(TUtils.textoComoPrints("\n>> Finalizado con codigo: "));
            codigo.agregar($"print(\"%e\", {codigoHayError.apuntador});");


            var codigoFinal = new Codigo();
            codigoFinal.agregar($"var H, P;");
            codigoFinal.agregar(TUtils.generarDeclaracionesTemporales());
            codigoFinal.agregar(codigo);
            
//            Console.WriteLine(codigoFinal.texto);

            return codigoFinal.texto;

        }
        
    }
}