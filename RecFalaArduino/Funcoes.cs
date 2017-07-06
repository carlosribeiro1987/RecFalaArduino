using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace RecFalaArduino {
    public class Funcoes {
        //Abre um programa pelo nome ou path do arquivo
        public static void AbrirPrograma(string Programa) {
                Process.Start(Programa);
        }
        public static void AbrirPrograma(string Programa, string Argumentos) {
            if (Argumentos != "")
                Process.Start(Programa, Argumentos);
            else
                Process.Start(Programa);
        }

        //Retorna o título  da janela ativa
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        public static string TituloJanelaAtiva() {
            const int nChars = 256;
            StringBuilder Buffer = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();
            if (handle == IntPtr.Zero)
                return "Nenhuma janela Ativa no momento";
            if (GetWindowText(handle, Buffer, nChars) > 0) {
                return Buffer.ToString();
            }
            return "Nenhuma janela ativa no momento.";
        }

        //Fechar Janela Ativa
        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, IntPtr wParam, IntPtr lParam);
        public const int WM_SYSCOMMAND = 0x0112;
        public const UInt32 WM_CLOSE = 0x0010;

        public static bool FecharJanela() {
            IntPtr hndJanelaAtiva = GetForegroundWindow();
            if (hndJanelaAtiva == IntPtr.Zero)
                return false;
            if (hndJanelaAtiva != IntPtr.Zero) {
                SendMessage(hndJanelaAtiva, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                return true;
            }
            else
                return false;
        }

        public static bool FecharJanela(IntPtr hwndJanela) {
            if (hwndJanela == IntPtr.Zero)
                return false;
            if (hwndJanela != IntPtr.Zero) {
                SendMessage(hwndJanela, WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                return true;
            }
            else
                return false;
        }
    }
}
