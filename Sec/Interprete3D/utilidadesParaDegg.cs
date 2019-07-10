using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using FastColoredTextBoxNS;
using Irony.Parsing;
using Sec;
using Sec.Interprete3D;
using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class utilidadesParaDegg
    {
        private static  String superStr="";
        
        //static Thread hilo = new Thread(metodos);
       
        
        public static void enciclar(nodo3d ins, entorno3d e)
        {


            SourceLocation loc = ins.location;
            
            if (Zide.breakPoints3d.ContainsKey(loc.Line))
            {
                
               
                String f = Zide.texTraducidBox.GetLineText(loc.Line);
                Zide.texTraducidBox.GotoNextBookmark(loc.Line);
                Zide.texTraducidBox.Navigate(loc.Line);
                Form tablaS = new reporteDebug3d(loc.Line,e,f);
                DialogResult res = new DialogResult();
                res = tablaS.ShowDialog();
                

            }
        }


    }
    
}

public class reporteDebug3d: Form
{
    
    private Button btnBuscarPoint;
    private DataGridView tabla;
    private DataGridView tablaMetodos;
    private FlowLayoutPanel panelGeneral;
    private FlowLayoutPanel panelComponentes;
    private Panel panelTabla;
    private entorno3d entorno;
    private readonly int linea;
    private Label labelLinea;
    private String lineaTexto;
    private FlowLayoutPanel botones;
    private Button btnSigLinea;
    private Button btnterminar;
    private Button btnTerminarEjercutar;
    
    
    
    public reporteDebug3d(int linea, entorno3d e, String texto)
    {

        entorno = e;
        AutoScroll = true;
        this.linea = linea;
        labelLinea = new Label();
        this.lineaTexto = texto;
        SetBounds(-150,-150,550,500);
        inicializar();
        llenarTablaEntornos();
        llenarSalida();
        
        
    }


    void llenarSalida()
    {
        labelLinea.Text = lineaTexto;
        labelLinea.Text += "\n\nlinea: " + linea;
        labelLinea.BackColor = Color.OldLace;
    }

    public void inicializar()
    {
        panelComponentes = new FlowLayoutPanel();
        panelGeneral = new FlowLayoutPanel();
        panelTabla = new Panel();
        panelGeneral.FlowDirection = FlowDirection.RightToLeft;
        botones = new FlowLayoutPanel();
        botones.FlowDirection = FlowDirection.RightToLeft;
        botones.SetBounds(0,0,400,50);
        
        
        btnBuscarPoint = new Button();
        btnSigLinea = new Button();
        btnterminar = new Button();
        btnTerminarEjercutar = new Button();


        btnBuscarPoint.Text = "SigBreakPoint";
        btnSigLinea.Text = "SigLinea";
        btnterminar.Text = "Ejecuar";
        btnTerminarEjercutar.Text = "Terminar Ejecucion";
        
        
        btnBuscarPoint.Click += btnDetener_Click3d;
        
        
        
        tabla = new DataGridView();
        tabla.SetBounds(0,0,360,310);
        tabla.Show();
        
        tablaMetodos = new DataGridView();
        tablaMetodos.SetBounds(330,0,150,310);
        tablaMetodos.Show();
        
        tabla.FirstDisplayedScrollingRowIndex = tabla.RowCount - 1;
        tablaMetodos.FirstDisplayedScrollingRowIndex = tabla.RowCount - 1;

        
        panelTabla.SetBounds(0,0,500,320);
        panelTabla.Controls.Add(tabla);
        panelTabla.Controls.Add(tablaMetodos);

        
        labelLinea.SetBounds(0,0,500,200);

        botones.Controls.Add(btnBuscarPoint);
        botones.Controls.Add(btnSigLinea);
        botones.Controls.Add(btnterminar);
        botones.Controls.Add(btnTerminarEjercutar);
        
        panelGeneral.Controls.Add(botones);
        panelGeneral.Controls.Add(panelTabla);
        panelGeneral.Controls.Add(labelLinea);
        
        panelGeneral.SetBounds(0,0,500,500);
        Controls.Add(panelGeneral);
        
        
    }

    void llenarTablaEntornos()
    {
        tabla.DataSource = entorno.mostrarVariablesGlobales();
        tablaMetodos.DataSource = entorno.mostrarMetodos();

    }

    private void btnDetener_Click3d(object sender, EventArgs e)
    {

        

        this.DialogResult = DialogResult.OK;


    }




}