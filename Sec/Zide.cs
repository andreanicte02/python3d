using System;
using System.Linq;
using System.Windows.Forms;
using Sec.Analizadores;

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

            RichTextBox t = new RichTextBox();
            t.SetBounds(5, 5, 480, 360);
            string title = "New File " + (tabEditor.TabCount + 1).ToString();
            TabPage myTabPage = new TabPage(title);
            myTabPage.Controls.Add(t);
            this.tabEditor.TabPages.Add(myTabPage);
        
        }

       

        private void analizar_click(object sender, EventArgs e)
        {
            String entrada = tabEditor.SelectedTab.Controls.Cast<Control>().FirstOrDefault(x => x is RichTextBox)?.Text;
            //analizar3d.ejecutarAnalizador(entrada);
            //Console.WriteLine("hola");
   
            analizarAltoNivel.probarAnalizadorAltoNivel(entrada);
            

        }

        private void btnCrearPestaña_click(object sender, EventArgs e)
        {
            agregaPestaña();
        }
        
     

    }
}