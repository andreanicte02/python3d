// El codigo original usa MessageBox para los errores de sintaxis; aqui se mandan a la consola de errores.
namespace System.Windows.Forms
{
    public static class MessageBox
    {
        public static void Show(string mensaje) => Sec.Zide.consoleErrores.Text += mensaje + "\n";
    }
}
