



//tipo 2 : Ambito
//tipo 3 : int
//tipo 4 : Double
// falta especificar los demas tipos


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
}

General crearEntorno(){
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

General registrar(General ambito, int[] llave, General valor){
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

/*
General masGeneral(General g1, General g2, int[] archivo, int fila, int columna){
    
    if (g1.tipo == 3 && g2.tipo == 3) {
        return crearIntGeneral(g1.valorInt + g2.valorInt);
    }
    
    
}
*/



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

    int[] a = "hola ";
    int[] b = "hola";
    int[] c = "sadfasdf";
    

    printlnString(concatString(a, c));

}