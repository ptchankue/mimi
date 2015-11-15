using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Net.Sockets;
using System.Net;

namespace prjMIMI_2
{
    public partial class frmNetwork : Form
    {
        //public IPEndPoint
        Socket m_socListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
/*
        IPAddress ipAddr = IPAddress.Parse("127.0.0.1");
        IPEndPoint remoteEP = new IPEndPoint(ipAddr, 8000);
        */
        //int tcpIndx = 0;
        int tcpByte = 0;
        byte[] tcpRecv = new byte[1024];

        public Socket tcpSock;

        public frmNetwork()
        {
            InitializeComponent();

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            /*           
            try
            {
                m_socListener.Connect(remoteEP);
                String szData = "Hi Patrick!";
                byte[] byData = Encoding.ASCII.GetBytes(szData);
                m_socListener.Send(byData);
                m_socListener.Close();
            }
            catch (SocketException se)
            {
                MessageBox.Show(se.Message);
            }
            */
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          /*/" m_socListener.Connect(remoteEP);
           TcpClient.Client

            string str = Encoding.UTF8.GetString  m_socListener.Receive(byData);
            m_socListener.Close();*/
            tcpByte = tcpSock.Available;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            TcpListener serverSocket = new TcpListener(IPAddress.Any, 4955);
           
            TcpClient clientSocket = default(TcpClient);
            serverSocket.Start();
            
            clientSocket = serverSocket.AcceptTcpClient();
            

            try
            {
                
                NetworkStream networkStream = clientSocket.GetStream();
                byte[] bytesFrom = new byte[10025];
                networkStream.Read(bytesFrom, 0, (int)clientSocket.ReceiveBufferSize);
                string dataFromClient = System.Text.Encoding.ASCII.GetString(bytesFrom);
                dataFromClient = dataFromClient.Substring(0, dataFromClient.IndexOf("$"));
                MessageBox.Show( dataFromClient);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
