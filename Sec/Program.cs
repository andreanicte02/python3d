using System;
using System.Windows.Forms;

namespace Sec
{
    internal class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            Console.WriteLine("holi");
            Zide f = new Zide();
            Application.EnableVisualStyles();
            Application.Run(f);
        }
    }
}