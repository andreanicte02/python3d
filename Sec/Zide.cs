using System;
using System.Linq;
using System.Windows.Forms;
using Sec.Analizadores;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms.VisualStyles;
using FastColoredTextBoxNS;

namespace Sec
{
    public class Zide: Form
    {
        private FlowLayoutPanel panel;
        private FlowLayoutPanel panelBotones;
        private FlowLayoutPanel panelEditores;
        private FlowLayoutPanel panelSalidas;
        
        private TabControl tabEditor;
        private Button btnAnalizar;
        private Button btnCrearPestaña;
        private Analizador3D analizar3d;
        private AanalizadorAltoNivel analizarAltoNivel;
        private Button bntAbrirArchivo;
        private TabControl tabeEditor3D;

        public TabControl tabConosola;
        //consolas de salidas
        public static FastColoredTextBox console;
        public static FastColoredTextBox consoleErrores;
        
        
        //
        
        //breakpoints listas
        Dictionary<int, int> breakPoints;
        Dictionary<int,int> breakPoints3d;
        
        public Zide():base()
        {
            instanciar();   
            crear();
            agregaPestaña();
            analizar3d= new Analizador3D();
            analizarAltoNivel = new AanalizadorAltoNivel();
            

        }

        void instanciar()
        {
            tabEditor = new TabControl();
            panel = new FlowLayoutPanel(); 
            btnAnalizar = new Button();
            btnCrearPestaña = new Button();
            panelBotones = new FlowLayoutPanel();
            bntAbrirArchivo = new Button();
            panelEditores = new FlowLayoutPanel();
            tabeEditor3D = new TabControl();
            
            //break points
            breakPoints = new Dictionary<int, int>();
            breakPoints3d = new Dictionary<int, int >();
            
            
            panelSalidas = new FlowLayoutPanel();
            tabConosola = new TabControl();
            console = new FastColoredTextBox();
            consoleErrores = new FastColoredTextBox();
            

        }

        public void crear()
        {
            btnAnalizar.Text = "Traducir3D";
            btnCrearPestaña.Text = "Add\nPestaña";
            bntAbrirArchivo.Text = "Abrir";


            WindowState = FormWindowState.Maximized;
            panel.FlowDirection = FlowDirection.TopDown;
            panelBotones.FlowDirection = FlowDirection.BottomUp;
            panelEditores.FlowDirection = FlowDirection.LeftToRight;
            panelSalidas.FlowDirection = FlowDirection.LeftToRight;
            
            //paneles
            panelEditores.SetBounds(0,0,1500,400);
            panelBotones.SetBounds(0,0,500,50);
            panelSalidas.SetBounds(0,0,1360,280);
            
            panel.SetBounds(0,0,2000,900);

            
            //editores
            tabEditor.SetBounds(0,0,650,400);
            tabeEditor3D.SetBounds(0,0,650,400);
   
            
            btnAnalizar.Click +=  analizar_click;
            btnCrearPestaña.Click += btnCrearPestaña_click;
            bntAbrirArchivo.Click += btnAbirirArchivo_click;
          
            //panel dos para botnes-------------------------------------------
            panelBotones.Controls.Add(btnAnalizar);
            panelBotones.Controls.Add(btnCrearPestaña);
            panelBotones.Controls.Add(bntAbrirArchivo);
            
            //editor del 3d----------------------------------------------------
         
            editor3DCrear();
            
            
            //paneles----------------------------------------------------------
            panelEditores.Controls.Add(tabEditor);
            panelEditores.Controls.Add(tabeEditor3D);
            
            
            panel.Controls.Add(panelBotones);
            panel.Controls.Add(panelEditores);
            consolasCrear();
            panel.Controls.Add(panelSalidas);
            
            
            Controls.Add(panel);
            
        }

        void consolasCrear()
        {
            tabConosola.SetBounds(0,0,1310,270);
            
            TabPage pageConsola = new TabPage("Console");
            pageConsola.BorderStyle = BorderStyle.Fixed3D;
            pageConsola.Controls.Add(console);
            tabConosola.TabPages.Add(pageConsola);
            console.SetBounds(0,0,1300,250);
            console.ReadOnly = false;
            
            TabPage pageErrores = new TabPage("Console Erroes");
            pageErrores.BorderStyle = BorderStyle.Fixed3D;
            pageErrores.Controls.Add(consoleErrores);
            tabConosola.TabPages.Add(pageErrores);
            consoleErrores.SetBounds(0,0,1300,250);
            consoleErrores.ReadOnly = true;
            
            
            
            
            //agregar al panel
            panelSalidas.Controls.Add(tabConosola);
            
            
            



        }

        public void editor3DCrear()
        {
            //definir propiedades del texbox
            FastColoredTextBox texTraducidBox= new FastColoredTextBox();
            texTraducidBox.MouseDoubleClick += pintarBrekPointSalida3D;
            texTraducidBox.BookmarkColor = Color.Gold;
            texTraducidBox.IndentBackColor = Color.Black;
            texTraducidBox.LineNumberColor= Color.White;
            
            //----------------------
            texTraducidBox.SetBounds(0, 5, 630, 360);
            string title = "3D";
            TabPage myTabPage = new TabPage(title);
            myTabPage.Controls.Add(texTraducidBox);
            myTabPage.BorderStyle = BorderStyle.Fixed3D;
            tabeEditor3D.TabPages.Add(myTabPage); 
            

        }

        void crearConsolaSalida()
        {
            
        }

        public void agregaPestaña()
        {
           
            FastColoredTextBox fastColor = new FastColoredTextBox();
            AutocompleteMenu auto = new AutocompleteMenu(fastColor);
            auto.Items.SetAutocompleteItems(palabrasReservadas());
      
            fastColor.SetBounds(0, 5, 630, 360);
            fastColor.MouseDoubleClick += pintarBrekPoint;
            fastColor.BookmarkColor=Color.Red;
            fastColor.LineNumberColor= Color.White;
            fastColor.IndentBackColor = Color.Black;
            fastColor.Language = Language.VB;
            
            string title = "New File " + (tabEditor.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            myTabPage.Controls.Add(fastColor);
            tabEditor.TabPages.Add(myTabPage);
            myTabPage.BorderStyle = BorderStyle.Fixed3D;

        }

        List<String> palabrasReservadas()
        {
            List<String> pr = new List<string>();
            pr.Add("False");
            pr.Add("None");
            pr.Add("True");
            pr.Add("and");
            pr.Add("as");
            pr.Add("break");
            pr.Add("class");
            pr.Add("continue");
            pr.Add("def");
            pr.Add("elif");
            pr.Add("for");
            pr.Add("from");
            pr.Add("or");
            pr.Add("not");
            pr.Add("lambda");
            pr.Add("import");
            pr.Add("return");
            

            return pr;
        }



        void pintarBrekPoint(object sender, MouseEventArgs e)
        {
            Range selec = ((FastColoredTextBox) sender).Selection;
            int numeroLinea = selec.Start.iLine;
            
            if (e.Button == MouseButtons.Right && !breakPoints.ContainsKey(numeroLinea)) {
                
                ((FastColoredTextBox) sender).BookmarkLine(selec.Start.iLine);
                breakPoints.Add(numeroLinea,numeroLinea);
            }else if (e.Button == MouseButtons.Right && breakPoints.ContainsKey(numeroLinea))
            {
                ((FastColoredTextBox) sender).UnbookmarkLine(selec.Start.iLine);
                breakPoints.Remove(numeroLinea);
                
            }


        }

        void pintarBrekPointSalida3D(object sender, MouseEventArgs e)
        {
            Range selec = ((FastColoredTextBox) sender).Selection;
            int numeroLinea = selec.Start.iLine;
            
            if (e.Button == MouseButtons.Right && !breakPoints3d.ContainsKey(numeroLinea)) {
                
                ((FastColoredTextBox) sender).BookmarkLine(selec.Start.iLine);
                breakPoints3d.Add(numeroLinea,numeroLinea);
            }else if (e.Button == MouseButtons.Right && breakPoints3d.ContainsKey(numeroLinea))
            {
                ((FastColoredTextBox) sender).UnbookmarkLine(selec.Start.iLine);
                breakPoints3d.Remove(numeroLinea);
                
            }

        }


        private void analizar_click(object sender, EventArgs e)
        {
      
            analizar3d = new Analizador3D();
            String entada = leer();
            analizar3d.ejecutarAnalizador(entada);
            

        }

        void btnAbirirArchivo_click(object sender, EventArgs e)
        {
            OpenFileDialog abrir = new OpenFileDialog();
            abrir.Title = "Abrir archivos de texto";
            abrir.Filter = "Archivo de Texto Plano  (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
            abrir.ShowDialog();
            if (abrir.FileName.Length == 0)
                return;

            System.IO.StreamReader sr = new System.IO.StreamReader(abrir.FileName, System.Text.Encoding.UTF8);
            String contenido = sr.ReadToEnd();
            sr.Close();
            
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);
            TabPage myTabPage = tabEditor.SelectedTab;

            entrada.Text = contenido;
            myTabPage.Text = abrir.FileName;


        }


        private void btnCrearPestaña_click(object sender, EventArgs e)
        {
            agregaPestaña();
        }
        
        static String leer()
        {
            string text = System.IO.File.ReadAllText(@"/home/nicte/Escritorio/prueba3d.pyton");

 
            return text;
        }

    


    }
}