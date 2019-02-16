/*
 Controlling a servo position using a potentiometer (variable resistor)
 by Michal Rinott <http://people.interaction-ivrea.it/m.rinott>

 modified on 8 Nov 2013
 by Scott Fitzgerald
 http://www.arduino.cc/en/Tutorial/Knob
*/

#include <Servo.h>

Servo rightEyeServo;  // create servo object to control a servo
Servo leftEyeServo;

//int potpin = 0;  // analog pin used to connect the potentiometer
int val;    // variable to read the value from the analog pin

void setup() {
  rightEyeServo.attach(6);  // attaches the servo on pin 9 to the servo object
  leftEyeServo.attach(7);
  rightEyeServo.write(90-40);
  leftEyeServo.write(90+40);
  delay(240);
  leftEyeServo.detach();
  rightEyeServo.detach();
}

void loop() {
  /*myservo.write(0); 
  delay(40); 
  myservo.write(180); 
  delay(40);*
  //val = analogRead(potpin);            // reads the value of the potentiometer (value between 0 and 1023)
  //val = map(val, 0, 1023, 0, 180);     // scale it to use it with the servo (value between 0 and 180)
  //myservo.write(val);                  // sets the servo position according to the scaled value
  //delay(15);                           // waits for the servo to get there
  */
}
