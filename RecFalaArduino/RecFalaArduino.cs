using ClassesAssistente;
using Loterias;
using Microsoft.Speech.Recognition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrevisaoTempoINPE;
using System.Diagnostics;
using System.Threading;

namespace RecFalaArduino {

    class ReconhecimendoFalaArduino {
       // System.Threading.Timer tmrArduino = new System.Threading.Timer(Arduino.LerSerial, null, 0, 1000);

        private static SpeechRecognitionEngine engineVoz = new SpeechRecognitionEngine();
        private static SpeechSynthesizer synthVoz = new SpeechSynthesizer();
        public enum Funcao { Nenhuma, DesligarPC, FecharAssistente, FecharJanela };
        public static Funcao FuncaoAtiva = Funcao.Nenhuma;
        public static ConsoleColor CorFundoConsole = Console.BackgroundColor;
        public static ConsoleColor CorFonteConsole = Console.ForegroundColor;
        const int codigoLocalidade = 837; //Utilizado na previsão do tempo
        // bool Ouvindo = false;
        static void Main(string[] args) {
            bool PodeFechar = false;
            #region INICIANDO O PROGRAMA          
            
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Console.SetWindowSize(120, 60);
            Console.ForegroundColor = ConsoleColor.Green;
            engineVoz = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("pt-BR"));
            engineVoz.SetInputToDefaultAudioDevice();
            Console.Title = "Reconhecimento de Fala + Controle Arduino por voz - Carlos Ribeiro";
            // Console.WriteLine("\n\tIniciando...\n");
            Console.Write(Graficos.Caixa(new string[] { "RECONHECIMENTO DE FALA" }, 100));
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

            #region PREVISÃO DO TEMPO           

            Choices c_prev_tempo = new Choices(MinhasOrdens.PrevisaoTempo);
            GrammarBuilder gb_prev_tempo = new GrammarBuilder();
            gb_prev_tempo.Append(c_prev_tempo);
            Grammar g_prev_tempo = new Grammar(gb_prev_tempo);
            g_prev_tempo.Name = "grammarPrevisaoTempo";

            #endregion PREVISÃO DO TEMPO


            #region CONFIRMAR COMANDO


            Choices c_confirmar = new Choices(MinhasOrdens.Confirmar);
            GrammarBuilder gb_confirmar = new GrammarBuilder();
            gb_confirmar.Append(c_confirmar);
            Grammar g_confirmar = new Grammar(gb_confirmar);
            g_confirmar.Name = "grammarConfirmar";

            #endregion CONFIRMAR


            #endregion PREPARANDO RECONHECIMENTO DE FALA

            #region CARREGAR GRAMMAR
            //  Console.Write("\t#############");
            engineVoz.LoadGrammar(g_conversas);
            //    Console.Write("#############");
            engineVoz.LoadGrammar(g_comandosSistema);
            //    Console.Write("#############");
            engineVoz.LoadGrammar(g_comandosArduino);
            //    Console.Write("#############");
            engineVoz.LoadGrammar(g_loterias);

            //  Console.Write("\t#############");
            engineVoz.LoadGrammar(g_prev_tempo);
            //     Console.Write("#############");
            engineVoz.LoadGrammar(g_confirmar);
            //    Console.Write("#############\n\n");

            #endregion CARREGAR GRAMMAR


            #region RECONHECIMENTO DE FALA

            do {
                synthVoz.SelectVoiceByHints(VoiceGender.Male);
                engineVoz.SpeechRecognized += ReconhecerVoz;
                engineVoz.RecognizeAsync(RecognizeMode.Multiple);
                Falar("Olá mestre, estou aqui para serví-lo. O que deseja?");
                LimparConsole();



                Console.ReadKey();
            } while (!PodeFechar);

        }

        /// <summary>
        /// Reconhece os comandos de voz envidos à engine de reconhecimento de voz.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ReconhecerVoz(object sender, SpeechRecognizedEventArgs e) {
            if (e.Result.Confidence >= 0.4F) {
                string OrdemFalada = e.Result.Text;
                string Confianca = string.Format("{0:#.##}%", e.Result.Confidence * 100);
                switch (e.Result.Grammar.Name) {
                    case "grammarConversas":
                        FraseColorida("\n\t[ CONVERSA ALEATÓRIA ]", ConsoleColor.DarkGreen);
                        FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                        ProcessarConversa(OrdemFalada);
                        break;
                    case "grammarComandosPC":
                        FraseColorida("\n\t[ COMANDO PC ]", ConsoleColor.DarkGreen);
                        FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                        ProcessarComandoPC(OrdemFalada);
                        break;
                    case "grammarArduino":
                        FraseColorida("\n\t[ COMANDO ARDUINO ]", ConsoleColor.DarkGreen);
                        FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                        ProcessarComandoArduino(OrdemFalada);
                        break;
                    case "grammarLoterias":
                        FraseColorida("\n\t[ LOTERIAS ]", ConsoleColor.DarkGreen);
                        FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                        ProcessarLoteria(OrdemFalada);
                        break;
                    case "grammarPrevisaoTempo":
                        FraseColorida("\n\t[PREVISÃO DO TEMPO] ", ConsoleColor.DarkGreen);
                        FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                        ProcessarPrevTempo(OrdemFalada);
                        break;
                    case "grammarConfirmar":
                        if (e.Result.Confidence > 0.7) {
                            FraseColorida("\n\t[ CONFIRMAÇÃO ]", ConsoleColor.DarkGreen);
                            FraseColorida(string.Format(" Minha Ordem: \"{0}\".\t[Confiança: {1}]\n", OrdemFalada, Confianca), ConsoleColor.DarkGreen);
                            ProcessarConfirmacao(OrdemFalada);
                        }
                        break;
                    default:
                        break;
                }

            }
            else {
                FraseColorida("\n\tAVISO: Comando não reconhecido!\n", ConsoleColor.Yellow);
            }
        }

        #endregion RECONHECIMENTO DE FALA
        /// <summary>
        /// Processamento dos comandos que manipulam o computador.
        /// </summary>
        /// <param name="Ordem">O comando recebido.</param>
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
                    FraseColorida("\n\t", ConsoleColor.Green, ConsoleColor.Black);
                    FraseColorida("DESLIGAR O COMPUTADOR?\n", ConsoleColor.Red, ConsoleColor.Yellow);
                    break;
                case "Cancelar Desligamento":
                case "Não quero mais desligar o PC":
                case "Não Desligar":
                    CancelarDesligamento();
                    Falar("Desligamento do computador cancelado.");
                    break;
                case "Sair":
                    FuncaoAtiva = Funcao.FecharAssistente;
                    Falar("Tem certeza que posso encerrar?");
                    break;

                case "Feche essa janela":
                case "Feche a janela":
                case "Fechar janela":
                    FuncaoAtiva = Funcao.FecharJanela;
                    Falar("Tem certeza que deseja fechar a janela " + Funcoes.TituloJanelaAtiva() + "?");
                    break;
                case "Qual é a janela ativa":
                    Console.Write("\n\tJanela Ativa: " + Funcoes.TituloJanelaAtiva() + "\n");
                    Falar("A janela ativa é: " + Funcoes.TituloJanelaAtiva());
                    break;
                case "Abrir Bloco de Notas":
                    Funcoes.AbrirPrograma("notepad.exe");
                    break;
                case "Abrir Calculadora":
                    Funcoes.AbrirPrograma("calc.exe");
                    break;
                case "Limpar Janela":
                case "Limpar Mensagens":
                case "Limpar Tela":
                case "Limpar a Tela":
                case "Limpar Console":
                    LimparConsole();
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }

        /// <summary>
        /// Processamento dos comandos de conversas
        /// </summary>
        /// <param name="Ordem">O comando recebido.</param>
        public static void ProcessarConversa(string Ordem) {
            switch (Ordem) {
                case "Cala a boca":
                case "Cale a boca":
                case "Cala boca":
                case "Silêncio":
                    Falar("Ok. Me desculpe.");
                    break;
                case "O que você é":
                case "Quem é você":
                    Falar("Sou um sóftuér criado por você querido mestre. Estou aqui para obedecê-lo.");
                    break;
                case "Como você se sente":
                    Falar("O que é sentir? Sou apenas um sóftuér,  não sei o que isso significa.");
                    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }

        /// <summary>
        /// Processamento dos comandos executados pelo Arduino.
        /// </summary>
        /// <param name="Ordem">O comando recebido.</param>
        public static void ProcessarComandoArduino(string Ordem) {
            Arduino arduino = new Arduino(3);
            switch (Ordem) {
                case "acender lâmpada":
                case "acender luz":
                case "ligar lâmpada":
                case "ligue a lâmpada":
                case "acenda a luz":
                case "ligue a luz":
                    if (arduino.EnviarComando("L"))
                        Falar("Lâmpada acesa.");
                    else {
                        FraseColorida(string.Format("\n\tERRO: Não Foi possível acessar o Arduino na porta {0}. Verifique a conexão.\n", arduino.PortaCOM), ConsoleColor.Red);
                        FraseColorida(Graficos.Caixa("Portas COM Ativas", arduino.PortasCOMAtivas), ConsoleColor.DarkRed);
                        Falar("Desculpe. Não foi possível acender a lâmpada");
                    }
                    break;
                case "apagar lâmpada":
                case "apagar luz":
                case "desligar lâmpada":
                case "desligue a lâmpada":
                case "apague a luz":
                case "desligue a luz":
                    if (arduino.EnviarComando("D"))
                        Falar("Lâmpada apagada.");
                    else {
                        FraseColorida("\n\tERRO: Não Foi possível acessar o Arduino. Verifique a conexão.", ConsoleColor.Red);
                        Falar("Desculpe. Não foi possível apagar a lâmpada");
                    }
                    break;
                
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido mestre?");
                    break;
            }
        }
        /// <summary>
        /// Processamento dos comandos para obtenção dos resultados das loterias.
        /// </summary>
        /// <param name="Ordem">O comando recebido.</param>
        public static void ProcessarLoteria(string Ordem) {
            switch (Ordem) {
                //LOTOFÁCIL
                case "Qual é o resultado da Lotofácil":
                case "Fale o resultado da Lotofácil":
                case "Me diz o resultado da Lotofácil":
                case "Me diga o resultado da Lotofácil":
                case "Diga-me o resultado da Lotofácil":
                    FalarCompleto("Espere um momento. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    Lotofacil lotofacil = new Lotofacil();
                    if (lotofacil.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(lotofacil.ResultadoArray);
                        FraseColorida(Graficos.Caixa("LOTOFÁCIL", Respostas.SeparaStrArrays(lotofacil.ResultadoArray, 5)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(lotofacil.Concurso) + "\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(lotofacil.DataSorteio.Date).Substring(0, 10) + "\n", ConsoleColor.Cyan);

                        Falar(string.Format("Os números do último concurso da Lotofácil, sorteados {0} foram: {1}", Frases.Data(lotofacil.DataSorteio), Resultado));
                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me méstri. Não consegui acessar o resultado.");
                    }
                    break;
                //MEGA SENA
                case "Qual é o resultado da Mega Sena":
                case "Fale o resultado da Mega Sena":
                case "Me diz o resultado da Mega Sena":
                case "Me diga o resultado da Mega Sena":
                case "Diga-me o resultado da Mega Sena":
                    FalarCompleto("Espere um momento. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    MegaSena megasena = new MegaSena();
                    if (megasena.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(megasena.ResultadoArray);
                        FraseColorida(Graficos.Caixa("MEGA SENA", Respostas.SeparaStrArrays(megasena.ResultadoArray, 6)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(megasena.Concurso) + "\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(megasena.DataSorteio).Substring(0, 10) + "\n", ConsoleColor.Cyan);
                        Falar(string.Format("Os números do último concurso da Mega-Sena, sorteados {0} foram: {1}", Frases.Data(megasena.DataSorteio), Resultado));


                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                    }
                    break;
                //LOTOMANIA
                case "Qual é o resultado da Lotomania":
                case "Fale o resultado da Lotomania":
                case "Me diz o resultado da Lotomania":
                case "Me diga o resultado da Lotomania":
                case "Diga-me o resultado da Lotomania":
                    FalarCompleto("Espere um momento. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    LotoMania lotomania = new LotoMania();
                    if (lotomania.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(lotomania.ResultadoArray);
                       FraseColorida(Graficos.Caixa("LOTOMANIA", Respostas.SeparaStrArrays(lotomania.ResultadoArray, 5)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(lotomania.Concurso)+"\n",ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(lotomania.DataSorteio.Date).Substring(0, 10)+"\n", ConsoleColor.Cyan);
                        Falar(string.Format("Os números do último concurso da Loto Mania, sorteados {0} foram: {1}", Frases.Data(lotomania.DataSorteio), Resultado));
                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                    }
                    break;
                //QUINA
                case "Qual é o resultado da Quina":
                case "Fale o resultado da Quina":
                case "Me diz o resultado da Quina":
                case "Me diga o resultado da Quina":
                case "Diga-me o resultado da Quina":
                    FalarCompleto("Espere um momento. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    Quina quina = new Quina();
                    if (quina.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(quina.ResultadoArray);
                        FraseColorida(Graficos.Caixa("QUINA", Respostas.SeparaStrArrays(quina.ResultadoArray, 5)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(quina.Concurso) + "\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(quina.DataSorteio.Date).Substring(0, 10) + "\n", ConsoleColor.Cyan);
                        Falar(string.Format("Os números do último concurso da Quina, sorteados {0} foram: {1}", Frases.Data(quina.DataSorteio), Resultado));
                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                    }
                    break;
                //DUPLA-SENA
                case "Qual é o resultado da Dupla Sena":
                case "Fale o resultado da Dupla Sena":
                case "Me diz o resultado da Dupla Sena":
                case "Me diga o resultado da Dupla Sena":
                case "Diga-me o resultado da Dupla Sena":
                    FalarCompleto("Espere um momento. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    DuplaSena duplasena = new DuplaSena();
                    if (duplasena.ObteveResultado) {
                        string Resultado1 = Respostas.SeparaStrArrays(duplasena.PrimeiroSorteioArray);
                        string Resultado2 = Respostas.SeparaStrArrays(duplasena.SegundoSorteioArray);
                        FraseColorida(Graficos.Caixa("DUPLA-SENA 1º Sorteio", Respostas.SeparaStrArrays(duplasena.PrimeiroSorteioArray, 6)), ConsoleColor.Cyan);
                        FraseColorida(Graficos.Caixa("DUPLA-SENA 2º Sorteio", Respostas.SeparaStrArrays(duplasena.SegundoSorteioArray, 6)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(duplasena.Concurso) + "\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(duplasena.DataSorteio.Date).Substring(0, 10) + "\n", ConsoleColor.Cyan);
                        Falar(string.Format("Os números do último concurso da Dupla-Sena, sorteados {0} foram: Primeiro sorteio: {1}. Segundo sorteio: {2}", Frases.Data(duplasena.DataSorteio), Resultado1, Resultado2));
                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                    }
                    break;


                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido méstrih?");
                    break;
            }
        }
        /// <summary>
        /// Processamento dos comandos para obtenção da previsão do tempo.
        /// </summary>
        /// <param name="Ordem">O comando recebido.</param>
        public static void ProcessarPrevTempo(string Ordem) {
            
            string strPrev;
            switch (Ordem) {
                //AMANHÃ
                case "Qual é a previsão do tempo de amanhã":
                case "Qual a previsão do tempo amanhã":
                case "Qual a previsão do tempo para amanha":
                    //Falar("Espere um momento. Vou conferir.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        if (prev.ObtevePrevisao) {
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == DateTime.Now.Date.AddDays(1)) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - "+ prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade + "-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para amanhã em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //DEPOIS DE AMANHÃ
                case "Qual é a previsão do tempo de depois de amanhã":
                case "Qual a previsão do tempo depois de amanhã":
                case "Qual a previsão do tempo para depois de amanha":
                    //Falar("Espere um momento. Vou conferir.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        if (prev.ObtevePrevisao) {
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == DateTime.Now.Date.AddDays(2)) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para depois de amanhã em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //DOMINGO
                case "Qual é a previsão do tempo de domingo":
                case "Qual a previsão do tempo domingo":
                case "Qual a previsão do tempo para domingo":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {                                
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Sunday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para o próximo domingo");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }) ,ConsoleColor.DarkCyan);                                
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para domingo em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;

                //SEGUNDA-FEIRA
                case "Qual a previsão do tempo segunda-feira":
                case "Qual a previsão do tempo para segunda-feira":
                case "Qual é a previsão do tempo de segunda":
                case "Qual a previsão do tempo segunda":
                case "Qual a previsão do tempo para segunda":
                case "Qual é a previsão do tempo de segunda-feira":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);

                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Monday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para a próxima segunda-feira");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) { 
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para segunda-feira em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //TERÇA-FEIRA
                case "Qual é a previsão do tempo de terça-feira":
                case "Qual a previsão do tempo terça-feira":
                case "Qual a previsão do tempo para terça-feira":
                case "Qual é a previsão do tempo de terça":
                case "Qual a previsão do tempo terça":
                case "Qual a previsão do tempo para terça":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Tuesday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para a próxima terça-feira");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para terça-feira em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //QUARTA-FEIRA

                case "Qual é a previsão do tempo de quarta-feira":
                case "Qual a previsão do tempo quarta-feira":
                case "Qual a previsão do tempo para quarta-feira":
                case "Qual é a previsão do tempo de quarta":
                case "Qual a previsão do tempo quarta":
                case "Qual a previsão do tempo para quarta":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Wednesday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para a próxima quarta-feira");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para quarta-feira em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //QUINTA-FEIRA
                case "Qual é a previsão do tempo de quinta-feira":
                case "Qual a previsão do tempo quinta-feira":
                case "Qual a previsão do tempo para quinta-feira":
                case "Qual é a previsão do tempo de quinta":
                case "Qual a previsão do tempo quinta":
                case "Qual a previsão do tempo para quinta":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Thursday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para a próxima quinta-feira");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo quinta-feira em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;
                //SEXTA-FEIRA
                case "Qual é a previsão do tempo de sexta-feira":
                case "Qual a previsão do tempo sexta-feira":
                case "Qual a previsão do tempo para sexta-feira":
                case "Qual é a previsão do tempo de sexta":
                case "Qual a previsão do tempo sexta":
                case "Qual a previsão do tempo para sexta":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site do INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Friday) && (count < 7));
                            if (count >= 7) {
                                Falar("Ainda não há previsão do tempo para a próxima sexta-feira.");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para sexta-feira em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.\n", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.");
                        }
                    }
                    break;

                //SÁBADO
                case "Qual a previsão do tempo sábado":
                case "Qual é a previsão do tempo de sábado":
                case "Qual a previsão do tempo para sábado":
                    FraseColorida("\n\tAVISO: Tentando conexão com o site ddo INPE...\n", ConsoleColor.Yellow);
                    using (PrevisaoSeteDias prev = new PrevisaoSeteDias(codigoLocalidade)) {
                        DateTime data = DateTime.Now.Date;
                        if (prev.ObtevePrevisao) {
                            int count = 0;
                            do {
                                data = data.AddDays(1);
                                count++;
                            } while ((data.DayOfWeek != DayOfWeek.Saturday) && (count < 7));
                            if(count >= 7) {
                                Falar("Ainda não há previsão do tempo para o próximo sábado");
                                FraseColorida(Graficos.Caixa(new string[] { "AINDA NÃO HÁ PREVISÃO DO TEMPO PARA ESTA DATA." }), ConsoleColor.DarkCyan);
                                break;
                            }
                            for (int i = 0; i < prev.TempoPrevisto.Length; i++) {
                                if (prev.DataPrevisao[i].Date == data) {
                                    strPrev = prev.TempoPrevisto[i];
                                    FraseColorida(Graficos.Caixa("PREVISÂO DO TEMPO - " + prev.DataPrevisao[i].Date.ToShortDateString(), new string[] { prev.Cidade + "-" + prev.Estado, " ", " ", strPrev.ToUpper() }), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\tPrevisão do tempo para " + prev.Cidade +"-"+prev.Estado+ " dia " + prev.DataPrevisao[i].Date.ToShortDateString(), ConsoleColor.DarkCyan);
                                    //FraseColorida("\n\t" + strPrev.ToUpper(), ConsoleColor.DarkCyan);
                                    Console.WriteLine("\n\t");
                                    Falar("A previsão do tempo para sábado em " + prev.Cidade + " é de " + strPrev);
                                }
                            }
                        }
                        else {
                            FraseColorida("\n\tERRO: Não foi possível acessar o site do INPE.", ConsoleColor.Red);
                            Falar("Desculpe-me mestre. Não consegui acessar a previsão do tempo.\n");
                        }
                    }
                    break;
            }
        }


        /// <summary>
        /// Processamendo da confirmação de comandos.
        /// </summary>
        /// <param name="Ordem">O comando recebido</param>
        public static void ProcessarConfirmacao(string Ordem) {
            switch (Ordem) {
                case "Sim":
                case "Tenho":
                case "Pode":
                case "Ok":
                    switch (FuncaoAtiva) {
                        case Funcao.DesligarPC:
                            FalarCompleto("Entendi. Vou desligar o computador.");
                            DesligarPC();
                            break;
                        case Funcao.FecharAssistente:
                            FalarCompleto("Ok. Até mais mestre, foi uma honra serví-lo.");
                            Environment.Exit(0);
                            break;
                        case Funcao.FecharJanela:
                            string janAtiva = Funcoes.TituloJanelaAtiva();
                            if (Funcoes.FecharJanela()) {
                                FraseColorida("\n\tJanela Fechada: " + janAtiva +"\n\n\t", ConsoleColor.DarkYellow);
                                Falar("Janela Fechada");
                                FuncaoAtiva = Funcao.Nenhuma;
                            }
                            else {
                                Console.Write("AVISO: Nenhuma janela ativa!");
                                Falar("Não há nenhuma janela ativa");
                            }
                            break;
                        default:
                            FraseColorida("\n\tAVISO: Nenhuma função ativa.\n", ConsoleColor.Yellow);
                            break;
                    }
                    break;
                case "Não":
                case "Cancelar":
                    switch (FuncaoAtiva) {
                        case Funcao.DesligarPC:
                            Falar("Ok. Não vou desligar o PC");
                            FuncaoAtiva = Funcao.Nenhuma;
                            break;
                        case Funcao.FecharAssistente:
                            Falar("Ok. É uma honra continuar a serví-lo, querido mestre.");
                            break;
                        case Funcao.FecharJanela:
                            Falar("Ok. Não vou fechar a janela.");
                            FuncaoAtiva = Funcao.Nenhuma;
                            break;
                        default:
                            FraseColorida("\n\tAVISO: Nenhuma função ativa.\n", ConsoleColor.Yellow);
                            FuncaoAtiva = Funcao.Nenhuma;
                            break;
                    }
                    break;
                default:
                    FuncaoAtiva = Funcao.Nenhuma;
                    break;
            }
        }

        /// <summary>
        /// Fala o texto de forma assíncrona. Pode ser interrompido.
        /// </summary>
        /// <param name="Texto">O texto a ser falado</param>
        private static void Falar(string Texto) {
            synthVoz.SpeakAsyncCancelAll();
            synthVoz.SpeakAsync(Texto);
        }

        /// <summary>
        /// Fala o texto de forma síncrona. Não pode ser interrompido.
        /// </summary>
        /// <param name="Texto">O texto a ser falado</param>
        private static void FalarCompleto(string Texto) {
            synthVoz.SpeakAsyncCancelAll();
            synthVoz.Speak(Texto);
        }


        private static void AbrirJanela(Form Janela) {
            Application.Run(Janela);
        }

        /// <summary>
        /// Exibe a frase com as cores especificadas
        /// </summary>
        /// <param name="Frase">O texto a ser colorido.</param>
        /// <param name="CorLetra">A cor do texto. Padrão: Verde.</param>
        /// <param name="CorFundo">A cor de fundo do texto. Padrão: Preto.</param>
        private static void FraseColorida(string Frase, ConsoleColor CorLetra = ConsoleColor.Green, ConsoleColor CorFundo = ConsoleColor.Black) {
            ConsoleColor CorLetraTemp = Console.ForegroundColor;
            ConsoleColor CorFundoTemp = Console.BackgroundColor;
            Console.ForegroundColor = CorLetra;
            Console.BackgroundColor = CorFundo;
            Console.Write(Frase);
            Console.ForegroundColor = CorLetraTemp;
            Console.BackgroundColor = CorFundoTemp;

        }

        /// <summary>
        /// Limpa a janela do console.
        /// </summary>
        private static void LimparConsole() {
            Console.Clear();
            Console.Write(Graficos.Caixa(new string[] { "RECONHECIMENTO DE FALA" }, 100));
            Console.WriteLine("\n\n\tEstou ouvindo. O que deseja?\n\t");
        }

        /// <summary>
        /// Executa o comando para desligamento do computador.
        /// </summary>
        /// <param name="tempo">O tempo de espera em segundos antes de desligar o computador (Padrão: 30 segundos). Útil caso seja necessário cancelar o desligamento.</param>
        /// <param name="mensagem">A mensagem de desligamento. Exibida pelo Windows em uma caixa de mensagem.</param>
        private static void DesligarPC(int tempo = 30, string mensagem = "Desligamento iniciado pelo Assistente Virtual") {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "shutdown";
            startInfo.Arguments = string.Format("-s -t {0} -c \"{1}\"", tempo, mensagem);
            Process.Start(startInfo);
        }

        /// <summary>
        /// Cancela o desligamento do computador.
        /// </summary>
        private static void CancelarDesligamento() {
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = true;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "shutdown";
            startInfo.Arguments = "-a";
            Process.Start(startInfo);
        }
    }
}
