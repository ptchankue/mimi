using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net;
using System.Net.Sockets;
using System.IO;

namespace prjMIMI_2
{
    public partial class frmCANbus : Form
    {
        //Int64 i = 0;
        TcpListener serverSocket;
        TcpClient clientSocket;

        public frmCANbus()
        {
            InitializeComponent();
        }

        private void frmCANbus_Load(object sender, EventArgs e)
        {
            //
            
        }

        public void MyThread()
        {

            TcpListener serverSocket = new TcpListener(IPAddress.Any, 4955);

            serverSocket.Start();
            TcpClient clientSocket = serverSocket.AcceptTcpClient();

            if (clientSocket.Connected)
            {
                NetworkStream net = clientSocket.GetStream();
                if (net.DataAvailable)
                {
                    string msg = new BinaryReader(net).ReadString();
                    //extractData(msg);
                    //Display speed and angle
                    //Console.WriteLine("Speed: {0} - Angle {1}", simSpeed, simSteer);
                    
                    txtData.AppendText(msg);

                    // Console.WriteLine("Max angle {0}, Min angle = {1}", maxAngle, minAngle);
                }
            }
            serverSocket.Stop();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                NetworkStream net = clientSocket.GetStream();
                string msg = new BinaryReader(net).ReadString();
                //txtData.Text+= Environment.NewLine + msg;    
                txtData.Text = msg;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                //connect to the network
                serverSocket = new TcpListener(IPAddress.Any, 5555);
                serverSocket.Start();
                clientSocket = serverSocket.AcceptTcpClient();
            }
        }
    }
}
