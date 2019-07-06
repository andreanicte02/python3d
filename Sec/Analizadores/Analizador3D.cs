using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Irony.Parsing;
using Sec.Interprete3D;
using Sec.Interprete3D.Entorno;

namespace Sec.Analizadores
{
    public class Analizador3D
    {
        public Analizador3D()
        {
            
        }
        
        public void ejecutarAnalizador(String entrada)
        {
            Gramatica3D ga = new Gramatica3D();
            Parser parser = new Parser(ga);

            ParseTree arbol = parser.Parse(entrada);

            ParseTreeNode raiz = arbol.Root;

            
            if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
            {

                if (raiz == null || arbol.ParserMessages.Count > 0 || arbol.HasErrors())
                {
                    //---------------------> Hay Errores      

                    foreach (var item in arbol.ParserMessages)
                    {
                        MessageBox.Show("Error->"+item.Message+" Line:"+item.Location.Line);

                    }
                    return;
                }
            }

            Utilidades.lInstrucciones = new List<Instruccion3d>();
            var instrucciones = (List<Instruccion3d>) arbol.Root.AstNode;
            Utilidades.lInstrucciones = instrucciones;
            
            entorno3d entornoGlobal = new entorno3d(null);
            Utilidades.recogerEtiquetas(entornoGlobal,instrucciones);
            Utilidades.ejecutarInstrucciones(entornoGlobal, instrucciones);

        }

    }
}