using System.Collections.Generic;

namespace Sec
{
    /// <summary>
    /// Reemplaza al formulario original (Zide.cs, Windows Forms): el compilador escribe en estas
    /// "cajas de texto" y la pagina web las lee.
    /// </summary>
    public class Zide
    {
        public static CajaTexto console = new CajaTexto();
        public static CajaTexto consoleErrores = new CajaTexto();
        public static CajaTexto texTraducidBox = new CajaTexto();
        public static CajaTexto textGramaticaT = new CajaTexto();

        // 0 = ejecutar sin depurar (la web no tiene depurador paso a paso)
        public static int inDebug3d = 0;
        public static Dictionary<int, int> breakPoints = new Dictionary<int, int>();
        public static Dictionary<int, int> breakPoints3d = new Dictionary<int, int>();
    }

    public class CajaTexto
    {
        public string Text { get; set; } = "";

        public string GetLineText(int linea) => "";
        public void GotoNextBookmark(int linea) { }
        public void Navigate(int linea) { }
    }
}
