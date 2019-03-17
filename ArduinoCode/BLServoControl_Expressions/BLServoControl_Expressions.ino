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
      BLU.println("Recvd my_name_is signal");
      expression_conversation();   
      break;
    case 'b': //show_appreciation
     //digitalWrite(LED_BUILTIN, LOW);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
      BLU.println("Recvd show_appreciation signal");
      expression_appreciation(); 
      break;
    case 'c': //show_disgust
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd show_disgust signal");
      expression_disgust();  
      break; 
    case 'd': //wash_hands_today_q
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd wash_hands_today_q signal");
      expression_singlesentence();
      break;
    case 'e': //wash_hands_aftr_toilet
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd wash_hands_aftr_toilet signal");
      expression_singlesentence();
      break;
    case 'f': //wash_hands_b4_meals
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd wash_hands_b4_meals signal");
      expression_singlesentence(); 
      break;  
    case 'g': //promise
     //digitalWrite(LED_BUILTIN, LOW);  
      BLU.println("Recvd Steps Signal");
      expression_conversation(); 
      break;   
    case 'h': //promise beginning
      BLU.println("Promises beginning Signal");
      expression_long_conversation();
      break;
    case 'i': //goodbye thank you
      BLU.println("goodbye signal");
      expression_goodbye();
      break;
    case 'j': //steps_left
      BLU.println("steps left signal");
      expression_eyesToLeft();
      break;
    case 'k': //steps_right
      BLU.println("steps right signal");
      expression_eyesToRight();
      break;
    case 'y': //debug_yaw
      BLU.println("Debug-yaw");
      debug_yaw();
      break;      
    case 'z': //debug_pitch
      BLU.println("Debug-pitch");
      debug_pitch();
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

void debug_yaw()
{
  reset_eyes();
  yawServo.write(yawZeroAngle+40);
  delay(700); 
  yawServo.write(yawZeroAngle-40);
  delay(700); 
  reset_eyes();  
}

void debug_pitch()
{
  reset_eyes();
  pitchServo.write(pitchZeroAngle+60);
  delay(700); 
  pitchServo.write(pitchZeroAngle-40);
  delay(700); 
  reset_eyes();
}

//Sooraj Code
void expression_disgust() {
  reset_eyes();
  
  pitchServo.write(pitchZeroAngle+20);
  for(int i=0;i<3;i++){
    yawServo.write(yawZeroAngle-40);
    delay(150);
    yawServo.write(yawZeroAngle+20);
    delay(150);
    yawServo.write(yawZeroAngle+40);
    delay(150);
  }
  reset_eyes();
  
}

void expression_appreciation(){
  reset_eyes();
  
  for(int i=0;i<8;i++){
    pitchServo.write(pitchZeroAngle+20);
    delay(140);
    pitchServo.write(pitchZeroAngle);
    delay(140);
  }
  reset_eyes();
  delay(250);
}

void expression_conversation() {
  reset_eyes();
  
  pitchServo.write(pitchZeroAngle+10);
  for(int i=0;i<1;i++){
    yawServo.write(yawZeroAngle+10);
    delay(600);
    yawServo.write(yawZeroAngle-20);
    delay(700);
    yawServo.write(yawZeroAngle+30);
    delay(800);
    yawServo.write(yawZeroAngle-10);
    delay(700);
    yawServo.write(yawZeroAngle-30);
    delay(600);
  } 
  reset_eyes();
}

void expression_long_conversation(){
  reset_eyes();
  
  pitchServo.write(pitchZeroAngle+10);
  for(int i=0;i<4;i++){
    yawServo.write(yawZeroAngle+10);
    delay(600);
    yawServo.write(yawZeroAngle-20);
    delay(700);
    yawServo.write(yawZeroAngle+30);
    delay(800);
    yawServo.write(yawZeroAngle-20);
    delay(700);
    yawServo.write(yawZeroAngle-30);
    delay(600);
  } 
  reset_eyes();
}

void expression_singlesentence(){
  reset_eyes();
  pitchServo.write(pitchZeroAngle+10);
  for(int i=0;i<2;i++){
    yawServo.write(yawZeroAngle+10);
    delay(200);
    yawServo.write(yawZeroAngle-20);
    delay(220);
    yawServo.write(yawZeroAngle+20);
    delay(180);  
  }  
  reset_eyes();
}

void expression_eyesToLeft(){
  reset_eyes();
  pitchServo.write(pitchZeroAngle+10);
  yawServo.write(yawZeroAngle+20);
  delay(220);
  reset_eyes();
}

void expression_eyesToRight(){
  reset_eyes();
  pitchServo.write(pitchZeroAngle+10);
  yawServo.write(yawZeroAngle-20);
  delay(220);
  reset_eyes();
} 

void expression_goodbye(){
    reset_eyes();
  
  pitchServo.write(pitchZeroAngle+10);
  for(int i=0;i<3;i++){
    yawServo.write(yawZeroAngle+20);
    delay(600);
    yawServo.write(yawZeroAngle-10);
    delay(700);
    yawServo.write(yawZeroAngle+20);
    delay(800);
    yawServo.write(yawZeroAngle-30);
    delay(700);
    yawServo.write(yawZeroAngle-20);
    delay(600);
  } 
  reset_eyes();
}

void expression_dontknow() {
  reset_eyes();
  
  for(int i=0;i<2;i++){
    yawServo.write(yawZeroAngle-40);
    pitchServo.write(pitchZeroAngle-30);
    delay(1000);
    yawServo.write(yawZeroAngle-10);
    pitchServo.write(pitchZeroAngle-10);
    delay(600);
  }  
  reset_eyes();
}

void expression_idle(){
  for(int i=0;i<3;i++){
    yawServo.write(yawZeroAngle-random(-20,20));
    delay(1000);
    pitchServo.write(pitchZeroAngle-random(30,-30));
    delay(1000);
  }  
  reset_eyes();
}


void reset_eyes(){
  yawServo.write(yawZeroAngle);
  pitchServo.write(pitchZeroAngle);
  delay(150);
}
