using Irony.Parsing;

namespace Sec.Analizadores
{
    public class GramaticaPython: Grammar
    {
   
        public override void CreateTokenFilters(LanguageData language, TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData, OutlineOptions.ProduceIndents |  OutlineOptions.CheckBraces, ToTerm(@"\"));
            filters.Add(outlineFilter);
        }
        
           public GramaticaPython():  base(caseSensitive: true)
        {
            

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
            var INICIO = new NonTerminal("INICIO");
           
            var sentencia = new NonTerminal("sentencia");
            var sentencias = new NonTerminal("sentencias");
            var listaSentencia = new  NonTerminal("listaSentencia");
            
            var asignar = new  NonTerminal("asignar");
            var exp = new  NonTerminal("exp");
            var crearClase = new  NonTerminal("crearClase");
            
            //pdoruccion de sentencias de clase
            var sentenciaClase = new  NonTerminal("sentenciaClase");
            var sentenciasClases = new  NonTerminal("sentenciasClases");
            var bloqueClase = new  NonTerminal("bloqueClase");
            
            //prouccion de sentencias locales
            var sentenciaLocal = new NonTerminal("sentenciaLocal");
            var bloqueLocal = new NonTerminal("bloqueLocal");
            var sentenciasLocales = new NonTerminal("sentenciasLocal");

            
            var crearMetodo = new  NonTerminal("crearMetodo");
            var sentenciaPrint = new  NonTerminal("sentenciaPrint");
            var exps        = new NonTerminal("listaExp");
            var listaId         = new NonTerminal("listaId");
            var parametros      = new NonTerminal("parametros");
            var listaExp     = new NonTerminal("parametrosExp");
            var sentenciaAlcance = new NonTerminal("sentenciaAlcance");
            var sentenciaWhile      = new NonTerminal("sentenciaWhile");
            var sentenciaIf         = new NonTerminal("sentenciaIf");
            var sentenciaElif       = new NonTerminal("sentenciaElif");
            var sentenciaReturn       = new NonTerminal("sentenciaReturn");
            var invocacionPunto       = new NonTerminal("invocacionPunto");
            var invocacion       = new NonTerminal("invocacion");    
            var sentenciaFor       = new NonTerminal("sentenciaFor");
            var sentenciaImport  = new NonTerminal("sentenciaImport");

            
            
            //arrayInit produccion de tuplas, arreglos, diccionarios conjunto
            var arrayInit  = new NonTerminal("arrayInit");
            
            var cuerpoArray  = new NonTerminal("cuerpoArray");
            var arrayElemento  = new NonTerminal("arrayElemento");
            var arrayElementos  = new NonTerminal("arrayElementos");
            
          

            var sentenciaDel  = new NonTerminal("sentenciaDel");
            
            //sentencia para esignar exp = linitAisgnar
            var initAsignar  = new NonTerminal("initAsignar");
            var lInitAsignar =  new NonTerminal("lInitAsignar");
            
            
            //sentencias de diccionario
            var cuerpoDiccionario = new NonTerminal("cuerpoDiccionario");
            var elementosDiccionario = new NonTerminal("elementosDiccionario");
            var elementoDiccionario = new NonTerminal("elementoDiccionario");
            
            
            var sentenciaOpTernario = new NonTerminal("sentenciaOpTernario");

            var sentenciaLambda = new NonTerminal("sentenciaLambda");




            //---------------------> ER
           
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            RegexBasedTerminal @decimal = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            RegexBasedTerminal id = new RegexBasedTerminal("id", "[A-Za-z]([A-Za-z|_|0-9]+)?");
            StringLiteral cadena = new StringLiteral("cadena", "\"", StringOptions.AllowsAllEscapes);

            
            //----------------------------------------------------------------------------------

            this.Root = INICIO;

            INICIO.Rule = listaSentencia;

            listaSentencia.Rule = Empty
                                  | sentencias;

            sentencias.Rule = sentencias + sentencia
                              | sentencia;

            sentencia.Rule = asignar + Eos
                             | crearClase
                             | sentenciaImport + Eos
                             | sentenciasLocales;

            crearClase.Rule = @class + id + dosp +Eos+ bloqueClase;

            bloqueClase.Rule = Indent + sentenciasClases + Dedent
                               | Empty;

            
                              

            sentenciasClases.Rule = sentenciasClases + sentenciaClase
                                    | sentenciaClase;

            //es como la de prueba
            sentenciaClase.Rule = asignar + Eos
                                  | crearMetodo;
                           
            //crear metodo

            crearMetodo.Rule = def + id + apar + parametros + cpar + dosp  + Eos+ bloqueLocal ;
            
            //bloque con las sentencias locales
            bloqueLocal.Rule = Indent + sentenciasLocales + Dedent
                               | Empty;
            
            
            sentenciasLocales.Rule = sentenciasLocales + sentenciaLocal
                                   | sentenciaLocal;

            sentenciaLocal.Rule = asignar + Eos
                                  | sentenciaPrint + Eos
                                  | sentenciaAlcance + Eos
                                  | sentenciaWhile
                                  | sentenciaIf
                                  | sentenciaReturn + Eos
                                  | sentenciaFor
                                  | sentenciaDel + Eos
                                  | exp + Eos //llamadas a metodos objetos
                                  | pass + Eos
                                  | @continue + Eos
                                  | @break + Eos;






            sentenciaPrint.Rule = print + apar + listaExp + cpar;

            asignar.Rule = exps + igual + lInitAsignar
                           | exps + masIgual + lInitAsignar
                           | exps + menosIgual + lInitAsignar
                           | exps + porIgual + lInitAsignar
                           | exps + divIgual + lInitAsignar
                           | exps + modIgual + lInitAsignar
                           | exps + potIgual + lInitAsignar
                           | exps + dobleDivIgual + lInitAsignar;


            lInitAsignar.Rule = lInitAsignar + coma + initAsignar //lista de posibles asignaciones
                                | initAsignar;

            initAsignar.Rule = exp
                               | arrayInit;
                               
            
            

            sentenciaAlcance.Rule = global + exps
                                    | nonlocal + exps;
            

            listaExp.Rule = exps 
                            | Empty;

            exps.Rule = exps + coma + exp
                            | exp;


            parametros.Rule = listaId
                              | Empty;

            listaId.Rule = listaId + coma + id
                           | id;

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

            exp.Rule = exp + mas + exp
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

                       | id
                       | numero
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


            invocacion.Rule = id + apar +listaExp + cpar; 


                       

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