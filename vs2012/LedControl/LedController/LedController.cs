using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Drawing;

namespace LedControl
{
    public class LedController
    {
        SerialPort sp;
        byte[] buffer;
        public int numberOfLeds;
        bool respondToAck = true;

        public LedController(int numberOfLeds, string comName, int baud)
        {
            this.numberOfLeds = numberOfLeds;
            buffer = new byte[(numberOfLeds * 3) + 6];
            buffer[0] = Convert.ToByte('A');
            buffer[1] = Convert.ToByte('d');
            buffer[2] = Convert.ToByte('a');

            buffer[3] = (byte)((numberOfLeds - 1) >> 8);
            buffer[4] = (byte)(numberOfLeds - 1);
            buffer[5] = (byte)(buffer[3] ^ buffer[4] ^ 85);

            sp = new SerialPort(comName, baudRate: baud);
            sp.DataReceived += sp_DataReceived;
            sp.Open();
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string sprl = sp.ReadLine();
            if (sprl == "Ada" && respondToAck)
            {
                sp.Write(buffer, 0, buffer.Length);
            }
        }

        public void ChangeColorOfLed(int index, Color color)
        {
            int position = Led.BufferPosition(buffer, 6, index, Led.Colorvalues.R);
            byte[] colors = Led.ConvertColorToRGB(color);
            buffer[position] = colors[0];
            buffer[position + 1] = colors[1];
            buffer[position + 2] = colors[2];
            sp.Write(buffer, 0, buffer.Length);
        }

    }

    
}
