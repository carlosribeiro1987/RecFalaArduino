﻿using ClassesAssistente;
using Loterias;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecFalaArduino {

    class ReconhecimendoFalaArduino {
        private static SpeechRecognitionEngine engineVoz = new SpeechRecognitionEngine();
        private static SpeechSynthesizer synthVoz = new SpeechSynthesizer();
        public enum Funcao { Nenhuma, DesligarPC, FecharAssistente };
        public static Funcao FuncaoAtiva = Funcao.Nenhuma;
        // bool Ouvindo = false;
        static void Main(string[] args) {
            bool PodeFechar = false;
            #region INICIANDO O PROGRAMA          
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

            engineVoz = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-BR"));
            engineVoz.SetInputToDefaultAudioDevice();
            Console.Title = "Reconhecimento de Fala + Controle Arduino por voz - Carlos Ribeiro";
            Console.WriteLine("\nIniciando...\n");
            #endregion INICIANDO

            #region PREPARANDO O RECONHECIMENTO DE FALA

            #region CONVERSAS

            Choices c_conversas = new Choices(MinhasOrdens.Conversas);
            GrammarBuilder gb_conversas = new GrammarBuilder();
            gb_conversas.Append(c_conversas);
            Grammar g_conversas = new Grammar(gb_conversas);
            g_conversas.Name = "grammarConversas";

            #endregion CONVERSAS

            #region COMANDOS EXECUTADOS NO COMPUTADOR

            Choices c_comandosSistema = new Choices(MinhasOrdens.Comandos);
            GrammarBuilder gb_comandosSistema = new GrammarBuilder();
            gb_comandosSistema.Append(c_comandosSistema);
            Grammar g_comandosSistema = new Grammar(gb_comandosSistema);
            g_comandosSistema.Name = "grammarComandosPC";

            #endregion COMANDOS COMPUTADOR

            #region COMANDOS EXECUTADOS NO ARDUINO

            Choices c_comandosArduino = new Choices(MinhasOrdens.Arduino);
            GrammarBuilder gb_comandosArduino = new GrammarBuilder();
            gb_comandosArduino.Append(c_comandosArduino);
            Grammar g_comandosArduino = new Grammar(gb_comandosArduino);
            g_comandosArduino.Name = "grammarArduino";

            #endregion COMANDOS ARDUINO

            #region RESULTADOS DAS LOTERIAS

            Choices c_loterias = new Choices(MinhasOrdens.Loterias);
            GrammarBuilder gb_loterias = new GrammarBuilder();
            gb_loterias.Append(c_loterias);
            Grammar g_loterias = new Grammar(gb_loterias);
            g_loterias.Name = "grammarLoterias";

            #endregion RESULTADOS DAS LOTERIAS

            #region CONFIRMAR COMANDO


            Choices c_confirmar = new Choices(MinhasOrdens.Confirmar);
            GrammarBuilder gb_confirmar = new GrammarBuilder();
            gb_confirmar.Append(c_confirmar);
            Grammar g_confirmar = new Grammar(gb_confirmar);
            g_confirmar.Name = "grammarConfirmar";

            #endregion CONFIRMAR


            #endregion PREPARANDO RECONHECIMENTO DE FALA

            #region CARREGAR GRAMMAR
            Console.Write("#############");
            engineVoz.LoadGrammar(g_conversas);
            Console.Write("#############");
            engineVoz.LoadGrammar(g_comandosSistema);
            Console.Write("#############");
            engineVoz.LoadGrammar(g_comandosArduino);
            Console.Write("#############");
            engineVoz.LoadGrammar(g_loterias);
            Console.Write("#############");
            engineVoz.LoadGrammar(g_confirmar);
            Console.Write("#############\n\n");

            #endregion CARREGAR GRAMMAR


            #region RECONHECIMENTO DE FALA

            do {
                //try {
                synthVoz.SelectVoiceByHints(VoiceGender.Male);
                engineVoz.SpeechRecognized += ReconhecerVoz;
                engineVoz.RecognizeAsync(RecognizeMode.Multiple);
                Falar("Olá mestre, estou aqui para serví-lo. O que deseja?");
                Console.WriteLine("\n\nEstou ouvindo. O que deseja?");
                // Console.WriteLine(Respostas.ResultadoLoterias(Respostas.Loteria.MegaSena));
                //  Console.WriteLine(Respostas.ResultadoLoterias(Respostas.Loteria.DuplaSena));
                //}
                //catch(Exception e) {
                //    Console.WriteLine("Ocorreu um erro: " + e);
                //}

                Console.ReadKey();
            } while (!PodeFechar);

        }

        private static void ReconhecerVoz(object sender, SpeechRecognizedEventArgs e) {
            if (e.Result.Confidence >= 0.4F) {
                string OrdemFalada = e.Result.Text;
                string Confianca = string.Format("{0:#.##}%", e.Result.Confidence * 100);
                switch (e.Result.Grammar.Name) {
                    case "grammarConversas":
                        Console.WriteLine("\nCONVERSA...");
                        Console.WriteLine(string.Format("Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca));
                        ProcessarConversa(OrdemFalada);
                        break;
                    case "grammarComandosPC":
                        Console.WriteLine("\nCOMANDO PC...");
                        Console.WriteLine(string.Format("\nMinha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca));
                        ProcessarComandoPC(OrdemFalada);
                        break;
                    case "grammarArduino":
                        Console.WriteLine("\nCOMANDO ARDUINO...");
                        Console.WriteLine(string.Format("Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca));
                        ProcessarComandoArduino(OrdemFalada);
                        break;
                    case "grammarLoterias":
                        Console.WriteLine("\nLOTERIAS...");
                        Console.WriteLine(string.Format("Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca));
                        ProcessarLoteria(OrdemFalada);
                        break;
                    case "grammarConfirmar":
                        if (e.Result.Confidence > 0.7) {
                            Console.WriteLine("\nCONFIRMAÇÃO...");
                            Console.WriteLine(string.Format("Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca));
                            ProcessarConfirmacao(OrdemFalada);
                        }
                        break;
                    default:
                        break;
                }

            }
            else {
                Console.WriteLine("\n\nCOMANDO NÃO RECONHECIDO!!!\nAmbiente ruidoso ou não há função implementada para o comando.");
            }
        }

        #endregion RECONHECIMENTO DE FALA

        public static void ProcessarComandoPC(string Ordem) {
            switch (Ordem) {
                case "Que horas são":
                    Falar(Frases.Hora());
                    break;
                case "Que dia é hoje":
                    Falar("Hoje é " + Frases.Data(DateTime.Now.Date));
                    break;
                case "Desligue o computador":
                case "Desligar computador":
                case "Desligar PC":
                case "Desligue o PC":
                    FuncaoAtiva = Funcao.DesligarPC;
                    Falar("Tem certeza que quer desligar o computador?");
                    break;
                case "Sair":
                    FuncaoAtiva = Funcao.FecharAssistente;
                    Falar("Tem certeza que posso encerrar?");
                    break;

                case "Feche essa janela":
                case "Feche a janela":
                case "Fechar janela":
                    Falar("Desculpe-me venerável méstri, ainda não sei fechar janelas!"); //Falta implementar
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }

        public static void ProcessarConversa(string Ordem) {
            switch (Ordem) {
                case "Cala a boca":
                case "Cale a boca":
                case "Cala boca":
                    Falar("Desculpe mestre! Não quis incomodar.");
                    break;
                case "O que você é":
                case "Quem é você":
                    Falar("Sou um sóftuér criado por você querido mestre. Estou aqui para obedecê-lo.");
                    break;
                case "Como você se sente":
                    Falar("Sentir? Sou apenas um sóftuér,  não sei o que isso significa.");
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }

        public static void ProcessarComandoArduino(string Ordem) {
            switch (Ordem) {
                case "acender lâmpada":
                case "acender luz":
                case "ligar lâmpada":
                case "ligue a lâmpada":
                case "acenda a luz":
                case "ligue a luz":
                    if (Arduino.EnviarComando("L"))
                        Falar("Lâmpada acesa.");
                    else {
                        Console.WriteLine("Não Foi possível acessar o Arduino.\nVerifique a conexão.");
                        Falar("Desculpe. Não foi possível acender a lâmpada");
                    }
                    break;
                case "apagar lâmpada":
                case "apagar luz":
                case "desligar lâmpada":
                case "desligue a lâmpada":
                case "apague a luz":
                case "desligue a luz":
                    if (Arduino.EnviarComando("D"))
                        Falar("Lâmpada apagada.");
                    else {
                        Console.WriteLine("Não Foi possível acessar o Arduino.\nVerifique a conexão.");
                        Falar("Desculpe. Não foi possível apagar a lâmpada");
                    }
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }

        public static void ProcessarLoteria(string Ordem) {
            switch (Ordem) {
                case "Qual é o resultado da Lotofácil":
                    FalarCompleto("Espere um momento mestre. Vou conferir o resultado.");
                    Console.WriteLine("\nTentando conexão com o site da Caixa...");
                    Lotofacil lotofacil = new Lotofacil();
                    if (lotofacil.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(lotofacil.ResultadoArray);
                        Console.WriteLine("\nResultado da Lotofácil:\n" + Resultado);

                        Falar(string.Format("Os números do último concurso da Lotofácil, sorteados {0} foram: {1}", Frases.Data(lotofacil.DataSorteio), Resultado));
                    }
                    else {
                        Console.WriteLine("\nNão foi possível acessar o site da Caixa.");
                        Falar("Desculpe-me méstri. Não consegui acessar o resultado.");
                    }
                    break;
                case "Qual é o resultado da Mega Sena":
                    FalarCompleto("Espere um momento mestre. Vou conferir o resultado.");
                    Console.WriteLine("\nTentando conexão com o site da Caixa...");
                    MegaSena megasena = new MegaSena();
                    if (megasena.ObteveResultado) {
                        Console.WriteLine("\n" + Respostas.SeparaStrArrays(megasena.ResultadoArray));
                        //Console.WriteLine("\n" + Respostas.Loteria.MegaSena);
                        Falar(string.Format("Os números do último concurso da Mega-Sena, sorteados {0} foram: {1}",
                                              Frases.Data(megasena.DataSorteio), megasena.ResultadoString.Replace(" ", ", ")));
                    }
                    else {
                        Console.WriteLine("\nNão foi possível acessar o site da Caixa.");
                        Falar("Desculpe-me méstri. Não consegui acessar o resultado.");

                    }
                    //  Falar(Respostas.ResultadoLoterias(Respostas.Loteria.MegaSena));
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido méstrih?");
                    break;
            }
        }
        public static void ProcessarConfirmacao(string Ordem) {
            switch (Ordem) {
                case "Sim":
                    switch (FuncaoAtiva) {
                        case Funcao.DesligarPC:
                            Falar("Entendi. Vou desligar o computador.");
                            //Implementar função para desligar
                            break;
                        case Funcao.FecharAssistente:
                            FalarCompleto("Ok. Até mais mestre, foi uma honra serví-lo.");
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("\nNenhuma função ativa!");
                            break;
                    }
                    break;
                case "Não":
                    switch (FuncaoAtiva) {
                        case Funcao.DesligarPC:
                            Falar("Ok. Não vou desligar o PC");
                            FuncaoAtiva = Funcao.Nenhuma;
                            break;
                        case Funcao.FecharAssistente:
                            Falar("Ok. É uma honra continuar a serví-lo, querido mestre.");
                            break;
                        default:
                            Console.WriteLine("\nNenhuma função ativa");
                            FuncaoAtiva = Funcao.Nenhuma;
                            break;
                    }
                    break;
                default:
                    FuncaoAtiva = Funcao.Nenhuma;
                    break;
            }
        }

        //Fala a frase, mas pode ser interrompido
        private static void Falar(string Texto) {
            synthVoz.SpeakAsyncCancelAll();
            synthVoz.SpeakAsync(Texto);
        }

        //Fala a frase até o final
        private static void FalarCompleto(string Texto) {
            synthVoz.SpeakAsyncCancelAll();
            synthVoz.Speak(Texto);
        }

        private static void AbrirJanela(Form Janela) {
            Application.Run(Janela);

        }
    }
}