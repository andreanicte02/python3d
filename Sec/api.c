



//tipo 2 : Ambito
//tipo 3 : int
//tipo 4 : Double
//tipo 5 : String
//tipo 6 : boolean
//falta especificar los demas tipos



struct Exception {
    int[] mensaje;
    int[] archivo;
    int linea;
    int columna;
}

struct Entrada {
    int[] llave;
    Entrada siguiente;
    General valor;
}

struct General {
    int valorInt;
    int[] valorString;
    double valorDouble;
    int tipo;
    
    Entrada primeraEntrada;
}



Exception crearException(int[] mensaje, int[] archivo, int linea, int columna){
    Exception e = new Exception;
    e.mensaje = mensaje;
    e.archivo = archivo;
    e.linea = linea;
    e.columna = columna;
    return e;
}

General creearAmbito(){
    General g = new General;
    g.tipo  = 2;
    return g;
}


General crearIntGeneral(int valor){
    General g = new General;
    g.valorInt = valor;
    g.tipo = 3;
    return g;
}

General crearDoubleGeneral(double valor){
    General g = new General;
    g.valorDouble = valor;
    g.tipo = 4;
    return g;
}

General crearStringGeneral(int[] valor){
    General g = new General;
    g.valorString = valor;
    g.tipo = 5;
    return g;
}

General crearBooleanGeneral(int valor){
    General g = new General;
    g.valorInt = valor;
    g.tipo = 6;
    return g;

}


//-----------------------------------

void registrar(General ambito, int[] llave, General valor){
    Entrada destino = buscarEntrada(ambito, llave);    
    if (destino == null){
        destino = crearEntrada(llave);
        destino.siguiente = ambito.primeraEntrada;
        ambito.primeraEntrada = destino;
    }
    destino.valor = valor; 
}
Entrada crearEntrada(int[] llave){
    Entrada entrada = new Entrada;
    entrada.llave = llave;
    return entrada;
}

Entrada buscarEntrada(General ambito, int[] llave){
    Entrada temp = ambito.primeraEntrada;
    
    while (temp != null){
        if (cadenasIguales(temp.llave, llave)) {
            return temp;
        }    
        temp = temp.siguiente;
    }    
    return null;    
}


Entrada buscarIdlocal(General ambito, int[] llave){
    Entrada entrada = buscarEntrada(ambito, llave);
    if (entrada == null){
        throw crearException("llave no encontrada", "asd.py", 12, 23);
    }
    return entrada;
}



//---------------------------------------- OPERACIONES BINARIAS ----------------------------------------


General masGeneral(General g1, General g2, int[] archivo, int fila, int columna){
    
    
    
    
    if (g1.tipo == 5 && g2.tipo == 5) {
    
         int[] a = concatString(g1.valorString, g2.valorString);
         return crearStringGeneral(a);
         
     
    } else if(g1.tipo == 5 && g2.tipo == 6){
    
        
         return crearStringGeneral(concatString(g1.valorString, stringBool(g2.valorInt)));
    
    } else if(g1.tipo == 6 && g2.tipo == 5){
    
         return crearStringGeneral(concatString(stringBool(g2.valorInt), g2.valorString));
         
    } else if(g1.tipo == 5 && g2.tipo == 3){
          
         return crearStringGeneral(concatString(g1.valorString, int_str(g2.valorInt)));
         
    } else if(g1.tipo == 3 && g2.tipo == 5){
              
        return crearStringGeneral(concatString(int_str(g1.valorInt), g2.valorString));
    } 
    
    
    
    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(g1.valorInt + g2.valorInt);
    
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorDouble + g2.valorDouble);
        
    }else if(g1.tipo == 3 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorInt + g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
        return crearDoubleGeneral(g1.valorDouble + g2.valorInt);
       
    }else if(g1.tipo == 6 && g2.tipo == 6){
    
                    
       return crearStringGeneral(concatString(stringBool(g1.valorInt), stringBool(g2.valorInt)));
            
    }
    
 
    throw crearException("tipos no compatibles", archivo, fila, columna);
    
    
}



General menosGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(g1.valorInt - g2.valorInt);
    
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorDouble - g2.valorDouble);
        
    }else if(g1.tipo == 3 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorInt - g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
        return crearDoubleGeneral(g1.valorDouble - g2.valorInt);
       
    }
    
 
    throw crearException("tipos no compatibles", "asd.py", 12, 23);
    
    
}



General porGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(g1.valorInt * g2.valorInt);
    
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorDouble * g2.valorDouble);
        
    }else if(g1.tipo == 3 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorInt * g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
        return crearDoubleGeneral(g1.valorDouble * g2.valorInt);
       
    }
    
    throw crearException("tipos no compatibles *",archivo, fila, columna); 
    
}


General divGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(g1.valorInt / g2.valorInt);
    
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorDouble / g2.valorDouble);
        
    }else if(g1.tipo == 3 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorInt / g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
        return crearDoubleGeneral(g1.valorDouble / g2.valorInt);
       
    }
    
    throw crearException("tipos no compatibles /", "asd.py", 12, 23); 
    
}


General mayorGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt > g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble > g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble > g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt > g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        return crearBooleanGeneral(cadenasMayores(g1.valorString, g2.valorString));
    
    }
    
   
    
    throw crearException("tipos no compatibles >", "asd.py", 12, 23); 
    
}

General menorGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt < g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble < g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble < g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt < g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        return crearBooleanGeneral(cadenasMayores(g1.valorString, g2.valorString));
    
    }
    
   
    
    throw crearException("tipos no compatibles <", archivo, fila, columna); 
    
}



General menorIgualGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt <= g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble <= g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble <= g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt <= g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        return crearBooleanGeneral(g1.valorString.length <= g2.valorString.length);
    
    }
    
    
    throw crearException("tipos no compatibles <=",archivo, fila, columna); 
    
}


General mayorIgualGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt >= g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble >= g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble >= g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt >= g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        return crearBooleanGeneral(g1.valorString.length >= g2.valorString.length);
    
    }
    
    
    throw crearException("tipos no compatibles <=", archivo, fila, columna); 
    
}


General igualIgualGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt == g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble == g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble == g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt == g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        return crearBooleanGeneral(cadenasIguales(g1.valorString, g2.valorString));
    
    }else if(g1.tipo  == 6 && g2.tipo == 6){
    
        return crearBooleanGeneral(g1.valorInt == g2.valorInt);
    }
    
    
    throw crearException("tipos no compatibles ==", archivo, fila, columna); 
    
}


General diferenteQueGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearBooleanGeneral(g1.valorInt == g2.valorInt);
        
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
       return crearBooleanGeneral(g1.valorDouble == g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
       return crearBooleanGeneral(g1.valorDouble == g2.valorInt);
         
    }else if(g1.tipo == 3 && g2.tipo == 4){
              
        return crearBooleanGeneral(g1.valorInt == g2.valorDouble);
              
    }else if(g1.tipo == 5 && g2.tipo == 5){
        
        int i = cadenasIguales(g1.valorString, g2.valorString);
        if(i == 1){
        
           return crearBooleanGeneral(0);
        }
        return crearBooleanGeneral(1);
    
    }else if(g1.tipo  == 6 && g2.tipo == 6){
         
        return crearBooleanGeneral(g1.valorInt != g2.valorInt);
             
    }
         
    
    
    throw crearException("tipos no compatibles ==", archivo, fila, columna); 
    
}
//-----------operadores logicos

General andGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 6 && g2.tipo == 6) {
    
        return crearBooleanGeneral(g1.valorInt && g2.valorInt);
        
    }
    
}


General orGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 6 && g2.tipo == 6) {
    
        return crearBooleanGeneral(g1.valorInt || g2.valorInt);
        
    }
    
}



/*
General divDobleGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(g1.valorInt // g2.valorInt);
    
    }else if(g1.tipo == 4 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorDouble // g2.valorDouble);
        
    }else if(g1.tipo == 3 && g2.tipo == 4){
    
        return crearDoubleGeneral(g1.valorInt // g2.valorDouble);
    
    }else if(g1.tipo == 4 && g2.tipo == 3){
         
        return crearDoubleGeneral(g1.valorDouble // g2.valorInt);
       
    }
    
    throw crearException("tipos no compatibles", "asd.py", 12, 23); 
    
}*/





//======================================================================================================

// aun no se ha integrado al lenguaje t
int[] concatString(int[] a, int[] b){
    int[] nuevo = new int[a.length + b.length];
    int c = 0;
    while (c < a.length){
        nuevo[c] = a[c];
        c = c + 1;
    }
    c = 0;
    while (c < b.length){
        nuevo[a.length + c] = b[c];
        c = c + 1;
    }
    return nuevo;
}

int cadenasIguales(int[] a, int[] b){
    if (a.length != b.length) {
        return 0;
    }
    int c = 0;
    while (c < a.length){
        if (a[c] != b[c]){
            return 0;
        }
        c = c + 1;
    }
    return 1;
}
//cadenas mayores
int cadenasMayores(int[] a, int[] b){

    if (a.length > b.length) {
        return 1;
    }
    return 0;
    
}

int cadenasMenores(int[] a, int[] b){

    if(a.length < b.length){
        
        return 1;
    }
    
    return 0;

}

int [] stringBool(int a){

    if(a == 0){
        int[] s = "false";
        return s;
    }else{
        int[] s = "true";
        return s;
    }

}

//--------------------------------------
int[] int_str(int n){
    if(n == 0){

        int [] a="0";
        return a;
    }

    int esNegativo = n<0;

    if(n<0){
        n = n*-1;
    }

    int c =0;
    int t =n;
    
    while(t> 0){
        int curr = t %10;
        t = t/10;
        c = c+1;
    }

    int size = 0;

    if(esNegativo == 1){

        size = c+1;
    }else{
        size = c;
    }

    int [] chars = new int[size];

    int iii = 0;
    while(iii<c){

        int curr = n %10;
        n = n/10;
        
        chars[size - iii - 1]= curr + '0';

        iii = iii+1;
    }

    if(esNegativo == 1){

        chars[0]= '-';
    }
    
    return chars;

}

//---------------------------------------

int[] double_str(double n){

    if(n == 0){

        return "0.00";
    }

    int parteEntera = (int) n;
    int parteDecimal = (int) ((n - parteEntera) * 100);
    
    if (parteDecimal < 0){
        parteDecimal = parteDecimal * -1;
    }

    int [] a = int_str(parteEntera);
    int [] b = int_str(parteDecimal);

    return concatString(a,b);
 
}
//-------------------------------------
double pow (double a ,int b){

    if(b == 0){
        return 1.0;
    }

    if(b<0){
        return 0.0;
    }

    double result = 1.00;
    int i = 0;
    while(i<b){

        result = result *a;
    }
    return result;

}


double  pow2(double a, double b){
    return pow(a,(int) b);
}

//--------------------------------------
void printlnInt(int i){
    !print(e, i);
    !print(c, 10);
}

void printlnString(int[] cadena){
    int i = 0;
    while (i < cadena.length){
        !print(c, cadena[i]);
        i = i + 1;
    }
    !print(c, 10);
}

//============

void main(){

    /*int[] a = "hola ";
    int[] b = "hola";
    int[] c = "sadfasdf";
    
    int d = 1;
    double e = 2.5;
    double f = 2.5;*/
    
    

    int [] a = "hola";
    General p0 = crearStringGeneral(a);
    
    int [] b = "hola2";
    General p1 = crearStringGeneral(a);
    
    General p3 = crearIntGeneral(1);
    
    General p4 = crearIntGeneral(3);
    
    General p10 = crearDoubleGeneral(4.5);
    
    General p6 =  crearDoubleGeneral(3.5);
        
    General p7 = crearBooleanGeneral(0);
    
    General p8 = crearBooleanGeneral(1);
        /*
        
        
         
    General gsuma = masGeneral(p0,p6,a,1,1);
    General gresta = menosGeneral(gsuma, p4,a,1,1);
    General gs2 = masGeneral(gsuma, p3,a,1,1);
    General g0 = porGeneral(gresta, p10 ,a,1,1);
    General g1 = divGeneral(p6, p4 ,a,1,1);
    General g2 = divGeneral(p4, p6 ,a,1,1);
    General g3 = igualIgualGeneral(p7, p8,a,1,1);
    
    General g4 = mayorGeneral(p0,p1 ,a,1,1);
    General g5 = menorGeneral(p7, p8,a,1,1);
    General g6 = mayorIgualGeneral(p0, p1,a,1,1);
    General g7 = menorIgualGeneral(p6, p7,a,1,1);
    General g8 = igualIgualGeneral(p4, p10,a,1,1);

       
        
    
        
    printlnString(gsuma.valorString);*/
    

}