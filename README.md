# Introduction
Temperature Controller is a sensor-based room temperature control. Using a microcontroller, namely Mbed, an analog temperature sensor, and a power switch tail, an ideal room temperature can be achieved. uLCD screen is used for monitoring.

A Windows application is used to configure the microcontroller, and also override its current state. Monitoring can also be performed via the Windows application. The project solution is provided in this repository. Notice Visual Studio 22 and .NET framework V6 was used to develop and build the project.

This project was developed by ***Joao Victor Cardoso Kdouk*** and ***Camryn Quam***.

# Used Parts
- Temperature Sensor (TMP36).
- LCD Screen (uLCD-144G2).
- Mbed (LPC1768).
- Power Switch Tail (80137).
- Windows Application (VS22 C# From Application).

# Schematic

Mbed RX connects to the uLCD TX, while its TX connects to the uLCD RX. Mbed analog pins are used to read the output of the temperature sensor (p20 in the schematic above). Finally, Mbed digital pins are used to control the power switch (p8 in the schematic above). Communication with the computer happens via USB (USBRX and USBTX) via serial. Below you can find a table with all connections:

<div align="center">

| Mbed Pin | Destination Pin |
| --- | ----------- |
| p8 | Power Switch Input (1) |
| p20 | TMP36 Middle Pin |
| p27 | uLCD TX |
| p28 | uLCD RX |
| p30 | uLCD Reset |
| VU | uLCD 5V |
| Gnd | uLCD GND |
| Vout | TMP36 Left Pin |
| GND | TMP36 Right Pin |

</div>

Notice a diode is used between the power switch input (1) and p8, to ensure a spike in the inductor current (caused by the relay) will not affect the mbed. Without this diode, switching the relay may cause the mbed to crash and restart.

# Source Code Structure

Source code is divided in two folders. mbed_src contains all source code that goes into the mbed, allowing for serial communication with the computer and programatic control over the AC unit. On the other hand, 
