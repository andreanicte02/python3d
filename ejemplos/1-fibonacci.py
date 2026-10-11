# Fibonacci recursivo (prueba original del proyecto)

def fibbonacci(n):
    if n == 0:
        return 0
    elif n == 1:
        return 1
    return fibbonacci(n - 2) + fibbonacci(n - 1)

print(fibbonacci(0))
print(fibbonacci(1))
print(fibbonacci(2))
print(fibbonacci(3))
print(fibbonacci(4))
print(fibbonacci(5))
print(fibbonacci(6))
