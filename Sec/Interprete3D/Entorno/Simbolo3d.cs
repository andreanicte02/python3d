namespace Sec.Interprete3D.Entorno
{
    public class Simbolo3d
    {
            
        
        private Instancia_3d valor;
    
       
        public Simbolo3d( Instancia_3d valorInicial){
           
            this.valor = valorInicial;
        }
    
    
    
        public Instancia_3d obtenerValor(){
            return this.valor;
        }
        public void definirValor(Instancia_3d valor){
            this.valor = valor;
        }
        
    }
}