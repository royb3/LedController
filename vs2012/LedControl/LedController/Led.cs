using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace LedControl
{
    public class Led
    {
        public enum Colorvalues{R, G, B}
        int index;

        Color color;

        public Led(int index)
        {
            this.index = index;
            color = Color.Black;
        }
        public Led(int index, Color color)
        {
            this.index = index;
            this.color = color;
        }



        public static int BufferPosition(byte[] buffer, int offset, int index, Colorvalues cv)
        {
            int bufferlength = buffer.Length;
            
            int position = offset - 1 + (index * 3) + 1;
            switch (cv)
            {
                case Colorvalues.G:
                    position += 1;
                    break;
                case Colorvalues.B:
                    position += 2;
                    break;
                default:
                    break;
            }
            return position;
        }

        public static byte[] ConvertColorToRGB(Color color)
        {
            return new byte[] { color.R, color.G, color.B };
        }
    }
}
