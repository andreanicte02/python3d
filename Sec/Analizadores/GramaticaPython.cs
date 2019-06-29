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

             
            

            //-----------------------------------------------SIGNOS PELA
           
            var dosp = ToTerm(":");
            var coma = ToTerm(",");
            Terminal apar = ToTerm("(");
            Terminal cpar = ToTerm(")");
            Terminal acor = ToTerm("[");
            Terminal ccor = ToTerm("]");
            //operadores aritmeticos
            Terminal por = ToTerm("*");
            Terminal div = ToTerm("/");
            Terminal mas = ToTerm("+");
            Terminal menos = ToTerm("-");
            Terminal pot = ToTerm("**");
            Terminal divDoble = ToTerm("//");
            Terminal modulo = ToTerm("%");
            
            
            
            Terminal punto = ToTerm(".");
            Terminal igual = ToTerm("=");
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


            

            



            //-------------------------------------------------------------NO TERMINALES
            var INICIO = new NonTerminal("INICIO");
           
            var sentencia = new NonTerminal("sentencia");
            var sentencias = new NonTerminal("sentencias");
            var listaSentencia = new  NonTerminal("listaSentencia");
            
            var asignar = new  NonTerminal("asignar");
            var exp = new  NonTerminal("exp");
            var crearClase = new  NonTerminal("crearClase");
            
            var sentenciaClase = new  NonTerminal("sentenciaClase");
            var sentenciasClases = new  NonTerminal("sentenciasClases");
            var bloqueClase = new  NonTerminal("bloqueClase");
            
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
                            |crearClase;

            crearClase.Rule = @class + id + dosp +Eos+ bloqueClase;

            bloqueClase.Rule = Indent + sentenciasClases + Dedent
                               | Empty;

            
                              

            sentenciasClases.Rule = sentenciasClases + sentenciaClase
                                    | sentenciaClase;

            //es como la de prueba
            sentenciaClase.Rule = asignar + Eos
                                  | crearMetodo;
                           
            //locales

            crearMetodo.Rule = def + id + apar + parametros + cpar + dosp  + Eos+ bloqueLocal ;
            
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
                                  | exp + Eos; //llamadas a metodos objetos
                                    





            sentenciaPrint.Rule = print + apar + listaExp + cpar;
                                 
            asignar.Rule = exp + igual + exp;

            sentenciaAlcance.Rule = global + exp
                                    | nonlocal + exp;
            

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

            sentenciaFor.Rule = @for + exp + @in + exp+ dosp + Eos + bloqueLocal;

            exp.Rule = exp + mas+ exp
                       | exp + menos+ exp
                       | exp + por + exp
                       | exp + div  + exp
                       | exp + divDoble + exp

                       
                       | exp + igualIgual + exp
                       | exp + diferenteQue + exp
                       
                       
                       
                       
                       | exp + mayor + exp
                       | exp + menor + exp
                       | exp + mayorIgualQue + exp
                       | exp + menorIgualQue  + exp
                       
                       | exp + or + exp
                       | exp + and + exp
                       | not + exp
                       | exp + isNot + exp
                       
                       | exp + notIn + exp
                       
                       | exp + andBinario + exp
                       | exp + orBinario + exp
                       | exp  + xorBinario + exp
                        
                       







                       | self //es como el this
                       
                       | id
                       | numero
                       | @decimal
                       | cadena
                       | @true
                       | @false
                       
                       
                       | invocacionPunto
                       | invocacion
                       | exp + punto + id
                       | @new + id + apar + listaExp + cpar
                       | @bool + apar + exp + cpar
                       | chr + apar + exp + cpar
                       | @float + apar + exp + cpar
                       | str + apar + exp + cpar //falta la fncion type

                       
                       | exp + acor + pot + exp + ccor
                       
                       | exp + shiftLet + exp
                       | exp + shiftRight + exp
                       
                       
                       
                       

                       | apar + exp + cpar;

    
            
            invocacion.Rule = id + apar +listaExp + cpar; 


                       

            invocacionPunto.Rule = exp + punto+ id+ apar +listaExp +cpar;

            
            //----------------------------------------------------------------------------------

            
            RegisterOperators(16, Associativity.Left, apar, cpar, punto, acor, ccor);
            
            RegisterOperators(15, Associativity.Right, not);
            
            
            RegisterOperators(14, Associativity.Right, pot); 
            


            RegisterOperators(13, Associativity.Right, @new); 

            RegisterOperators(12, Associativity.Left, por, div,modulo, divDoble); 
           
            RegisterOperators(11, Associativity.Left, mas, menos); 
           
            RegisterOperators(10, Associativity.Left, shiftLet, shiftRight); 
            
            RegisterOperators(9, Associativity.Left , mayor, menor, mayorIgualQue, menorIgualQue, isNot, notIn, @in, @is);
            
            RegisterOperators(7, Associativity.Left, igualIgual, diferenteQue);
            
       
            RegisterOperators(6, Associativity.Left, andBinario);
            
            RegisterOperators(5, Associativity.Left, xorBinario);
            
            RegisterOperators(4, Associativity.Left, orBinario);
            
            RegisterOperators(3, Associativity.Left, and);
            
            
            RegisterOperators(2, Associativity.Left, or);
           
            RegisterOperators(1, Associativity.Right, igual);



    }
           
     
    }
}