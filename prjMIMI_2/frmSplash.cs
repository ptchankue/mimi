using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Speech.Synthesis;


namespace prjMIMI_2
{
    public partial class frmSplash : Form
    {

        SpeechSynthesizer synth = new SpeechSynthesizer();
        public frmSplash()
        {
            InitializeComponent();
            synth.Rate = -2;
            synth.Speak("Welcome to the usability evaluation");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
