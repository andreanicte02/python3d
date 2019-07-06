using System.Collections.Generic;
using System.Linq;
using Irony.Parsing;

namespace Sec.Analizadores
{
    public delegate object DefinitionWithNodeDelegate(dynamic[] values, ParseTreeNode node);
    public delegate object DefinitionDelegate(dynamic[] values);
    
    public class AutoNonTerminal : NonTerminal
    {
        public AutoNonTerminal(string name = "") : base(name)
        {
            //sintentizar el primer valor ast
            AstConfig.NodeCreator = (context, node) =>
            {
                if (node.ChildNodes.Count > 0)
                {
                    node.AstNode = node.ChildNodes[0].AstNode;
                }
            };
        }
    }

    public static class IronyUtils
    
    {
        
        //equivalente a crear un no terminal y definir reglas.....
        public static NonTerminal produccion(BnfExpression _)
        {
//            var nonTerminal = new NonTerminal("") {Rule = expression};
//            nonTerminal._accion()
//            return nonTerminal;
            return new NonTerminal("") {Rule = _};
        }
        
        
        public static void ConfigList<T>(this Grammar gramar, bool atLeastOne, AutoNonTerminal startList , AutoNonTerminal element, string separator = null)
        {
            // List<T>
            var elements = new AutoNonTerminal();

            if (atLeastOne)
            {
                startList.Rule = elements;
            }
            else
            {
                startList.Rule = elements
                                | produccion(gramar.Empty)._accion(v => new List<T>())
                    ;
            }
            
            var secondIndex = separator == null ? 1 : 2;

            var rule = separator == null ? elements + element : elements + separator + element;

            elements
                    .Rule = produccion(rule)._accion(v =>
                            {
                                List<T> list = v[0];
                                list.Add(v[secondIndex]);
                                return list;
                            })
                            | produccion(element)._accion(v => new List<T> {v[0]})
                ;
        }
    }
    
    public static class IronyExtensions
    {
        
        /**
         * Cuando se tiene seteado la bandera de GenerateAst en Irony, todos los nodos creados deben de tener
         * definido una accion en NodeCreator, este metodo setea una accion que no hace nada.
         */
        public static T _accion<T>(this T term) where T: BnfTerm
        {
            term.AstConfig.NodeCreator = (c, n) => { };
            return term;
        }
        
        /// <summary>
        /// Registra una accion que se ejecutara cuando se reduzca la produccion.
        /// 
        /// Esta accion debe de crear un objeto que representa un nodo Ast.
        /// 
        /// Recibe un array de los valores que han generado las acciones de los hijos, y tambien recibe el ParseTreeNode que genera Irony.
        /// Esta funcion es usualmente usado solo con los literales. 
        /// </summary>
        /// <param name="term"></param>
        /// <param name="definitionDelegate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T _accion<T>(this T term , DefinitionWithNodeDelegate definitionDelegate) where T: BnfTerm
        {
            term.AstConfig.NodeCreator = (c, n) =>
            {
                var values = n.ChildNodes.Select(child => child.AstNode).ToArray();                                      
                n.AstNode = definitionDelegate(values, n);
            };
            return term;
        }
        
        /// <summary>
        /// Registra una accion que se ejecutara cuando se reduzca la produccion.
        /// 
        /// Esta accion debe de crear un objeto que representa un nodo Ast.
        /// 
        /// La accion recibe un array de los valores que han generado las acciones de los hijos.
        /// </summary>
        /// <param name="term"></param>
        /// <param name="definitionDelegate"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T _accion<T>(this T term, DefinitionDelegate definitionDelegate) where T : BnfTerm
        {
            term.AstConfig.NodeCreator = (c, n) =>
            {
                var values = n.ChildNodes.Select(child => (dynamic) child.AstNode).ToArray();
                n.AstNode = definitionDelegate(values);
            };
            return term;
        }
    }
}