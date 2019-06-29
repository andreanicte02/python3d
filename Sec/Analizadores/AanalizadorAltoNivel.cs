using System;
using System.Windows.Forms;
using Irony.Parsing;

namespace Sec.Analizadores
{
    public class AanalizadorAltoNivel
    {
        public AanalizadorAltoNivel()
        {
            
        }

        public void probarAnalizadorAltoNivel(String entrada)
        {
            
            GramaticaPython ga = new GramaticaPython();
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
            
            
        }


    }
}