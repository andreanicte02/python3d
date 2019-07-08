using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class Decmal: Instancia_3d
    {
        private double valor;
        
        public Decmal(double valor) : base("instancia@decimal")
        {
            this.valor = valor;
        }
        
        public double obtenerValor()
        {
            return this.valor;
        }
        
        
        public override String stringBonito() {

            return this.valor+"";
        }

    }
}