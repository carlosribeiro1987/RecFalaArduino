using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace RecFalaArduino {
    public class Arduino {
        byte comPort;
        int i;
        SerialPort arduinoPort;
        string lastRead;
        string[] allRead = new string[300];
        string lastWrite;


        public Arduino(byte COMPort) {
            i = 0;
            comPort = COMPort;
            arduinoPort = new SerialPort(string.Format("COM{0}", comPort), 9600, Parity.None, 8, StopBits.One);
        }


        public bool EnviarComando(string Comando) {
            try {
                arduinoPort.Open();
                arduinoPort.WriteLine(Comando);
                arduinoPort.ReadChar();
                lastRead = arduinoPort.ReadExisting();
                allRead[i++] = lastRead;
                arduinoPort.Close();
                return true;
            }
            catch {
                return false;
            }
        }


        public string PortaCOM {
            get { return string.Format("COM{0}", comPort); }
        }

        public string[] PortasCOMAtivas {
            get { return SerialPort.GetPortNames(); }
        }

    }
}
