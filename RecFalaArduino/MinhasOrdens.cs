using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecFalaArduino {
    /// <summary>
    /// Classe contendo
    /// </summary>
    public class MinhasOrdens {

        #region COMANDOS EXECUTADOS NO COMPUTADOR
        public static string[] Confirmar = {
            "Sim",
            "Não",
            "Pode Fechar",
            "Tenho",
            "Cancelar",
            "Ok"
        };
        public static string[] Comandos = {
            "Limpar Janela",
            "Limpar Mensagens",
            "Limpar Tela",
            "Limpar a Tela",
            "Limpar Console",

            "Que horas são",
            "Me diga as horas",

            "Que dia é hoje",
            "Me diga a data",
            "Data de hoje",

            "Desligue o computador",
            "Desligar computador",
            "Desligar PC",
            "Desligue o PC",

            "Cancelar Desligamento",
            "Não quero mais desligar o PC",
            "Não Desligar",

            "Abrir o Facebook",
            "Abrir Calculadora",
            "Abrir Bloco de Notas",
            "Qual é a janela ativa",

            

            "Trocar de janela", 
            "Alternar janela",
            "Mudar de janela",

            "Feche essa janela",
            "Feche a janela",
            "Fechar janela",
            "Sair",
            "Encerrar",

        };
        #endregion COMANDOS EXECUTADOS NO COMPUTADOR

        #region CONVERSAS ALEATÓRIAS
        public static string[] Conversas = {
            "Cala a boca",
            "Cale a boca",
            "Cala boca",
            "Silêncio",
            "O que você é",
            "Quem é você",
            "Como você se sente",

        };
        #endregion CONVERSAS ALEATÓRIAS

        #region COMANDOS EXECUTADOS NO ARDUINO
        public static string[] Arduino = {
            //ACENDER LÂMPADA
            "acender lâmpada",
            "acender luz",
            "ligar lâmpada",
            "ligue a lâmpada",
            "acenda a luz",
            "ligue a luz",
            //APAGAR LÂMPADA
            "apagar lâmpada",
            "apagar luz",
            "desligar lâmpada",
            "desligue a lâmpada",
            "apague a luz",
            "desligue a luz",
        };

        #endregion COMANDOS EXECUTADOS NO ARDUINO

        #region RESULTADOS DAS LOTERIAS
        public static string[] Loterias = {
            //LOTOFÁCIL
            "Qual é o resultado da Lotofácil",
            "Fale o resultado da Lotofácil",
            "Me diz o resultado da Lotofácil",
            "Me diga o resultado da Lotofácil",
            "Diga-me o resultado da Lotofácil",

            "Mostre na tela o resultado da Lotofácil",
            "Mostre o resultado da Lotofácil na tela",
            "Mostre-me o resultado da Lotofácil",            

            //LOTOMANIA
            "Qual é o resultado da Lotomania",
            "Fale o resultado da Lotomania",
            "Me diz o resultado da Lotomania",
            "Me diga o resultado da Lotomania",
            "Diga-me o resultado da Lotomania",

            "Mostre na tela o resultado da Lotomania",
            "Mostre o resultado da Lotomania na tela",
            "Mostre-me o resultado da Lotomania",

            //MEGA-SENA
            "Qual é o resultado da Mega Sena",
            "Fale o resultado da Mega Sena",
            "Me diz o resultado da Mega Sena",
            "Me diga o resultado da Mega Sena",
            "Diga-me o resultado da Mega Sena",

            "Mostre na tela o resultado da Mega Sena",
            "Mostre o resultado da Mega Sena na tela",
            "Mostre-me o resultado da Mega Sena",

            //DUPLA-SENA
            "Qual é o resultado da Dupla Sena",
            "Fale o resultado da Dupla Sena",
            "Me diz o resultado da Dupla Sena",
            "Me diga o resultado da Dupla Sena",
            "Diga-me o resultado da Dupla Sena",

            "Mostre na tela o resultado da Dupla Sena",
            "Mostre o resultado da Dupla Sena na tela",
            "Mostre-me o resultado da Dupla Sena",

            //QUINA
            "Qual é o resultado da Quina",
            "Fale o resultado da Quina",
            "Me diz o resultado da Quina",
            "Me diga o resultado da Quina",
            "Diga-me o resultado da Quina",

            "Mostre na tela o resultado da Quina",
            "Mostre o resultado da Quina na tela",
            "Mostre-me o resultado da Quina",
        };
        #endregion  RESULTADOS DAS LOTERIAS

        public static string[] PrevisaoTempo = {
            "Qual é a previsão do tempo de amanhã",
            "Qual a previsão do tempo amanhã",
            "Qual a previsão do tempo para amanha",

            "Qual é a previsão do tempo de depois de amanhã",
            "Qual a previsão do tempo depois de amanhã",
            "Qual a previsão do tempo para depois de amanha",

            "Qual é a previsão do tempo de segunda-feira",
            "Qual a previsão do tempo segunda-feira",
            "Qual a previsão do tempo para segunda-feira",
            "Qual é a previsão do tempo de segunda",
            "Qual a previsão do tempo segunda",
            "Qual a previsão do tempo para segunda",

            "Qual é a previsão do tempo de terça-feira",
            "Qual a previsão do tempo terça-feira",
            "Qual a previsão do tempo para terça-feira",
            "Qual é a previsão do tempo de terça",
            "Qual a previsão do tempo terça",
            "Qual a previsão do tempo para terça",

            "Qual é a previsão do tempo de quarta-feira",
            "Qual a previsão do tempo quarta-feira",
            "Qual a previsão do tempo para quarta-feira",
            "Qual é a previsão do tempo de quarta",
            "Qual a previsão do tempo quarta",
            "Qual a previsão do tempo para quarta",

            "Qual é a previsão do tempo de quinta-feira",
            "Qual a previsão do tempo quinta-feira",
            "Qual a previsão do tempo para quinta-feira",
            "Qual é a previsão do tempo de quinta",
            "Qual a previsão do tempo quinta",
            "Qual a previsão do tempo para quinta",

            "Qual é a previsão do tempo de sexta-feira",
            "Qual a previsão do tempo sexta-feira",
            "Qual a previsão do tempo para sexta-feira",
            "Qual é a previsão do tempo de sexta",
            "Qual a previsão do tempo sexta",
            "Qual a previsão do tempo para sexta",

            "Qual é a previsão do tempo de sábado",
            "Qual a previsão do tempo sábado",
            "Qual a previsão do tempo para sábado",

            "Qual é a previsão do tempo de domingo",
            "Qual a previsão do tempo domingo",
            "Qual a previsão do tempo para domingo",

        };
    }
}
