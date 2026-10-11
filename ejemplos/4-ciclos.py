# Ciclos while con break y continue: factorial y numeros primos

def factorial(n):
    resultado = 1
    while n > 1:
        resultado = resultado * n
        n = n - 1
    return resultado

def residuo(a, b):
    while a > b or a == b:
        a = a - b
    return a

def es_primo(n):
    if n < 2:
        return False
    divisor = 2
    while divisor * divisor < n + 1:
        if residuo(n, divisor) == 0:
            return False
        divisor = divisor + 1
    return True

print("5! = " + str(factorial(5)))
print("10! = " + str(factorial(10)))

n = 0
while True:
    n = n + 1
    if n > 30:
        break
    if not es_primo(n):
        continue
    print(str(n) + " es primo")
