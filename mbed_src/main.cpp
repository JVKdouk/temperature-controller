#include "mbed.h"
#include "rtos.h"
#include "uLCD_4DGL.h"

BusOut leds(LED1, LED2, LED3, LED4);
Serial pc(USBTX, USBRX);
AnalogIn tmp(p20);
uLCD_4DGL uLCD(p28,p27,p30);
DigitalOut ctrl(p8);

Mutex LCD;

enum Status {
    AUTO,
    _ON,
    _OFF
};

volatile Status status = AUTO;
volatile int temperature = 0;
volatile int setPoint = 70;

void temp_disp();

void refresh_status() {
  leds = 0;

  if (status == AUTO)
    leds[0] = 1;
  else if (status == _ON)
    leds[1] = 1;
  else if (status == _OFF)
    leds[2] = 1;
}

void decode_command() {
  if (pc.getc() == '!') {
    char command = pc.getc();
    if (command == 'B') {
      switch (pc.getc()) {
      case '1':
        status = AUTO;
        break;
      case '2':
        status = _ON;
        break;
      case '3':
        status = _OFF;
        break;
      default:
        status = _OFF;
        break;
      }

      refresh_status();
    }

    if (command == 'T') {
        int firstDigit = pc.getc() - '0';
        int secondDigit = pc.getc() - '0';
        int thirdDigit = pc.getc() - '0';

        setPoint = firstDigit * 100 + secondDigit * 10 + thirdDigit;

        LCD.lock();
        uLCD.locate(3,3);
        uLCD.color(WHITE);
        uLCD.printf("%d F  ", setPoint);
        LCD.unlock();
    }    
  }
}

void read_sensor() {
  while (1) {
    temperature = ((tmp * 3300) - 500) / 10;
    temperature = (temperature * 9 / 5) + 32;

    if (temperature > 100) temperature = 100;
    else if (temperature < 0) temperature = 0;

    if (temperature < 0) {
      pc.printf("%d\n", 0);
    } else if (temperature > 100) {
      pc.printf("%d\n", 100);
    } else {
      pc.printf("%d\n", temperature);
    }

    Thread::wait(5000);
  }
}

void communicate_pc() {
  if (pc.readable()) {
    decode_command();
  }
}

void temp_disp() {
    while (1) {
        LCD.lock();
        uLCD.color(BLUE);
        uLCD.locate(3,1);
        uLCD.printf("%d F  ", temperature);
        uLCD.locate(3,3);
        uLCD.color(WHITE);
        uLCD.printf("%d F  ", setPoint);

        if (status == AUTO) {
            uLCD.locate(3,6);
            uLCD.color(GREEN);
            uLCD.printf("%s", ctrl == 1 ? "ON " : "OFF");
        } else {
            uLCD.locate(3,6);
            uLCD.color(GREEN);
            uLCD.printf("   ");
        }

        uLCD.locate(3,7);
        uLCD.color(GREEN);
        uLCD.printf("%s", status == _ON ? "ON  " : (status == _OFF ? "OFF " : "AUTO"));
        LCD.unlock();
        Thread::wait(5000);
    }
} 

void temp_bar() {
    while (1) {
        LCD.lock();
        uLCD.filled_rectangle(10, 100, 20, 10, RED); 
        uLCD.filled_rectangle(10, 10 + int(100.0 - temperature), 20, 10, BLACK);
        LCD.unlock();
        Thread::wait(5000);
    }
}

void power() {
    while (1) {
        switch (status) {
            case _ON:
                ctrl = 1;
                break;
            case _OFF:
                ctrl = 0;
                break;
            case AUTO:
                if (temperature - setPoint >= 1) ctrl = 1;
                else if (setPoint - temperature >= 1) ctrl = 0;
                break;
        }
	    Thread::wait(5000);
    }
}

int main() {
  uLCD.cls();
  uLCD.filled_rectangle(0,118,30,15, WHITE);
  uLCD.filled_circle(15,15,15, WHITE);
  uLCD.filled_circle(15,113,15, WHITE);
  uLCD.filled_circle(15,110,10, RED);
  uLCD.text_width(2);
  uLCD.text_height(2);
  
  refresh_status();

  Thread SensorReader(read_sensor);
  Thread TemperatureDisplay(temp_disp);
  Thread Thermometer(temp_bar);
  Thread PowerSwitch(power);

  while (1) {
    communicate_pc();
    Thread::wait(500);
  }
}
