
using System;
using System.Collections.Generic;
using Irony.Parsing;
using Sec.Interprete3D;
using Sec.Interprete3D.Instruccion;
using Sec.Interprete3D.Instruccion.Imprimir;
using Sec.Interprete3D.Instruccion.Label;
using Sec.Interprete3D.Instruccion.Metodos;
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
            

            INICIO.Rule = produccion(sentencias)._accion(values=> values[0]);

     
            
            this.ConfigList<Instruccion3d>(false,sentencias,sentencia);


            sentencia.Rule = produccion(sentenciaMetodo)._accion(values => values[0])
                             | produccion(methodDeclaration)._accion(values => values[0])
                             | produccion(varDeclaration)._accion(values => values[0]);

            

            //la accion que se registra solo a no termianles
            //asignacion debe tener obligatoriamente metodo nodecreathor
            assigment.Rule = produccion(id + igual + op + pcoma)._accion(values =>
                             {
                                 Console.WriteLine($"Reconoci una asignacion para {values[0]} ");
                                 return new NodoAsignar(values[0], values[2]);
                             })
                             | produccion(stack + acor + literal + ccor + igual + literal + pcoma)._accion(values=> new NodoStackId(values[2],values[5]))
                             | produccion(heap + acor + literal + ccor + igual + literal + pcoma)._accion(values => new NodoAsignarHeapId(values[2],values[5]))
                             | produccion(id + igual + heap + acor + literal + ccor + pcoma)._accion(values=> new NodoAsignarIdHeap(values[0], values[4]))
                             | produccion(id + igual + stack + acor + literal + ccor + pcoma)._accion(values=> new NodoAsingarIdStack(values[0],values[4]))
                ;
                

                            
                
            
            destiniOfJump.Rule = produccion(id + dosp)._accion(values => new NodoLabel(values[0].ToString()));
            
            inconditionalJump.Rule = produccion(goTo + id + pcoma)._accion(values => new NodoJumpLabel(values[0].ToString()));

            conditionalJump.Rule = produccion(iff + apar + relationalOperation + cpar + goTo+id+pcoma)._accion(values=> new NodoIf(values[2], new NodoJumpLabel(values[5])));

            conditionalJumpIfFalse.Rule = produccion(ifFalse + apar + relationalOperation + cpar + goTo + id + pcoma)._accion(values => new NodoIfFalse(values[2],new NodoJumpLabel(values [5])));

            methodInvocation.Rule = call + id + pcoma;

            varDeclaration.Rule = produccion(varr + stack + acor + ccor + pcoma)._accion(values=> new NodoDeclararStack())
                                  | produccion(varr + heap + acor + ccor + pcoma)._accion(values => new NodoDeclararHeap())
                                  | produccion( varr + idList + pcoma)._accion(values => new NodoDeclararVars(values[1]))
                                  | produccion(varr + id + igual + literal + pcoma)._accion(values => new NodoSoloDeclarar(values[1],values[3]));
                

            
            
            printStatement.Rule = produccion(print + apar +comilla + pc + comilla + coma +literal+ cpar + pcoma)._accion(values => new NodoPrintC(values[6]))
                                  |produccion(print + apar +comilla + pd + comilla + coma +literal+ cpar + pcoma)._accion(values => new NodoPrintF(values[6]))
                                  |produccion(print + apar +comilla + pi + comilla + coma +literal+ cpar + pcoma)._accion(values => new NodoPrintI(values[6]))

                ;

            cleanScope.Rule = clean + apar + literal + coma + literal + cpar + pcoma;


            methodDeclaration.Rule = produccion(proc + id + begin + sentenciasMetodo+ end)._accion(values => new NodoDeclararMetodo(values[1],values[3]) );


            
            this.ConfigList<Instruccion3d>(false,sentenciasMetodo,sentenciaMetodo);
            

            inputStatement.Rule = input + apar + literal +cpar + pcoma;

            sentenciaMetodo.Rule = 
                                    produccion(printStatement)._accion(values => values[0])
                                   | produccion(methodInvocation)._accion(values => values[0])
                                   | produccion(assigment)._accion(values=> values[0])
                                   | produccion(destiniOfJump)._accion(values => values [0])
                                   | produccion(inconditionalJump)._accion(values => values [0])
                                   | produccion(conditionalJump)._accion(values => values [0])
                                   | produccion(conditionalJumpIfFalse)._accion(values => values[0])
                                   | cleanScope 
                                   | inputStatement
                                    ;

            

            this.ConfigList<Instruccion3d>(true,idList,idStatment,",");
     
            
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
            
            
            LanguageFlags |= LanguageFlags.CreateAst;
            Root = INICIO;
            
            //----------------------------------------------------------------------------------


            //this.MarkPunctuation("(",")");
            //this.MarkPunctuation(";");
          


        }
        
        
        
    }
}