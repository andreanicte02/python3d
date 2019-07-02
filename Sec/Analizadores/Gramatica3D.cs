
using System;
using Irony.Parsing;
using Sec.Interprete3D;
using Sec.Interprete3D.Instruccion;
using static Sec.Analizadores.IronyUtils;

namespace Sec.Analizadores
{
    public class Gramatica3D : Grammar
    {
          public Gramatica3D():  base(caseSensitive: true)
        {
            LanguageFlags |= LanguageFlags.CreateAst;
            
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
            var INICIO = new AutoNonTerminal("INICIO");
            var assigment = new AutoNonTerminal("assigment");
            var op = new AutoNonTerminal("op");
            var literal = new AutoNonTerminal("literal");
            
            var inconditionalJump = new AutoNonTerminal("inconditionalJump");
            var destiniOfJump = new AutoNonTerminal("destiniOfJump");
            var conditionalJump = new AutoNonTerminal("conditionalJump");
            var relationalOperation = new AutoNonTerminal("relationalOperation");
            var conditionalJumpIfFalse = new AutoNonTerminal("conditionalJumpIfFalse");
            var methodDeclaration = new AutoNonTerminal("methodDeclaration");
            var methodInvocation = new AutoNonTerminal("methodInvocation");
            var methodBody = new AutoNonTerminal("methodBody");
            var sentenciaMetodo = new AutoNonTerminal("sentenciaMetodo");
            var sentenciasMetodo = new AutoNonTerminal("sentenciasMetodo");

            var cleanScope = new AutoNonTerminal("cleanScope");
            

            
            var printStatement = new AutoNonTerminal("printStatement");
            var varDeclaration = new AutoNonTerminal("varDeclaration");

            var idList = new AutoNonTerminal("idList");
            
           
            var sentencia = new AutoNonTerminal("sentencia");
            var sentencias = new AutoNonTerminal("sentencias");
            var listaSentencia = new  AutoNonTerminal("listaSentencia");
            
            
            var inputStatement = new  AutoNonTerminal("inputStatement");


            var idStatment = new  AutoNonTerminal("idStatment");

            
            //---------------------> ER
           
//            var numero = new RegexBasedTerminal("numero", "[0-9]+");
//            numero._accion((values, node) => node.Token)

            var numero = new NumberLiteral("Entero", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            numero._accion((v, n) => n.Token.Value);
            
            var decimall = new NumberLiteral("Decimal");
            decimall._accion((v, n) => n.Token.Value);


//            RegexBasedTerminal decimall = new RegexBasedTerminal("decimal", "[0-9]+[.][0-9]+");
            RegexBasedTerminal id = new RegexBasedTerminal("id", "[A-Za-z]([A-Za-z|_|0-9]+)?");
            id._accion((values, node) => node.Token.Text);


            
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

            

            //la accion que se registra solo a no termianles
            //asignacion debe tener obligatoriamente metodo nodecreathor
            assigment.Rule = produccion(id + igual + op + pcoma)._accion(values =>
                             {
                                 Console.WriteLine("Se encontro una asignacion ");
                                 Console.WriteLine(values[0]);
                                 Console.WriteLine(values[2]);
                                 
                                 //---------------------------------------------->
                                 
                                 return null;
                             })
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
                                  | produccion( varr + idList + pcoma)._accion(values => new NodoDeclararVars(null))
                                  | varr + id + igual + literal + pcoma;
                

            
            
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

            

            this.ConfigList<Instruccion3d>(false,idList, idStatment,",");
            
            idList.Rule = produccion(idList + coma + idStatment)
                          | produccion(idStatment)
                ;

            idStatment.Rule = produccion(id)._accion(values=> new NodoDeclararVar(values[0]));
            



            relationalOperation.Rule = produccion(literal + mayor + literal)._accion(values => new NodoMayor(values[0], values[2]))
                      | produccion(literal + menor + literal)._accion(values => new NodoMenor(values[0], values[2]))
                      | produccion(literal + mayorIgual + literal)._accion(values => new NodoMayorIgual(values[0], values[2]))
                      | produccion(literal + menorIgual + literal)._accion(values => new NodoMenorIgual(values[0], values[2]))
                      | produccion(literal + igualIgual + literal)._accion(values => new NodoIgualQue(values[0], values[2]))
                      | produccion(literal + diferente + literal)._accion(values => new NodoDifernteQue(values[0], values[2]))
                ;


            op.Rule =  produccion(literal + mas + literal)._accion(values=> new NodoSuma(values[0], values[2]))
                      | produccion(literal + menos + literal)._accion(values=> new NodoResta(values[0], values[2]))
                      | produccion(literal + por + literal)._accion(values=> new NodoMultiplicacion(values[0], values[2]))
                      | produccion(literal + div + literal)._accion(values=> new NodoDivision(values[0], values[2]))
                      | produccion(literal)._accion(values => values[0]) //se sube el dato
                ;


            

            literal.Rule = produccion(numero)._accion(values => new NodoCrearNumero(values[0]))
                            | produccion(id)._accion(values => new NodoBuscarId(values[0]))
                            | produccion(decimall)._accion(values => new NodoCrearDecimal(values[0]))
                ;
            
            //----------------------------------------------------------------------------------


            this.MarkPunctuation("(",")");
            this.MarkPunctuation(";");
          


        }
        
        
        
    }
}