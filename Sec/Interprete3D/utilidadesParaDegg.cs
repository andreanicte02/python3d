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
    private Label entrada;
    private String lineaTexto;
    
    
    
    public reporteDebug3d(int linea, entorno3d e, String texto)
    {

        entorno = e;
        AutoScroll = true;
        this.linea = linea;
        entrada = new Label();
        this.lineaTexto = texto;
        SetBounds(-150,-150,600,600);
        inicializar();
        llenarTablaEntornos();
        llenarSalida();
        
        
    }


    void llenarSalida()
    {
        entrada.Text = lineaTexto;
        entrada.BackColor = Color.OldLace;
    }

    public void inicializar()
    {
        panelComponentes = new FlowLayoutPanel();
        panelGeneral = new FlowLayoutPanel();
        panelTabla = new Panel();
        panelGeneral.FlowDirection = FlowDirection.LeftToRight;
        
        
        btnBuscarPoint = new Button();
        
        
        btnBuscarPoint.Text = "Buscar";
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

        
        entrada.SetBounds(0,0,200,20);
        panelGeneral.Controls.Add(btnBuscarPoint);
        panelGeneral.Controls.Add(panelTabla);
        panelGeneral.Controls.Add(entrada);
        
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