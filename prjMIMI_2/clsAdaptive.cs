using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace prjMIMI_2
{
    class clsAdaptive
    {

        /*
         * 
         * Inputs: Dialogue history, task progression, workload manager
         * 
         * Knowledge Base: user model, task model, contextual model
         * 
         * Adaptation Engine: pause and resume according to rules
         * 
         */

        public Queue q = new Queue();


        //Distraction level

        //Adaptation effect

        public void TrainingNeuralNetwork()
        {

        }

        public byte estDistractionLevel()
        {
            return 0;
        }
    }

    #region Neural Network Implementation 3 Layers
    class NeuralNetwork
    {
        private int num_in;
        private int num_hid;
        private int num_out;
        private double[,] i_to_h_wts;
        private double[,] h_to_o_wts;
        private double[] inputs;
        private double[] hidden;
        private double[] outputs;
        private double learningRate = 0.3;
        private Random gen = new Random();

        public NeuralNetwork(int num_in, int num_hid, int num_out)
        {
            this.num_in = num_in;
            this.num_hid = num_hid;
            this.num_out = num_out;
            i_to_h_wts = new double[num_in + 1, num_hid];
            h_to_o_wts = new double[num_hid + 1, num_out];
            inputs = new double[num_in + 1];
            hidden = new double[num_hid + 1];
            outputs = new double[num_out];
        }

        public void initialiseNetwork()
        {
            // Set the input value for bias node
            inputs[num_in] = 1.0;
            hidden[num_hid] = 1.0;
            // Set weights between input & hidden nodes.
            for (int i = 0; i < num_in + 1; i++)
            {
                for (int j = 0; j < num_hid; j++)
                {
                    // Set random weights between -2 & 2
                    i_to_h_wts[i, j] = (gen.NextDouble() * 4) - 2;
                }
            }
            // Set weights between hidden & output nodes.
            for (int i = 0; i < num_hid + 1; i++)
            {
                for (int j = 0; j < num_out; j++)
                {
                    // Set random weights between -2 & 2
                    h_to_o_wts[i, j] = (gen.NextDouble() * 4) - 2;
                }
            }
        }

        public void pass_forward(double[] applied_inputs)
        {
            // Load a set of inputs into our current inputs
            for (int i = 0; i < num_in; i++)
            {
                inputs[i] = applied_inputs[i];
            }

            // Forward to hidden nodes, and calculate activations in hidden layer
            for (int i = 0; i < num_hid; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < num_in + 1; j++)
                {
                    sum += inputs[j] * i_to_h_wts[j, i];
                }
                //Linear function: threshold
                /*if (sum>=0)
                    hidden[i] = 1;
                else
                    hidden[i] = 0;*/
                hidden[i] = SigmoidActivationFunction.processValue(sum);
            }

            // Forward to output nodes, and calculate activations in output layer
            for (int i = 0; i < num_out; i++)
            {
                double sum = 0.0;
                for (int j = 0; j < num_hid + 1; j++)
                {
                    sum += hidden[j] * h_to_o_wts[j, i];
                }
                //pass the sum, through the activation function, Sigmoid in this case
                //which allows for backward differentation
                outputs[i] = SigmoidActivationFunction.processValue(sum);


            }
        }

    #endregion

        #region Public Properties

        /// <summary>
        /// gets / sets the number of input nodes for the Neural Network
        /// </summary>
        public int NumberOfInputs
        {
            get { return num_in; }
            set { num_in = value; }
        }

        /// <summary>
        /// gets / sets the number of hidden nodes for the Neural Network
        /// </summary>
        public int NumberOfHidden
        {
            get { return num_hid; }
            set { num_hid = value; }
        }

        /// <summary>
        /// gets / sets the number of output nodes for the Neural Network
        /// </summary>
        public int NumberOfOutputs
        {
            get { return num_out; }
            set { num_out = value; }
        }

        /// <summary>
        /// gets / sets the input to hidden weights for the Neural Network
        /// </summary>
        public double[,] InputToHiddenWeights
        {
            get { return i_to_h_wts; }
            set { i_to_h_wts = value; }
        }

        /// <summary>
        /// gets / sets the hidden to output weights for the Neural Network
        /// </summary>
        public double[,] HiddenToOutputWeights
        {
            get { return h_to_o_wts; }
            set { h_to_o_wts = value; }
        }

        /// <summary>
        /// gets / sets the input values for the Neural Network
        /// </summary>
        public double[] Inputs
        {
            get { return inputs; }
            set { inputs = value; }
        }

        /// <summary>
        /// gets / sets the hidden values for the Neural Network
        /// </summary>
        public double[] Hidden
        {
            get { return hidden; }
            set { hidden = value; }
        }

        /// <summary>
        /// gets / sets the outputs values for the Neural Network
        /// </summary>
        public double[] Outputs
        {
            get { return outputs; }
            set { outputs = value; }
        }

        /// <summary>
        /// gets / sets the LearningRate (eta) value for the Neural Network
        /// </summary>
        public double LearningRate
        {
            get { return learningRate; }
            set { learningRate = value; }
        }
        #endregion

    }
    #region    Simoid function Implementation

    public class SigmoidActivationFunction
    {
        /// <summary>
        /// Takes a value for a current network node, and applies a sigmoid
        /// activation function to it, which is then returned
        /// </summary>
        /// <param name="x">The value to apply the activation to</param>
        /// <returns>The activation value, after a sigmoid function</returns>
        public static double processValue(double x)
        {
            return 1.0 / (1.0 + Math.Pow(Math.E, -x));
        }
    }
    #endregion
}
