using System;
using System.Collections.Generic;

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
            Console.WriteLine($"Se declarara struct {id}");
            _id = id;
            _declaraciones = declaraciones;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();
            
            var tipo = new ZStruct(entorno, _declaraciones);
            entorno.declararTipo(_id, tipo);

            return codigo;
        }
    }

    public class TNodoBusquedaTipo : TNodo
    {
        private readonly string _id;

        public TNodoBusquedaTipo(string id)
        {
            Console.WriteLine($"Se buscara el tipo {id}");
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
            Console.WriteLine("SE declara funcion " + id);
            
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
        public ZFuncion(ZEntorno entornoPadre, ZEntorno tipoRetorno, string id, List<NodoAst> nodosParametro, List<NodoAst> sentencias)
        {
            
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
            Console.WriteLine($"Se declarara var {id}");
            _nodoTipo = nodoTipo;
            _id = id;
            _nodoExpr = nodoExpr;
        }
        
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigoTipo = _nodoTipo.GenerarCodigo(entorno);
            var referencia = new ZReferencia(codigoTipo.tipo);
            entorno.declararPropiedad(_id, referencia);

            if (this._nodoExpr != null)
            {
                // TODO: retorno el codigo generado por la asignacion
            }
            
            return Codigo.vacio;
        }
    }

    public class TNodoCrearInt: TNodo
    {
        private readonly int _valor;

        public TNodoCrearInt(int valor)
        {
            _valor = valor;
        }
        public override Codigo IntentarGenerarCodigo(ZEntorno entorno)
        {
            var codigo = new Codigo();
            codigo.tipo = ZTipos.tipoInt;
            codigo.apuntador = $"{_valor}";
            codigo.extra = $"{_valor}";
            return codigo;
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

    

}    