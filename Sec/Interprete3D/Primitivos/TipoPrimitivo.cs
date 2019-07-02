using Sec.Interprete3D.Entorno;

namespace Sec.Interprete3D.Primitivos
{
    public class TipoPrimitivo
    {
        public static Instancia_3d tipoNumero = new Instancia_3d("Entero", null, null);

        public static Instancia_3d tipoDecimal = new Instancia_3d("Double", null, null);
        
        public static Instancia_3d tipBool = new Instancia_3d("Booleano", null, null);
        
        public static Instancia_3d tipoNulo = new Instancia_3d("nulo", null, null);
       
        
        public static Instancia_3d instanciaNulo = new Instancia_3d("1234@nulo", null, tipoNulo);
    }
}