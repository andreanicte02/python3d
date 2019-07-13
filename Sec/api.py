class _Nodo:
    """docstring for cualquierCosa"""
    def __init__(self, cualquierCosa):
        self.cualquierCosa = cualquierCosa
        self.siguente = None
        self.anterior = None

class List:
    """docstring for lusuario"""
    def __init__(self):
        self.primero = None
        self.anterior = None

    def append(self, cualquierCosa):

        elemento = _Nodo(cualquierCosa)

        if self.primero == None:
            self.primero = elemento
            elemento.siguente = elemento
            elemento.anterior = elemento
        else:
            temporal = self.primero
            while temporal.siguente != self.primero:
                temporal = temporal.siguente

            self.primero.anterior= elemento
            temporal.siguente= elemento
            elemento.anterior = temporal
            elemento.siguente = self.primero


    def get(self, x):
        temporal = self.primero
        contador = 0
        while temporal.siguente != self.primero:

            if contador == x :
                return temporal.cualquierCosa

            contador = contador + 1
            temporal = temporal.siguente




    def recorrer(self):

        reporte=''
        temporal = self.primero
        while temporal.siguente!= self.primero:
            reporte = str(reporte)+str(temporal.cualquierCosa)+'->'
            temporal = temporal.siguente

        reporte =str(reporte)+str(temporal.cualquierCosa)
        print ('Usuarios:')
        print (reporte)


        re = ''
        while temporal!= self.primero:
            re= str(re)+str(temporal.cualquierCosa)+'->'
            temporal = temporal.anterior

        re = str(re)+str(temporal.cualquierCosa)
        print (re)

    #print 'el nombre es:' + str(temporal.nombre)

    def existe(self, nombre):
        temporal = self.primero
        while temporal.siguente!= self.primero:
            if str(temporal.cualquierCosa)== str(nombre):
                return True
            temporal = temporal.siguente
        if str(temporal.cualquierCosa) == str(nombre):
            return True #si es true existe
        else:
            return False