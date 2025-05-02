using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;

namespace Llaveremos.SharedLibrary.Logs
{
    //todas las apis podran acceder a esta clase
    public class LogException
    {
        //recibe como parametro un error que la aplicacion pueda tener
        public static void LogExceptions(Exception ex)
        {
            LogToFile(ex.Message);
            LogToConsole(ex.Message);
            LogToDebugger(ex.Message);
        }

        //guardar el error que dio el log en un archivo
        public static void LogToFile(string message) => Log.Information(message);
        //guardar el error que dio el log en la consola
        public static void LogToConsole(string message) => Log.Warning(message);
        //depurar el error que dio el log (mostrar informacion detallada del error)
        public static void LogToDebugger(string message) => Log.Debug(message);
    }
}
