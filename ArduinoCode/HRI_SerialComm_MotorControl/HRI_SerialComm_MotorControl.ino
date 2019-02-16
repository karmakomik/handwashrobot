#include <Servo.h>

Servo rightEyeServo;  // create servo object to control a servo
Servo leftEyeServo;

String inputString = "";         // a String to hold incoming data
bool stringComplete = false;  // whether the string is complete

void setup()
{
  Serial.begin(9600);
  //pinMode(LED_BUILTIN, OUTPUT);
  // reserve 200 bytes for the inputString:
  inputString.reserve(200);  
  rightEyeServo.attach(8);  // attaches the servo on pin 9 to the servo object
  leftEyeServo.attach(10);
  //rightEyeServo.write(90-40);
  //leftEyeServo.write(90+40); 
  //delay(240);
}

void loop()
{
    if (stringComplete) 
    {
      Serial.println(inputString + "recvd");
      if(inputString == "SMILE\n")
      {
        ///rightEyeServo.attach(8);  // attaches the servo on pin 9 to the servo object
        //leftEyeServo.attach(10);
        rightEyeServo.write(90-40);
        leftEyeServo.write(90+40);  
        //delay(240);
        //leftEyeServo.detach();
        //rightEyeServo.detach();        
      }
      else if(inputString == "ANGRY\n")
      {
        //rightEyeServo.attach(8);  // attaches the servo on pin 9 to the servo object
        //leftEyeServo.attach(10);
        rightEyeServo.write(90+40);
        leftEyeServo.write(90-40);  
        //delay(240);
        //leftEyeServo.detach();
        //rightEyeServo.detach();  
      }
      
      // clear the string:
      inputString = "";
      stringComplete = false;
    }
  //Serial.write(45); // send a byte with the value 45
  
  //int bytesSent = Serial.println("hri test"); //send the string "hello" and return the length of the string.



  /*digitalWrite(LED_BUILTIN, HIGH);   // turn the LED on (HIGH is the voltage level)
  delay(500);                       // wait for a second
  digitalWrite(LED_BUILTIN, LOW);    // turn the LED off by making the voltage LOW
  delay(500);                       // wait for a second*/
}

/*
  SerialEvent occurs whenever a new data comes in the hardware serial RX. This
  routine is run between each time loop() runs, so using delay inside loop can
  delay response. Multiple bytes of data may be available.
*/
void serialEvent() {
  while (Serial.available()) {
    // get the new byte:
    char inChar = (char)Serial.read();
    // add it to the inputString:
    inputString += inChar;
    // if the incoming character is a newline, set a flag so the main loop can
    // do something about it:
    if (inChar == '\n') {
      stringComplete = true;
    }
  }
}
