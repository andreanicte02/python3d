



//tipo 2 : Ambito
//tipo 3 : int
//tipo 4 : Double
//tipo 5 : String
//tipo 6 : boolean
//tipo 7 : funcion
//tipo 8 : void
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

struct InfoFuncion {
    int id;
    int cantidadParametros;
    General ambitoCapturado;
}

struct General {
    int valorInt;
    int[] valorString;
    double valorDouble;
    int tipo;
    
    General anterior;
    InfoFuncion infoFuncion;    
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

General crearGeneralFuncion(int id, int cantidadParametros, General ambitoCapturado){
    General g = new General;
    g.tipo = 7;
    g.infoFuncion = new InfoFuncion;
    g.infoFuncion.id = id;
    g.infoFuncion.cantidadParametros = cantidadParametros;
    g.infoFuncion.ambitoCapturado = ambitoCapturado;
    return g;
}

General crearAmbito(General ambitoAnterior){
    General g = new General;
    g.tipo  = 2;
    g.anterior = ambitoAnterior;
    return g;
}
General crearVoidGeneral(){
    General g = new General;
    g.tipo = 8;
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
    Entrada destino = buscarEntradaEnActual(ambito, llave);    
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

Entrada buscarEntradaEnActual(General ambito, int[] llave){
    Entrada temp = ambito.primeraEntrada;
    
    while (temp != null){
        if (cadenasIguales(temp.llave, llave)) {
            return temp;
        }    
        temp = temp.siguiente;
    }    
    return null;    
}

// busca en todos los ambitos
General buscarIdlocal(General ambito, int[] llave, int[] archivo, int linea, int columna){
    
    General temp = ambito;
    while (temp != null){
        Entrada entrada = buscarEntradaEnActual(ambito, llave);
        if (entrada != null){
            return entrada.valor;
        }
        temp = temp.anterior;
    }

    throw crearException("llave no encontrada", archivo, linea, columna);
}

void printGenerales(General[] generales){
    int i = 0;
    while (i < generales.length){
        printGeneral(generales[i]);
        !print(c, ' ');
        i = i  + 1;
    }
    !print(c, 10);
}
void printGeneral(General g){
    if (g.tipo == 3){
        printInt(g.valorInt);
    }
    else if (g.tipo == 4){
        printDouble(g.valorDouble);
    }
    else if (g.tipo == 5){
        printString(g.valorString);
    }
    else if (g.tipo == 6){
        printString(stringBool(g.valorInt));
    }
    else {
        printString("<aun no definido>");    
    }
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
        
    } else if(g1.tipo == 5 &&  g2.tipo == 4){
        
        return crearStringGeneral(concatString(g1.valorString, double_str(g2.valorDouble)));
        
    } else if(g1.tipo == 4 &&  g2.tipo == 5){
              
        return crearStringGeneral(concatString(double_str(g1.valorDouble), g2.valorString)) ;
              
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

//-----------pow 

General potenciaGeneral(General g1, General g2, int[] archivo, int fila, int columna){

    if (g1.tipo == 3 && g2.tipo == 3) {
    
        return crearIntGeneral(pow_int(g1.valorInt, g2.valorInt));
        
    } else if(g1.tipo == 4 && g2.tipo == 4){
        
        return crearDoubleGeneral(pow2(g1.valorDouble, g2.valorDouble));
        
    } else if(g1.tipo ==3 && g2.tipo == 4 ){
        
        return crearIntGeneral(pow3(g1.valorInt, g2.valorDouble));
        
    } else if(g1.tipo ==4 && g2.tipo == 3 ){
              
        return crearDoubleGeneral(pow_double(g1.valorDouble, g2.valorInt));
              
    } 
    
    throw crearException("tipos no compatibles **", archivo, fila, columna); 
    
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
double pow_double (double a ,int b){

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

int pow_int (int a ,int b){

    if(b == 0){
        return 1;
    }

    if(b<0){
        return 0;
    }

    int result = 1;
    int i = 0;
    while(i<b){

        result = result *a;
    }
    return result;

}



double  pow2(double a, double b){
    return pow_double(a,(int) b);
}

int pow3 (int a, double b){
    return pow_int(a, (int) b);
}
//--------------------------------------
void printDouble(double d){
    !print(d, d);
}
void printInt(int i){
    !print(e, i);
}
void printString(int[] cadena){
    int i = 0;
    while (i < cadena.length){
        !print(c, cadena[i]);
        i = i + 1;
    }
}
void printlnInt(int i){
    !print(e, i);
    !print(c, 10);
}

void printlnString(int[] cadena){
    printString(cadena);
    !print(c, 10);
}

//============

void main(){
    python_main(null, null);
}