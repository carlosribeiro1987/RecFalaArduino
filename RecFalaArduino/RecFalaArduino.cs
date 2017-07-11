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


namespace RecFalaArduino {

    class ReconhecimendoFalaArduino {
        private static SpeechRecognitionEngine engineVoz = new SpeechRecognitionEngine();
        private static SpeechSynthesizer synthVoz = new SpeechSynthesizer();
        public enum Funcao { Nenhuma, DesligarPC, FecharAssistente, FecharJanela };
        public static Funcao FuncaoAtiva = Funcao.Nenhuma;
        public static ConsoleColor CorFundoConsole = Console.BackgroundColor;
        public static ConsoleColor CorFonteConsole = Console.ForegroundColor;
        // bool Ouvindo = false;
        static void Main(string[] args) {
            bool PodeFechar = false;
            #region INICIANDO O PROGRAMA          
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();

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
                Console.WriteLine("\n\n\tEstou ouvindo. O que deseja?");



                Console.ReadKey();
            } while (!PodeFechar);

        }

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
                    FraseColorida("DESLIGAR O COMPUTADOR?", ConsoleColor.Red, ConsoleColor.Yellow);
                    break;
                case "Sair":
                    FuncaoAtiva = Funcao.FecharAssistente;
                    Falar("Tem certeza que posso encerrar?");
                    break;

                case "Feche essa janela":
                case "Feche a janela":
                case "Fechar janela":
                    FuncaoAtiva = Funcao.FecharJanela;
                    Falar("Tem certeza que deseja fechar a janela " + Funcoes.TituloJanelaAtiva()+"?");                   
                    break;
                case "Qual é a janela ativa":
                    Console.Write("\n\tJanela Ativa: " + Funcoes.TituloJanelaAtiva()+"\n");
                    Falar("A janela ativa é: "+Funcoes.TituloJanelaAtiva());
                    break;
                case "Abrir Bloco de Notas":
                    Funcoes.AbrirPrograma("notepad.exe");
                    break;
                case "Abrir Calculadora":
                    Funcoes.AbrirPrograma("calc.exe");
                    break;
                case "Limpar Janela":
                case "Limpar Mensagens":
                    LimparConsole();
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
                        FraseColorida("\n\tERRO: Não Foi possível acessar o Arduino. Verifique a conexão.", ConsoleColor.Red);
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
                        FraseColorida("\n\tERRO: Não Foi possível acessar o Arduino. Verifique a conexão.", ConsoleColor.Red);
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
                //LOTOFÁCIL
                case "Qual é o resultado da Lotofácil":
                    FalarCompleto("Espere um momento mestre. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    Lotofacil lotofacil = new Lotofacil();
                    if (lotofacil.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(lotofacil.ResultadoArray);
                        // Console.Write("\n\tResultado da Lotofácil:\n\t[ " + Resultado + " ]\n");
                        FraseColorida(Graficos.Caixa("LOTOFÁCIL", Respostas.SeparaStrArrays(lotofacil.ResultadoArray, 5)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(lotofacil.Concurso)+"\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(lotofacil.DataSorteio.Date).Substring(0, 10)+"\n", ConsoleColor.Cyan);

                        Falar(string.Format("Os números do último concurso da Lotofácil, sorteados {0} foram: {1}", Frases.Data(lotofacil.DataSorteio), Resultado));
                    }
                    else {
                        FraseColorida("\nERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me méstri. Não consegui acessar o resultado.");
                    }
                    break;
                //MEGA SENA
                case "Qual é o resultado da Mega Sena":
                    FalarCompleto("Espere um momento mestre. Vou conferir o resultado.");
                    FraseColorida("\n\tAVISO: Tentando conexão com o site da Caixa...\n", ConsoleColor.Yellow);
                    MegaSena megasena = new MegaSena();
                    if (megasena.ObteveResultado) {
                        string Resultado = Respostas.SeparaStrArrays(megasena.ResultadoArray);
                        FraseColorida(Graficos.Caixa("MEGA SENA", Respostas.SeparaStrArrays(megasena.ResultadoArray, 6)), ConsoleColor.Cyan);
                        FraseColorida("\tNúmero do Concurso: " + Convert.ToString(megasena.Concurso)+"\n", ConsoleColor.Cyan);
                        FraseColorida("\tData do Sorteio: " + Convert.ToString(megasena.DataSorteio).Substring(0, 10)+"\n", ConsoleColor.Cyan);
                        Falar(string.Format("Os números do último concurso da Mega-Sena, sorteados {0} foram: {1}", Frases.Data(megasena.DataSorteio), Resultado));
                        
                        
                    }
                    else {
                        FraseColorida("\n\tERRO: Não foi possível acessar o site da Caixa.", ConsoleColor.Red);
                        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                    }
                    break;
                //case "Qual é o resultado da Lotomania":
                //case "Fale o resultado da Lotomania":
                //case "Me diz o resultado da Lotomania":
                //case "Me diga o resultado da Lotomania":
                //case "Diga-me o resultado da Lotomania":
                //    FalarCompleto("Espere um momento mestre. Vou conferir o resultado.");
                //    Console.Write("\n\t[ AVISO ] Tentando conexão com o site da Caixa...\n");
                //    LotoMania lotomania = new LotoMania();
                //    if (lotomania.ObteveResultado) {
                //        string Resultado = Respostas.SeparaStrArrays(lotomania.ResultadoArray);
                //        Console.Write(Graficos.Caixa("LOTOMANIA", Respostas.SeparaStrArrays(lotomania.ResultadoArray, 5)));
                //        Falar(string.Format("Os números do último concurso da Loto Mania, sorteados {0} foram: {1}", Frases.Data(lotomania.DataSorteio), Resultado));

                //        Console.WriteLine("\tNúmero do Concurso: " + Convert.ToString(lotomania.Concurso));
                //        Console.WriteLine("\tData do Sorteio: " + Convert.ToString(lotomania.DataSorteio.Date));
                //    }
                //    else {
                //        Console.WriteLine("\n\t[ ERRO ] Não foi possível acessar o site da Caixa.");
                //        Falar("Desculpe-me mestre. Não consegui acessar o resultado.");

                //    }
                //    break;
                default:
                    Falar("Desculpe-me, não entendi. Podes repetir querido méstrih?");
                    break;
            }
        }
        public static void ProcessarConfirmacao(string Ordem) {
            switch (Ordem) {
                case "Sim":
                case "Tenho":
                case "Pode Fechar":
                case "Ok":
                    switch (FuncaoAtiva) {
                        case Funcao.DesligarPC:
                            Falar("Entendi. Vou desligar o computador.");
                            //Implementar função para desligar
                            break;
                        case Funcao.FecharAssistente:
                            FalarCompleto("Ok. Até mais mestre, foi uma honra serví-lo.");
                            Environment.Exit(0);
                            break;
                        case Funcao.FecharJanela:
                            string janAtiva = Funcoes.TituloJanelaAtiva();
                            if (Funcoes.FecharJanela()) {
                                FraseColorida("\n\tJanela Fechada: " + janAtiva, ConsoleColor.DarkYellow);
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

        private static void FraseColorida(string Frase, ConsoleColor CorLetra = ConsoleColor.Green, ConsoleColor CorFundo = ConsoleColor.Black) {
            ConsoleColor CorLetraTemp = Console.ForegroundColor;
            ConsoleColor CorFundoTemp = Console.BackgroundColor;
            Console.ForegroundColor = CorLetra;
            Console.BackgroundColor = CorFundo;
            Console.Write(Frase);
            Console.ForegroundColor = CorLetraTemp;
            Console.BackgroundColor = CorFundoTemp;

        }

        private static void LimparConsole() {
            Console.Clear();
            Console.Write(Graficos.Caixa(new string[] { "RECONHECIMENTO DE FALA" }, 100));
        }
    }
}
