#include <Servo.h>
#include <SoftwareSerial.h>

SoftwareSerial BLU(6, 7); // RX, TX
Servo pitchServo;  // create servo object to control a servo
Servo yawServo;

int pitchZeroAngle = 80;
int yawZeroAngle = 100;

char val;

void setup() 
{
  //pinMode(LED_BUILTIN, OUTPUT);
  BLU.begin(9600);
  //Serial.begin(9600);
  BLU.println("Bluetooth work test");
  yawServo.attach(9);  // attaches the servo on pin 9 to the servo object
  pitchServo.attach(8);
  yawServo.write(yawZeroAngle);
  pitchServo.write(pitchZeroAngle);
}

void parseCommand(char input) 
{
  switch (input) 
  {
    case 'a': //my_name_is
      //digitalWrite(LED_BUILTIN, HIGH);   
      BLU.println("Recvd 1");
      yawServo.write(yawZeroAngle+40);
      delay(700); 
      yawServo.write(yawZeroAngle-40);
      delay(700);    
      break;
    case 'b': //show_appreciation
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      pitchServo.write(pitchZeroAngle+60);
      delay(700); 
      pitchServo.write(pitchZeroAngle-40);
      delay(700);  
      break;
    case 'c': //show_disgust
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      pitchServo.write(pitchZeroAngle+60);
      delay(700); 
      pitchServo.write(pitchZeroAngle-40);
      delay(700);  
      break; 
    case 'd': //wash_hands_today_q
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      pitchServo.write(pitchZeroAngle+60);
      delay(700); 
      pitchServo.write(pitchZeroAngle-40);
      delay(700);  
      break;
    case 'e': //wash_hands_aftr_toilet
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      pitchServo.write(pitchZeroAngle+60);
      delay(700); 
      pitchServo.write(pitchZeroAngle-40);
      delay(700);  
      break;
    case 'f': //wash_hands_b4_meals
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd 2");
      pitchServo.write(pitchZeroAngle+60);
      delay(700); 
      pitchServo.write(pitchZeroAngle-40);
      delay(700);  
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
