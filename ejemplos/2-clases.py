# Clases con atributos y metodos

class Punto:
    def __init__(self, x, y):
        self.x = x
        self.y = y

    def mover(self, dx, dy):
        self.x = self.x + dx
        self.y = self.y + dy

    def describir(self):
        return "(" + str(self.x) + ", " + str(self.y) + ")"

class Rectangulo:
    def __init__(self, esquina, ancho, alto):
        self.esquina = esquina
        self.ancho = ancho
        self.alto = alto

    def area(self):
        return self.ancho * self.alto

p = Punto(1, 2)
print("punto inicial: " + p.describir())
p.mover(3, 4)
print("despues de mover: " + p.describir())

r = Rectangulo(p, 5, 3)
print("area del rectangulo: " + str(r.area()))
print("esquina: " + r.esquina.describir())
