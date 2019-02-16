#include <Servo.h>
#include <SoftwareSerial.h>

SoftwareSerial BLU(6, 7); // RX, TX
Servo rightEyeServo;  // create servo object to control a servo
Servo leftEyeServo;

char val;

void setup() 
{
  //pinMode(LED_BUILTIN, OUTPUT);
  BLU.begin(9600);
  //Serial.begin(9600);
  BLU.println("Bluetooth work test");
  rightEyeServo.attach(9);  
  leftEyeServo.attach(10);
}

void parseCommand(char input) 
{
  switch (input) 
  {
    case '1': //Smile
      digitalWrite(LED_BUILTIN, HIGH);   
      BLU.println("Recvd 1");
      //rightEyeServo.attach(8);  
      //leftEyeServo.attach(10);
      rightEyeServo.write(90-40);
      leftEyeServo.write(90+40);  
      //delay(240);
      //leftEyeServo.detach();
      //rightEyeServo.detach();       
      break;
    case '2': //Angry
     digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      //rightEyeServo.attach(8); 
      //leftEyeServo.attach(10);
      rightEyeServo.write(90+40);
      leftEyeServo.write(90-40);  
      //delay(240);
      //leftEyeServo.detach();
      //rightEyeServo.detach();  
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
    //BLU.println("working");
    val=BLU.read(); 
    //Serial.println(val); // Use the IDE's Tools > Serial Monitor
    parseCommand(val); // parse the input
  }
}
