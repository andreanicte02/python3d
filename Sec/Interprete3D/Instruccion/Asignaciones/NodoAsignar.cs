using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoAsignar:Instruccion3d



    {
        private String id;
        private Instruccion3d valor;

        public NodoAsignar(String id, Instruccion3d valor)
        {
            this.id = id;
            this.valor = valor;
        }

        public override object ejecutarSin(entorno3d e)
        {

            var simb = NodoBuscarId.buscarId(e,id);
            var algo1 = valor.ejecutarSin(e);
            var valor1 = Utilidades.desenvolver(algo1);
            
            simb.definirValor(valor1);




            return null;
        }

        public override object recogerEtiquetas(entorno3d e)
        {
            return null;
        }
    }
}