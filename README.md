# Python → código de tres direcciones

Compilador de un subconjunto de **Python** hecho en C# con [Irony](https://github.com/IronyProject/Irony).
Compila en tres etapas y ejecuta el resultado con su propio intérprete:

1. **Python → T**: traduce el programa a T, un lenguaje intermedio tipo C. Todo programa se compila junto con
   `Sec/api.c`, una biblioteca en T con los tipos dinámicos, los operadores y `print`.
2. **T → 3D**: genera código de tres direcciones (temporales, etiquetas, `stack` y `heap`).
3. **Intérprete de 3D**: ejecuta el código y muestra la salida.

Soporta clases con atributos y métodos, funciones recursivas, `if/elif/else`, `while` con `break` y `continue`,
`return`, `print` y `str()`.

**Demo:** https://andreanicte02.github.io/python3d/

## Cómo funciona la demo

El compilador construye el AST de forma recursiva y necesita más pila de la que permite un navegador, así que no
corre en vivo en la página. En cada publicación, GitHub Actions ejecuta **el compilador original** (.NET 8) sobre
los ejemplos de `ejemplos/` y la página muestra el resultado de cada etapa: el Python, el código T, el 3D y la
salida, con el tiempo de cada una.

## Estructura

```
Sec/          el compilador original (IDE de Windows Forms)
Py3dCore/     compila Sec/ como librería .NET 8, reemplazando el formulario y el depurador por versiones mínimas
Generador/    corre el compilador sobre cada ejemplo y genera resultados.json
ejemplos/     programas de ejemplo en Python
web/          la página
```

## Desarrollo

Con el [SDK de .NET 8](https://dotnet.microsoft.com/download):

```bash
web/build.sh    # genera web/publicar/ (la página y resultados.json)
```

Para agregar un ejemplo basta con poner un `.py` en `ejemplos/` (la primera línea es un comentario que lo describe).
Cada push a `master` publica la página con `.github/workflows/pages.yml`.
