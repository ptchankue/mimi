using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.IO;

namespace prjMIMI_2
{
    public partial class frmNN : Form
    {
        public frmNN()
        {
            InitializeComponent();
        }
        
        static int num_pattern;  //actual number minus 1
        static int num_in = 30;
        static int num_out = 3;
        static int num_hid = 100;

        private NeuralNetwork nn = new NeuralNetwork(num_in, num_hid, num_out);
        private Random gen = new Random();
        private int training_times = 1000;

        double[,] train; 
        double[,] targ; 


        double err;

        private void Load_TrainingSet(string stream)
        {
            StreamReader sr = new StreamReader(stream);

            try
            {
                for (int i = 0; i < num_pattern; i++)
                {
                    string d = sr.ReadLine();
                    for (int j = 0; j < num_in; j++)
                    {
                        train[i, j] = Convert.ToDouble(d.Substring(j, 1));
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Load_OutputSet(string stream)
        {
            StreamReader sr = new StreamReader(stream);

            try
            {
                for (int i = 0; i < num_pattern; i++)
                {
                    string d = sr.ReadLine();
                    for (int j = 0; j < num_out; j++)
                    {
                        targ[i, j] = Convert.ToDouble(d.Substring(j, 1));
                    }
                }
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            nn.initialiseNetwork();
            doTraining(training_times);
            Cursor.Current = Cursors.Default;
            btnRun.Enabled = true;
            SaveWeights();
        }

        private string DecToBin(int n, int len)
        {
            string str, tmp;int q, r;
            str = tmp = "";
            do
            {
                q = n / 2;
                r = n - q * 2;
                str += Convert.ToString (r);
                n = q;
            } while (n >= 2);
            str += Convert.ToString(q);
            int c = str.Length;
            for (int i = 0; i < len - c; i++)
                str += "0";
            //reverse
            for (int i = 0; i < str.Length; i++)
            {
                tmp += str.Substring(str.Length - i - 1, 1);
            }
            return tmp;
        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            string strSpeed = DecToBin(Convert.ToInt16(txtSpeed.Text), 8); //8bits
            string strDSpeed = DecToBin(Convert.ToInt16(txtDSpeed.Text) + 80, 8);
            string strAngle = DecToBin(Convert.ToInt16(txtAngle.Text) + 80, 7);
            string strDAngle = DecToBin(Convert.ToInt16(txtDAngle.Text) + 80, 7);
            string strInput = strSpeed + strDSpeed + strAngle + strDAngle;

            double[] input = new double[num_in];
            for (int i = 0; i < num_in; i++)
                input[i] = Convert.ToInt16(strInput.Substring(i, 1));

            nn.pass_forward(input);

            string r = Convert.ToInt16(nn.Outputs[0]).ToString();
            r += Convert.ToInt16(nn.Outputs[1]).ToString();
            r += Convert.ToInt16(nn.Outputs[2]).ToString();
            switch (r)
            {
                case "001": txtLevel.Text = "Lowest";
                    break;
                case "010": txtLevel.Text = "Low";
                    break;
                case "011": txtLevel.Text = "Medium";
                    break;
                case "100": txtLevel.Text = "High";
                    break;
                case "101": txtLevel.Text = "Highest";
                    break;
            }
            //MessageBox.Show(r);
        }

        public void doTraining(int training_times)
        {

            this.training_times = training_times;

            for (int i = 0; i < this.training_times; i++)
            {
                //loop through all training set examples
                for (int j = 0; j < num_pattern; j++)
                {   //feed forward through network
                    nn.pass_forward(getTrainSet(j));
                    //do the weight changes (pass back)
                    train_network(getTargSet(j));//get
                }
            }
            MessageBox.Show("Training \n Done!\n Last error " + err.ToString());
        }

        private double[] getTrainSet(int x)
        {
            //return the pattern number idx
            double[] trainValues = new double[num_in];
            for (int i = 0; i < num_in; i++)
                trainValues[i] = train[x, i];

            return trainValues;
        }

        private double[] getTargSet(int x)
        {
            //return the pattern number idx
            double[] targValues = new double[num_out];
            for (int i = 0; i < num_out; i++)
                targValues[i] = targ[x, i];

            return targValues;
        }

        private void train_network(double[] target)
        {
            //get momentum values (delta values from last pass)
            double[] delta_hidden = new double[nn.NumberOfHidden + 1];
            double[] delta_outputs = new double[nn.NumberOfOutputs];

            // Get the delta value for the output layer
            for (int i = 0; i < nn.NumberOfOutputs; i++)
            {
                delta_outputs[i] =
                    nn.Outputs[i] * (1.0 - nn.Outputs[i]) * (target[i] - nn.Outputs[i]);
            }
            // Get the delta value for the hidden layer
            for (int i = 0; i < nn.NumberOfHidden + 1; i++)
            {
                double error = 0.0;
                for (int j = 0; j < nn.NumberOfOutputs; j++)
                {
                    error += nn.HiddenToOutputWeights[i, j] * delta_outputs[j];
                }
                delta_hidden[i] = nn.Hidden[i] * (1.0 - nn.Hidden[i]) * error;
                err = error;
            }
            // Now update the weights between hidden & output layer
            for (int i = 0; i < nn.NumberOfOutputs; i++)
            {
                for (int j = 0; j < nn.NumberOfHidden + 1; j++)
                {
                    nn.HiddenToOutputWeights[j, i] += nn.LearningRate * delta_outputs[i] * nn.Hidden[j];
                }
            }
            // Now update the weights between input & hidden layer
            for (int i = 0; i < nn.NumberOfHidden; i++)
            {
                for (int j = 0; j < nn.NumberOfInputs + 1; j++)
                {
                    nn.InputToHiddenWeights[j, i] += nn.LearningRate * delta_hidden[i] * nn.Inputs[j];
                }
            }
        }

        //Save weights
        private void SaveWeights()
        {
            StreamWriter sw = new StreamWriter("Weights.txt");
            for (int i = 0; i < num_in + 1; i++)
            {
                for (int j = 0; j < num_hid; j++)
                {
                    sw.WriteLine(nn.InputToHiddenWeights[i, j] );
                }
            }
            // Set weights between hidden & output nodes.
            for (int i = 0; i < num_hid + 1; i++)
            {
                for (int j = 0; j < num_out; j++)
                {
                    sw.WriteLine(nn.HiddenToOutputWeights[i, j]);
                }
            }
        }

        //Load weights
        private void LoadWeights(string stream)
        {
            
        }

        private void frmNN_Load(object sender, EventArgs e)
        {
            num_pattern = GetnbPattern("NNInput.txt");
            //initialising global variables
            train = new double[num_pattern, num_in];
            targ = new double[num_pattern, num_out];
            btnRun.Enabled = false;
            //Loading inputs and output
            Load_TrainingSet("NNInput.txt");
            Load_OutputSet("NNOutput.txt");
        }

        private int GetnbPattern(string stream)
        {
            StreamReader sr = new StreamReader(stream);
            string data; int i = 0;

            while ((data=sr.ReadLine())!=null)
            {
                i++;
            }
            return i;
        }

        private void btnLoadwght_Click(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader("Weights.txt");
            for (int i = 0; i < num_in + 1; i++)
            {
                for (int j = 0; j < num_hid; j++)
                {
                    nn.InputToHiddenWeights[i, j] = Convert.ToDouble(sr.ReadLine());
                }
            }
            // Set weights between hidden & output nodes.
            for (int i = 0; i < num_hid + 1; i++)
            {
                for (int j = 0; j < num_out; j++)
                {
                    nn.HiddenToOutputWeights[i, j] = Convert.ToDouble(sr.ReadLine());
                }
            }

            btnRun.Enabled = true;
        }

    }
}
