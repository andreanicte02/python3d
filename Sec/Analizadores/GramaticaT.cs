using System;
using System.Collections.Generic;
using Irony.Parsing;
using static Sec.Analizadores.IronyUtils;

namespace Sec.Analizadores
{
    public class GramaticaT : Grammar
    {
        public GramaticaT() : base(true)
        {
            CommentTerminal COMENTARIO_SIMPLE = new CommentTerminal("comentario_simple", "//", "\n", "\r\n");
            CommentTerminal COMENTARIO_MULT = new CommentTerminal("comentario_mult", "/*", "*/");
            NonGrammarTerminals.Add(COMENTARIO_SIMPLE);
            NonGrammarTerminals.Add(COMENTARIO_MULT);
            
            
            var inicio = new AutoNonTerminal();
            
            var sentencias = new AutoNonTerminal();
            var sentencia = new AutoNonTerminal();
            this.ConfigList<NodoAst>(false, sentencias, sentencia);

            var sentenciasFuncion = new AutoNonTerminal();
            var sentenciaFuncion = new AutoNonTerminal();

            var declaracionStruct = new AutoNonTerminal();
            
            var declaracionesPropiedad = new AutoNonTerminal();
            var declaracionPropiedad = new AutoNonTerminal();
            this.ConfigList<NodoAst>(true, declaracionesPropiedad, declaracionPropiedad);
            
            var declaracionFuncion = new AutoNonTerminal();

            var declaracionesParametro = new AutoNonTerminal();
            var declaracionParametro = new AutoNonTerminal();

            var argumentos = new AutoNonTerminal();

            var tipo = new AutoNonTerminal();

            var declaracion = new AutoNonTerminal();
            var asignacion = new AutoNonTerminal();
            var sentenciaIf = new AutoNonTerminal();
            var retorno = new AutoNonTerminal();
            
            var expr = new AutoNonTerminal();
            var invocacion = new AutoNonTerminal();

            var @int = new NumberLiteral("Entero", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            @int._accion((v, n) => n.Token.Value);
            var @double = new NumberLiteral("Decimal");
            @double._accion((v, n) => n.Token.Value);
            var @string = new StringLiteral("String", "\"");
            @string._accion((v, n) => n.Token.Value);
            
            var @char = new StringLiteral("Char", "'");
            @char._accion((v, n) => n.Token.Value);

            var id = new IdentifierTerminal("Identificador");
            id._accion((v, n) => n.Token.Value);

            
            
            // Reglas
            inicio.Rule = produccion(sentencias)._accionT(v => v[0]);
                ;

            sentencia.Rule = produccion(declaracionStruct)._accionT(values => values[0])
                             | produccion(declaracionFuncion)._accionT(values => values[0])
                ;

            declaracionStruct.Rule = produccion("struct" + id + "{" + declaracionesPropiedad + "}")
                                         ._accionT(v => new TNodoDeclaracionStruct(v[1], v[3]))
//                                     | produccion(ToTerm("struct") + "$" + "{" + declaracionesPropiedad + "}")
//                                         ._accionT(v => new TNodoDeclaracionStruct(v[1], v[3]))
                ;
            declaracionPropiedad.Rule = produccion(tipo + id + ";")
                    ._accionT(v => new TNodoDeclaracionVar(v[0], v[1]))
                ;
            
            
            var tipoUnitario = new AutoNonTerminal();

            tipo.Rule = produccion(tipo + "[]")._accionT(v => new TNodoTipoCompuesto(v[0]))
                        | tipoUnitario
                ;
            
            tipoUnitario.Rule = produccion(id)._accionT(v => new TNodoBusquedaTipo(v[0]))
                        | produccion("void")._accionT(v => new TNodoBusquedaTipo("void"))
                        | produccion("int")._accionT(v => new TNodoBusquedaTipo("int"))
                        | produccion("double")._accionT(v => new TNodoBusquedaTipo("double"))
                ;

            declaracionFuncion.Rule =
                produccion(tipo + id + "(" + declaracionesParametro + ")" + "{" + sentenciasFuncion + "}")
                    ._accionT(v => new TNodoDeclaracionFuncion(v[0], v[1], v[3], v[6]))
                ;

            //configuracion de listas
            this.ConfigList<NodoAst>(false, declaracionesParametro, declaracionParametro, ",");
            
            declaracionParametro.Rule = produccion(tipo + id)._accionT(v => new TNodoDeclaracionVar(v[0], v[1]));
                ;

            this.ConfigList<NodoAst>(false, sentenciasFuncion, sentenciaFuncion);
            
            sentenciaFuncion.Rule = produccion(asignacion + ";")._accionT(v => v[0])
                                    | produccion(declaracion + ";")._accionT(v => v[0])
                                    | produccion(retorno + ";")._accionT(v => v[0])
                                    | invocacion + ";"
                                    | sentenciaIf
                                    | produccion(ToTerm("!") + "print" + "(" + id + "," + expr + ")" + ";")._accionT(v => new TNodoPrintSimple(v[3], v[5]))
                                    | produccion("throw" + expr + ";" )._accionT(v => new TNodoThrow(v[1]))
                                    | produccion(ToTerm("while") + "(" + expr + ")" +"{"+ sentenciasFuncion + "}")._accionT(v => new TNodoWhile(v[2], v[5]))
                                    | produccion(ToTerm("do") +"{"+ sentenciasFuncion + "}" + "while" + "(" + expr + ")")._accionT(v => new TNodoDoWhile(v[6], v[2]))
                                    | produccion("break" + ToTerm(";"))._accion(v => new TNodoBreak())
                                    | produccion("continue" + ToTerm(";"))._accion(v => new TNodoContinue())
                                    | produccion(ToTerm("?") + "{" + sentenciasFuncion + "}")._accion(v => new TNodoNoSe(v[2]))
                ;

            declaracion.Rule = produccion(tipo + id + "=" + expr)
                                   ._accionT(v => new TNodoDeclaracionVar(v[0], v[1], v[3]))
                               | produccion(tipo + id)._accionT(v => new TNodoDeclaracionVar(v[0], v[1]))
                ;


            sentenciaIf.Rule = produccion(ToTerm("if") + "(" + expr + ")" + "{" + sentenciasFuncion + "}" + "else" +
                                          "{" + sentenciasFuncion + "}")
                                   ._accionT(v => new TNodoIf(v[2], v[5], v[9]))
                               | produccion(ToTerm("if") + "(" + expr + ")" + "{" + sentenciasFuncion + "}")
                                   ._accionT(v => new TNodoIf(v[2], v[5], new List<NodoAst>()))
                               | produccion(ToTerm("if") + "(" + expr + ")" + "{" + sentenciasFuncion + "}" + "else" +
                                            sentenciaIf)
                                   ._accionT(v => new TNodoIf(v[2], v[5], new List<NodoAst>() {v[8]}))

                ;
            
            asignacion.Rule = produccion(expr + "=" + expr)
                    ._accionT(v => new TNodoAsignacion(v[0], v[2]))
                ;
            retorno.Rule = produccion("return" + expr)._accionT(v => new TNodoReturn(v[1]))
                | produccion("return")._accionT(v => new TNodoReturn() )
                ;


            expr.Rule = produccion(expr + "+" + expr)._accionT(v => new TNodoSuma(v[0], v[2]))
                        | produccion(expr + "-" + expr)._accionT(v => new TNodoResta(v[0], v[2]))
                        | produccion(expr + "*" + expr)._accionT(v => new TNodoMultiplicacion(v[0], v[2]))
                        | produccion(expr + "/" + expr)._accionT(v => new TNodoDivision(v[0], v[2]))
                        | produccion(expr + "%" + expr)._accionT(v => new TNodoModulo(v[0], v[2]))
                        | produccion(expr + "." + id)._accionT(v => new TNodoAccessoPunto(v[0], v[2]))
                        | produccion(expr + "==" + expr)._accionT(v => new TNodoIgualIgual(v[0], v[2]))
                        | produccion(expr + "!=" + expr)._accionT(v => new TNodoNoIgual(v[0], v[2]))
                        | produccion(expr + "&&" + expr)._accionT(v => new TNodoAnd(v[0], v[2]))
                        | produccion(expr + "||" + expr)._accionT(v => new TNodoOr(v[0], v[2]))
                        | produccion(expr + ">" + expr)._accionT(v => new TNodoMayor(v[0], v[2]))
                        | produccion(expr + "<" + expr)._accionT(v => new TNodoMenor(v[0], v[2]))
                        | produccion(expr + ">=" + expr)._accionT(v => new TNodoMayorIgual(v[0], v[2]))
                        | produccion(expr + "<=" + expr)._accionT(v => new TNodoMenorIgual(v[0], v[2]))
                        | produccion(expr + "[" + expr + "]")._accionT(v => new TNodoAccessoArray(v[0], v[2]))
                        | produccion("new" + tipo + "[" + expr + "]")._accionT(v => new TNodoCrearArray(v[1], v[3]))
                        | produccion("new" + tipo)._accionT(v => new TNodoCrearInstancia(v[1]))
                        | produccion(@string)._accionT(v => new TNodoCrearString(v[0]))
                        | produccion(@char)._accionT(v => new TNodoCrearIntConChar(v[0]))
                        | produccion("#" + expr + "#")._accionT(v => new TNodoReservarMemoria(v[1]))
                        | produccion("(" + id + ")" + expr + ReduceHere())._accionT(v => new TNodoCasteo(v[1], v[3]))
                        | produccion("(" + ToTerm("int") + ")" + expr + ReduceHere())._accionT(v => new TNodoCasteoInt(v[3]))
//                        | produccion("(" + tipo + ")" + expr)._accionT(v => new TNodoCasteo2(v[1], v[3]))
                        | produccion("(" + expr + ")")._accionT(v => v[1])
                        | produccion("@" + tipo)._accionT(v => new TNodoObtenerEspacio(v[1]))
                        | produccion(@double)._accionT(v => new TNodoCrearDouble(v[0]))
                        | produccion(@int)._accionT(v => new TNodoCrearInt(v[0]))
                        | produccion(id)._accionT(v => new TNodoBuscarPropiedad(v[0]))
                        | produccion("$")._accion(v => new TNodoObtenerDollar())
                        | produccion("null")._accionT(v => new TNodoCrearNull())
                        | produccion("-" + expr + ReduceHere())._accionT(v => new TNodoMenosUnario(v[1]))
                        | invocacion
                ;

            invocacion.Rule = produccion(id + "(" + argumentos  + ")")._accionT( v => new TNodoInvocar(v[0], v[2]))
            ;

            this.ConfigList<NodoAst>(false, argumentos, expr, ",");


            RegisterOperators(4, "||");
            RegisterOperators(4, "&&");
            RegisterOperators(8, "==", "!=");
            
            RegisterOperators(9, "<", ">", "<=", ">=");
            RegisterOperators(11, "+", "-");
            RegisterOperators(12, "*", "/", "%");

            RegisterOperators(16, ".", "[", "]");
            


            LanguageFlags |= LanguageFlags.CreateAst;
            Root = inicio;
        }
    }
}