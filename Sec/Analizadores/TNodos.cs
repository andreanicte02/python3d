using System;
using System.Collections.Generic;
using System.Linq;

namespace Sec.Analizadores
{
    
    public interface NodoAst
    {
        Codigo GenerarCodigo(ZEntorno entorno);
    }

    public abstract class TNodo: NodoAst
    {
        public Codigo GenerarCodigo(ZEntorno entorno)
        {
            return IntentarGenerarCodigo(entorno);
        }
        
        //

        public abstract Codigo IntentarGenerarCodigo(ZEntorno entorno);
    }


    public class TNodoDeclaracionStruct: TNodo
    {
        private readonly string _id;
        private readonly List<NodoAst> _declaraciones;

        public TNodoDeclaracionStruct(string id, List<NodoAst> declaraciones)
        {
//            Console.WriteLine($"Se declarara struct {id}");
            _id = id;
            _declaraciones = declaraciones;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();
            
            var tipo = new ZStruct(entorno, _id, _declaraciones);
            entorno.declararTipo(_id, tipo);

            return codigo;
        }
    }

    public class TNodoThrow : TNodo
    {
        private readonly NodoAst _nodoExpr;

        public TNodoThrow(NodoAst nodoExpr)
        {
            _nodoExpr = nodoExpr;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {

            var codigoExpr = TUtils.desenvolver(_nodoExpr.GenerarCodigo(entorno));
            
            var codigo = new Codigo();

            var codigoHayError = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));
            
            // TODO: captura de linea
//            var codigoLinea = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
//            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));
//            
            // TODO: Captura de columna
//            var codigoHayError = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
//            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));

            var codigoValor = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.valor);
            var codigoAsignacionValor = TUtils.asignar(codigoValor, codigoExpr);
            
            codigo.agregar(codigoAsignacionHayError);
            codigo.agregar(codigoAsignacionValor);
            codigo.agregar($"goto {entorno.labelFin};");
            return codigo;
        }
    }

    public class TNodoBusquedaTipo : TNodo
    {
        private readonly string _id;

        public TNodoBusquedaTipo(string id)
        {
            _id = id;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var tipo = entorno.obtenerTipoDeclarado(_id);
            
            var codigo = new Codigo();
            codigo.tipo = tipo;
            return codigo;
        }
    }

    public class TNodoDeclaracionFuncion : TNodo
    {
        private readonly NodoAst _nodoTipoRetorno;
        private readonly string _id;
        private readonly List<NodoAst> _nodosParametro;
        private readonly List<NodoAst> _sentencias;

        public TNodoDeclaracionFuncion(
            NodoAst nodoTipoRetorno,
            string id,
            List<NodoAst> nodosParametro,
            List<NodoAst> sentencias
            )
        {
//            Console.WriteLine("SE declara funcion " + id);
            
            _nodoTipoRetorno = nodoTipoRetorno;
            _id = id;
            _nodosParametro = nodosParametro;
            _sentencias = sentencias;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipoRetorno = _nodoTipoRetorno.GenerarCodigo(entorno);
            
            var zFuncion = new ZFuncion(entorno, codigoTipoRetorno.tipo, _id, _nodosParametro, _sentencias);
            
            entorno.declararZFuncion(_id, zFuncion);

            return Codigo.vacio;
        }
    }
    


    public class ZFuncion
    {
        public ZEntorno TipoRetorno { get; }
        public const string RETURN = "return";
        
//        private readonly ZEntorno _tipoRetorno;
//        private readonly string _id;
        public string Id { get; }
        private readonly List<NodoAst> _sentencias;
        public ZEntorno Entorno { get; }

        public List<string> NombresParametro => Entorno.NombresPropiedad.Where(s => s != RETURN).ToList();

        public ZFuncion(ZEntorno entornoPadre, ZEntorno tipoRetorno, string id, List<NodoAst> nodosParametro, List<NodoAst> sentencias)
        {
            TipoRetorno = tipoRetorno;
            Entorno = new ZEntorno(entornoPadre);
            
            Entorno.declararPropiedad(RETURN, new ZReferencia(tipoRetorno));
                
            // se registran los parametros en el entorno de la funcion
            foreach (var nodoParametro in nodosParametro)
            {
                nodoParametro.GenerarCodigo(Entorno);
            }
            
            Id = id;
            _sentencias = sentencias;
        }

        public Codigo generarCodigoZFuncion()
        {
            Entorno.labelFin = TUtils.generarLabel();

            var codigo = new Codigo();
            codigo.agregar($"proc {Id} begin");

            foreach (var sentencia in _sentencias)
            {
                GestorTemp.limpiar();
                var codigoSentencia = sentencia.GenerarCodigo(Entorno);
                codigo.agregar(codigoSentencia);
            }
            
            codigo.agregar($"{Entorno.labelFin}:");
            codigo.agregar($"end");
            return codigo;
        }
    }

    public class TNodoAsignacion : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoAsignacion(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoDestino = _e1.GenerarCodigo(entorno);
            var codigoValor = _e2.GenerarCodigo(entorno);
            return TUtils.asignar(codigoDestino, codigoValor);
        }
    }

    public class TNodoReturn : TNodo
    {
        private readonly NodoAst _e1;

        public TNodoReturn(NodoAst e1 = null)
        {
            _e1 = e1;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {

            var codigo = new Codigo();
            
            if (_e1 != null)
            {
                var c1 = _e1.GenerarCodigo(entorno);
                var codigoAsignar = TUtils.asignar(TUtils.obtenerPropiedad(entorno, ZFuncion.RETURN), c1);
                codigo.agregar(codigoAsignar);
            }
            else
            {
                var c = new Codigo();
                c.tipo = ZTipos.tipoVoid;
                c.apuntador = "0";
                c.extra = "void";
                var codigoAsignar = TUtils.asignar(TUtils.obtenerPropiedad(entorno, ZFuncion.RETURN), c);
                codigo.agregar(codigoAsignar);
            }
            
            codigo.agregar($"goto {entorno.labelFin};");
            codigo.comentar($"return;");

            return codigo;
        }
    }
    

    public class TNodoDeclaracionVar : TNodo
    {
        private readonly NodoAst _nodoTipo;
        private readonly string _id;
        private readonly NodoAst _nodoExpr;


        public TNodoDeclaracionVar(NodoAst nodoTipo, string id): this(nodoTipo, id, null)
        {
        }
        public TNodoDeclaracionVar(NodoAst nodoTipo, string id, NodoAst nodoExpr)
        {
//            Console.WriteLine($"Se declarara var {id}");
            _nodoTipo = nodoTipo;
            _id = id;
            _nodoExpr = nodoExpr;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);
            var referencia = new ZReferencia(codigoTipo.tipo);
            entorno.declararPropiedad(_id, referencia);

            if (_nodoExpr != null)
            {
//                Console.WriteLine($"hay una asignacion a {_id}");
                
                return TUtils.asignar(
                    TUtils.obtenerPropiedad(entorno, _id),
                    _nodoExpr.GenerarCodigo(entorno)
                );
            }
            
            return Codigo.vacio;
        }
    }

    public class TNodoInvocar : TNodo
    {
        private readonly string _id;
        private readonly List<NodoAst> _nodosArgumento;

        public TNodoInvocar(string id, List<NodoAst> nodosArgumento)
        {
            _id = id;
            _nodosArgumento = nodosArgumento;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {

            var codigosArgumento = _nodosArgumento.Select(n => TUtils.desenvolver(n.GenerarCodigo(entorno))).ToList();
            return TUtils.invocar(entorno, _id, codigosArgumento);

        }
    }

    public class TNodoAccessoPunto : TNodo
    {
        private readonly NodoAst _nodoValor;
        private readonly string _id;

        public TNodoAccessoPunto(NodoAst nodoValor, string id)
        {
            _nodoValor = nodoValor;
            _id = id;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));
            
            return TUtils.accesoPunto(codigoValor, _id);
        }
    }
    
    public class TNodoBuscarPropiedad: TNodo
    {
        private readonly string _id;

        public TNodoBuscarPropiedad(string id)
        {
            _id = id;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            
            return TUtils.obtenerPropiedad(entorno, _id);
        }
    }



    public class TNodoObtenerEspacio : TNodo
    {
        private readonly NodoAst _nodoTipo;

        public TNodoObtenerEspacio(NodoAst nodoTipo)
        {
            _nodoTipo = nodoTipo;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);
            
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoInt;
            codigo.apuntador = $"{codigoTipo.tipo.espacioEnMemoria}";
            codigo.extra = $"sizeof {codigoTipo.tipo.Nombre}";
            return codigo;
        }
    }
    public class TNodoCasteo : TNodo
    {
//        private readonly NodoAst _nodoTipo;
        private readonly string _idTipo;
        private readonly NodoAst _e;

        public TNodoCasteo(string idTipo, NodoAst e)
        {
//            _nodoTipo = nodoTipo;
            _idTipo = idTipo;
            _e = e;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
//            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);
            var tipo = entorno.obtenerTipoDeclarado(_idTipo);
            
            var c = TUtils.desenvolver(_e.GenerarCodigo(entorno));
            
            var codigo = new Codigo();
            codigo.tipo = tipo;
            
            codigo.agregar(c);
            codigo.apuntador = c.apuntador;
            codigo.extra = $"(({codigo.tipo.Nombre}) {c.extra}";

            return codigo;
        }
    }
    
    public class TNodoReservarMemoria : TNodo
    {
        private readonly NodoAst _e1;

        public TNodoReservarMemoria(NodoAst e1)
        {
            _e1 = e1;
        }
        
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            if (c1.tipo != ZTipos.tipoInt)
            {
                throw new SemanticException($"Solo se puede reservar una cantidad entera de memoria");
            }
            
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoInt;
            
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            
            codigo.agregar(c1);
            codigo.agregar($"{codigo.apuntador} = H;");
            codigo.comentar($"captura de direccion");
            codigo.agregar($"H = H + {c1.apuntador}; ");
            codigo.comentar($"({c1.extra}) bloques reservados");

            codigo.extra = $"[({c1.extra}) bloques]";

            GestorTemp.usarTemp(c1.apuntador);
            
            return codigo;
        }
    }


    public class TNodoSuma : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoSuma(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var tipo = TUtils.expandirTipoNumerico(c1.tipo, c2.tipo);

            if (tipo == null)
            {
                throw new SemanticException($"Operador + no apto para '{c1.tipo.Nombre}' y '{c2.tipo.Nombre}' ");
            }
            
            var codigo = new Codigo();
            codigo.tipo = tipo;
            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.apuntador = TUtils.generarTemporal();

            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);
            GestorTemp.agregarTemp(codigo.apuntador);
            
            codigo.agregar($"{codigo.apuntador} = {c1.apuntador} + {c2.apuntador};");
            codigo.comentar($"{c1.extra} + {c2.extra}");
            codigo.extra = $"{c1.extra} + {c2.extra}";
            
            return codigo;

        }
    }

    public class TNodoCrearInt: TNodo
    {
        private readonly int _valor;

        public TNodoCrearInt(int valor)
        {
            _valor = valor;
        }

        public static Codigo crearCodigoInt(int valor)
        {
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoInt;
            codigo.apuntador = $"{valor}";
            codigo.extra = $"{valor}";
            return codigo;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            return crearCodigoInt(_valor);
        }
    }

    public class TNodoCrearDouble : TNodo
    {
        private readonly double _valor;

        public TNodoCrearDouble(double valor)
        {
            _valor = valor;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoDouble;
            codigo.apuntador = $"{_valor}";
            codigo.extra = $"{_valor}";
            return codigo;
        }
    }

    public class TNodoPrintSimple : TNodo
    {
        private readonly string _id;
        private readonly NodoAst _nodoValor;

        public TNodoPrintSimple(string id, NodoAst nodoValor)
        {
            _id = id;
            _nodoValor = nodoValor;

            switch (id)
            {
                case "c":
                case "e":
                case "d":
                    break;
                default:
                    throw new SemanticException($"Letra {id} no soportado para simplePrint");
            }
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {

            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            if (codigoValor.tipo != ZTipos.tipoInt && codigoValor.tipo != ZTipos.tipoDouble)
            {
                throw new SemanticException("Simple print solo soporta entero o double");
            }

            var codigo = new Codigo();
            codigo.agregar(codigoValor);
            codigo.agregar($"print(\"%{_id}\", {codigoValor.apuntador});");
            
            return codigo;
        }
    }

    

}    