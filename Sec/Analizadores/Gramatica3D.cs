
using Irony.Parsing;

namespace Sec.Analizadores
{
    public class Gramatica3D : Grammar
    {
          public Gramatica3D():  base(caseSensitive: true)
        {
            
            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/*", "*/");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);

            //-------------------------------------------------------------------------------------------------palabras resevadas
            MarkReservedWords("%f");
            MarkReservedWords("%i");
            MarkReservedWords("%c");
            MarkReservedWords("print");
            MarkReservedWords("if");
            MarkReservedWords("then");
            MarkReservedWords("goto");
            MarkReservedWords("proc");
            MarkReservedWords("begin");
            MarkReservedWords("end");
            MarkReservedWords("call");
            MarkReservedWords("dec");
            MarkReservedWords("var");
            MarkReservedWords("stack");
            MarkReservedWords("heap");
            MarkReservedWords("ifFalse");
            MarkReservedWords("$$_clean_scope");

            var pd = ToTerm("%f");
            var pi = ToTerm("%i");
            var pc = ToTerm("%c");
            var print = ToTerm("print");
            var iff = ToTerm("if");
            var goTo = ToTerm("goto");
            var proc = ToTerm("proc");
            var begin = ToTerm("begin");
            var end = ToTerm("end");
            var call = ToTerm("call");
            var varr = ToTerm("var");
            var stack = ToTerm("stack");
            var heap = ToTerm("heap");
            var ifFalse = ToTerm("ifFalse");
            var clean = ToTerm("$$_clean_scope");
            var input = ToTerm("input");

            
            

            //-----------------------------------------------SIGNOS PELA
            var igual = ToTerm("=");
            var dosp = ToTerm(":");
            var coma = ToTerm(",");
            var pcoma = ToTerm(";");
            var apar = ToTerm("(");
            var cpar = ToTerm(")");
            var acor = ToTerm("[");
            var ccor = ToTerm("]");
            var mas = ToTerm("+");
            var menos = ToTerm("-");
            var por = ToTerm("*");
            var div = ToTerm("/");
            var mayor = ToTerm(">");
            var menor = ToTerm("<");
            var mayorIgual = ToTerm(">=");
            var menorIgual = ToTerm("<=");
            var diferente = ToTerm("!=");
            var igualIgual = ToTerm("==");
            var comilla = ToTerm("\"");




            //-------------------------------------------------------------NO TERMINALES
            var INICIO = new NonTerminal("INICIO");
            var assigment = new NonTerminal("assigment");
            var op = new NonTerminal("op");
            var literal = new NonTerminal("literal");
            
            var inconditionalJump = new NonTerminal("inconditionalJump");
            var destiniOfJump = new NonTerminal("destiniOfJump");
            var conditionalJump = new NonTerminal("conditionalJump");
            var relationalOperation = new NonTerminal("relationalOperation");
            var conditionalJumpIfFalse = new NonTerminal("conditionalJumpIfFalse");
            var methodDeclaration = new NonTerminal("methodDeclaration");
            var methodInvocation = new NonTerminal("methodInvocation");
            var methodBody = new NonTerminal("methodBody");
            var sentenciaMetodo = new NonTerminal("sentenciaMetodo");
            var sentenciasMetodo = new NonTerminal("sentenciasMetodo");

            var cleanScope = new NonTerminal("cleanScope");
            

            
            var printStatement = new NonTerminal("printStatement");
            var varDeclaration = new NonTerminal("varDeclaration");

            var idList = new NonTerminal("idList");
            
           
            var sentencia = new NonTerminal("sentencia");
            var sentencias = new NonTerminal("sentencias");
            var listaSentencia = new  NonTerminal("listaSentencia");
            
            
            var inputStatement = new  NonTerminal("inputStatement");


            
            //---------------------> ER
           
            RegexBasedTerminal numero = new RegexBasedTerminal("numero", "[0-9]+");
            RegexBasedTerminal decimall = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            RegexBasedTerminal id = new RegexBasedTerminal("id", "[A-Za-z]([A-Za-z|_|0-9]+)?");


            
            //----------------------------------------------------------------------------------

            this.Root = INICIO;

            INICIO.Rule = listaSentencia;

            listaSentencia.Rule = sentencias
                                    |Empty;

            sentencias.Rule = sentencias + sentencia
                              | sentencia
                ;

            sentencia.Rule = sentenciaMetodo
                             | methodDeclaration
                             | varDeclaration;





            assigment.Rule = id + igual + op + pcoma
                             | stack + acor + literal + ccor + igual + literal + pcoma
                             | heap + acor + literal + ccor + igual + literal + pcoma
                             | id + igual + heap + acor + literal + ccor + pcoma
                             | id + igual + stack + acor + literal + ccor + pcoma
                ;
                

                            
                
            
            destiniOfJump.Rule = id + dosp;
            
            inconditionalJump.Rule = goTo + id + pcoma;

            conditionalJump.Rule = iff + apar + relationalOperation + cpar + goTo+id+pcoma;

            conditionalJumpIfFalse.Rule = ifFalse + apar + relationalOperation + cpar + goTo + id + pcoma;

            methodInvocation.Rule = call + id + pcoma;

            varDeclaration.Rule = varr + stack + acor + ccor + pcoma
                                  | varr + heap + acor + ccor + pcoma
                                  | varr + idList + pcoma
                                  | varr + id + igual + literal + pcoma;
                ;

            
            
            printStatement.Rule = print + apar +comilla + pc + comilla + coma +literal+ cpar + pcoma
                                  |print + apar +comilla + pd + comilla + coma +literal+ cpar + pcoma
                                  |print + apar +comilla + pi + comilla + coma +literal+ cpar + pcoma

                ;

            cleanScope.Rule = clean + apar + literal + coma + literal + cpar + pcoma;


            methodDeclaration.Rule = proc + id + begin + methodBody+ end;

            methodBody.Rule= sentenciasMetodo
                            | Empty;

            sentenciasMetodo.Rule = sentenciasMetodo + sentenciaMetodo
                                    | sentenciaMetodo
                ;

            inputStatement.Rule = input + apar + literal +cpar + pcoma;

            sentenciaMetodo.Rule = 
                                    printStatement
                                   | methodInvocation
                                   | assigment
                                   | destiniOfJump
                                   | inconditionalJump
                                   | conditionalJump
                                   | conditionalJumpIfFalse
                                   | cleanScope 
                                   | inputStatement
                                    ;




            idList.Rule = idList + coma + id
                          | id
                ;
            



            relationalOperation.Rule = literal + mayor + literal
                      | literal + menor + literal
                      | literal + mayorIgual + literal
                      | literal + menorIgual + literal
                      | literal + igualIgual + literal
                      | literal + diferente + literal
                ;


            op.Rule = literal + mas + literal
                      | literal + menos + literal
                      | literal + por + literal
                      | literal + div + literal
                      
                      | literal
                ;


            

            literal.Rule = numero
                            | id
                            | decimall
                ;
            
            //----------------------------------------------------------------------------------


            this.MarkPunctuation("(",")");
            this.MarkPunctuation(";");
          


        }
        
        
        
    }
}