using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecFalaArduino {
    public class Graficos {

        public static string Caixa(string[] Texto) {
            string LinhaSuperior = "";
            string LinhaInferior = "";
            string LinhaTexto = "";
            string LinhaBranco = "";
            string Caixa = "";
            int LarguraTexto = 0;
            //String mais longa do array
            for (int i = 0; i < Texto.Length; i++) {
                if (Texto[i].Length > i)
                    LarguraTexto = Texto[i].Length;
            }

            //Linha Superior
            LinhaSuperior += "\t╔";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaSuperior += "═";
            LinhaSuperior += "╗\n";

            //Linha em Branco
            LinhaBranco = "\t║";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaBranco += " ";
            LinhaBranco += "║\n";

            //Linhas com Texto
            for (int i = 0; i < Texto.Length; i++) {
                LinhaTexto += "\t║ " + Texto[i];
                for (int j = 0; j < LarguraTexto - Texto[i].Length; j++) {
                    LinhaTexto += " ";
                }
                LinhaTexto += " ║\n";
            }

            //Linha Inferior
            LinhaInferior += "\t╚";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaInferior += "═";
            LinhaInferior += "╝\n";

            Caixa = string.Format("\n{0}{1}{2}{3}{4}\n", LinhaSuperior, LinhaBranco, LinhaTexto, LinhaBranco, LinhaInferior);
            return Caixa;
        }

        public static string Caixa(string[] Texto, int Largura) {
            string LinhaSuperior = "";
            string LinhaInferior = "";
            string LinhaTexto = "";
            string LinhaBranco = "";
            string Caixa = "";
            int LarguraTexto = 0;
            //String mais longa do array
            for (int i = 0; i < Texto.Length; i++) {
                if (Texto[i].Length > i)
                    LarguraTexto = Texto[i].Length;
            }

            if (LarguraTexto <= Largura)
                LarguraTexto = Largura;
            if (LarguraTexto > Console.WindowWidth - 20)
                LarguraTexto = Console.WindowWidth - 20;

            //Linha Superior
            LinhaSuperior += "\t╔";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaSuperior += "═";
            LinhaSuperior += "╗\n";

            //Linha em Branco
            LinhaBranco = "\t║";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaBranco += " ";
            LinhaBranco += "║\n";

            //Linhas com Texto
            for (int i = 0; i < Texto.Length; i++) {
                LinhaTexto += "\t║ " + Texto[i];
                for (int j = 0; j < LarguraTexto - Texto[i].Length; j++) {
                    LinhaTexto += " ";
                }
                LinhaTexto += " ║\n";
            }

            //Linha Inferior
            LinhaInferior += "\t╚";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaInferior += "═";
            LinhaInferior += "╝\n";

            Caixa = string.Format("\n{0}{1}{2}{3}{4}\n", LinhaSuperior, LinhaBranco, LinhaTexto, LinhaBranco, LinhaInferior);
            return Caixa;
        }

        public static string Caixa(string Titulo, string[] Texto) {
            string LinhaSuperior = "";
            string LinhaInferior = "";
            string LinhaTexto = "";
            string LinhaBranco = "";
            string LinhaTitulo = "";
            string SeparaTitulo = "";
            string Caixa = "";
            int LarguraTexto = 0;
            //String mais longa do array
            for (int i = 0; i < Texto.Length; i++) {
                if (Texto[i].Length > i)
                    LarguraTexto = Texto[i].Length;
            }
            if (LarguraTexto < Titulo.Length)
                LarguraTexto = Titulo.Length;

            //Linha Superior
            LinhaSuperior += "\t╔";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaSuperior += "═";
            LinhaSuperior += "╗\n";

            //Linha Inferior
            LinhaInferior += "\t╚";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaInferior += "═";
            LinhaInferior += "╝\n";

            //Linha Separador Titulo
            SeparaTitulo += "\t╠";
            for (int i = 0; i < LarguraTexto + 2; i++)
                SeparaTitulo += "═";
            SeparaTitulo += "╣\n";


            //Linha em Branco
            LinhaBranco = "\t║";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaBranco += " ";
            LinhaBranco += "║\n";

            //Linha do Título
            LinhaTitulo += "\t║ ";
            for (int i = 0; i < (LarguraTexto - Titulo.Length) / 2; i++)
                LinhaTitulo += " ";
            LinhaTitulo += Titulo;
            for (int i = 0; i < (LarguraTexto - Titulo.Length) / 2; i++)
                LinhaTitulo += " ";
            LinhaTitulo += " ║\n";
            int lala = LinhaTitulo.IndexOf('\n') - 1;
            if (LinhaTitulo.Length < LinhaSuperior.Length) {
                string temp;
                while (LinhaTitulo.Length < LinhaSuperior.Length) {
                   temp = LinhaTitulo.Insert(LinhaTitulo.IndexOf('\n') - 1, " ");
                    LinhaTitulo = temp;
                }
            }
            else if (LinhaTitulo.Length > LinhaSuperior.Length) {
                string temp;
                while (LinhaTitulo.Length > LinhaSuperior.Length) {
                    temp = LinhaTitulo.Remove(LinhaTitulo.IndexOf('\n') - 1, 1);
                    LinhaTitulo = temp;
                }
            }

            //Linhas com Texto
            for (int i = 0; i < Texto.Length; i++) {
                LinhaTexto += "\t║ " + Texto[i];
                for (int j = 0; j < LarguraTexto - Texto[i].Length; j++) {
                    LinhaTexto += " ";
                }
                LinhaTexto += " ║\n";
            }



            Caixa = string.Format("\n{0}{1}{2}{3}{4}{5}{6}\n", LinhaSuperior, LinhaTitulo, SeparaTitulo, LinhaBranco, LinhaTexto, LinhaBranco, LinhaInferior);
            return Caixa;
            
        }

        public static string Caixa(string Titulo, string[] Texto, int Largura) {
            string LinhaSuperior = "";
            string LinhaInferior = "";
            string LinhaTexto = "";
            string LinhaBranco = "";
            string LinhaTitulo = "";
            string SeparaTitulo = "";
            string Caixa = "";
            int LarguraTexto = 0;
            //String mais longa do array
            for (int i = 0; i < Texto.Length; i++) {
                if (Texto[i].Length > i)
                    LarguraTexto = Texto[i].Length;
            }
            if (LarguraTexto < Titulo.Length)
                LarguraTexto = Titulo.Length;
            if (LarguraTexto <= Largura)
                LarguraTexto = Largura;
            if (LarguraTexto > Console.WindowWidth - 20)
                LarguraTexto = Console.WindowWidth - 20;

            //Linha Superior
            LinhaSuperior += "\t╔";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaSuperior += "═";
            LinhaSuperior += "╗\n";

            //Linha Inferior
            LinhaInferior += "\t╚";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaInferior += "═";
            LinhaInferior += "╝\n";

            //Linha Separador Titulo
            SeparaTitulo += "\t╠";
            for (int i = 0; i < LarguraTexto + 2; i++)
                SeparaTitulo += "═";
            SeparaTitulo += "╣\n";


            //Linha em Branco
            LinhaBranco = "\t║";
            for (int i = 0; i < LarguraTexto + 2; i++)
                LinhaBranco += " ";
            LinhaBranco += "║\n";

            //Linha do Título
            LinhaTitulo += "\t║ ";
            for (int i = 0; i < (LarguraTexto - Titulo.Length) / 2; i++)
                LinhaTitulo += " ";
            LinhaTitulo += Titulo;
            for (int i = 0; i < (LarguraTexto - Titulo.Length) / 2; i++)
                LinhaTitulo += " ";
            LinhaTitulo += " ║\n";

            if (LinhaTitulo.Length < LarguraTexto) {
                while (LinhaTitulo.Length < LarguraTexto)
                    LinhaTitulo.Replace("║\n", " ║\n");
            }
            else if (LinhaTitulo.Length > LarguraTexto) {
                while (LinhaTitulo.Length > LarguraTexto)
                    LinhaTitulo.Replace(" ║\n", "║\n");
            }

            //Linhas com Texto
            for (int i = 0; i < Texto.Length; i++) {
                LinhaTexto += "\t║ " + Texto[i];
                for (int j = 0; j < LarguraTexto - Texto[i].Length; j++) {
                    LinhaTexto += " ";
                }
                LinhaTexto += " ║\n";
            }



            Caixa = string.Format("\n{0}{1}{2}{3}{4}{5}{6}\n", LinhaSuperior, LinhaTitulo, SeparaTitulo, LinhaBranco, LinhaTexto, LinhaBranco, LinhaInferior);
            return Caixa;
        }
    }
}
