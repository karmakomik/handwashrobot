/*
 Controlling a servo position using a potentiometer (variable resistor)
 by Michal Rinott <http://people.interaction-ivrea.it/m.rinott>

 modified on 8 Nov 2013
 by Scott Fitzgerald
 http://www.arduino.cc/en/Tutorial/Knob
*/

#include <Servo.h>

Servo pitchServo;  // create servo object to control a servo
Servo yawServo;

int pitchZeroAngle = 80;
int yawZeroAngle = 100;

//int potpin = 0;  // analog pin used to connect the potentiometer
int val;    // variable to read the value from the analog pin

void setup() {
  yawServo.attach(9);  // attaches the servo on pin 9 to the servo object
  pitchServo.attach(8);
  yawServo.write(yawZeroAngle);
  pitchServo.write(pitchZeroAngle);
  //delay(240);
  //leftEyeServo.detach();
  //rightEyeServo.detach();
  //show_disgust();
}

void loop() {
  yawServo.write(yawZeroAngle+40);
  delay(700); 
  yawServo.write(yawZeroAngle-40);
  delay(700); 
  pitchServo.write(pitchZeroAngle+60);
  delay(700); 
  pitchServo.write(pitchZeroAngle-40);
  delay(700); 
}
