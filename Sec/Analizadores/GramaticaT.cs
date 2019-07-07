using System;
using Irony.Parsing;
using static Sec.Analizadores.IronyUtils;

namespace Sec.Analizadores
{
    public class GramaticaT : Grammar
    {
        public GramaticaT() : base(true)
        {
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
            var retorno = new AutoNonTerminal();
            
            var expr = new AutoNonTerminal();
            var invocacion = new AutoNonTerminal();

            var @int = new NumberLiteral("Entero", NumberOptions.IntOnly | NumberOptions.NoDotAfterInt);
            @int._accion((v, n) => n.Token.Value);
            var @double = new NumberLiteral("Decimal");
            @double._accion((v, n) => n.Token.Value);

            var id = new IdentifierTerminal("Identificador");
            id._accion((v, n) => n.Token.Value);

            
            
            // Reglas
            inicio.Rule = produccion(sentencias)._accion(v => v[0]);
                ;

            sentencia.Rule = produccion(declaracionStruct)._accion(values => values[0])
                             | produccion(declaracionFuncion)._accion(values => values[0])
                ;

            declaracionStruct.Rule = produccion("struct" + id + "{" + declaracionesPropiedad + "}")
                                         ._accion(v => new TNodoDeclaracionStruct(v[1], v[3]))
//                                     | produccion(ToTerm("struct") + "$" + "{" + declaracionesPropiedad + "}")
//                                         ._accion(v => new TNodoDeclaracionStruct(v[1], v[3]))
                ;
            declaracionPropiedad.Rule = produccion(tipo + id + ";")
                    ._accion(v => new TNodoDeclaracionVar(v[0], v[1]))
                ;
            
            tipo.Rule = produccion(id)._accion(v => new TNodoBusquedaTipo(v[0]))
                        | produccion("void")._accion(v => new TNodoBusquedaTipo("void"))
                        | produccion("int")._accion(v => new TNodoBusquedaTipo("int"))
                        | produccion("double")._accion(v => new TNodoBusquedaTipo("double"))
                ;

            declaracionFuncion.Rule =
                produccion(tipo + id + "(" + declaracionesParametro + ")" + "{" + sentenciasFuncion + "}")
                    ._accion(v => new TNodoDeclaracionFuncion(v[0], v[1], v[3], v[6]))
                ;

            //configuracion de listas
            this.ConfigList<NodoAst>(false, declaracionesParametro, declaracionParametro, ",");
            
            declaracionParametro.Rule = produccion(tipo + id)._accion(v => new TNodoDeclaracionVar(v[0], v[1]));
                ;

            this.ConfigList<NodoAst>(false, sentenciasFuncion, sentenciaFuncion);
            
            sentenciaFuncion.Rule = produccion(asignacion + ";")._accion(v => v[0])
                                    | produccion(declaracion + ";")._accion(v => v[0])
                                    | produccion(retorno + ";")._accion(v => v[0])
                                    | invocacion + ";"
                                    | produccion(ToTerm("!") + "print" + "(" + id + "," + expr + ")" + ";")._accion(v => new TNodoPrintSimple(v[3], v[5]))
                                    | produccion("throw" + expr + ";" )._accion(v => new TNodoThrow(v[1]))
                ;

            declaracion.Rule = produccion(tipo + id + "=" + expr)
                                   ._accion(v => new TNodoDeclaracionVar(v[0], v[1], v[3]))
                               | produccion(tipo + id)._accion(v => new TNodoDeclaracionVar(v[0], v[1]))
                ;

            asignacion.Rule = produccion(expr + "=" + expr)
                    ._accion(v => new TNodoAsignacion(v[0], v[2]))
                ;
            retorno.Rule = produccion("return" + expr)._accion(v => new TNodoReturn(v[1]))
                | produccion("return")._accion(v => new TNodoReturn() )
                ;


            expr.Rule = produccion(expr + "+" + expr)._accion(v => new TNodoSuma(v[0], v[2]))
                        | produccion(expr + "." + id)._accion(v => new TNodoAccessoPunto(v[0], v[2]))
                        | produccion("#" + expr + "#")._accion(v => new TNodoReservarMemoria(v[1]))
                        | produccion("(" + id + ")" + expr)._accion(v => new TNodoCasteo(v[1], v[3]))
                        | produccion("(" + expr + ")")._accion(v => v[1])
                        | produccion("@" + tipo)._accion(v => new TNodoObtenerEspacio(v[1]))
                        | produccion(@double)._accion(v => new TNodoCrearDouble(v[0]))
                        | produccion(@int)._accion(v => new TNodoCrearInt(v[0]))
                        | produccion(id)._accion(v => new TNodoBuscarPropiedad(v[0]))
                        | invocacion
                ;
            
            invocacion.Rule = produccion(id + "(" + argumentos  + ")")._accion( v => new TNodoInvocar(v[0], v[2]))
            ;

            this.ConfigList<NodoAst>(false, argumentos, expr, ",");

            
            RegisterOperators(1, "+", "-");
//            RegisterOperators(2,  "(");
            RegisterOperators(3, ".");

            LanguageFlags |= LanguageFlags.CreateAst;
            Root = inicio;
        }
    }
}