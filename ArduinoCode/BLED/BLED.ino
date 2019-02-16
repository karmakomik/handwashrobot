#include <SoftwareSerial.h>
SoftwareSerial BLU(10, 11); // RX, TX

char val;

void setup() 
{
  pinMode(LED_BUILTIN, OUTPUT);
  BLU.begin(9600);
  //Serial.begin(9600);
  BLU.println("Bluetooth work test");
}

void parseCommand(char input) 
{
  switch (input) 
  {
    case '1': 
      digitalWrite(LED_BUILTIN, HIGH);   
      BLU.println("Recvd 1");
      break;
    case '2':
      digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      break;
  }
}

    
void loop() 
{
  //Serial.println("BLU.available() : " + BLU.available());
  //BLU.println("test");
  // Check for incoming command
  if (BLU.available()) 
  {
    //Serial.println("working");
    val=BLU.read(); 
    //Serial.println(val); // Use the IDE's Tools > Serial Monitor
    parseCommand(val); // parse the input
  }
}
