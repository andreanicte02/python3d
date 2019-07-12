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

        public string extra { get; set; }

        public void agregar(CodigoT codigoSentencia)
        {
            agregar(codigoSentencia.texto);
        }

        public string texto
        {
            get
            {
                var a = _lineas.Select(l =>
                {
                    var asdf = l.Split('\n').Select(s => prefix + s);
                    return string.Join("\n", asdf);
                });
                return string.Join("\n", a);
            }
        }

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
        public string metodo_instancia => $"objeto_instancia_{_numeroClase}";
        

        public PClase(string id, List<NodoPythonAst> sentencias)
        {
            _id = id;
            _sentencias = sentencias;
            _numeroClase = PUtils.generarNumeroClase();
        }

        public CodigoT GenerarCodigoClase()
        {
            return PUtils.generarBloqueClase(metodo, metodo_instancia, _id, _sentencias, "\t");
        }
    }
    public class PFuncion
    {
        private readonly string _id;
        private readonly List<string> _parametros;
        private readonly List<NodoPythonAst> _sentencias;
        private readonly string _archivo;
        public int numeroFuncion { get; set; }

        public PFuncion(string id, List<string> parametros, List<NodoPythonAst> sentencias, string archivo)
        {
            _id = id;
            _parametros = parametros;
            _sentencias = sentencias;
            _archivo = archivo;
            
            numeroFuncion = PUtils.generarNumeroFunciones();
        }

        public string nombre => $"py_fun_{numeroFuncion}";

        public CodigoT GenerarCodigoFuncion()
        {
            return PUtils.generarBloqueFuncion(nombre, _id, _parametros, _sentencias, "\t", _archivo);
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
        public string archivo { get; set; }
        public int linea { get; set; }
        public int columna { get; set; }
        
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
        
        public static CodigoT buscarId(string id, int linea, int columna)
        {
            var codigo = new CodigoT();
            codigo.valor = $"buscarIdlocal(ambito, \"{id}\", archivo, {linea}, {columna});";
            codigo.registro = "registrar(ambito, \"" + id + "\", {0});";
            return codigo;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            return buscarId(_id, this.linea, this.columna);
        }
    }

    public class PNodoIf : PNodo
    {
        private readonly NodoPythonAst _argumento;
        private readonly List<NodoPythonAst> _nodosTrue;
        private readonly List<NodoPythonAst> _nodosFalse;

        public PNodoIf(NodoPythonAst argumento, List<NodoPythonAst> nodosTrue, List<NodoPythonAst> nodosFalse = null)
        {
            _argumento = argumento;
            _nodosTrue = nodosTrue;
            _nodosFalse = nodosFalse ?? new List<NodoPythonAst>() ;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();

            var codigoArgumento = PUtils.desenvolver(_argumento.GenerarCodigo(entorno));
            
            codigo.agregar(codigoArgumento);
            codigo.agregar($"if (esVerdaderoGeneral({codigoArgumento.apuntador}))" + "{");
            var codigoTrue = PUtils.generarBloque(_nodosTrue, "\t", entorno);
            codigo.agregar(codigoTrue);
            codigo.agregar("}");
            codigo.agregar("else {");
            var codigoFalse = PUtils.generarBloque(_nodosFalse, "\t", entorno);
            codigo.agregar(codigoFalse);
            codigo.agregar("}");

            return codigo;
        }
    }

    
    public class PNodoAccessoPunto : PNodo
    {
        private readonly NodoPythonAst _nodoValor;
        private readonly string _id;

        public PNodoAccessoPunto(NodoPythonAst nodoValor, string id)
        {
            _nodoValor = nodoValor;
            _id = id;
        }
        
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();

            var c = PUtils.desenvolver(_nodoValor.GenerarCodigo(entorno));

            codigo.agregar(c);
//            codigo.agregar($"buscarPropiedad({c.apuntador},"+ "\"" + _id + "\"" + $", archivo, {this.linea}, {this.columna});");
            

            codigo.valor = $"buscarPropiedad({c.apuntador},"+ "\"" + _id + "\"" + $", archivo, {this.linea}, {this.columna});";
            codigo.registro = $"registrar({c.apuntador}, \"" + _id + "\", {0});";
            return codigo;

            return codigo;
            
        }
    }
    
    public class PNodoPrint : PNodo
    {
        private readonly List<NodoPythonAst> _nodosArgumento;

        public PNodoPrint(List<NodoPythonAst> nodosArgumento)
        {
//            _id = id;
            _nodosArgumento = nodosArgumento;
        }
        
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();

            var temp = PUtils.generarTemporal();
            codigo.agregar($"General[] {temp} = new General[{_nodosArgumento.Count}];");
            
            for (int i = 0; i < _nodosArgumento.Count; i++)
            {
                var cArg = PUtils.desenvolver(_nodosArgumento[i].GenerarCodigo(entorno));
                codigo.agregar(cArg);
                codigo.agregar($"{temp}[{i}] = {cArg.apuntador};");
            }
            
            codigo.agregar($"printGenerales({temp});");

            return codigo;
            
        }
    }
    
    public class PNodoInvocacionPunto : PNodo
    {
        private readonly NodoPythonAst _nodo;
        private readonly string _id;
        private readonly List<NodoPythonAst> _nodosArgumento;

        public PNodoInvocacionPunto(NodoPythonAst nodo, string id, List<NodoPythonAst> nodosArgumento)
        {
            _nodo = nodo;
            _id = id;
            _nodosArgumento = nodosArgumento;
        }
        
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();

            var c = PUtils.desenvolver(_nodo.GenerarCodigo(entorno));
            
            
            codigo.agregar(c);

            var temp1 = PUtils.generarTemporal();
            codigo.agregar($"General {temp1} = buscarIdEnObjeto({c.apuntador}, \"{_id}\", archivo, {linea}, {columna});");

            
            var temp = PUtils.generarTemporal();
            codigo.agregar($"General[] {temp} = new General[{_nodosArgumento.Count}];");
            
            for (int i = 0; i < _nodosArgumento.Count; i++)
            {
                var cArg = PUtils.desenvolver(_nodosArgumento[i].GenerarCodigo(entorno));
                codigo.agregar(cArg);
                codigo.agregar($"{temp}[{i}] = {cArg.apuntador};");
            }

            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = invocar({temp1}, {temp}, archivo, {this.linea}, {this.columna});");

            return codigo;
            
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

            var cMetodo = PUtils.desenvolver(PNodoBuscarId.buscarId(_id, this.linea, this.columna));
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
            codigo.agregar($"General {codigo.apuntador} = invocar({cMetodo.apuntador}, {temp}, archivo, {this.linea}, {this.columna});");

            return codigo;
            
        }
    }
    
    public class PNodoCrearBooleano : PNodo
    {
        private readonly int _valor;
        public PNodoCrearBooleano(int valor)
        {
            _valor = valor;
        }
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearBooleanGeneral({_valor});");
            return codigo;
        }
    }
    
    
    public class PnodoCrearNull : PNodo
    {

        public PnodoCrearNull()
        {
            
            
        }

        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador}= crearNullGeneral();");
            return codigo;
        }
    }
    
    

    public class PNodoCrearCadena : PNodo
    {
        private readonly string _valor;
        public PNodoCrearCadena(string valor)
        {
            _valor = valor;
        }
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearStringGeneral(\"{_valor}\");");

            return codigo;
        }
    }

    public class PNodoCrearDouble : PNodo
    {
        private readonly double _valor;
        public PNodoCrearDouble(double valor)
        {
            _valor = valor;
        }
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var codigo = new CodigoT();
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearDoubleGeneral({_valor});");

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
            codigo.agregar($"General {codigo.apuntador} = crearIntGeneral({_valor});");

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
            codigo.agregar($"General {codigo.apuntador} = {pClase.metodo}(ambito);");
            codigo.agregar($"registrar(ambit, \"{_id}\", {codigo.apuntador});");
            return codigo;
        }
    }

    public class PNodoCrearMetodo : PNodo
    {
        public string Id { get; }
        private readonly List<string> _parametros;
        private readonly List<NodoPythonAst> _sentencias;

        public PNodoCrearMetodo(string id, List<string> parametros, List<NodoPythonAst> sentencias)
        {
            Id = id;
            _parametros = parametros;
            _sentencias = sentencias;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var pFuncion = new PFuncion(Id, _parametros, _sentencias, this.archivo);
            entorno.agregarFuncionPython(pFuncion);
            
            var codigo = new CodigoT();
            codigo.extra = pFuncion.nombre;
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = crearGeneralFuncion({pFuncion.numeroFuncion},{_parametros.Count}, ambito); // {Id}");
            codigo.agregar($"registrar(ambito, \"{Id}\", {codigo.apuntador});");
            return codigo;
        }
    }




    public class PNodoReturn : PNodo
    {
        private readonly NodoPythonAst _nodo;

        public PNodoReturn(NodoPythonAst nodo)
        {
            _nodo = nodo;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var c = PUtils.desenvolver(_nodo.GenerarCodigo(entorno));
            var codigo = new CodigoT();
            codigo.agregar(c);
            codigo.agregar($"return {c.apuntador};");
            return codigo;
        }
    }
    


    public class PNodoOperacionBinaria : PNodo
    {
        private readonly NodoPythonAst _e1;
        private readonly NodoPythonAst _e2;
        private readonly string _nombreFuncion;

        public PNodoOperacionBinaria(NodoPythonAst e1, NodoPythonAst e2, string nombreFuncion)
        {
            _e1 = e1;
            _e2 = e2;
            _nombreFuncion = nombreFuncion;
        }
        
        public override CodigoT IntentarGenerarCodigo(PEntorno entorno)
        {
            var c1 = PUtils.desenvolver(_e1.GenerarCodigo(entorno));
            var c2 = PUtils.desenvolver(_e2.GenerarCodigo(entorno));
            
            var codigo = new CodigoT();
            codigo.agregar(c1);
            codigo.agregar(c2);
            codigo.apuntador = PUtils.generarTemporal();
            codigo.agregar($"General {codigo.apuntador} = {_nombreFuncion}({c1.apuntador}, {c2.apuntador}, archivo, {this.linea}, {this.columna});");
            return codigo;

        }
    }
   


}