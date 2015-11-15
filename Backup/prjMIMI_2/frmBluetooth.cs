using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.IO;
using System.Threading;

namespace prjMIMI_2
{
    public partial class frmBluetooth : Form
    {
        SerialPort sp = new SerialPort("COM20");
        //SerialPort sp2 = new SerialPort("COM21");

        private AutoResetEvent receiveNow=new AutoResetEvent(false);

        byte[] bytes = new byte[256]; string data;

        public frmBluetooth()
        {
            InitializeComponent();
        }

        private void frmBluetooth_Load(object sender, EventArgs e)
        {
            sp.NewLine = "\r\n";
            sp.Parity = Parity.None;
            sp.DataBits = 8;
            sp.StopBits = StopBits.One;
            sp.DtrEnable = true;
            sp.WriteBufferSize = 1024;

            sp.Open(); //sp2.Open();
            //sp.WriteLine("ATE0");
            //sp.WriteLine("ATDT6632");
            //sp.BaseStream.Flush();
            //sp.WriteLine("My string to be sent");
            //sp.BaseStream.Flush();
            sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
        }
        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
                receiveNow.Set();

            string str;
            str = sp.ReadExisting();
            MessageBox.Show(str);
            File.AppendAllText("log_sender.txt", str + "\r\n");
            
        }

        private void frmBluetooth_FormClosed(object sender, FormClosedEventArgs e)
        {
            sp.Close();
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            sp.WriteLine("ATD"+txtNumber.Text+";");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            sp.WriteLine("AT+CGMI");
            //Thread.Sleep(500);
            //data = sp2.ReadExisting();
            MessageBox.Show(ReadResponse(300));
            
        }

        private string ReadResponse(int timeout)
        {
            string buffer = string.Empty;
            do
            {
                if (receiveNow.WaitOne(timeout, false))
                {
                    string t = sp.ReadExisting();
                    buffer += t;
                }
                else
                {
                    if (buffer.Length > 0)
                        throw new ApplicationException("Response received is incomplete.");
                    else
                        throw new ApplicationException("No data received from phone.");
                }
            }
            while (!buffer.EndsWith("\r\nOK\r\n") && !buffer.EndsWith("\r\nERROR\r\n"));
            return buffer;
        }


    }
}

/*int i;
                while ((i = stream.Read(bytes, 0, bytes.Lenght)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }
*/