using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;
using System.Windows.Forms;

namespace LedControl
{
    public class LedController
    {
        public SerialPort sp;
        byte[] buffer;
        public int numberOfLeds;
        bool respondToAck = true;
        Monitor monitor;
        public LedController(int numberOfLeds, string comName, int baud,  Monitor monitor)
        {
            this.monitor = monitor;
            this.numberOfLeds = numberOfLeds;
            buffer = new byte[(numberOfLeds * 3) + 6];
            buffer[0] = Convert.ToByte('A');
            buffer[1] = Convert.ToByte('d');
            buffer[2] = Convert.ToByte('a');

            buffer[3] = (byte)((numberOfLeds - 1));
            buffer[4] = (byte)(0);
            buffer[5] = (byte)(buffer[3] ^ buffer[4] ^ 0x55);

            sp = new SerialPort(comName, baudRate: baud);
            sp.DataReceived += sp_DataReceived;
            sp.Open();
            
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sprl = sp.ReadLine();
            monitor.listBox1.Invoke((MethodInvoker)delegate()
            {
                monitor.listBox1.Items.Add(sprl);
            });
            if (sprl == "Ada" && respondToAck)
            {
                sp.Write(buffer, 0, buffer.Length);
            }
            sp.DiscardInBuffer();
            sp.DiscardOutBuffer();
        }

        public void ChangeColorOfLed(int index, Color color)
        {
            byte[] colors = Led.ConvertColorToRGB(color);
            buffer[Led.BufferPosition(buffer, 6, index, Led.Colorvalues.R)] = colors[0];
            buffer[Led.BufferPosition(buffer, 6, index, Led.Colorvalues.G)] = colors[1];
            buffer[Led.BufferPosition(buffer, 6, index, Led.Colorvalues.B)] = colors[2];
            sp.Write(buffer, 0, buffer.Length);
        }

    }

    
}
