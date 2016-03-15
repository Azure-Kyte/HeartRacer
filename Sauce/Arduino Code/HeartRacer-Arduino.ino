/**************************************************
  ** HeartRacer Arduino Controller code **
  Communicates  jump/duck input to HeartRacer through 57600 Baud serial on port 1.

  Required Hardware:
  - Photosensitive LRD on pin A0
  - Pressure sensitive resistor on pin A1 (Pressure plate)
  - LEDs on pins 12 and 13 for debug output
  - Laser pointed at LRD on either +5V (Always on) or pin 11 (Enable only when running)
  
  Outputs single byte on recieving 'G', bit 0 and bit 1 representing jump and duck respectively
  
  Tune the DUCK_THRESHOLD and JUMP_THRESHOLD based on hardware in question.
  
  ************************************************
  Copyright (c) 2015, Scott Wilson
  All rights reserved.

  Redistribution and use in source and binary forms, with or without
  modification, are permitted provided that the following conditions are met:

  1. Redistributions of source code must retain the above copyright notice, this
     list of conditions and the following disclaimer.
  2. Redistributions in binary form must reproduce the above copyright notice,
     this list of conditions and the following disclaimer in the documentation
     and/or other materials provided with the distribution.

  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
  ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
  WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
  DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
  ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
  (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
  LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
  ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
  SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
**************************************************/

/***
* Constants
***/
const int led = 13;
const int duckLed = 12;
const int jumpPin = A1;
const int duckPower = 11;
const int duckPin = A0;
const int DUCK_THRESHOLD = 45;
const int JUMP_THRESHOLD = 300;

/***
* Globals
***/
int jumpSensor = 0;
int duckSensor = 0;
int inByte = 0;

/***
* Initial setup code
***/
void setup()
{
  Serial.begin(57600);
  while (!Serial) {
    ; // wait for serial port to connect. Needed for Leonardo only
  }

  // Pin modes
  pinMode(led, OUTPUT);
  pinMode(duckLed, OUTPUT);
  pinMode(duckPower, OUTPUT);
  
  // Laser and LEDs off
  digitalWrite(led, LOW);
  digitalWrite(duckLed, LOW);
  digitalWrite(duckPower, LOW);
  
  establishContact();
}

/***
* Main loop, called continuously
***/
void loop()
{
  // Keep track of inputs
  jumpSensor = analogRead(jumpPin);
  duckSensor = analogRead(duckPin);
  
  // Read the serial port if data available
  if (Serial.available() > 0) {
    inByte = Serial.read();
    
    if (inByte == 'G') { // Get input
      byte out = 0;
      out |= (jumpSensor <= JUMP_THRESHOLD);
      out |= (duckSensor <= DUCK_THRESHOLD) << 1;
      Serial.write(out);
    } else if (inByte == 'R') { // Reset controller
      // LEDs off, Laser off
      digitalWrite(led, LOW);
      digitalWrite(duckLed, LOW);
      digitalWrite(duckPower, LOW);
      
      // Flush out the serial buffer, close and reopen
      Serial.println("");
      serialFlush();
      Serial.end();
      delay(10);
      Serial.begin(57600);
      
      establishContact();
      
      // Reread Inputs
      jumpSensor = analogRead(jumpPin);
      duckSensor = analogRead(duckPin);
    }
  }
  
  // Set debugging leds
  digitalWrite(led, jumpSensor <= JUMP_THRESHOLD ? HIGH : LOW);
  digitalWrite(duckLed, duckSensor <= DUCK_THRESHOLD ? HIGH : LOW);
  
  delay(1); // Let the ADC settle
}

/***
* Establish serial connection with game
***/
void establishContact() {
  // Wait for a response
  while (Serial.available() <= 0) {
    Serial.println("HI");
    delay(300);
  }

  // Turn on the laser
  digitalWrite(duckPower, HIGH);
}

/***
* Flushes the serial buffers
***/
void serialFlush(){
  while(Serial.available() > 0) {
    char t = Serial.read();
  }
  Serial.flush();
}
