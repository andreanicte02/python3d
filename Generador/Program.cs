using System.Diagnostics;
using System.Text.Json;
using Sec;
using Sec.Analizadores;
using Sec.Interprete3D;

// uso: dotnet run --project Generador -- <carpeta de ejemplos> <api.c> <salida.json>
// Python -> T -> 3D -> ejecucion, igual que los botones Python->T, Generar3d y Ejecutar3D del IDE original.
var carpeta = args[0];
var api = File.ReadAllText(args[1]);
var salidaJson = args[2];

var consola = Console.Out;
var resultados = new List<object>();

foreach (var ruta in Directory.GetFiles(carpeta, "*.py").OrderBy(r => r))
{
    var python = File.ReadAllText(ruta);
    var lineas = python.Split('\n');
    Zide.console.Text = "";
    Zide.consoleErrores.Text = "";
    string? codigoT = null, codigo3D = null;
    long msT = 0, ms3D = 0, msEjecucion = 0;

    // el compilador escribe mucha depuracion con Console.WriteLine
    Console.SetOut(TextWriter.Null);
    var cronometro = Stopwatch.StartNew();
    try
    {
        codigoT = api + "\n\n" + PUtils.generarCodigoT(python);
        msT = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        codigo3D = TUtils.generarCodigo3D(codigoT);
        ms3D = cronometro.ElapsedMilliseconds;

        cronometro.Restart();
        new Analizador3D().ejecutarAnalizador(codigo3D);
        msEjecucion = cronometro.ElapsedMilliseconds;
    }
    catch (SyntaxException e) { Zide.consoleErrores.Text += $"Error sintactico: {e.Message} linea {e.Line + 1}, columna {e.Column}\n"; }
    catch (PythonSemanticException e) { Zide.consoleErrores.Text += $"Error semantico en Python: {e.Message}\n"; }
    catch (SemanticException e) { Zide.consoleErrores.Text += $"Error semantico en T: {e.Message}\n"; }
    catch (SemanticError3d e) { Zide.consoleErrores.Text += $"Error al ejecutar el 3D: {e.Message}\n"; }
    finally { Console.SetOut(consola); }

    resultados.Add(new
    {
        archivo = Path.GetFileName(ruta),
        // la primera linea del ejemplo es un comentario que lo describe
        descripcion = lineas[0].TrimStart('#', ' ').Trim(),
        python,
        // api.c es igual para todos: la pagina lo muestra aparte
        codigoT = codigoT?[(api.Length + 2)..],
        codigo3D,
        salida = Zide.console.Text,
        errores = Zide.consoleErrores.Text,
        ms = new { pythonAT = msT, tA3D = ms3D, ejecucion = msEjecucion },
    });
    Console.WriteLine($"{Path.GetFileName(ruta)}: T {msT} ms, 3D {ms3D} ms, ejecucion {msEjecucion} ms {(Zide.consoleErrores.Text.Length > 0 ? "CON ERRORES" : "")}");
}

File.WriteAllText(salidaJson, JsonSerializer.Serialize(new
{
    generado = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm 'UTC'"),
    api,
    ejemplos = resultados,
}));
Console.WriteLine($"-> {salidaJson}");
