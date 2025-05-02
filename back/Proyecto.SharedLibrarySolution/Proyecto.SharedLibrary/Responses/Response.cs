using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Llaveremos.SharedLibrary.Responses
{
    //clase publica a la que se podra acceder a traves de la libreria compartida
    //record es un tipo de clase inmutable (sus carac no pueden ser modificadas una vez se instancia el objeto)
    //la clase encapsula las respuestas del servidor
    public record Response(bool Flag = false, string Message = null!);
}
