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
            

            var tipo = new AutoNonTerminal();

            var declaracion = new AutoNonTerminal();
            var asignacion = new AutoNonTerminal();
            
            var expr = new AutoNonTerminal();

            
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
            
            sentenciaFuncion.Rule = produccion(asignacion)._accion(v => v[0])
                                    | produccion(declaracion + ";")._accion(v => v[0])
                ;

            declaracion.Rule = produccion(tipo + id + "=" + expr)
                                   ._accion(v => new TNodoDeclaracionVar(v[0], v[1], v[3]))
                               | produccion(tipo + id)._accion(v => new TNodoDeclaracionVar(v[0], v[1]))
                ;

            asignacion.Rule = produccion(expr + "=" + expr)
                ;

            expr.Rule = produccion(expr + "+" + expr)
                        | produccion(@double)._accion(v => new TNodoCrearDouble(v[0]))
                        | produccion(@int)._accion(v => new TNodoCrearInt(v[0]))
                        | produccion(id)
                ;
            



            LanguageFlags |= LanguageFlags.CreateAst;
            Root = inicio;
        }
    }
}