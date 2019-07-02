using System;
using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class Decimal:Instancia_3d
    {
        private double valor;

        public Decimal(double valor): base("instancia@entero", null, TipoPrimitivo.tipoDecimal)
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