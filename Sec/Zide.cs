using System;
using System.Linq;
using System.Windows.Forms;
using Sec.Analizadores;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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
        
        private AanalizadorAltoNivel analizarAltoNivel;
        private Button bntAbrirArchivo;
        private TabControl tabeEditor3D;


        //-------------------
        public TabControl tabConosola;
        //consolas de salidas
        public static FastColoredTextBox console;
        public static FastColoredTextBox consoleErrores;
        
        
        //
        
        //breakpoints listas
        Dictionary<int, int> breakPoints;
        public static Dictionary<int,int> breakPoints3d;
        
        //botn guardasr -----

        private Button btnGuardar;
        
        
        //botones 3d----------
        private Button btnEjecutar3D;
        private Button btnAbrir3d;
        private Button btnDebug3d;
        private Button generar3D;
        


        /// <summary>
        /// Debugger del 3d
        /// </summary>
        //-----------------------------------------
        public  static Timer timer3d;
        public  static int inDebug3d;
        
        public static FastColoredTextBox texTraducidBox;
        public static FastColoredTextBox textGramaticaT;
        
        //-----------------------------------------
        
        
        public Zide():base()
        {
            instanciar();   
            crear();
            agregaPestaña();
            inDebug3d = 0;
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
            console.Text = "";
            consoleErrores.Text = "";
            
            //mas botones
            btnGuardar = new Button();
            btnEjecutar3D = new Button();
            btnAbrir3d = new Button();
            btnDebug3d = new Button();
            generar3D = new Button();
            
            //timer
           
            texTraducidBox= new FastColoredTextBox();
            textGramaticaT = new FastColoredTextBox();
            //instanciarTimer3d();
            

        }

       

        public void crear()
        {
            btnAnalizar.Text = "Python->T";
            btnCrearPestaña.Text = "AddPestaña";
            bntAbrirArchivo.Text = "Abrir";
            btnGuardar.Text = "Guardar";
            btnEjecutar3D.Text = "Ejecutar3D";
            btnAbrir3d.Text = "Abrir3d";
            btnDebug3d.Text = "Deb3d";
            generar3D.Text ="Generar3d";
            

            WindowState = FormWindowState.Maximized;
            panel.FlowDirection = FlowDirection.TopDown;
            panelBotones.FlowDirection = FlowDirection.BottomUp;
            panelEditores.FlowDirection = FlowDirection.LeftToRight;
            panelSalidas.FlowDirection = FlowDirection.LeftToRight;
            
            //paneles
            panelEditores.SetBounds(0,0,1500,400);
            panelBotones.SetBounds(0,0,1000,50);
            panelSalidas.SetBounds(0,0,1360,280);
            
            panel.SetBounds(0,0,2000,900);

            
            //editores
            tabEditor.SetBounds(0,0,650,400);
            tabeEditor3D.SetBounds(0,0,650,400);
   
            //panel dos para botnes-------------------------------------------
            panelBotones.Controls.Add(btnAnalizar);
            panelBotones.Controls.Add(btnCrearPestaña);
            panelBotones.Controls.Add(bntAbrirArchivo);
            panelBotones.Controls.Add(btnGuardar);
            panelBotones.Controls.Add(btnEjecutar3D);
            panelBotones.Controls.Add(btnAbrir3d);
            panelBotones.Controls.Add(btnDebug3d);
            panelBotones.Controls.Add(generar3D);

            //acciones de bontos --------------------------------
            btnAnalizar.Click +=  python_T_Click;
            btnCrearPestaña.Click += btnCrearPestaña_click;
            bntAbrirArchivo.Click += btnAbirirArchivo_click;
            btnGuardar.Click += btnGuardarArchivo_click;
            btnEjecutar3D.Click += btnEjecutar3d_Click;
            btnAbrir3d.Click += btnAbrir3d_Click;
            btnDebug3d.Click += btnDebug3d_click;
            generar3D.Click += generar3d_click;
            
  
            
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
           
            texTraducidBox.MouseDoubleClick += pintarBrekPointSalida3D;
            texTraducidBox.BookmarkColor = Color.Gold;
            texTraducidBox.IndentBackColor = Color.Black;
            texTraducidBox.LineNumberColor= Color.White;
            texTraducidBox.CurrentLineColor = Color.Gray;
            
            
            //----------------------
            texTraducidBox.SetBounds(0, 5, 630, 360);
            textGramaticaT.SetBounds(0,5,630, 360);
            
            
           

            TabPage myTabPage = new TabPage("3d");
            TabPage pagT = new TabPage("T");
            
            
            myTabPage.Controls.Add(texTraducidBox);
            pagT.Controls.Add((textGramaticaT));
            
            myTabPage.BorderStyle = BorderStyle.Fixed3D;
            
            
            tabeEditor3D.TabPages.Add(myTabPage); 
            tabeEditor3D.TabPages.Add(pagT);

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
            fastColor.CurrentLineColor = Color.Gray;
            
            string title = "New File:";
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


        private void python_T_Click(object sender, EventArgs e)
        {

            textGramaticaT.Text = "";
           
            string entrada2 = File.ReadAllText(@"./../../api.c");
            textGramaticaT.Text += entrada2;
            //
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);

            if (!string.IsNullOrEmpty(entrada.Text))
            {
                textGramaticaT.Text += "\n";
                textGramaticaT.Text += "\n";
                textGramaticaT.Text += PUtils.generarCodigoT(entrada.Text);
            }


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

        void btnGuardarArchivo_click(object sender, EventArgs e)
        {
            TabPage actual = tabEditor.SelectedTab;
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);
            if (actual.Text == "New File:")
            {
                SaveFileDialog saveArch = new SaveFileDialog();
                
                
                saveArch.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*"  ;
                saveArch.FilterIndex = 2 ;
               // saveArch.RestoreDirectory = true ;
 
                if(saveArch.ShowDialog() == DialogResult.OK)
                {
                    //Console.WriteLine(saveArch.FileName);
                    using (StreamWriter writer = new StreamWriter(saveArch.FileName))
                    {
                        writer.Write(entrada.Text);
                    }

                    actual.Text = saveArch.FileName;
                }
            }
            else
            {
                using (StreamWriter writer = new StreamWriter(actual.Text))
                {
                    writer.Write(entrada.Text);
                    MessageBox.Show("[Archivo guardado] ["+actual.Text+"]");  
                }

            }


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
        

        void btnAbrir3d_Click(object sender, EventArgs e)
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
            
            var entrada = tabeEditor3D.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);
            TabPage myTabPage = tabeEditor3D.SelectedTab;

            entrada.Text = contenido;
            myTabPage.Text = abrir.FileName;


        }
        
        private void btnEjecutar3d_Click(object sender, EventArgs e)
        {
            inDebug3d = 0;
            console.Text = "";
            TabPage actual = tabeEditor3D.SelectedTab;
            var entrada = tabeEditor3D.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);
            Analizador3D analizar = new Analizador3D();
            analizar.ejecutarAnalizador(entrada.Text);
            
           
        }
        
        private void btnDebug3d_click(object sender, EventArgs e)
        {

        
            inDebug3d = 1;
            console.Text = "";
            var entrada = tabeEditor3D.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is FastColoredTextBox);
            Analizador3D analizar = new Analizador3D();
            analizar.ejecutarAnalizador(entrada.Text);
            
        }
        
        private void generar3d_click(object sender, EventArgs e)
        {

            consoleErrores.Text = "";
            try
            {
                if (!string.IsNullOrEmpty(textGramaticaT.Text))
                {
                    texTraducidBox.Text = TUtils.generarCodigo3D(textGramaticaT.Text);
                }
            }
            catch (SyntaxException syntaxException)
            {
                Console.WriteLine("Error sintactico en T:");
                Console.WriteLine(syntaxException.Message);
                Console.WriteLine(syntaxException.Line);
                Console.WriteLine(syntaxException.Column);

                consoleErrores.Text += "Error sintactico en T:\n";
                consoleErrores.Text += syntaxException.Message+"\n";
                consoleErrores.Text += syntaxException.Line+"\n";
                consoleErrores.Text += syntaxException.Column+"\n";
                
                
            }
            catch (SemanticException semanticException)
            {
                Console.WriteLine("Error semantico en T:");
                Console.WriteLine(semanticException.Message);
                Console.WriteLine(semanticException.Location.Line);
                Console.WriteLine(semanticException.Location.Column);
                
                consoleErrores.Text += "Error semantico en T: :\n";
                consoleErrores.Text += semanticException.Message +":\n";
                consoleErrores.Text += semanticException.Location.Line+":\n";
                consoleErrores.Text += semanticException.Location.Column+":\n";
                
                
            }
        }
            
        
        
    

        //0 sin debug
        //1 en debug
        //1 salto break a break;
        //2 linea a linea;
        //3 no ejecutar nada









    }
}