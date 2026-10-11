// Muestra el recorrido de cada ejemplo por el compilador: Python -> T -> 3D -> salida.
// resultados.json lo genera Generador/ corriendo el compilador original en cada publicacion.

const ETAPAS = [
    { id: 'python', titulo: 'Python', detalle: 'el programa fuente' },
    { id: 'codigoT', titulo: 'Código T', detalle: 'lenguaje intermedio tipo C' },
    { id: 'codigo3D', titulo: 'Código 3D', detalle: 'tres direcciones: temporales, etiquetas, stack y heap' },
    { id: 'salida', titulo: 'Salida', detalle: 'lo que imprime el intérprete de 3D' },
];

const NOTAS = {
    python: 'El programa que se compila.',
    codigoT: 'Traducción del programa a T. Se compila junto con api.c, la biblioteca en T con los tipos, operaciones y print.',
    codigo3D: 'Código de tres direcciones de api.c más el programa. Lo ejecuta el intérprete de 3D del proyecto.',
    salida: 'Salida real de ejecutar el código 3D.',
    api: 'api.c: la "biblioteca estándar" en lenguaje T que usa todo programa (tipos dinámicos, operadores, print).',
};

let datos = null;
let ejemplo = null;
let vista = 'python';

const $ = (id) => document.getElementById(id);
const lineas = (texto) => (texto ? texto.split('\n').length : 0);
const miles = (n) => n.toLocaleString('es');

function pintarEtapas() {
    const lista = $('etapas');
    lista.textContent = '';
    const tiempos = { codigoT: ejemplo.ms.pythonAT, codigo3D: ejemplo.ms.tA3D, salida: ejemplo.ms.ejecucion };
    for (const etapa of ETAPAS) {
        const li = document.createElement('li');
        const boton = document.createElement('button');
        boton.className = vista === etapa.id ? 'activa' : '';
        boton.addEventListener('click', () => mostrar(etapa.id));
        const texto = ejemplo[etapa.id] ?? '';
        boton.innerHTML = `<strong></strong><span></span><small></small>`;
        boton.querySelector('strong').textContent = etapa.titulo;
        boton.querySelector('span').textContent = etapa.detalle;
        boton.querySelector('small').textContent =
            `${miles(lineas(texto))} líneas` + (tiempos[etapa.id] !== undefined ? ` · ${miles(tiempos[etapa.id])} ms` : '');
        li.appendChild(boton);
        lista.appendChild(li);
    }
}

function mostrar(id) {
    vista = id;
    pintarEtapas();

    const pestanias = $('pestanias');
    pestanias.textContent = '';
    if (id === 'codigoT' || id === 'api') {
        for (const [clave, titulo] of [['codigoT', 'Traducción del programa'], ['api', 'api.c']]) {
            const boton = document.createElement('button');
            boton.textContent = titulo;
            boton.className = id === clave ? 'activa' : '';
            boton.addEventListener('click', () => mostrar(clave));
            pestanias.appendChild(boton);
        }
    }

    $('nota').textContent = NOTAS[id];
    const codigo = $('codigo');
    codigo.className = 'codigo' + (id === 'salida' ? ' salida' : '');
    if (id === 'api') {
        codigo.textContent = datos.api;
    } else if (id === 'salida') {
        codigo.textContent = ejemplo.salida + (ejemplo.errores ? '\n' + ejemplo.errores : '');
    } else {
        codigo.textContent = ejemplo[id] ?? '(no se generó por un error en una etapa anterior)';
    }
    codigo.scrollTop = 0;
}

function elegir(indice) {
    ejemplo = datos.ejemplos[indice];
    $('descripcion').textContent = ejemplo.descripcion;
    mostrar(vista === 'api' ? 'codigoT' : vista);
}

fetch('resultados.json')
    .then((r) => r.json())
    .then((json) => {
        datos = json;
        const select = $('ejemplo');
        datos.ejemplos.forEach((e, i) => {
            const opcion = document.createElement('option');
            opcion.value = i;
            opcion.textContent = e.archivo;
            select.appendChild(opcion);
        });
        select.addEventListener('change', () => elegir(Number(select.value)));
        $('pie').textContent =
            `Generado el ${datos.generado} corriendo el compilador original (.NET) sobre cada ejemplo. ` +
            'Los tiempos son de esa ejecución e incluyen construir las gramáticas de Irony.';
        elegir(0);
    })
    .catch((error) => {
        $('codigo').textContent = 'No se pudieron cargar los resultados: ' + error.message;
    });
