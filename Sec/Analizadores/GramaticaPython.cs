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
             var @class = ToTerm("class");
             var print = ToTerm("print");
             var def = ToTerm("def");
             var global = ToTerm("global");
             var nonlocal = ToTerm("nonlocal");

            

            //-----------------------------------------------SIGNOS PELA
            var igual = ToTerm("=");
            var dosp = ToTerm(":");
            var coma = ToTerm(",");
            var pcoma = ToTerm(";");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var acor = ToTerm("[");
            var ccor = ToTerm("]");
        


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

            
         
            var sentenciaLocal = new  NonTerminal("sentenciaLocal");
            var sentenciasLocal = new  NonTerminal("sentenciasLocal");
            var bloqueLocal = new  NonTerminal("bloqueLocal");
            
            var crearMetodo = new  NonTerminal("crearMetodo");
            var sentenciaPrint = new  NonTerminal("sentenciaPrint");
            var listaExp        = new NonTerminal("listaExp");
            var listaId         = new NonTerminal("listaId");
            var parametros      = new NonTerminal("parametros");
            var sentenciaAlcance = new NonTerminal("sentenciaAlcance");

            
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
                                  | sentenciaPrint + Eos
                                  | crearMetodo
                                  | sentenciaAlcance + Eos;
             



            crearMetodo.Rule = def + id + apar + parametros + cpar + dosp + Eos + bloqueClase ;
            
            sentenciaPrint.Rule = print + apar + listaExp + cpar; 
            
            asignar.Rule = exp + igual + exp;

            sentenciaAlcance.Rule = global + exp
                                    | nonlocal + exp;

            

            listaExp.Rule = listaExp + coma + exp
                            | exp;


            parametros.Rule = listaId
                              | Empty;

            listaId.Rule = listaId + coma + id
                           | id;

            exp.Rule = exp + ToTerm("+") + exp
                       | exp + ToTerm("-") + exp
                       | exp + ToTerm("*") + exp
                       | exp + ToTerm("/") + exp
                       | id
                       | numero
                       | @decimal
                       | cadena;

                

            
            //----------------------------------------------------------------------------------

            RegisterOperators(1, Associativity.Left, "+", "-");             //MAS, MENOS
            RegisterOperators(2, Associativity.Left, "*", "/"); 

            this.MarkPunctuation("(",")");
            this.MarkPunctuation(";");

    }
           
     
    }
}