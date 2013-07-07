using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;


namespace LedControl
{
    public partial class Form1 : Form
    {
        LedController lc = new LedController(50, SerialPort.GetPortNames()[0], 115200);
        public Form1()
        {
            
            InitializeComponent();

            for (int i = 1; i <= 50; i++)
            {
                ListViewItem li = new ListViewItem(string.Format("Led #{0}", i), i - 1);
                

                listView1.Items.Add(li);
            }
            listView1.ItemSelectionChanged += listView1_ItemSelectionChanged;
            
        }

        void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (e.IsSelected)
            {
                ColorDialog cd = new ColorDialog();
                cd.ShowDialog(this);
                e.Item.BackColor = cd.Color;
                lc.ChangeColorOfLed(e.ItemIndex, cd.Color);
            }
        }
    }
}
