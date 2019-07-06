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
        public static readonly ZEntorno tipoInt = new ZEntorno(null, "int");
        public static readonly ZEntorno tipoDouble = new ZEntorno(null, "double");
        public static readonly ZEntorno tipoVoid = new ZEntorno(null, "void");
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
        public string textoValor { get; set; }
       
        public string extra { get; set; }

        public void append(Codigo codigoSentencia)
        {
            append(codigoSentencia.texto);
        }

        public string texto => string.Join("\n", _lineas);

        public void comentar(string comentario)
        {
            if (_lineas.Count == 0)
                return;

            var ultimaLinea = _lineas[_lineas.Count - 1];
            _lineas.RemoveAt(_lineas.Count - 1);

            if (ultimaLinea.Length < 25)
            {
                ultimaLinea = ultimaLinea.PadRight(25);
            }

            ultimaLinea += "// " + comentario;
            _lineas.Add(ultimaLinea);
            
        }
    }
    
    public class ZReferencia: ZEntorno
    {
        public ZEntorno tipo { get; }

        public ZReferencia(ZEntorno tipo)
        {
            this.tipo = tipo;
        }
    }

    public class ZStruct : ZEntorno
    {
        private readonly List<NodoAst> _declaraciones;
        
        public ZStruct(ZEntorno entornoPadre, string nombre, List<NodoAst> declaraciones): base(entornoPadre, nombre)
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
        
        public string labelFin { get; set; }

        public ZEntorno entornoPadre { get;  }
        public string Nombre { get; }

        private readonly Dictionary<string, ZEntorno> _tipos = new Dictionary<string, ZEntorno>();
//        private readonly OrderedDictionary _propiedades = new OrderedDictionary();
        
        private readonly Dictionary<string, ZEntorno> _propiedades = new Dictionary<string, ZEntorno>();
        private readonly List<string> _indices = new List<string>();
        
        private readonly Dictionary<string, ZFuncion> _zFunciones = new Dictionary<string, ZFuncion>();

        public ZEntorno(ZEntorno entornoPadre = null, string nombre = "")
        {
            this.entornoPadre = entornoPadre;
            Nombre = nombre;

            if (entornoPadre != null)
            {
                labelFin = entornoPadre.labelFin;
            }
        }

        public List<ZFuncion> obtenerFunciones()
        {
            return _zFunciones.Values.ToList();
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
        

        public void declararPropiedad(string id, ZEntorno zEntorno)
        {
            if (_propiedades.ContainsKey(id))
            {
                throw new SemanticException($"var {id} ya declarado");
            }

            _indices.Add(id);
            _propiedades[id] = zEntorno;
        }

        public bool tienePropiedad(string id)
        {
            return _propiedades.ContainsKey(id);
        }

        public (ZEntorno, int) obtenerPropiedadConDireccionRelativa(string id)
        {
            if (!_propiedades.ContainsKey(id))
            {
                throw new SemanticException($"var {id} no declarado");
            }

            return ( _propiedades[id], _indices.IndexOf(id));
        }

        public int espacioEnMemoria => _indices.Count;
        

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