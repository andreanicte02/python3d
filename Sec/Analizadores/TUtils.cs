using System;
using System.Collections.Generic;

namespace Sec.Analizadores
{


    public class GestorTemp
    {
        private static readonly List<string> _temporales = new List<string>();

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
    }

    
    
    public class TUtils
    {
        
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



        public static int espacioAlOrigen(ZEntorno entorno)
        {
            var total = 0;
            for (var temporal = entorno.entornoPadre; temporal != null; temporal = temporal.entornoPadre)
            {
                total += temporal.espacioEnMemoria;
            }

            return total;
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

        public static Codigo generarYDesenvolver(NodoAst nodoAst, ZEntorno entorno)
        {
            var codigo = nodoAst.GenerarCodigo(entorno);
            return desenvolver(codigo);
        }
        public static Codigo desenvolver(Codigo codigo)
        {
            if (codigo.tipo is ZReferencia zReferencia)
            {
                var c = new Codigo();
                c.append(codigo);
                c.tipo = zReferencia.tipo;
                
                c.apuntador = TUtils.generarTemporal();
                GestorTemp.agregarTemp(c.apuntador);
                GestorTemp.usarTemp(codigo.apuntador);
                c.append($"{c.apuntador} = {codigo.textoValor};");
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
                    
                    codigo.append(codigoDestino);
                    GestorTemp.usarTemp(codigoDestino.apuntador);

                    codigo.append(codigoValor);
                    GestorTemp.usarTemp(codigoValor.apuntador);

                    codigo.append($"{codigoDestino.textoValor} = {codigoValor.apuntador};");
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
                    
                codigo.append($"{codigo.apuntador} = P + {direccion};");
                codigo.comentar($"&{id}");

                codigo.textoValor = $"stack[{codigo.apuntador}]";
                codigo.extra = $"{id}";
                codigo.tipo = propiedad;

                return codigo;
            }
            
            throw new SemanticException($"var {id} no declarado");
        }
        
    }
}