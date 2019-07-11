
using Irony.Parsing;
using Sec.Interprete3D;
using Sec.Interprete3D.Aritmeticas;
using Sec.Interprete3D.Declaraciones;
using Sec.Interprete3D.intefaces;
using Sec.Interprete3D.Primitivos;
using Sec.Interprete3D.Print;
using Sec.Interprete3D.Relacionales;
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
            MarkReservedWords("%d");
            MarkReservedWords("%e");
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

            var pf = ToTerm("%d");
            var pe = ToTerm("%e");
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
            var mod = ToTerm("%");
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
            var sentenciaMetodo = new AutoNonTerminal("sentenciaMetodo");
            var sentenciasMetodo = new AutoNonTerminal("sentenciasMetodo");

            var cleanScope = new AutoNonTerminal("cleanScope");
            

            
            var printStatement = new AutoNonTerminal("printStatement");
            var varDeclaration = new AutoNonTerminal("varDeclaration");

            var idList = new AutoNonTerminal("idList");
            
           
            var sentencia = new AutoNonTerminal("sentencia");
            var sentencias = new AutoNonTerminal("sentencias");
            
            
            var inputStatement = new  AutoNonTerminal("inputStatement");


            var idStatment = new  AutoNonTerminal("idStatment");

            
            //---------------------> ER
           
            var decimall = new NumberLiteral("Decimal");
            decimall._accion((v, n) => n.Token.Value);
            
            RegexBasedTerminal id = new RegexBasedTerminal("id", "[A-Za-z]([A-Za-z|_|0-9]+)?");
            id._accion((values, node) => node.Token.Text);


            
            //----------------------------------------------------------------------------------
            

            INICIO.Rule = produccion(sentencias)._acciond3D(0,values=> values[0]);

     
            
            this.ConfigList<nodo3d>(false,sentencias,sentencia);


            sentencia.Rule = produccion(sentenciaMetodo)._acciond3D(0,values => values[0])
                             | produccion(methodDeclaration)._acciond3D(0,values => values[0])
                             | produccion(varDeclaration)._acciond3D(0,values => values[0]);

            

            //la accion que se registra solo a no termianles
            //asignacion debe tener obligatoriamente metodo nodecreathor
            assigment.Rule = produccion(id + igual + op + pcoma)._acciond3D(1,values =>
                             {
                                 //Console.WriteLine($"Reconoci una asignacion para {values[0]} ");
                                 return new asignar(values[0], values[2]);
                             })
                             | produccion(stack + acor + literal + ccor + igual + literal + pcoma)._acciond3D(0,values=> new stack_id(values[2],values[5]))
                             | produccion(heap + acor + literal + ccor + igual + literal + pcoma)._acciond3D(0,values => new heap_id(values[2],values[5]))
                             | produccion(id + igual + heap + acor + literal + ccor + pcoma)._acciond3D(1,values=> new id_heap(values[0], values[4]))
                             | produccion(id + igual + stack + acor + literal + ccor + pcoma)._acciond3D(1,values=> new id_stack(values[0],values[4]))
                ;
                

                            
                
            
            destiniOfJump.Rule = produccion(id + dosp)._acciond3D(1,values => new nodoLabel(values[0]));
            
            inconditionalJump.Rule = produccion(goTo + id + pcoma)._acciond3D(0,values => new goTo(values[1]));

            conditionalJump.Rule = produccion(iff + apar + relationalOperation + cpar + goTo+id+pcoma)._acciond3D(0,values=> new nodoIf(values[2], new goTo(values[5])));

            //onditionalJumpIfFalse.Rule = produccion(ifFalse + apar + relationalOperation + cpar + goTo + id + pcoma)._acciond3D(0,values => new NodoIfFalse(values[2],new NodoJumpLabel(values [5])));

            methodInvocation.Rule = produccion(call + id + pcoma)._acciond3D(0,values=> new nodoCallMetodo(values[1]));

            varDeclaration.Rule = 
                                   produccion( varr + idList + pcoma)._acciond3D(0,values => new DeclararListaId(values[1]))
                                  | produccion(varr + id + igual + literal + pcoma)._acciond3D(0,values => new DeclararVariable(values[1],values[3]));
                

            
            
            printStatement.Rule = produccion(print + apar +comilla + pc + comilla + coma +literal+ cpar + pcoma)._acciond3D(0,values => new printC(values[6]))
                                  |produccion(print + apar +comilla + pf + comilla + coma +literal+ cpar + pcoma)._acciond3D(0,values => new printF(values[6]))
                                  |produccion(print + apar +comilla + pe + comilla + coma +literal+ cpar + pcoma)._acciond3D(0,values => new printI(values[6]))

                ;

            cleanScope.Rule = clean + apar + literal + coma + literal + cpar + pcoma;


            methodDeclaration.Rule = produccion(proc + id + begin + sentenciasMetodo+ end)._acciond3D(0,values => new DeclararMetodo(values[1],values[3]) );


            
            this.ConfigList<nodo3d>(false,sentenciasMetodo,sentenciaMetodo);
            

            inputStatement.Rule = input + apar + literal +cpar + pcoma;

            sentenciaMetodo.Rule = 
                                    produccion(printStatement)._acciond3D(0,values => values[0])
                                   | produccion(methodInvocation)._acciond3D(0,values => values[0])
                                   | produccion(assigment)._acciond3D(0,values=> values[0])
                                   | produccion(destiniOfJump)._acciond3D(0,values => values [0])
                                   | produccion(inconditionalJump)._acciond3D(0,values => values [0])
                                   | produccion(conditionalJump)._acciond3D(0,values => values [0])
                                   //| produccion(conditionalJumpIfFalse)._acciond3D(0,values => values[0])
                                   | cleanScope 
                                   | inputStatement
                                    ;

            

            this.ConfigList<NodoID>(true,idList,idStatment,",");
     
            
            idStatment.Rule = produccion(id)._acciond3D(0,values=> new NodoID(values[0]));
            



            relationalOperation.Rule = produccion(literal + mayor + literal)._acciond3D(1,values => new mayor(values[0], values[2]))
                      | produccion(literal + menor + literal)._acciond3D(1,values => new menor(values[0], values[2]))
                      | produccion(literal + mayorIgual + literal)._acciond3D(1,values => new mayorIgual(values[0], values[2]))
                      | produccion(literal + menorIgual + literal)._acciond3D(1,values => new menorIgual(values[0], values[2]))
                      | produccion(literal + igualIgual + literal)._acciond3D(1,values => new igualIgual(values[0], values[2]))
                      | produccion(literal + diferente + literal)._acciond3D(1,values => new diferente(values[0], values[2]))
                ;


            op.Rule =  produccion(literal + mas + literal)._acciond3D(1,values=> new suma(values[0], values[2]))
                      | produccion(literal + menos + literal)._acciond3D(1,values=> new resta(values[0], values[2]))
                      | produccion(literal + por + literal)._acciond3D(1,values=> new mult(values[0], values[2]))
                      | produccion(literal + div + literal)._acciond3D(1,values=> new div(values[0], values[2]))
                      | produccion(literal + mod + literal)._acciond3D(1,values => new modu(values[0], values[2]))
                      | produccion(literal)._acciond3D(1,values => values[0]) //se sube el dato
                ;


            

            literal.Rule = produccion(decimall)._acciond3D(0,values => new NodoNum( values[0]))
                            | produccion(id)._acciond3D(0,values => new NodoBuscarId(values[0]))
                            | produccion(decimall)._acciond3D(0,values => new NodoNum(values[0]))
                ;
            
            
            LanguageFlags |= LanguageFlags.CreateAst;
            Root = INICIO;
            
            //----------------------------------------------------------------------------------


            


        }
        
        
        
    }
}