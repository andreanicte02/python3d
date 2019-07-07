using System;
using System.Linq;
using System.Windows.Forms;
using Sec.Analizadores;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using FastColoredTextBoxNS;

namespace Sec
{
    public class Zide: Form
    {
        private FlowLayoutPanel panel;
        private FlowLayoutPanel panelBotones;
        private TabControl tabEditor;
        private Button btnAnalizar;
        private Button btnCrearPestaña;
        private Analizador3D analizar3d;
        private AanalizadorAltoNivel analizarAltoNivel;
        private Dictionary<int, int> breakPoints;
        private Button bntAbrirArchivo;
        
        public Zide():base()
        {
            instanciar();   
            crear();
            agregaPestaña();
            this.analizar3d= new Analizador3D();
            this.analizarAltoNivel = new AanalizadorAltoNivel();
            

        }

        void instanciar()
        {
            tabEditor = new TabControl();
            panel = new FlowLayoutPanel(); 
            btnAnalizar = new Button();
            btnCrearPestaña = new Button();
            panelBotones = new FlowLayoutPanel();
            breakPoints = new Dictionary<int, int>();
            bntAbrirArchivo = new Button();

        }

        public void crear()
        {
            btnAnalizar.Text = "Analizar";
            btnCrearPestaña.Text = "Add";
            bntAbrirArchivo.Text = "Abrir";
            
            this.SetBounds(0,0,1000,720);
            this.panelBotones.FlowDirection = FlowDirection.BottomUp;
            this.panelBotones.SetBounds(0,0,500,50);
            this.tabEditor.SetBounds(0,0,500,400);
            this.panel.SetBounds(0,0,1000,700);

            this.btnAnalizar.Click += new EventHandler(analizar_click);
            this.btnCrearPestaña.Click += new EventHandler(btnCrearPestaña_click);
            
          
            //panel dos para botnes-------------------------------------------
            this.panelBotones.Controls.Add(btnAnalizar);
            this.panelBotones.Controls.Add(btnCrearPestaña);
            this.panelBotones.Controls.Add(bntAbrirArchivo);
            //----------------------------------------------------------------
            
            this.panel.Controls.Add(panelBotones);
            this.panel.Controls.Add(tabEditor);
            this.Controls.Add(panel);
            
        }

        public void agregaPestaña()
        {
           
            FastColoredTextBox fastColor = new FastColoredTextBox();
            AutocompleteMenu auto = new AutocompleteMenu(fastColor);
            auto.Items.SetAutocompleteItems(palabrasReservadas());
      
            fastColor.SetBounds(0, 5, 480, 360);
            fastColor.MouseDoubleClick += pintarBrekPoint;
            fastColor.BookmarkColor=Color.Red;
            fastColor.LineNumberColor= Color.White;
            fastColor.IndentBackColor = Color.Black;
            fastColor.Language = Language.PHP;
            
            string title = "New File " + (tabEditor.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            myTabPage.Controls.Add(fastColor);
            tabEditor.TabPages.Add(myTabPage); 
            
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

    

        #region todo a pata

        

       
        public void udpateEnumeracion()
        {
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is RichTextBox);

            var etiquietaLinea = tabEditor.SelectedTab.Controls.Cast<Control>().First(x => x is Label);
            etiquietaLinea.Font = new Font("Arial", 12); //12
            etiquietaLinea.SetBounds(0,7,25,355);
            entrada.Font = new Font("Arial", 9); //9
            Point pos = new Point(0, 0);
            int firstIndex = ((RichTextBox)entrada).GetCharIndexFromPosition(pos);
            int firstLine = ((RichTextBox)entrada).GetLineFromCharIndex(firstIndex);
 
            pos.X = ClientRectangle.Width;
            pos.Y = ClientRectangle.Height;
            int lastIndex = ((RichTextBox)entrada).GetCharIndexFromPosition(pos);
            int lastLine = ((RichTextBox)entrada).GetLineFromCharIndex(lastIndex);

           
            pos = ((RichTextBox)entrada).GetPositionFromCharIndex(lastIndex);

            //finally, renumber label
            etiquietaLinea.Text = "";
            for (int i = firstLine; i < lastLine + 1; i++)
            {
                etiquietaLinea.Text += i + 1 + "\n";
            }



        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is RichTextBox);
            var etiquietaLinea = tabEditor.SelectedTab.Controls.Cast<Control>().First(x => x is Label);


            int d =((RichTextBox) entrada).GetPositionFromCharIndex(0).Y % 
                    ((RichTextBox) entrada).Font.Height + 1;
            etiquietaLinea.Location = new Point(0, d);
            udpateEnumeracion();

        }
        
        void pintarBreakPoint(object sender, EventArgs e)
        {
        
            var entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is RichTextBox);
          
            int firstcharindex = ((RichTextBox) entrada).GetFirstCharIndexOfCurrentLine();

            int currentline = ((RichTextBox) entrada).GetLineFromCharIndex(firstcharindex);
            int pos = ((RichTextBox) entrada).GetCharIndexFromPosition(Cursor.Position);
            Console.WriteLine(pos);
            Console.WriteLine(currentline);
            
                
                
            

        }
        
        public int obTamLinea(String text, int linea)
        {
            String []t2 = text.Split('\n');
            return t2[linea].Length-1;
        }
        
        
        private void textChange(object sender, EventArgs e)
        {
            
            
            udpateEnumeracion();


        }

        #endregion


        private void analizar_click(object sender, EventArgs e)
        {
      
            analizar3d = new Analizador3D();
            String entada = leer();
            analizar3d.ejecutarAnalizador(entada);
            

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