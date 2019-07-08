using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class Numero: Instancia_3d
    {
        private int valor;


        public Numero(int valor) : base("instancia@entero")
        {
            this.valor = valor;
        }

        public int obtenerValor()
        {
            return this.valor;
        }
        
        
        public override String stringBonito() {

            return this.valor+"";
        }
        
    }
}