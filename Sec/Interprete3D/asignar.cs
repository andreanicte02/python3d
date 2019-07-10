using Sec.Interprete3D.intefaces;

namespace Sec.Interprete3D
{
    public class asignar:nodo3d
    {
        private string id;
        private nodo3d valor;

        public asignar(string id, nodo3d valor)
        {
            this.id = id;
            this.valor = valor;
        }

        public override object ejecutar(entorno3d e)
        {
            variables v = e.obtenerVar(id);
            double va = (double) valor.ejecutar(e);

            v.valor = va;
            
            return null;


        }

        public override object recoger(entorno3d e, bool inMetodo, string nameMetodo)
        {
            return null;
        }
    }
}