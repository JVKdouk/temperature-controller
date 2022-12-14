# Introduction
Temperature Controller is a sensor-based room temperature control. Using a microcontroller, namely Mbed, an analog temperature sensor, and a power switch tail, an ideal room temperature can be achieved. uLCD screen is used for monitoring.

A Windows application is used to configure the microcontroller, and also override its current state. Monitoring can also be performed via the Windows application. The project solution is provided in this repository. Notice Visual Studio 22 and .NET framework V6 was used to develop and build the project.

This project was developed by ***Joao Victor Cardoso Kdouk*** and ***Camryn Quam***.

# Demo Video

<div style="width: 400px; padding-bottom: 20px;" align="center">

   [![Temperature Controller Demo](https://img.youtube.com/vi/FWbZGmumuwk/0.jpg)](https://www.youtube.com/watch?v=FWbZGmumuwk)
   <br>
   (Click on the image above to watch the video)
   <br>
   <br>
   
</div>

# Used Parts
- Temperature Sensor (TMP36).
- LCD Screen (uLCD-144G2).
- Mbed (LPC1768).
- Power Switch Tail (80137).
- Windows Application (VS22 C# From Application).

# Schematic

<div style="width: 400px; padding-bottom: 20px;" align="center">
   <img src="assets/schematic-live.jpeg?raw=true" width="600" />
   <br>
   (Compact schematic of the device)
   <br>
   <br>
   <img src="assets/schematic.jpeg?raw=true" width="600" />
   <br>
   <br>
</div>

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

This repository holds both the code for the mbed device and the windows application. Click of the folders above to navigate the project.

Source code is divided in two folders. mbed_src contains all source code that goes into the mbed, allowing for serial communication with the computer and programatic control over the AC unit. On the other hand, win_app contains the solution files to debug and build the final Windows application, allowing for communication with the Mbed, as well as override of its functionality.

Notice the Mbed source code depends on two libraries, namely RTOS (Real-Time OS) and uLCD for communcation with the LCD display. RTOS is used to coordinate multiple sensors and actions at the same time, with timely reaction to external inputs. The project can be performed without RTOS, however, screen display (and animation) may lag behind. Serial communication may also suffer without RTOS, since so many components are disputing the processor attention.

The Windows Form Application source code depends on the System.IO.Ports library, since it facilitates serial communication with the Mbed. Below you can find common serial codes used to transmit and receive messages across the components in serial:

<div align="center">

| Command | Description |
| --- | ----------- |
| !B1 | Set system mode to automatic. In this mode, AC is turned on or off based on the current temperature and set-point |
| !B2 | Set system mode to on |
| !B3 | Turn off system |
| !Txxx | Set the set-point to xxx |

</div>

The only message that is transmited by the Mbed is the current temperature, is then read by the Windows Application and displayed to the end user.

# Images

<div style="width: 400px; padding-bottom: 20px;" align="center">
   <img src="assets/open.jpeg?raw=true" width="600" />
   <br>
   (Open Embedded Device)
   <br>
   <br>
</div>

<div style="width: 400px; padding-bottom: 20px;" align="center">
   <img src="assets/closed.jpeg?raw=true" width="600" />
   <br>
   (Closed Embedded Device)
   <br>
   <br>
</div>

<div style="width: 400px; padding-bottom: 20px;" align="center">
   <img src="assets/winapp.png?raw=true" width="600" />
   <br>
   (Windows Application for Monitoring)
   <br>
   <br>
</div>

# Future Plans

- Add humidity sensor and ability to control a humidifier.
- Change to a more powerful Power Switch, allowing to control multiple devices at the same time.
- Improve monitoring and control Windows application.
- Create a Linux and MacOS application.
