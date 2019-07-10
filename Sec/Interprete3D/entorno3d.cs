using System;
using System.Collections.Generic;

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



    }
}