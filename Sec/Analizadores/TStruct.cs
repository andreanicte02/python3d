using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Sec.Analizadores
{
    public class SemanticException : ApplicationException
    {
        public SemanticException(string message) : base(message)
        {
            
        }
    }

    public class ZTipos
    {
        public static readonly ZEntorno tipoInt = new ZEntorno();
        public static readonly ZEntorno tipoDouble = new ZEntorno();
        public static readonly ZEntorno tipoVoid = new ZEntorno();
    }
    
    
    public class Codigo
    {
        
        public static Codigo vacio = new Codigo();

        private readonly List<string> _lineas = new List<string>();

        public void append(string texto)
        {
            if (texto.Trim().Length == 0)
            {
                return;
            }
            this._lineas.Add(texto);
        }

        public ZEntorno tipo { get; set; }
        public string apuntador { get; set; }
        public string extra { get; set; }
    }
    
    public class ZReferencia: ZEntorno
    {
        private readonly ZEntorno _tipo;

        public ZReferencia(ZEntorno tipo)
        {
            _tipo = tipo;
        }
    }

    public class ZStruct : ZEntorno
    {
        private readonly List<NodoAst> _declaraciones;
        
        public ZStruct(ZEntorno entornoPadre, List<NodoAst> declaraciones): base(entornoPadre)
        {
            _declaraciones = declaraciones;
        }

        public void declararMiembros()
        {
            foreach (var declaracion in _declaraciones)
            {
                declaracion.GenerarCodigo(this);
            }
        }
    }
    
    public class ZEntorno
    {

        private readonly ZEntorno entornoPadre;
        private readonly Dictionary<string, ZEntorno> _tipos = new Dictionary<string, ZEntorno>();
//        private readonly OrderedDictionary _propiedades = new OrderedDictionary();
        private readonly Dictionary<string, ZEntorno> _propiedades = new Dictionary<string, ZEntorno>();
        private readonly Dictionary<string, ZFuncion> _zFunciones = new Dictionary<string, ZFuncion>();

        public ZEntorno(ZEntorno entornoPadre = null)
        {
            this.entornoPadre = entornoPadre;
        }

        public List<ZEntorno> obtenerTipos()
        {
            return _tipos.Values.ToList();
        }

        public ZEntorno obtenerTipoDeclarado(string id)
        {
            for (ZEntorno temporal = this; temporal != null; temporal = temporal.entornoPadre )
            {
                if (temporal._tipos.ContainsKey(id))
                {
                    return temporal._tipos[id];
                }
            }
            
            throw new SemanticException($"tipo {id} no declarado");
        }

        public void declararZFuncion(string id, ZFuncion zFuncion)
        {
            if (_zFunciones.ContainsKey(id))
            {
                throw new SemanticException($"funcion {id} ya declarada");
            }

            _zFunciones[id] = zFuncion;
        }
        public void declararTipo(string id, ZEntorno tipo)
        {
            if (_tipos.ContainsKey(id))
            {
                throw new SemanticException($"tipo {id} ya declarado");
            }
            _tipos[id] = tipo;
        }

//        public void contienePropiedad(string id)
//        {
//            foreach (var entrada in _propiedades.Keys) {
//            }
//
//        }

        public void declararPropiedad(string id, ZEntorno zEntorno)
        {

            if (_propiedades.ContainsKey(id))
            {
                throw new SemanticException($"var {id} ya declarado");
            }
            _propiedades[id] = zEntorno;
        }

        public void imprimirTipos()
        {
            Console.WriteLine("Tipos: ");
            foreach (var tipo in _tipos)
            {
                Console.WriteLine($"tipo: {tipo}");
            }
            
            Console.WriteLine("Funciones: ");
            foreach (var fun in _zFunciones)
            {
                Console.WriteLine($"fun: {fun}");
            }
        }
        
    }
}