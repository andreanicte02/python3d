using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class Bol: Instancia_3d
    {
        private bool valor;

        public Bol(bool valor) : base("instancia@Booleano", null, TipoPrimitivo.tipBool)
        {
            this.valor = valor;
        }
        
        public bool obtenerValor() {

            return this.valor;
        }


    }
}