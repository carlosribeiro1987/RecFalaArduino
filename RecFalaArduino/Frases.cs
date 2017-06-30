using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassesAssistente {
    public class Frases {
        public static string Hora() {
            string strHora = string.Empty;
            int horas = DateTime.Now.Hour;
            int minutos = DateTime.Now.Minute;

            if (minutos == 0) {
                switch (horas) {
                    case 0:
                        strHora= "Agora é meia-noite";
                        break;
                    case 1:
                        strHora = "Agora é uma hora.";
                        break;
                    case 2:
                        strHora = "Agora são duas horas.";
                        break;
                    case 12:
                        strHora = "Agora é meio-dia.";
                        break;
                    case 21:
                        strHora = "Agora são vinte e uma horas.";
                        break;
                    case 22:
                        strHora = "Agora são vinte e duas horas.";
                        break;
                    default:
                        strHora = string.Format("Agora são {0} horas.", horas);
                        break;
                }
            }
            else if (minutos == 1) {
                switch (horas) {
                    case 1:
                        strHora = "Agora é uma hora e um minuto.";
                        break;
                    case 2:
                        strHora = "Agora são duas horas e um minuto.";
                        break;
                    case 21:
                        strHora = "Agora são vinte e uma horas e um minuto.";
                        break;
                    case 22:
                        strHora = "Agora são vinte e duas horas e um minuto.";
                        break;
                    default:
                        strHora = string.Format("Agora são {0} horas e um minuto.", horas);
                        break;
                }
            }
            else {
                switch (horas) {
                    case 1:
                        strHora = string.Format("Agora é uma hora e {0} minutos.", minutos);
                        break;
                    case 2:
                        strHora = string.Format("Agora são duas horas e {0} minutos.", minutos);
                        break;
                    case 21:
                        strHora = string.Format("Agora são vinte e uma horas e {0} minutos.", minutos);
                        break;
                    case 22:
                        strHora = string.Format("Agora são vinte e duas horas e {0} minutos.", minutos);
                        break;
                    default:
                        strHora = string.Format("Agora são {0} horas e {1} minutos.", horas, minutos);
                        break;
                }
            }
            return strHora;
        }

        public static string Data(DateTime Data) {
            string strData = string.Empty;
            string dia;
            int ano = Data.Year;
            if (Data.Day == 1)
                dia = "primeiro";
            else
                dia = Convert.ToString(Data.Day);

            strData = string.Format("{0}, {1} de {2} de {3}", DiaDaSemana(Data), dia, Mes(Data), ano);
            return strData;
        }


        static string Mes(DateTime Data) {
            int Mes = Data.Month;
            string NomeMes = "";
            switch (Mes) {
                case 1:
                    NomeMes = "Janeiro";
                    break;
                case 2:
                    NomeMes = "Fevereiro";
                    break;
                case 3:
                    NomeMes = "Março";
                    break;
                case 4:
                    NomeMes = "Abril";
                    break;
                case 5:
                    NomeMes = "Maio";
                    break;
                case 6:
                    NomeMes = "Junho";
                    break;
                case 7:
                    NomeMes = "Julho";
                    break;
                case 8:
                    NomeMes = "Agosto";
                    break;
                case 9:
                    NomeMes = "Setembro";
                    break;
                case 10:
                    NomeMes = "Outubro";
                    break;
                case 11:
                    NomeMes = "Novembro";
                    break;
                case 12:
                    NomeMes = "Dezembro";
                    break;
            }
            return NomeMes;
        }
        static string DiaDaSemana(DateTime Data) {
            int DiaSemana = Convert.ToInt16(Data.DayOfWeek);
            string NomeDiaSemana = "";
            switch (DiaSemana) {
                case 0:
                    NomeDiaSemana = "Domingo";
                    break;
                case 1:
                    NomeDiaSemana = "Segunda-Feira";
                    break;
                case 2:
                    NomeDiaSemana = "Terça-Feira";
                    break;
                case 3:
                    NomeDiaSemana = "Quarta-Feira";
                    break;
                case 4:
                    NomeDiaSemana = "Quinta-Feira";
                    break;
                case 5:
                    NomeDiaSemana = "Sexta-Feira";
                    break;
                case 6:
                    NomeDiaSemana = "Sábado";
                    break;
            }
            return NomeDiaSemana;
        }
    }
}