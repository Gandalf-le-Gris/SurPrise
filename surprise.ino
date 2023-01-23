#include <SoftwareSerial.h>
#include <Wire.h>
#include <Adafruit_INA219.h>

#define RECBLUE 2
#define SENDBLUE 3
#define SWITCH 13


// Initialise the variables
bool plugNotCut = true;
const char dataRequest = 'r';
const char cutPlug = 'c';
const char activatePlug = 'a';

// Data storing
float[96] dailyData;
int cursorOnDay = 0;
float[7] weeklyData;
int cursorOnWeek = 0;

// Init bluetooth
SoftwareSerial bluetooth(RECBLUE,SENDBLUE);



// Keeping time of when to measure conusmption
// The value will quickly become too large for an int to store
unsigned long previousMillis = 0;  // will store last time we measured data
const long interval = 900000;  // interval at which to blink (15 minutes = 900000 milliseconds)

void setup()
{ 
  // Bluetooth initialisation
  bluetooth.begin(115200);
  Serial.begin(9600);
  pinMode(ledpin,OUTPUT);
  pinMode(RECBLUE,INPUT);
  pinMode(SENDBLUE,OUTPUT);
  bluetooth.print('$');
  bluetooth.print('$');
  bluetooth.print('$');
  delay(100);
  bluetooth.print("SM,0\r"); // slave mode
  bluetooth.print("SA,0\r"); // open mode
  bluetooth.print("R,1\r"); // Redémarre pour mettre les modifs en marche
  bluetooth.print("---\r");
}
void loop()
{
  if (plugNotCut) {    
    // Do it only every 10/30 minutes
    unsigned long currentMillis = millis();
    if (currentMillis - previousMillis >= interval) {
        // save the last time you blinked the LED
        previousMillis = currentMillis;

        // Measure the consumption
        float energy_Wh = getConsumption();
              
        // Write the values in the internal memory
        if (cursorOnDay < 96) {
          dailyData[cursorOnDay] = energy_Wh;
          cursorOnDay ++;
        } else {
          if (cursorOnWeek == 7) {
            cursorOnWeek = 0; // We erase the previous data and hope they had been read by the application
          }
          cursorOnDay = 0

          float somme = 0,0;
          for (int i = 0 ; i < NB_SAMPLE ; i++) {   
              somme += tab[i] ; //somme des valeurs du tableau
          }
          weeklyData[cursorOnWeek] = somme / 96;
          cursorOnWeek ++;          
        }
     }
  }

  while (bluetooth.available() > 0) {
    char request = bluetooth.read();
    switch (request) {
      case dataRequest :
        // Send data to the app (state + consumption + date)
        float data = 0.0;
        bluetooth.write('d'); // Code to indiquate this is the start of the daily data
        while (cursorOnDay >= 0) {
          data = dailyData[cursorOnDay];
          bluetooth.write(data);
          cursorOnDay--;
        }
        bluetooth.write('w'); // Code to indiquate this is the start of the weekly data
        while (cursorOnWeek >= 0) {
          data = weeklyData[cursorOnWeek];
          bluetooth.write(data);
          cursorOnWeek--;
        }
      break;

      case cutPlug :
          if (plugNotCut) {
            plugNotCut = false;
            // Actually cut the current
            digitalWrite(SWITCH,LOW);
            // Confirm by bluetooth
            bluetooth.write('k'); // Code de validation
          }
      break;

      case activatePlug :
          if (!plugNotCut) {
            plugNotCut = true;
            // Actually put back in business the current
            digitalWrite(SWITCH,HIGH);
            // Confirm by bluetooth
            bluetooth.write('k');// Code de validation
      break;
    }
  }
}

// Function that reads the consumption of the socket
float getConsumption(){
  long time_s=millis()/(1000); // convert time to sec
  float busVoltage_V = ina219.getBusVoltage_V();
  float shuntVoltage_mV = ina219.getShuntVoltage_mV();
  float voltage_V = busVoltage_V + (shuntVoltage_mV / 1000);
  float current_mA = ina219.getCurrent_mA();
  //power_mW = ina219.getPower_mW(); 
  float power_mW=current_mA*voltage_V; 
  return energy_Wh=(power_mW*time_s)/3600;   //energy in watt hour
}
