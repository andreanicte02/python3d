using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class TipoPrimitivo
    {
        public static Instancia_3d tipoNumero = new Instancia_3d("Entero");

        public static Instancia_3d tipoDecimal = new Instancia_3d("Double");
        
        public static Instancia_3d tipBool = new Instancia_3d("Booleano");
        
        public static Instancia_3d tipoNulo = new Instancia_3d("nulo");
       
        
        public static Instancia_3d instanciaNulo = new Instancia_3d("1234@nulo");
    }
}