using System.Collections.Generic;
using Irony.Ast;
using Irony.Parsing;
using static Sec.Analizadores.IronyUtils;

namespace Sec.Analizadores
{
    public class GramaticaPython: Grammar
    {
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
        
           public GramaticaPython():  base(caseSensitive: true)
        {
            LanguageFlags |= LanguageFlags.CreateAst;
            
            
//            AstContext

            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/*", "*/");
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
             MarkReservedWords("self");
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
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);
            
            var numero = new NumberLiteral("Entero", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            numero._accion((v, n) => n.Token.Value);
            var @decimal = new NumberLiteral("Decimal");
            @decimal._accion((v, n) => n.Token.Value);

            var id = new IdentifierTerminal("Identificador");
            id._accion((v, n) => n.Token.Value);

            cadena._accion((v, n) => n.Token.Value);

            
            //----------------------------------------------------------------------------------

            this.Root = INICIO;
            

            INICIO.Rule = listaSentencia;
//            INICIO.Rule = asignar + Eos;
            
            
            
            this.ConfigList<NodoPythonAst>(true, listaSentencia, sentencia);
            

            sentencia.Rule = sentenciaLocal
//                asignar + Eos
//                             | crearMetodo
//                             | crearClase
//                             | sentenciaImport + Eos
//                             | sentenciasLocales
                ;

//            crearClase.Rule = @class + id + dosp +Eos+ bloqueClase;
            
            crearClase.Rule = produccion(@class + id + dosp +Eos+ bloqueLocal)
                    ._accion(v => new PNodoCrearClase(v[1], v[3]))
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
                    ._accion(v => new PNodoCrearMetodo(v[1], v[3], v[6]))
                ;
            
            var parametro = new AutoNonTerminal();
            parametro.Rule = produccion(id)._accion(v => v[0]);

            this.ConfigList<string>(false, parametros, parametro, ",");
            



            
            //bloque con las sentencias locales
            bloqueLocal.Rule = produccion(Indent + sentenciasLocales + Dedent)._accion(v => v[0])
                               | produccion(Empty)._accion(v => new List<NodoPythonAst>())
                ;
            
            
            this.ConfigList<NodoPythonAst>(true, sentenciasLocales, sentenciaLocal);


            sentenciaLocal.Rule = asignar + Eos
                                  | crearMetodo
                                  | crearClase
                                  | exp + Eos
//                                  | sentenciaPrint + Eos
//                                  | sentenciaAlcance + Eos
//                                  | sentenciaWhile
//                                  | sentenciaIf
//                                  | sentenciaReturn + Eos
//                                  | sentenciaFor
//                                  | sentenciaDel + Eos
//                                  | exp + Eos //llamadas a metodos objetos
//                                  | pass + Eos
//                                  | @continue + Eos
//                                  | @break + Eos
                ;






            sentenciaPrint.Rule = print + apar + listaExp + cpar;

            asignar.Rule = produccion(exps + igual + exps)._accion(v => new PNodoAsignacion(v[0], v[2]))
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

            




            sentenciaWhile.Rule = @while + exp + dosp  + Eos+ bloqueLocal ;
            

            sentenciaIf.Rule = @if + exp + dosp + Eos + bloqueLocal
                               | @if + exp + dosp + Eos + bloqueLocal + @else + dosp + Eos + bloqueLocal
                               | @if + exp + dosp + Eos + bloqueLocal + elif +sentenciaElif;
            
            sentenciaElif.Rule =  exp + dosp + Eos + bloqueLocal
                                 | exp + dosp + Eos + bloqueLocal + @else + dosp + Eos + bloqueLocal
                                 | exp + dosp + Eos + bloqueLocal + elif +sentenciaElif;



            sentenciaReturn.Rule = @return + exp;

            sentenciaFor.Rule =
                @for + exp + dosp + Eos + bloqueLocal //exps -> xq aveces puede estar iterando diccionarios
                | @for + exp + dosp + Eos + bloqueLocal + @else+dosp + Eos + bloqueLocal; // hay un for con else :(
                    //me faltaria el for para el que se usa parr recorre los diccionarios    


                    
            exp.Rule = produccion(id)._accion(v => new PNodoBuscarId(v[0]))
                | produccion(numero)._accion(v => new PNodoCrearEntero(v[0]))
                | invocacion
                ;
                    
            var asdf = new AutoNonTerminal();
            asdf.Rule = exp + mas + exp
                       | exp + menos + exp
                       | exp + por + exp
                       | exp + div + exp
                       | exp + divDoble + exp


                       | exp + igualIgual + exp
                       | exp + diferenteQue + exp




                       | exp + mayor + exp
                       | exp + menor + exp
                       | exp + mayorIgualQue + exp
                       | exp + menorIgualQue + exp

                       | exp + or + exp
                       | exp + and + exp
                       | not + exp
                       | exp + isNot + exp

                       | exp + notIn + exp

                       | exp + andBinario + exp
                       | exp + orBinario + exp
                       | exp + xorBinario + exp


                       | self //es como el this

                       | produccion(id)._accion(v => new PNodoBuscarId(v[0]))
                       | produccion(numero)._accion(v => new PNodoCrearEntero(v[0])) 
                       | @decimal
                       | cadena
                       | @true
                       | @false
                       | None


                       | invocacionPunto
                       | invocacion
                       | exp + punto + id
                       | @new + id + apar + listaExp + cpar  //alparecer ni se usa
                       
                       
                       | @bool + apar + exp + cpar
                       | chr + apar + exp + cpar
                       | @float + apar + exp + cpar
                       | str + apar + exp + cpar //falta la fncion type
                       

                       | exp + acor + pot + exp + ccor

                       | exp + shiftLet + exp
                       | exp + shiftRight + exp





                       | apar + exp + cpar
                       | sentenciaOpTernario
                       | sentenciaLambda
                       
                       | type + apar + exp + cpar //devuele el tipo de loq ue se lemanda
                       | type + apar + exp+ coma + exp + coma + exp + cpar //dice que devuele un nuevo tipo de objeto
                       | len  + apar + exp  + cpar
                       | @int + apar + exp   + cpar
                       | @int + apar + exp + coma + exp + cpar
                       | input + apar + cpar
                       
                       
                       | exp +@in +exp
                       | exp + @is + exp 
                       
                ;
            
            
            //nose si agregar la funcion range, enumerate, dic, rservate, sorted


            invocacion.Rule = produccion(id + apar +listaExp + cpar)._accion(v => new PNodoInvocar(v[0], v[2]))
                ; 


                       

            invocacionPunto.Rule = exp + punto+ id+ apar +listaExp +cpar;

            sentenciaImport.Rule = import + cadena;

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