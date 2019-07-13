using System;
using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Sec.Analizadores
{

    public interface NodoAst
    {
        Codigo GenerarCodigo(ZEntorno entorno);

    }

    public abstract class TNodo : NodoAst
    {
        public SourceLocation location;

        public Codigo GenerarCodigo(ZEntorno entorno)
        {
            try
            {
                return IntentarGenerarCodigo(entorno);
            }
            catch (SemanticException e)
            {
                e.Location = this.location;
                throw;
            }
        }


        //

        public abstract Codigo IntentarGenerarCodigo(ZEntorno entorno);
    }


    public class TNodoDeclaracionStruct : TNodo
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



            // TODO: captura de linea
//            var codigoLinea = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
//            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));
//            
            // TODO: Captura de columna
//            var codigoHayError = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
//            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));

            var codigoValor = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.valor);
            var codigoAsignacionValor = TUtils.asignar(codigoValor, codigoExpr, true);

            codigo.agregar(codigoAsignacionValor);
            
            var codigoHayError = TUtils.accesoPunto(ZTipos.codigoDollar, PropiedadDollar.hayError);
            var codigoAsignacionHayError = TUtils.asignar(codigoHayError, TNodoCrearInt.crearCodigoInt(1));
            codigo.agregar(codigoAsignacionHayError);
            
            codigo.agregar($"goto {entorno.labelFin};");
            return codigo;
        }
    }


//    public static Dictio


    public class TNodoTipoCompuesto : TNodo
    {
        private readonly NodoAst _nodoTipo;

        public TNodoTipoCompuesto(NodoAst nodoTipo)
        {
            _nodoTipo = nodoTipo;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);


            var codigo = new Codigo();
            codigo.tipo = TUtils.obtenerTipoDesdeBancoArray(codigoTipo.tipo);
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

//        public List<string> NombresParametro => Entorno.NombresPropiedad.Where(s => s != RETURN).ToList();
        public List<string> NombresParametro { get; }


        public ZFuncion(ZEntorno entornoPadre, ZEntorno tipoRetorno, string id, List<NodoAst> nodosParametro,
            List<NodoAst> sentencias)
        {
            TipoRetorno = tipoRetorno;
            Entorno = new ZEntorno(entornoPadre);

            Entorno.declararPropiedad(RETURN, new ZReferencia(tipoRetorno));

            // se registran los parametros en el entorno de la funcion
            foreach (var nodoParametro in nodosParametro)
            {
                nodoParametro.GenerarCodigo(Entorno);
            }

            NombresParametro = Entorno.NombresPropiedad.Where(s => s != RETURN).ToList();

            Id = id;
            _sentencias = sentencias;
        }

        public Codigo generarCodigoZFuncion()
        {
            Entorno.labelFin = TUtils.generarLabel();

            var codigo = new Codigo();
            codigo.agregar($"proc {Id} begin");

            var codigoBloque = TUtils.generarCodigoBloque(_sentencias, Entorno);
            codigo.agregar(codigoBloque);

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


    public class TNodoBreak : TNodo
    {
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            if (string.IsNullOrEmpty(entorno.labelLoopEnd)) {
                throw new SemanticException($"sentencia 'break' no esta dentro ningun bucle");
            }

            var code = new Codigo();
            code.agregar($"goto {entorno.labelLoopEnd};");
            code.comentar($"break --> {entorno.labelLoopEnd}");

            return code;
        }
    }

    public class TNodoContinue : TNodo
    {
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            if (string.IsNullOrEmpty(entorno.labelLoopBegin)) {
                throw new SemanticException($"sentencia 'continue' no esta dentro ningun bucle");
            }

            var code = new Codigo();
            code.agregar($"goto {entorno.labelLoopBegin};");
            code.comentar($"continue --> {entorno.labelLoopBegin}");

            return code;
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


            var codigoReturn = TUtils.obtenerPropiedad(entorno, ZFuncion.RETURN);
            Codigo c1 = null;

            if (_e1 != null)
            {
                c1 = _e1.GenerarCodigo(entorno);
            }
            else
            {
                c1 = new Codigo();
                c1.tipo = ZTipos.tipoVoid;
                c1.apuntador = "0";
                c1.extra = "void";
            }

            var codigoAsignar = TUtils.asignar(codigoReturn, c1);
            codigo.agregar(codigoAsignar);

            codigo.agregar($"goto {entorno.labelFin};");
            codigo.comentar($"return;");

            entorno.TieneReturn = true;

            return codigo;
        }
    }


    public class TNodoDeclaracionVar : TNodo
    {
        private readonly NodoAst _nodoTipo;
        private readonly string _id;
        private readonly NodoAst _nodoExpr;


        public TNodoDeclaracionVar(NodoAst nodoTipo, string id) : this(nodoTipo, id, null)
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

    public class TNodoObtenerDollar : TNodo
    {
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            return ZTipos.codigoDollar;
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

    public class TNodoBuscarPropiedad : TNodo
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

    public class TNodoCasteo2 : TNodo
    {
        private readonly NodoAst _nodoTipo;
        private readonly NodoAst _e;

        public TNodoCasteo2(NodoAst nodoTipo, NodoAst e)
        {
            _nodoTipo = nodoTipo;
            _e = e;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);
//            var tipo = entorno.obtenerTipoDeclarado(_idTipo);

            var c = TUtils.desenvolver(_e.GenerarCodigo(entorno));

            var codigo = new Codigo();
            codigo.tipo = codigoTipo.tipo;

            codigo.agregar(c);
            codigo.apuntador = c.apuntador;
            codigo.extra = $"(({codigo.tipo.Nombre}) {c.extra}";

            return codigo;
        }
    }

    public class TNodoCasteoInt : TNodo
    {

        private readonly NodoAst _e;

        public TNodoCasteoInt(NodoAst e)
        {
            _e = e;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            
            var c = TUtils.desenvolver(_e.GenerarCodigo(entorno));
            
            var codigo = new Codigo();
            
            if (c.tipo == ZTipos.tipoDouble) {

                codigo.agregar(c.texto);

                var temp1 = TUtils.generarTemporal();
                GestorTemp.usarTemp(c.apuntador);
                codigo.agregar($"{temp1} = {c.apuntador} % 1;");

                codigo.apuntador = TUtils.generarTemporal();
                codigo.agregar($"{codigo.apuntador} = {c.apuntador} - {temp1};");

                GestorTemp.agregarTemp(codigo.apuntador);

                codigo.extra = $"(int) {c.extra}";
                codigo.tipo = ZTipos.tipoInt;

            }
            else
            {
                codigo.tipo = ZTipos.tipoInt;
                codigo.agregar(c);
                codigo.apuntador = c.apuntador;
                codigo.extra = $"(({codigo.tipo.Nombre}) {c.extra}";
            }
            




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

    public class TNodoCrearString : TNodo
    {
        private readonly string _valor;

        public TNodoCrearString(string valor)
        {
            _valor = valor;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c =
                TNodoCrearArray.crearArray(entorno, ZTipos.tipoInt, TNodoCrearInt.crearCodigoInt(_valor.Length));

            var temp = TUtils.generarTemporal();

            for (var i = 0; i < _valor.Length; i++)
            {
                c.agregar($"{temp} = {c.apuntador} + {1 + i};");
                var charCode = (int) _valor[i];
                c.agregar($"heap[{temp}] = {charCode};");
            }

            return c;
        }
    }


    public class TNodoCrearArray : TNodo
    {
        private readonly NodoAst _nodoTipo;
        private readonly NodoAst _nodoValor;

        public TNodoCrearArray(NodoAst nodoTipo, NodoAst nodoValor)
        {
            _nodoTipo = nodoTipo;
            _nodoValor = nodoValor;
        }

        public static Codigo crearArray(ZEntorno entorno, ZEntorno tipo, Codigo codigoValor)
        {
//            var tipo = _nodoTipo.GenerarCodigo(entorno).tipo;
//            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            if (codigoValor.tipo != ZTipos.tipoInt)
            {
                throw new SemanticException($"Solo se puede reservar una cantidad entera de memoria");
            }

            var codigo = new Codigo();
            codigo.tipo = TUtils.obtenerTipoDesdeBancoArray(tipo);

            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);

            codigo.agregar(codigoValor);
            codigo.agregar($"{codigo.apuntador} = H;");
            codigo.comentar($"captura de direccion");

            var temp = TUtils.generarTemporal();
            codigo.agregar($"{temp} = {codigoValor.apuntador} + 1;");
            codigo.comentar("array.legth espacio");
            codigo.agregar($"H = H + {temp}; ");
            codigo.comentar($"({codigoValor.extra}) bloques reservados");

            codigo.agregar($"heap[{codigo.apuntador}] = {codigoValor.apuntador};");

            codigo.extra = $"[new {tipo.Nombre}({codigoValor.extra}) ]";

            GestorTemp.usarTemp(codigoValor.apuntador);

            return codigo;
        }


        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var tipo = _nodoTipo.GenerarCodigo(entorno).tipo;
            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            return crearArray(entorno, tipo, codigoValor);

//            if (codigoValor.tipo != ZTipos.tipoInt)
//            {
//                throw new SemanticException($"Solo se puede reservar una cantidad entera de memoria");
//            }
//            
//            var codigo = new Codigo();
//            codigo.tipo = TUtils.obtenerTipoDesdeBancoArray(tipo);
//            
//            codigo.apuntador = TUtils.generarTemporal();
//            GestorTemp.agregarTemp(codigo.apuntador);
//            
//            codigo.agregar(codigoValor);
//            codigo.agregar($"{codigo.apuntador} = H;");
//            codigo.comentar($"captura de direccion");
//
//            var temp = TUtils.generarTemporal();
//            codigo.agregar($"{temp} = {codigoValor.apuntador} + 1;");
//            codigo.comentar("array.legth espacio");
//            codigo.agregar($"H = H + {temp}; ");
//            codigo.comentar($"({codigoValor.extra}) bloques reservados");
//            
//            codigo.agregar($"{codigo.apuntador} = {codigoValor.apuntador};");
//
//            codigo.extra = $"[new {tipo.Nombre}({codigoValor.extra}) ]";
//
//            GestorTemp.usarTemp(codigoValor.apuntador);
//            
//            return codigo;
        }
    }


    public class TNodoCrearInstancia : TNodo
    {
        private readonly NodoAst _nodoTipo;

        public TNodoCrearInstancia(NodoAst nodoTipo)
        {
            _nodoTipo = nodoTipo;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var tipo = _nodoTipo.GenerarCodigo(entorno).tipo;

            var codigo = new Codigo();
            codigo.tipo = tipo;

            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);

            codigo.agregar($"{codigo.apuntador} = H;");
            codigo.comentar($"captura de direccion");
            codigo.agregar($"H = H + {tipo.espacioEnMemoria}; ");
            codigo.comentar($"(new {tipo.Nombre})");

            codigo.extra = $"new {tipo.Nombre}";


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


    public class TNodoMenor : TNodoRelacional
    {
        public TNodoMenor(NodoAst e1, NodoAst e2) : base(e1, e2, "<")
        {
        }
    }

    public class TNodoMenorIgual : TNodoRelacional
    {
        public TNodoMenorIgual(NodoAst e1, NodoAst e2) : base(e1, e2, "<=")
        {
        }
    }


    public class TNodoMayor : TNodoRelacional
    {
        public TNodoMayor(NodoAst e1, NodoAst e2) : base(e1, e2, ">")
        {
        }
    }

    public class TNodoMayorIgual : TNodoRelacional
    {
        public TNodoMayorIgual(NodoAst e1, NodoAst e2) : base(e1, e2, ">=")
        {
        }
    }



//    TNodoMayor

    public class TNodoRelacional : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;
        private readonly string _simbolo;

        public TNodoRelacional(NodoAst e1, NodoAst e2, string simbolo)
        {
            _e1 = e1;
            _e2 = e2;
            _simbolo = simbolo;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var codigo = new Codigo();


            if (!TUtils.esNumerico(c1.tipo) || !TUtils.esNumerico(c2.tipo))
            {
                throw new SemanticException($"{_simbolo} solo puede ser comparado con numerico");
            }


            var labelFin = TUtils.generarLabel();

            codigo.tipo = ZTipos.tipoInt;
            codigo.extra = $"{c1.extra} {_simbolo} {c2.extra}";
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);

            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.agregar($"{codigo.apuntador} = 1;");
            codigo.comentar($"...");

            codigo.agregar($"if ({c1.apuntador} {_simbolo} {c2.apuntador}) goto {labelFin};");
            codigo.comentar($"{c1.extra} {_simbolo} {c2.extra}");

            codigo.agregar($"{codigo.apuntador} = 0;");
            codigo.comentar("...");

            codigo.agregar($"{labelFin}:");
            codigo.comentar("..");

            return codigo;
        }
    }




    public class TNodoOr : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoOr(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var codigo = new Codigo();


            if (!TUtils.esNumerico(c1.tipo) || !TUtils.esNumerico(c2.tipo))
            {
                throw new SemanticException("&& solo puede ser comparado con numerico");
            }


            var labelFin = TUtils.generarLabel();

            codigo.tipo = ZTipos.tipoInt;
            codigo.extra = $"{c1.extra} || {c2.extra}";
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);

            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.agregar($"{codigo.apuntador} = 1;");
            codigo.comentar($"...");

            codigo.agregar($"if ({c1.apuntador} != 0) goto {labelFin};");
            codigo.comentar($"{c1.extra} is false");

            codigo.agregar($"if ({c2.apuntador} != 0) goto {labelFin};");
            codigo.comentar($"{c2.extra} is false");

            codigo.agregar($"{codigo.apuntador} = 0;");
            codigo.comentar("...");

            codigo.agregar($"{labelFin}:");
            codigo.comentar("..");

            return codigo;
        }
    }

    public class TNodoAnd : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoAnd(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var codigo = new Codigo();


            if (!TUtils.esNumerico(c1.tipo) || !TUtils.esNumerico(c2.tipo))
            {
                throw new SemanticException("&& solo puede ser comparado con numerico");
            }


            var labelFin = TUtils.generarLabel();

            codigo.tipo = ZTipos.tipoInt;
            codigo.extra = $"{c1.extra} && {c2.extra}";
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);

            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.agregar($"{codigo.apuntador} = 0;");
            codigo.comentar($"...");

            codigo.agregar($"if ({c1.apuntador} == 0) goto {labelFin};");
            codigo.comentar($"{c1.extra} is false");

            codigo.agregar($"if ({c2.apuntador} == 0) goto {labelFin};");
            codigo.comentar($"{c2.extra} is false");

            codigo.agregar($"{codigo.apuntador} = 1;");
            codigo.comentar("...");

            codigo.agregar($"{labelFin}:");
            codigo.comentar("..");

            return codigo;
        }
    }

    public class TNodoIgualIgual : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoIgualIgual(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var codigo = new Codigo();

            var labelFin = TUtils.generarLabel();

            codigo.tipo = ZTipos.tipoInt;
            codigo.extra = $"{c1.extra} == {c2.extra}";
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);

            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.agregar($"{codigo.apuntador} = 1;");
            codigo.comentar($"...");

            codigo.agregar($"if ({c1.apuntador} == {c2.apuntador}) goto {labelFin};");
            codigo.comentar($"{c1.extra} == {c2.extra}");

            codigo.agregar($"{codigo.apuntador} = 0;");
            codigo.comentar("...");

            codigo.agregar($"{labelFin}:");
            codigo.comentar("..");

            return codigo;
        }
    }


    public class TNodoNoIgual : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoNoIgual(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            var codigo = new Codigo();

            var labelFin = TUtils.generarLabel();

            codigo.tipo = ZTipos.tipoInt;
            codigo.extra = $"{c1.extra} != {c2.extra}";
            codigo.apuntador = TUtils.generarTemporal();
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);

            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.agregar($"{codigo.apuntador} = 1;");
            codigo.comentar($"...");

            codigo.agregar($"if ({c1.apuntador} != {c2.apuntador}) goto {labelFin};");
            codigo.comentar($"{c1.extra} == {c2.extra}");

            codigo.agregar($"{codigo.apuntador} = 0;");
            codigo.comentar("...");

            codigo.agregar($"{labelFin}:");
            codigo.comentar("..");

            return codigo;
        }
    }


    public class TNodoDivision : TNodo
    {
        
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoDivision(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();

            var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            codigo.agregar(c1.texto);
            codigo.agregar(c2.texto);
            

            var tipo = TUtils.expandirTipoNumerico(c1.tipo, c2.tipo);

            if (tipo == null)
            {
                throw new SemanticException($"Operador * no apto para '{c1.tipo.Nombre}' y '{c2.tipo.Nombre}' ");
            }

            codigo.tipo = tipo;
            codigo.extra = $"{c1.extra}/{c2.extra}";
            
            if (tipo != ZTipos.tipoInt)
            {
                codigo.apuntador = TUtils.generarTemporal();
                
                codigo.agregar($"{codigo.apuntador} = {c1.apuntador} / {c2.apuntador};");
                codigo.comentar($"{c1.extra} / {c2.extra}");
                
                GestorTemp.usarTemp(c1.apuntador);
                GestorTemp.usarTemp(c2.apuntador);
                GestorTemp.agregarTemp(codigo.apuntador);
            }
            else
            {
                var temp1 = TUtils.generarTemporal();
                codigo.agregar($"{temp1} = {c1.apuntador} % {c2.apuntador};");
                
                GestorTemp.usarTemp(c1.apuntador);
                GestorTemp.usarTemp(c2.apuntador);

                var temp2 = TUtils.generarTemporal();
                codigo.agregar($"{temp2} = {c1.apuntador} - {temp1};");

                codigo.apuntador = TUtils.generarTemporal();
                codigo.agregar($"{codigo.apuntador} = {temp2} / {c2.apuntador};");
                codigo.comentar($"{c1.extra} / {c2.extra}");
                
                GestorTemp.agregarTemp(codigo.apuntador);
            }

            return codigo;
        }
    }

public class TNodoMultiplicacion : TNodo
    {

        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoMultiplicacion(NodoAst e1, NodoAst e2)
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
                throw new SemanticException($"Operador * no apto para '{c1.tipo.Nombre}' y '{c2.tipo.Nombre}' ");
            }
            
            var codigo = new Codigo();
            codigo.tipo = tipo;
            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.apuntador = TUtils.generarTemporal();

            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);
            GestorTemp.agregarTemp(codigo.apuntador);
            
            codigo.agregar($"{codigo.apuntador} = {c1.apuntador} * {c2.apuntador};");
            codigo.comentar($"{c1.extra} * {c2.extra}");
            codigo.extra = $"{c1.extra} * {c2.extra}";
            
            return codigo;
        }
    }

public class TNodoModulo : TNodo
{
    private readonly NodoAst _e1;
    private readonly NodoAst _e2;

    public TNodoModulo(NodoAst e1, NodoAst e2)
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
            throw new SemanticException($"Operador % no apto para '{c1.tipo.Nombre}' y '{c2.tipo.Nombre}' ");
        }
            
        var codigo = new Codigo();
        codigo.tipo = tipo;
        codigo.agregar(c1);
        codigo.agregar(c2);
        codigo.apuntador = TUtils.generarTemporal();

        GestorTemp.usarTemp(c1.apuntador);
        GestorTemp.usarTemp(c2.apuntador);
        GestorTemp.agregarTemp(codigo.apuntador);
            
        codigo.agregar($"{codigo.apuntador} = {c1.apuntador} % {c2.apuntador};");
        codigo.comentar($"{c1.extra} % {c2.extra}");
        codigo.extra = $"{c1.extra} % {c2.extra}";
            
        return codigo;

    }
}


public class TNodoMenosUnario : TNodo
{
    
    private readonly NodoAst _e1;

    public TNodoMenosUnario(NodoAst e1)
    {
        _e1 = e1;
    }
    
    public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
    {
        var code = new Codigo();
        
        var c1 = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
        
        code.agregar(c1.texto);

        GestorTemp.usarTemp(c1.apuntador);


        if (!TUtils.esNumerico(c1.tipo))
        {
            throw new SemanticException($"Operador - no apto para '{c1.tipo.Nombre}'");
        }

        code.tipo = c1.tipo;


        var oneTemp = TUtils.generarTemporal();
        code.agregar($"{oneTemp} = 0 - 1;");
        code.comentar("-1");
        code.apuntador = TUtils.generarTemporal();
        code.agregar($"{code.apuntador} = {oneTemp} * {c1.apuntador};");
        
        code.extra = $"-({c1.extra})";
        code.comentar(code.extra);
        
        GestorTemp.agregarTemp(code.apuntador);

        return code;
    }
}

    public class TNodoResta : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;

        public TNodoResta(NodoAst e1, NodoAst e2)
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
                throw new SemanticException($"Operador - no apto para '{c1.tipo.Nombre}' y '{c2.tipo.Nombre}' ");
            }
            
            var codigo = new Codigo();
            codigo.tipo = tipo;
            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.apuntador = TUtils.generarTemporal();

            GestorTemp.usarTemp(c1.apuntador);
            GestorTemp.usarTemp(c2.apuntador);
            GestorTemp.agregarTemp(codigo.apuntador);
            
            codigo.agregar($"{codigo.apuntador} = {c1.apuntador} - {c2.apuntador};");
            codigo.comentar($"{c1.extra} - {c2.extra}");
            codigo.extra = $"{c1.extra} - {c2.extra}";
            
            return codigo;

        }
    }

    public class TNodoAccessoArray : TNodo
    {
        private readonly NodoAst _e1;
        private readonly NodoAst _e2;
        
        public TNodoAccessoArray(NodoAst e1, NodoAst e2)
        {
            _e1 = e1;
            _e2 = e2;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var ca = TUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var ci = TUtils.desenvolver(_e2.GenerarCodigo(entorno));

            if (!(ca.tipo is ZArray))
            {
                throw new SemanticException("Operador [] solo valido con arrays, no valido para " + ca.tipo.Nombre);
            }

            if (!TUtils.esNumerico(ci.tipo))
            {
                throw new SemanticException("Debe de ser un indice entero, no valido para " + ci.tipo.Nombre);
            }
            
            var codigo = new Codigo();
            codigo.tipo = new ZReferencia((ca.tipo as ZArray).TipoContenido);
            codigo.extra = $"{ca.extra}[{ci.extra}]";

            
            codigo.agregar(ca);

            var temp = TUtils.generarTemporal();
            codigo.agregar($"{temp} = {ca.apuntador} + 1;");
            codigo.comentar("bloque length");
            
            GestorTemp.usarTemp(ca.apuntador);

            codigo.agregar(ci);
            
            codigo.apuntador = TUtils.generarTemporal();
            codigo.agregar($"{codigo.apuntador} = {temp} + {ci.apuntador};");
            
            GestorTemp.agregarTemp(codigo.apuntador);
            GestorTemp.usarTemp(ci.apuntador);
            
            codigo.comentar($"&{ca.extra}[{ci.extra}]");
            codigo.textoValor = $"heap[{codigo.apuntador}]";

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
    
    
    public class TNodoCrearNull: TNodo
    {

        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoNull;
            codigo.apuntador = $"0";
            codigo.extra = $"null";
            return codigo;
        }
    }
    
    public class TNodoCrearIntConChar: TNodo
    {
        private readonly string _valor;

        public TNodoCrearIntConChar(string valor)
        {
            _valor = valor;
        }

        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            if (_valor.Length == 0)
            {
                return TNodoCrearInt.crearCodigoInt(0);
            }

            if (_valor.Length > 1)
            {
                throw new SemanticException($"un char no puede tener mas de 1 caracter: '{_valor}'");
            }
            
            return TNodoCrearInt.crearCodigoInt(_valor[0]);
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

    public class TNodoDoWhile : TNodo
    {
        
        private readonly NodoAst _nodoValor;
        private readonly List<NodoAst> _nodosTrue;

        public TNodoDoWhile(NodoAst nodoValor, List<NodoAst> nodosTrue)
        {
            _nodoValor = nodoValor;
            _nodosTrue = nodosTrue;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {


            var etiquetaInicio = TUtils.generarLabel();
            var etiquetaFin = TUtils.generarLabel();

            
            var entornoTrue = new ZEntorno(entorno);
            entornoTrue.labelLoopBegin = etiquetaInicio;
            entornoTrue.labelLoopEnd = etiquetaFin;

            var codigo = new Codigo();
            
            codigo.agregar($"{etiquetaInicio}:");
            
            codigo.agregar(TUtils.generarCodigoBloque(_nodosTrue, entornoTrue));
            
            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));
            if (codigoValor.tipo != ZTipos.tipoInt)
            {
                throw new SemanticException("Argumento de if debe de ser numerico");
            }
            codigo.agregar(codigoValor);
            codigo.agregar($"if ({codigoValor.apuntador} == 0) goto {etiquetaFin};");
            codigo.comentar($"while ({codigoValor.extra})" + "{");
            
            
            codigo.agregar($"goto {etiquetaInicio};");
            codigo.comentar("}");

            codigo.agregar($"{etiquetaFin}:");

            entorno.TieneReturn = false; // la entrada al bucle es condicional

            return codigo;
            
            
            
            
        }
    }


    public class TNodoNoSe : TNodo
    {
        
        private readonly List<NodoAst> _nodosTrue;

        public TNodoNoSe(List<NodoAst> nodosTrue)
        {
            _nodosTrue = nodosTrue;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var entornoTrue = new ZEntorno(entorno);
            
            var codigo = new Codigo();
            
            var etiquetaFin = TUtils.generarLabel();
            entornoTrue.labelFin = etiquetaFin;
            
            codigo.agregar(TUtils.generarCodigoBloque(_nodosTrue, entornoTrue));
            codigo.agregar($"{etiquetaFin}:");

            entorno.TieneReturn = entornoTrue.TieneReturn; // la entrada al bucle es condicional

            return codigo;
        }
    }
    
    
//    public class TNodoDoWhile : TNodo
//    {
//        
//        private readonly NodoAst _nodoValor;
//        private readonly List<NodoAst> _nodosTrue;
//
//        public TNodoDoWhile(NodoAst nodoValor, List<NodoAst> nodosTrue)
//        {
//            _nodoValor = nodoValor;
//            _nodosTrue = nodosTrue;
//        }
//        
//        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
//        {
//            
//            var etiquetaInicio = TUtils.generarLabel();
//            var etiquetaFin = TUtils.generarLabel();
//            
//            var entornoTrue = new ZEntorno(entorno);
//            entornoTrue.labelLoopBegin = etiquetaInicio;
//            entornoTrue.labelLoopEnd = etiquetaFin;
//
//            var codigo = new Codigo();
//            
//            codigo.agregar($"{etiquetaInicio}:");
//            
//
//            
//            codigo.agregar(TUtils.generarCodigoBloque(_nodosTrue, entornoTrue));
//            
//            
//            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));
//
//            if (codigoValor.tipo != ZTipos.tipoInt)
//            {
//                throw new SemanticException("Argumento de if debe de ser numerico");
//            }
//            codigo.agregar(codigoValor);
//
//            codigo.agregar($"if ({codigoValor.apuntador} == 0) goto {etiquetaFin};");
//            codigo.comentar($"while ({codigoValor.extra})" + "{");
//            
//            
//            codigo.agregar($"goto {etiquetaInicio};");
//            codigo.comentar("}");
//
//            codigo.agregar($"{etiquetaFin}:");
//
//            entorno.TieneReturn = false; // la entrada al bucle es condicional
//
//            return codigo;
//        }
//    }
    
    public class TNodoWhile : TNodo
    {
        
        private readonly NodoAst _nodoValor;
        private readonly List<NodoAst> _nodosTrue;

        public TNodoWhile(NodoAst nodoValor, List<NodoAst> nodosTrue)
        {
            _nodoValor = nodoValor;
            _nodosTrue = nodosTrue;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            if (codigoValor.tipo != ZTipos.tipoInt)
            {
                throw new SemanticException("Argumento de if debe de ser numerico");
            }

            var etiquetaInicio = TUtils.generarLabel();
            var etiquetaFin = TUtils.generarLabel();

            
            var entornoTrue = new ZEntorno(entorno);
            entornoTrue.labelLoopBegin = etiquetaInicio;
            entornoTrue.labelLoopEnd = etiquetaFin;

            var codigo = new Codigo();
            
            codigo.agregar($"{etiquetaInicio}:");
            codigo.agregar(codigoValor);

            codigo.agregar($"if ({codigoValor.apuntador} == 0) goto {etiquetaFin};");
            codigo.comentar($"while ({codigoValor.extra})" + "{");
            
            codigo.agregar(TUtils.generarCodigoBloque(_nodosTrue, entornoTrue));
            
            codigo.agregar($"goto {etiquetaInicio};");
            codigo.comentar("}");

            codigo.agregar($"{etiquetaFin}:");

            entorno.TieneReturn = false; // la entrada al bucle es condicional

            return codigo;
        }
    }

    public class TNodoIf : TNodo
    {
        private readonly NodoAst _nodoValor;
        private readonly List<NodoAst> _nodosTrue;
        private readonly List<NodoAst> _nodosFalse;

        public TNodoIf(NodoAst nodoValor, List<NodoAst> nodosTrue, List<NodoAst> nodosFalse)
        {
            _nodoValor = nodoValor;
            _nodosTrue = nodosTrue;
            _nodosFalse = nodosFalse;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoValor = TUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            if (codigoValor.tipo != ZTipos.tipoInt)
            {
                throw new SemanticException("Argumento de if debe de ser numerico");
            }

            var etiquetaFalse = TUtils.generarLabel();
            var etiquetaFinIf = TUtils.generarLabel();

            
            var entornoIfTrue = new ZEntorno(entorno);
            var entornoIfFalse = new ZEntorno(entorno);
            var codigo = new Codigo();
            
            codigo.agregar(codigoValor);

            codigo.agregar($"if ({codigoValor.apuntador} == 0) goto {etiquetaFalse};");
            codigo.comentar($"if ({codigoValor.extra})" + "{");
            
            codigo.agregar(TUtils.generarCodigoBloque(_nodosTrue, entornoIfTrue));
            
            codigo.agregar($"goto {etiquetaFinIf};");
            codigo.comentar("}");

            codigo.agregar($"{etiquetaFalse}:");
            codigo.comentar("else {");
            
            codigo.agregar(TUtils.generarCodigoBloque(_nodosFalse, entornoIfFalse));
            
            codigo.agregar($"{etiquetaFinIf}:");
            codigo.comentar("}");

            entorno.TieneReturn = entornoIfTrue.TieneReturn && entornoIfFalse.TieneReturn;

            return codigo;

        }
    }

    

}    