using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Sec.Interprete3D
{
    public class entorno3d
    {
        public Double[] stack;
        public Double[] heap;
        public Dictionary<String, variables> vars = new Dictionary<string, variables>();
        public Dictionary<String, metodos> metos = new Dictionary<string, metodos>();

        public entorno3d()
        {
            stack = new double[860000];
            heap = new double[860000];
        }

        public void registrarVar(String id ,variables var)
        {
            if (vars.ContainsKey(id))
            {
                throw new SemanticError3d("ya se ha registrado -> " + id);
            }
            
            vars.Add(id, var);
        }

        public variables obtenerVar(String id)
        {
            if (!vars.ContainsKey(id))
            {
                throw new SemanticError3d("no se ha registrado -> " + id);
            }

            return vars[id];

        }

        public void registrarMetodo(String id, metodos m)
        {
            if (metos.ContainsKey(id))
            {
                throw new SemanticError3d("ya se ha registrado -> " + id);
            }
            
            metos.Add(id, m);
            
        }
        
        public metodos obtenerMetodo(String id)
        {
            if (!metos.ContainsKey(id))
            {
                throw new SemanticError3d("no se ha registrado -> " + id);
            }

            return metos[id];

        }

        public DataTable mostrarVariablesGlobales()
        {
            DataTable tabla= new DataTable();
            tabla.Columns.Add("variable");
            tabla.Columns.Add("valor");
            tabla.Columns.Add("metodo");
           

            
            foreach(KeyValuePair<string, variables> entry in vars)
            {
                
                tabla.Rows.Add(entry.Key, entry.Value.valor, entry.Value.NombreMetodo);
                
            }
            
           
            return tabla;
        }


        public DataTable mostrarMetodos()
        {
            DataTable tabla= new DataTable();
            tabla.Columns.Add("Metodo");
            foreach(KeyValuePair<string, metodos> entry in metos)
            {
                
                tabla.Rows.Add(entry.Key);
                
            }
            
           
            return tabla;
        }

    }
}