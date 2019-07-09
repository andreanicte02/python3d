using System.Collections.Generic;
using System.Linq;

namespace Sec.Analizadores
{

    
    public class CodigoT
    {
        

        private readonly List<string> _lineas = new List<string>();

        public void agregar(string texto)
        {
            if (texto.Trim().Length == 0)
            {
                return;
            }
            this._lineas.Add(texto);
        }

//        public ZEntorno tipo { get; set; }

        public string prefix { get; set; } = "";

        public string apuntador { get; set; } = "";

        public bool ambiguo => string.IsNullOrEmpty(apuntador);
        public string valor { get; set; } = "";
        public string registro { get; set; } = "";

//        public string extra { get; set; }

        public void agregar(CodigoT codigoSentencia)
        {
            agregar(codigoSentencia.texto);
        }

        public string texto => string.Join("\n", _lineas.Select(l => prefix + l) );

//        public void comentar(string comentario)
//        {
//            if (_lineas.Count == 0)
//                return;
//
//            var ultimaLinea = _lineas[_lineas.Count - 1];
//            _lineas.RemoveAt(_lineas.Count - 1);
//
//            if (ultimaLinea.Length < 25)
//            {
//                ultimaLinea = ultimaLinea.PadRight(25);
//            }
//
//            ultimaLinea += "// " + comentario;
//            _lineas.Add(ultimaLinea);
//            
//        }

//        public Codigo copiaApuntador()
//        {
//            var codigo = new Codigo();
//            codigo.apuntador = this.apuntador;
//            codigo.extra = this.extra;
//            codigo.tipo = this.tipo;
//            codigo.textoValor = this.textoValor;
//            return codigo;
//        }
    }

    public class PClase
    {
        private readonly string _id;
        private readonly List<NodoPythonAst> _sentencias;

        private readonly int _numeroClase;
        public string metodo => $"objeto_clase_{_numeroClase}_{_id}";
        

        public PClase(string id, List<NodoPythonAst> sentencias)
        {
            _id = id;
            _sentencias = sentencias;
            _numeroClase = PUtils.generarNumeroClase();
        }

        public CodigoT GenerarCodigoClase()
        {
            return PUtils.generarBloqueClase(metodo, _sentencias, "\t");
        }
    }
    public class PFuncion
    {
        private readonly string _id;
        private readonly List<NodoPythonAst> _sentencias;

        public PFuncion(string id, List<NodoPythonAst> sentencias)
        {
            _id = id;
            _sentencias = sentencias;
        }

        public CodigoT GenerarCodigoFuncion()
        {
            return PUtils.generarBloqueFuncion(_id, _sentencias, "\t");
        }
    }
    
    public class PEntorno
    {
        private readonly List<PFuncion> _funcionesP = new List<PFuncion>();
        private readonly List<PClase> _clasesP = new List<PClase>();

        public void agregarFuncionPython(PFuncion pFuncion)
        {
            _funcionesP.Add(pFuncion);
        }

        public List<PFuncion> obtenerPFunciones()
        {
//            return _funcionesP.Values.ToList();
            return _funcionesP;
        }
        
        public void agregarClasePython(PClase pClase)
        {
//            _funcionesP[id] = pFuncion;
            _clasesP.Add(pClase);
        }

        public List<PClase> obtenerPClasese()
        {
//            return _funcionesP.Values.ToList();
            return _clasesP;
        }
    }
    
    public interface NodoPythonAst
    {
        CodigoT GenerarCodigo(PEntorno entorno);
    }
    
    public abstract class PNodo: NodoPythonAst
    {
        public CodigoT GenerarCodigo(PEntorno entorno)
        {
            return IntentarGenerarCodigo(entorno);
        }
        
        public abstract CodigoT IntentarGenerarCodigo(PEntorno entorno);
    }

    public class PNodoAsignacion : PNodo
    {
        private readonly List<NodoPythonAst> _nodosDestino;
        private readonly List<NodoPythonAst> _nodosValor;

        public PNodoAsignacion(List<NodoPythonAst> nodosDestino, List<NodoPythonAst> nodosValor)
        {
            _nodosDestino = nodosDestino;
            _nodosValor = nodosValor;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            if (_nodosDestino.Count != _nodosValor.Count)
            {
                throw new PythonSemanticException("numero diferente de destinos y valores en asignacion");
            }
            
            var codigo = new CodigoT();

            for (var i = 0; i < _nodosDestino.Count; i++)
            {
                var c1 = _nodosDestino[i].GenerarCodigo(entorno);
                if (!c1.ambiguo)
                {
                    throw new PythonSemanticException("destino no valido para asignacion");
                }
                
                var c2 = PUtils.desenvolver(_nodosValor[i].GenerarCodigo(entorno));

                codigo.agregar(c2);
                codigo.agregar(string.Format(c1.registro, c2.apuntador));
//                codigo.agregar($"asignar({c1.apuntador}, {c2.apuntador});");
            }

            return codigo;
        }
    }


    public class PNodoBuscarId : PNodo
    {
        private readonly string _id;

        public PNodoBuscarId(string id)
        {
            _id = id;
        }
        
        public static CodigoT buscarId(string id)
        {
            var codigo = new CodigoT();
            codigo.valor = $"buscarIdlocal(ambito, \"{id}\");";
            codigo.registro = "registrar(ambito, \"" + id + "\", {0});";
            return codigo;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            return buscarId(_id);
        }
    }

    public class PNodoInvocar : PNodo
    {
        private readonly string _id;
        private readonly List<NodoPythonAst> _nodosArgumento;

        public PNodoInvocar(string id, List<NodoPythonAst> nodosArgumento)
        {
            _id = id;
            _nodosArgumento = nodosArgumento;
        }
        
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();

            var cMetodo = PUtils.desenvolver(PNodoBuscarId.buscarId(_id));
            codigo.agregar(cMetodo);

            var temp = PUtils.generarTemporal();
            codigo.agregar($"General[] {temp} = new General[{_nodosArgumento.Count}];");
            
            for (int i = 0; i < _nodosArgumento.Count; i++)
            {
                var cArg = PUtils.desenvolver(_nodosArgumento[i].GenerarCodigo(entorno));
                codigo.agregar(cArg);
                codigo.agregar($"{temp}[{i}] = {cArg.apuntador};");
            }

            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"{codigo.apuntador} = invocar({cMetodo.apuntador}, {temp});");

            return codigo;
            
        }
    }

    public class PNodoCrearEntero : PNodo
    {
        private readonly int _valor;

        public PNodoCrearEntero(int valor)
        {
            _valor = valor;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearGeneralEntero({_valor});");

            return codigo;
        }
    }


    public class PNodoCrearClase : PNodo
    {
        private readonly string _id;
        private readonly List<NodoPythonAst> _sentencias;

        public PNodoCrearClase(string id, List<NodoPythonAst> sentencias)
        {
            _id = id;
            _sentencias = sentencias;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var pClase = new PClase(_id, _sentencias);
            entorno.agregarClasePython(pClase);

            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = {pClase.metodo}();");
            codigo.agregar($"registrar(ambit, \"{_id}\", {codigo.apuntador});");
            return codigo;
        }
    }

    public class PNodoCrearMetodo : PNodo
    {
        private readonly string _id;
        private readonly List<NodoPythonAst> _sentencias;

        public PNodoCrearMetodo(string id, List<NodoPythonAst> sentencias)
        {
            _id = id;
            _sentencias = sentencias;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var pFuncion = new PFuncion(_id, _sentencias);
            entorno.agregarFuncionPython(pFuncion);
            
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearGeneralFuncion(); // {_id}");
            codigo.agregar($"registrar(ambito, \"{_id}\", {codigo.apuntador});");
            return codigo;
        }
    }
    
   


}