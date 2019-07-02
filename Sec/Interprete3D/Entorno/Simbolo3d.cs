namespace Sec.Interprete3D.Entorno
{
    public class Simbolo3d
    {
            
        private  Instancia_3d instanciaTipo;
        private Instancia_3d valor;
    
       
        public Simbolo3d(Instancia_3d instanciaTipo, Instancia_3d valorInicial){
            this.instanciaTipo = instanciaTipo;
            this.valor = valorInicial;
        }
    
        public Instancia_3d obtenerInstanciaTipo(){
            return this.instanciaTipo;
        }
    
    
        public Instancia_3d obtenerValor(){
            return this.valor;
        }
        public void definirValor(Instancia_3d valor){
            this.valor = valor;
        }
        
    }
}