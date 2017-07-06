using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Loterias;
using ClassesAssistente;

namespace RecFalaArduino {
    public class Respostas {
        public enum Loteria { Nenhuma, DuplaSena, LoteriaFederal, LotoFacil, LotoMania, MegaSena, Quina }



        public static string SeparaStrArrays(string[] StrArray) {
            string Retorno = string.Empty;
            string temp = string.Empty;

            for (int i = 0; i < StrArray.Length; i++) {
                string atual = StrArray[i];
                if (i == StrArray.Length - 1)
                    temp = string.Format("{0} e {1}", temp, atual);
                else {
                    if (i > 0)
                        temp = string.Format("{0}, {1}", temp, atual);
                    else
                        temp = atual;
                }
            }

            Retorno = temp;

            return Retorno;
        }

        public static string[] SeparaStrArrays(string[] StrArray, int ItemsPorLinha) {
            int numLinhas = StrArray.Length / ItemsPorLinha;
            string[] Retorno = new string[numLinhas];
            string temp = string.Empty;
            int cont = 0;
            int linha = 0;

            while (cont < StrArray.Length) {
                for (int i = 0; i < ItemsPorLinha; i++) {
                    if (i < ItemsPorLinha) {
                        temp += StrArray[cont] + " ";
                        cont++;
                    }
                    else {
                        temp += StrArray[cont];
                        cont++;
                    }
                }
                Retorno[linha] = temp;
                linha++;
                temp = "";
            }
            //Retorno = temp;
                        
            return Retorno;
        }
    }

}
