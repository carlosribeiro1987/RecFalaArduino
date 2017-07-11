#define releLampada 13

char leitura;

void setup()
{

  //Inicializa comunicação Serial
  Serial.begin(9600);
  //Seta o pino indicado por rele como saída
  pinMode(releLampada, OUTPUT);
  //Mantem rele desligado assim que iniciar o programa
  digitalWrite(releLampada, LOW);
}

void loop()
{
  //Verifica se há conexão com a serial
  while (Serial.available() > 0)
  {
    //Lê o dado vindo da Serial e armazena na variável leitura
    leitura = Serial.read();
    //Se a variável leitura for igual a 'd' ou 'D' ela Desliga rele
    if (leitura == 'd' || leitura == 'D'){
      digitalWrite(releLampada, LOW);
    }
    /*Senão verifica se a variável leitura é
      igual a 'l' ou 'L' ela Liga rele */
    else if (leitura == 'l' || leitura == 'L') {
      digitalWrite(releLampada, HIGH);
    }
    Serial.println(leitura);
  }

}
