using System;
using System.Linq;
using System.Windows.Forms;
using Sec.Analizadores;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;

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
        private int num_linea;
        
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
            this.tabEditor = new TabControl();
            this.panel = new FlowLayoutPanel(); 
            this.btnAnalizar = new Button();
            this.btnCrearPestaña = new Button();
            this.panelBotones = new FlowLayoutPanel();

        }

        public void crear()
        {
            this.btnAnalizar.Text = "Analizar";
            this.btnCrearPestaña.Text = "Add";
            
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
            //----------------------------------------------------------------
            
            this.panel.Controls.Add(panelBotones);
            this.panel.Controls.Add(tabEditor);
            this.Controls.Add(panel);
            
        }

        public void agregaPestaña()
        {

            RichTextBox tTexbox = new RichTextBox();
            Label lLabel = new Label();
            tTexbox.SetBounds(25, 5, 480, 360);
            
            tTexbox.TextChanged += textChange;
            
            
            tTexbox.VScroll += richTextBox1_VScroll;
            string title = "New File " + (tabEditor.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            myTabPage.Controls.Add(tTexbox);
            myTabPage.Controls.Add(lLabel);
            this.tabEditor.TabPages.Add(myTabPage);
        
        }

        private void textChange(object sender, EventArgs e)
        {
            
            
            udpateEnumeracion();


        }

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