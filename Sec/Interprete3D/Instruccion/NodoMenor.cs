using System;
using Sec.Interprete3D.Entorno;
using Sec.Interprete3D.Primitivos;

namespace Sec.Interprete3D.Instruccion
{
    public class NodoMenor:Instruccion3d
    {
        
        private Instruccion3d l1; //literal 1
        private Instruccion3d l2; //literal 2

        public NodoMenor(Instruccion3d l1, Instruccion3d l2)
        {
            this.l1 = l1;
            this.l2 = l2;
        }

        public override object ejecutarSin(entorno3d e)
        {


            Object algo1 = l1.ejecutarSin(e);
            Object algo2 = l2.ejecutarSin(e);
            
            Instancia_3d valor1 = Utilidades.desenvolver(algo1);
            Instancia_3d valor2 = Utilidades.desenvolver(algo2);

            if (valor1 is Numero && valor2 is Numero)
            {
                Numero n1 = (Numero) valor1;
                Numero n2 = (Numero) valor2;
                return new Bol(n1.obtenerValor() < n2.obtenerValor());
                
            }else if (valor1 is Decmal && valor2 is Decmal)
            {
                Decmal n1 = (Decmal) valor1;
                Decmal n2 = (Decmal) valor2;
                return  new Bol(n1.obtenerValor() < n2.obtenerValor());
               
            }else if (valor1 is Decmal && valor2 is Numero)
            {
                Decmal n1 = (Decmal) valor1;
                Numero n2 = (Numero) valor2;
                return  new Bol(n1.obtenerValor() < n2.obtenerValor());
                
            }else if (valor1 is Numero && valor2 is Decmal)
            {
                
                Numero n1 = (Numero) valor1;
                Decmal n2 = (Decmal) valor2;
                return  new Bol(n1.obtenerValor() < n2.obtenerValor());
                
            }

            throw  new SemanticError3d("Erro al ejecutar <");
        }
    }
}