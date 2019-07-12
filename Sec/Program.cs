using System;
using System.Windows.Forms;
using Sec.Analizadores;

namespace Sec
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            Console.WriteLine("holi");
//            TUtils.pruebaT();
            
            Zide f = new Zide();
            Application.EnableVisualStyles();
            Application.Run(f);
        }
    }
}