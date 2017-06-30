using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace RecFalaArduino {
    public class Arduino {
        public static bool EnviarComando(string Comando) {
            try {
                SerialPort arduino = new SerialPort("COM8", 9600, Parity.None, 8, StopBits.One);
                arduino.Open();
                arduino.WriteLine(Comando);
                arduino.Close();
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
