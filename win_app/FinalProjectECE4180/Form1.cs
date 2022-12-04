using System.Diagnostics;
using System.IO.Ports;

namespace FinalProjectECE4180
{
    enum Status
    {
        AUTO,
        ON,
        OFF
    }

    public partial class Form1 : Form
    {
        Status status = Status.AUTO;
        SerialPort sensorSerial;
        int level = 0;
        int setPoint = 70;

        private void UpdateApplicationStatus()
        {
            if (sensorSerial == null || !sensorSerial.IsOpen)
            {
                trackBar1.Enabled = false;
                trackBar2.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                button3.Enabled = false;
            }
            else
            {
                trackBar1.Enabled = true;
                trackBar2.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
            }
        } 

        private void InitSerial(String serialPort)
        {
            sensorSerial = new SerialPort(serialPort);
            sensorSerial.BaudRate = 9600;
            sensorSerial.Open();
            UpdateApplicationStatus();
            CheckSensor();
            label6.Text = sensorSerial.IsOpen ? "Online" : "Offline";
        } 

        private void ChangeButtonState()
        {
            button1.BackColor = SystemColors.ControlLightLight;
            button2.BackColor = SystemColors.ControlLightLight;
            button3.BackColor = SystemColors.ControlLightLight;

            switch (status)
            {
                case Status.AUTO:
                    button1.BackColor = SystemColors.InactiveCaption;
                    label4.Text = "Automatic";
                    break;
                case Status.ON:
                    button2.BackColor = SystemColors.InactiveCaption;
                    label4.Text = "Sensor Ignored";
                    break;
                case Status.OFF:
                    button3.BackColor = SystemColors.InactiveCaption;
                    label4.Text = "System Offline";
                    break;
            }
        }

        public Form1()
        {
            InitializeComponent();
            ChangeButtonState();
            UpdateApplicationStatus();
            UpdateSetPoint(setPoint);

            trackBar2.Value = setPoint;

            String[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                comboBox1.Items.Add(port);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            status = Status.AUTO;
            sensorSerial.Write("!B1");
            ChangeButtonState();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            status = Status.ON;
            sensorSerial.Write("!B2");
            ChangeButtonState();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            status = Status.OFF;
            sensorSerial.Write("!B3");
            ChangeButtonState();
        }

        private void UpdateSetPoint(int value)
        {
            setPoint = value;
            label9.Text = setPoint.ToString() + " F";
        }

        private void CheckSensor()
        {
            if (!sensorSerial.IsOpen)
            {
                return;
            }

            if (sensorSerial.BytesToRead == 0)
            {
                return;
            }

            String strTemperature = sensorSerial.ReadLine();
            int temperature = 0;
            bool conversionWorked = Int32.TryParse(strTemperature, out temperature);

            if (conversionWorked)
            {
                trackBar1.Value = temperature;
                label7.Text = temperature.ToString() + " F";
                int diff = temperature - setPoint;
                
                if (diff > 0)
                {
                    // Four levels
                    level = (int)(diff / 7.5);
                }
                else
                {
                    level = 0;
                }
            }
        }

        private void SensorClock_Tick(object sender, EventArgs e)
        {
            if (sensorSerial != null && sensorSerial.IsOpen) CheckSensor();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            String serialPort = comboBox1.Text;
            InitSerial(serialPort);
        }

        private void panel6_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            
        }

        private void trackBar2_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateSetPoint(trackBar2.Value);
            string val = setPoint.ToString();


            if (val.Length == 1)
                sensorSerial.Write("!T00" + val);
            else if (val.Length == 2)
                sensorSerial.Write("!T0" + val);
            else
                sensorSerial.Write("!T" + val);
        }
    }
}