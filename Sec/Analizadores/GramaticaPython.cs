using System.Collections.Generic;
using Irony.Ast;
using Irony.Parsing;
using static Sec.Analizadores.IronyUtils;

namespace Sec.Analizadores
{
    public class GramaticaPython: Grammar
    {
        public string Archivo { get; }
//        public override void BuildAst(LanguageData language, ParseTree parseTree)
//        {
//            if (!LanguageFlags.IsSet(LanguageFlags.CreateAst))
//                return;
//            var astContext = new AstContext(language);
//            var astBuilder = new AstBuilder(astContext);
//            astBuilder.BuildAst(parseTree);
//        }

        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents |  OutlineOptions.CheckBraces, ToTerm(@"\"));
            filters.Add(outlineFilter);

        }
        
//        public override
        
           public GramaticaPython(string archivo = ""):  base(caseSensitive: true)
        {
            Archivo = archivo;
            LanguageFlags |= LanguageFlags.CreateAst;
            
            
//            AstContext

            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "#", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "\"\"\"", "\"\"\"");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);

            //-------------------------------------------------------------------------------------------------palabras resevadas
             MarkReservedWords("class");
             MarkReservedWords("print");
             MarkReservedWords("def");
             MarkReservedWords("global");
             MarkReservedWords("nonlocal");
             MarkReservedWords("if");
             MarkReservedWords("while");
             MarkReservedWords("else");
             MarkReservedWords("elif");
             MarkReservedWords("return");
//             MarkReservedWords("self");
             MarkReservedWords("new");
             MarkReservedWords("for");
             MarkReservedWords("in");
             MarkReservedWords("bool");
             MarkReservedWords("True");
             MarkReservedWords("False");
             MarkReservedWords("chr");
             MarkReservedWords("float");
             MarkReservedWords("str");
             MarkReservedWords("is");
             MarkReservedWords("import");
             MarkReservedWords("del");
             MarkReservedWords("pass");
             MarkReservedWords("continue");
             MarkReservedWords("break");
             MarkReservedWords("lambda");
             MarkReservedWords("None");
             MarkReservedWords("type");
             MarkReservedWords("len");
             MarkReservedWords("int");
             MarkReservedWords("input");
             
             
             var @class = ToTerm("class");
             var print = ToTerm("print");
             var def = ToTerm("def");
             var global = ToTerm("global");
             var nonlocal = ToTerm("nonlocal");
             var @if = ToTerm("if");
             var @while = ToTerm("while");
             var @else = ToTerm("else");
             var elif = ToTerm("elif");
             var @return = ToTerm("return");
             var self = ToTerm("self");
             Terminal @new = ToTerm("new");
             var @for = ToTerm("for");
             var @in = ToTerm("in");
             var @bool = ToTerm("bool");
             var @true = ToTerm("True");
             var @false = ToTerm("False");
             var chr = ToTerm("chr");
             var @float = ToTerm("float");
             var str = ToTerm("str");
             var @is = ToTerm("is");
             var import = ToTerm("import");
             var del = ToTerm("del");
             var pass = ToTerm("pass");
             var @continue = ToTerm("continue");
             var @break = ToTerm("break");
             var lambda = ToTerm("lambda");
             var None = ToTerm("None");
             
             var type = ToTerm("type");
             var len = ToTerm("len");
             var @int = ToTerm("int");
             var input = ToTerm("input");
             


            //-----------------------------------------------SIGNOS PELA
           
            var dosp = ToTerm(":");
            var coma = ToTerm(",");
            Terminal apar = ToTerm("(");
            Terminal cpar = ToTerm(")");
            Terminal acor = ToTerm("[");
            Terminal ccor = ToTerm("]");
            Terminal alla = ToTerm("{");
            Terminal clla = ToTerm("}");
            //operadores aritmeticos
            Terminal por = ToTerm("*");
            Terminal div = ToTerm("/");
            Terminal mas = ToTerm("+");
            Terminal menos = ToTerm("-");
            Terminal pot = ToTerm("**");
            Terminal divDoble = ToTerm("//");
            Terminal modulo = ToTerm("%");
            
            
            
            Terminal punto = ToTerm(".");
           
            Terminal igualIgual = ToTerm("==");
            Terminal diferenteQue = ToTerm("!=");     
           
            Terminal shiftLet = ToTerm("<<");
            Terminal shiftRight = ToTerm(">>");
            //operadores binarios
            Terminal andBinario = ToTerm("&");
            Terminal orBinario = ToTerm("|");
            Terminal xorBinario = ToTerm("^");
            
            //operacdores realcioneals
            Terminal mayor = ToTerm(">");
            Terminal menor = ToTerm("<");
            Terminal mayorIgualQue = ToTerm(">=");
            Terminal menorIgualQue = ToTerm("<=");
            
            //operacdores logicos
            Terminal and = ToTerm("and");
            Terminal or = ToTerm("or");
            Terminal not = ToTerm("not");
            Terminal isNot = ToTerm("is not");
            Terminal notIn = ToTerm("not in");
            
            //asignacion
            Terminal igual = ToTerm("=");
            Terminal masIgual = ToTerm("+=");
            Terminal menosIgual = ToTerm("-=");
            Terminal porIgual = ToTerm("*=");
            Terminal divIgual = ToTerm("/=");
            Terminal modIgual = ToTerm("%=");
            Terminal potIgual = ToTerm("**=");
            Terminal dobleDivIgual = ToTerm("//=");

            

            



            //-------------------------------------------------------------NO TERMINALES
            var INICIO = new AutoNonTerminal("INICIO");
           
            var sentencia = new AutoNonTerminal("sentencia");
            var sentencias = new AutoNonTerminal("sentencias");
            var listaSentencia = new  AutoNonTerminal("listaSentencia");
            
            var asignar = new  AutoNonTerminal("asignar");
            var exp = new  AutoNonTerminal("exp");
            var crearClase = new  AutoNonTerminal("crearClase");
            
            //pdoruccion de sentencias de clase
            var sentenciaClase = new  AutoNonTerminal("sentenciaClase");
            var sentenciasClases = new  AutoNonTerminal("sentenciasClases");
            var bloqueClase = new  AutoNonTerminal("bloqueClase");
            
            //prouccion de sentencias locales
            var sentenciaLocal = new AutoNonTerminal("sentenciaLocal");
            var bloqueLocal = new AutoNonTerminal("bloqueLocal");
            var sentenciasLocales = new AutoNonTerminal("sentenciasLocal");

            
            var crearMetodo = new  AutoNonTerminal("crearMetodo");
            var sentenciaPrint = new  AutoNonTerminal("sentenciaPrint");
            var exps        = new AutoNonTerminal("listaExp");
            var listaId         = new AutoNonTerminal("listaId");
            var parametros      = new AutoNonTerminal("parametros");
            var listaExp     = new AutoNonTerminal("parametrosExp");
            var sentenciaAlcance = new AutoNonTerminal("sentenciaAlcance");
            var sentenciaWhile      = new AutoNonTerminal("sentenciaWhile");
            var sentenciaIf         = new AutoNonTerminal("sentenciaIf");
            var sentenciaElif       = new AutoNonTerminal("sentenciaElif");
            var sentenciaReturn       = new AutoNonTerminal("sentenciaReturn");
            var invocacionPunto       = new AutoNonTerminal("invocacionPunto");
            var invocacion       = new AutoNonTerminal("invocacion");    
            var sentenciaFor       = new AutoNonTerminal("sentenciaFor");
            var sentenciaImport  = new AutoNonTerminal("sentenciaImport");

            
            
            //arrayInit produccion de tuplas, arreglos, diccionarios conjunto
            var arrayInit  = new NonTerminal("arrayInit");
            
            var cuerpoArray  = new AutoNonTerminal("cuerpoArray");
            var arrayElemento  = new AutoNonTerminal("arrayElemento");
            var arrayElementos  = new AutoNonTerminal("arrayElementos");
            
          

            var sentenciaDel  = new NonTerminal("sentenciaDel");
            
            //sentencia para esignar exp = linitAisgnar
            var initAsignar  = new AutoNonTerminal("initAsignar");
            var lInitAsignar =  new AutoNonTerminal("lInitAsignar");
            
            
            //sentencias de diccionario
            var cuerpoDiccionario = new AutoNonTerminal("cuerpoDiccionario");
            var elementosDiccionario = new AutoNonTerminal("elementosDiccionario");
            var elementoDiccionario = new AutoNonTerminal("elementoDiccionario");
            
            
            var sentenciaOpTernario = new AutoNonTerminal("sentenciaOpTernario");

            var sentenciaLambda = new AutoNonTerminal("sentenciaLambda");




            //---------------------> ER
           
//            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
//            RegexBasedTerminal @decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
//            RegexBasedTerminal id = new RegexBasedTerminal("id", "[A-Za-z]([A-Za-z|_|0-9]+)?");
            StringLiteral cadena = new StringLiteral("cadena", "'", StringOptions.AllowsAllEscapes);
            StringLiteral cadena2 = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);
            
            var numero = new NumberLiteral("Entero", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            numero._accion((v, n) => n.Token.Value);
            var @decimal = new NumberLiteral("Decimal");
            @decimal._accion((v, n) => n.Token.Value);

            var id = new IdentifierTerminal("Identificador");
            id._accion((v, n) => n.Token.Value);

            cadena._accion((v, n) => n.Token.Value);
            cadena2._accion((v, n) => n.Token.Value);

            
            //----------------------------------------------------------------------------------

            this.Root = INICIO;
            

            INICIO.Rule = listaSentencia;
//            INICIO.Rule = asignar + Eos;
            
            
            
            this.ConfigList<NodoPythonAst>(true, listaSentencia, sentencia);
            

            sentencia.Rule = sentenciaLocal
//                asignar + Eos
//                             | crearMetodo
//                             | crearClase
                             | sentenciaImport + Eos
//                             | sentenciasLocales
                ;

//            crearClase.Rule = @class + id + dosp +Eos+ bloqueClase;
            
            crearClase.Rule = produccion(@class + id + dosp +Eos+ bloqueLocal)
                    ._accionP(v => new PNodoCrearClase(v[1], v[3]))
                ;

            bloqueClase.Rule = Indent + sentenciasClases + Dedent
                               | Empty;

            
                              

            sentenciasClases.Rule = sentenciasClases + sentenciaClase
                                    | sentenciaClase;

            //es como la de prueba
            sentenciaClase.Rule = asignar + Eos
                                  | crearMetodo;
                           
            //crear metodo

            crearMetodo.Rule = produccion(def + id + apar + parametros + cpar + dosp + Eos + bloqueLocal)
                    ._accionP(v => new PNodoCrearMetodo(v[1], v[3], v[6]))
                ;
            
            var parametro = new AutoNonTerminal();
            parametro.Rule = produccion(id)._accionP(v => v[0]);

            this.ConfigList<string>(false, parametros, parametro, ",");
            



            
            //bloque con las sentencias locales
            bloqueLocal.Rule = produccion(Indent + sentenciasLocales + Dedent)._accionP(v => v[0])
                               | produccion(Empty)._accionP(v => new List<NodoPythonAst>())
                ;
            
            
            this.ConfigList<NodoPythonAst>(true, sentenciasLocales, sentenciaLocal);


            sentenciaLocal.Rule = asignar + Eos
                                  | crearMetodo
                                  | crearClase
                                  | exp + Eos
                                  | sentenciaPrint + Eos
//                                  | sentenciaAlcance + Eos
                                  | sentenciaWhile
                                  | sentenciaIf
                                  | sentenciaReturn + Eos
//                                  | sentenciaFor
//                                  | sentenciaDel + Eos
//                                  | exp + Eos //llamadas a metodos objetos
//                                  | pass + Eos
                                  | produccion(@continue + Eos)._accion(v => new PNodoContinue())
                                  | produccion(@break + Eos)._accion(v => new PNodoBreak())
                ;






            sentenciaPrint.Rule = produccion(print + apar + listaExp + cpar)._accionP(v => new PNodoPrint(v[2]))
                ;

            asignar.Rule = produccion(exps + igual + exps)._accionP(v => new PNodoAsignacion(v[0], v[2]))
//            produccion(exps + igual + lInitAsignar)
//                           | exps + masIgual + lInitAsignar
//                           | exps + menosIgual + lInitAsignar
//                           | exps + porIgual + lInitAsignar
//                           | exps + divIgual + lInitAsignar
//                           | exps + modIgual + lInitAsignar
//                           | exps + potIgual + lInitAsignar
//                           | exps + dobleDivIgual + lInitAsignar
                ;
            
            this.ConfigList<NodoPythonAst>(true, exps, exp, ",");
            
//            exps.Rule = exps + coma + exp
//                        | exp;


            lInitAsignar.Rule = lInitAsignar + coma + initAsignar //lista de posibles asignaciones
                                | initAsignar;

            initAsignar.Rule = exp
                               | arrayInit;
                               
            
            

            sentenciaAlcance.Rule = global + exps
                                    | nonlocal + exps;
            

            
            this.ConfigList<NodoPythonAst>(false, listaExp, exp, ",");
//            listaExp.Rule = exps 
//                            | Empty;

            




            sentenciaWhile.Rule = produccion(@while + exp + dosp  + Eos+ bloqueLocal) ._accionP(v => new PNodoWhile(v[1], v[3]));
            

            sentenciaIf.Rule =  produccion(@if + exp + dosp + Eos + bloqueLocal)._accionP(v => new PNodoIf(v[1], v[3]))
                               | produccion(@if + exp + dosp + Eos + bloqueLocal + @else + dosp + Eos + bloqueLocal)._accionP(v => new PNodoIf(v[1], v[3], v[6]))
                               | produccion(@if + exp + dosp + Eos + bloqueLocal + elif +sentenciaElif)._accionP(v => new PNodoIf(v[1], v[3], new List<NodoPythonAst>() { v[5] } ))
                ;
            
            sentenciaElif.Rule =  produccion(exp + dosp + Eos + bloqueLocal)._accionP(v => new PNodoIf(v[0], v[2]))
                                 | produccion(exp + dosp + Eos + bloqueLocal + @else + dosp + Eos + bloqueLocal)._accionP(v => new PNodoIf(v[0], v[2], v[5]))
                                 | produccion(exp + dosp + Eos + bloqueLocal + elif +sentenciaElif)._accionP(v => new PNodoIf(v[0], v[2], new List<NodoPythonAst>() { v[4] } ))
                ;



            sentenciaReturn.Rule =  produccion(@return + exp)._accionP(v => new PNodoReturn(v[1]));

            sentenciaFor.Rule =
                @for + exp + dosp + Eos + bloqueLocal //exps -> xq aveces puede estar iterando diccionarios
                | @for + exp + dosp + Eos + bloqueLocal + @else+dosp + Eos + bloqueLocal; // hay un for con else :(
                    //me faltaria el for para el que se usa parr recorre los diccionarios    



                    exp.Rule = produccion(id)._accionP(v => new PNodoBuscarId(v[0]))
                               | produccion(@decimal)._accionP(v => new PNodoCrearDouble(v[0]) )
                               | produccion(numero)._accionP(v => new PNodoCrearEntero(v[0]))
                               
                               | produccion(cadena)._accionP(v => new PNodoCrearCadena(v[0]) )
                               | produccion(cadena2)._accionP(v => new PNodoCrearCadena(v[0]) )
                               | produccion(@true)._accionP(v => new PNodoCrearBooleano(1))
                               | produccion(@false)._accionP(v => new PNodoCrearBooleano(0))
                               
                               | produccion(exp + punto + id)._accionP(v => new PNodoAccessoPunto(v[0], v[2]))

                               | invocacionPunto
                               | invocacion

                               | produccion(exp + igualIgual + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "igualIgualGeneral"))
                               | produccion(exp + diferenteQue + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "diferenteQueGeneral"))

                               | produccion(exp + mayor + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "mayorGeneral"))
                               | produccion(exp + menor + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "menorGeneral"))
                               | produccion(exp + mayorIgualQue + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "mayorIgualQueGeneral"))
                               | produccion(exp + menorIgualQue + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "menorIgualQueGeneral"))

                               | produccion(exp + mas + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "masGeneral"))
                               | produccion(exp + menos + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "menosGeneral"))
                               | produccion(exp + por + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "porGeneral"))
                               | produccion(exp + div + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "divGeneral"))
                               | produccion(exp + divDoble + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "divDobleGeneral"))

                               | produccion(exp + or + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "orGeneral"))
                               | produccion(exp + and + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "andGeneral"))

                               | produccion(exp + isNot + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "isNotGeneral"))



                               | produccion(exp + @in + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "inGeneral"))
                               | produccion(exp + @is + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "isGeneral"))
                               | produccion(exp + notIn + exp)
                                   ._accionP(v => new PNodoOperacionBinaria(v[0], v[2], "notInGeneral"))


                               | produccion(exp + andBinario + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "andBinarioGenral"))
                               | produccion(exp + orBinario + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "orBinarioGeneral"))
                               | produccion(exp + xorBinario + exp)._accionP(v =>
                                   new PNodoOperacionBinaria(v[0], v[2], "xorBinarioGeneral"))
                               
                               
                               | produccion(None)._accionP(v=>new PnodoCrearNull())
                               
                               | produccion(menos + exp + ReduceHere())._accionP(v => new PNodoOperacionUnaria(v[1], "uMenosGEneral"))
                               
                               | produccion(mas + exp + ReduceHere())._accionP(v => new PNodoOperacionUnaria(v[1], "uMasGeneral"))
                               
                               | produccion(not + exp)._accionP(v => new PNodoOperacionUnaria(v[1], "notGeneral"))
                               
                               | produccion( @int + apar + exp   + cpar)._accionP(v => new PNodoOperacionUnaria(v[2], "castIntGeneral"))
//                               | produccion( @bool + apar + exp + cpar )._accionP(v => new PNodoOperacionUnaria(v[2], "castStrGeneral"))
//                               | produccion( chr + apar + exp + cpar)._accionP(v => new PNodoOperacionUnaria(v[2], "notGeneral"))
                               | produccion( @float + apar + exp + cpar)._accionP(v => new PNodoOperacionUnaria(v[2], "castFloatGeneral"))
                               | produccion( str + apar + exp + cpar )._accionP(v => new PNodoOperacionUnaria(v[2], "castStrGeneral"))

//                               | produccion(exp + acor + exp + ccor)
                               
                ;
                    
                    
                    
                    
                    
            var asdf = new AutoNonTerminal();
            asdf.Rule = produccion(id)._accionP(v => new PNodoBuscarId(v[0]))
                        | produccion(numero)._accionP(v => new PNodoCrearEntero(v[0]))



                        | @new + id + apar + listaExp + cpar  //alparecer ni se usa
                       
                       




                       | exp + shiftLet + exp
                       | exp + shiftRight + exp

                        

                       | apar + exp + cpar
                       | sentenciaOpTernario
                       | sentenciaLambda
                       
                       | type + apar + exp + cpar //devuele el tipo de loq ue se lemanda
                       | type + apar + exp+ coma + exp + coma + exp + cpar //dice que devuele un nuevo tipo de objeto
                       | len  + apar + exp  + cpar

                       | @int + apar + exp + coma + exp + cpar
                       | input + apar + cpar
                       
                       

                       
                ;
            
            
            //nose si agregar la funcion range, enumerate, dic, rservate, sorted


            invocacion.Rule = produccion(id + apar + listaExp + cpar)._accionP(v => new PNodoInvocar(v[0], v[2]))
                              | produccion("@" + id + apar + listaExp + cpar)._accionP(v => new PNodoInvocacionHope(v[1], v[3]))
                ;

            
            invocacionPunto.Rule = produccion(exp + punto + id + apar + listaExp + cpar)._accionP(v => new PNodoInvocacionPunto(v[0], v[2], v[4]))
                ;

            sentenciaImport.Rule = produccion(import + cadena)._accion(v => new PNodoImport(v[1]))
                                   | produccion(import + cadena2)._accion(v => new PNodoImport(v[1]))
                ;

            arrayInit.Rule = acor + cuerpoArray + ccor           //listas
                             | apar + cuerpoArray + cpar        //tuplas y secuencias
                             | alla + cuerpoArray + clla        //conjuntos
                             | alla + cuerpoDiccionario + clla; //diccionarios

            cuerpoDiccionario.Rule = elementosDiccionario
                                     | Empty;
            
            elementosDiccionario.Rule = elementosDiccionario + coma +elementoDiccionario
                                        | elementoDiccionario;

            elementoDiccionario.Rule = exp + dosp + exp;


            cuerpoArray.Rule = arrayElementos
                               | Empty;

            arrayElementos.Rule = arrayElementos + coma+ arrayElemento
                                  | arrayElemento;

            arrayElemento.Rule = exp
                                 | arrayInit; //no se si agregar : [:]

            sentenciaDel.Rule = del +exp;

            sentenciaOpTernario.Rule = exp + @if + exp + @else + exp;

            sentenciaLambda.Rule = lambda + parametros + dosp + exp; 

            //----------------------------------------------------------------------------------

            
            RegisterOperators(16, Associativity.Left, apar, cpar, punto, acor, ccor);
            
            RegisterOperators(15, Associativity.Right, not);
            
            
            RegisterOperators(14, Associativity.Right, pot); 
            


            RegisterOperators(14, Associativity.Right, @new); 

            RegisterOperators(13, Associativity.Left, por, div,modulo, divDoble); 
           
            RegisterOperators(12, Associativity.Left, mas, menos); 
           
            RegisterOperators(11, Associativity.Left, shiftLet, shiftRight); 
            
            RegisterOperators(10, Associativity.Left , mayor, menor, mayorIgualQue, menorIgualQue, isNot, notIn, @in, @is);
            
            RegisterOperators(9, Associativity.Left, igualIgual, diferenteQue);
            
       
            RegisterOperators(8, Associativity.Left, andBinario);
            
            RegisterOperators(7, Associativity.Left, xorBinario);
            
            RegisterOperators(6, Associativity.Left, orBinario);
            
            RegisterOperators(5, Associativity.Left, and);
            
            
            RegisterOperators(4, Associativity.Left, or);
           
            RegisterOperators(3, Associativity.Right, @if,@else);
            
            
            RegisterOperators(2, Associativity.Right, lambda);


            RegisterOperators(1, Associativity.Right, igual,masIgual,menosIgual,divIgual,porIgual,modIgual,potIgual,dobleDivIgual);

            // alv :(
    }
           
     
    }
}