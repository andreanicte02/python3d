# Lista enlazada simple hecha con clases

class Nodo:
    def __init__(self, valor):
        self.valor = valor
        self.siguiente = None

class Lista:
    def __init__(self):
        self.primero = None
        self.tamano = 0

    def agregar(self, valor):
        nuevo = Nodo(valor)
        if self.primero == None:
            self.primero = nuevo
        else:
            actual = self.primero
            while actual.siguiente != None:
                actual = actual.siguiente
            actual.siguiente = nuevo
        self.tamano = self.tamano + 1

    def imprimir(self):
        actual = self.primero
        texto = ""
        while actual != None:
            texto = texto + str(actual.valor) + " -> "
            actual = actual.siguiente
        print(texto + "None")

lista = Lista()
lista.agregar(10)
lista.agregar(20)
lista.agregar(30)
lista.imprimir()
print("tamano: " + str(lista.tamano))
