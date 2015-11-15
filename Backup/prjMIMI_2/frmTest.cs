using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Speech.Recognition;
using System.Speech.Synthesis;

namespace prjMIMI_2
{
    public partial class frmTest : Form
    {
        SpeechRecognitionEngine sp;
        SpeechSynthesizer synth = new SpeechSynthesizer();

        int nbRing;
        clsSound snd = new clsSound();

        public frmTest()
        {
            InitializeComponent();
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            synth.Rate = -2;
            synth.Volume = 100;
            synth.Speak("Welcome to my application");

            sp = new SpeechRecognitionEngine();
            sp.SetInputToDefaultAudioDevice();
            sp.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognised);
            sp.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(SpeechDetected);
            //LoadGrammar("Calltext.grxml");
            LoadGrammar("answer.grxml");
        }
        private void LoadGrammar(string file)
        {
            Grammar gram = new Grammar(file);
            sp.UnloadAllGrammars();
            sp.LoadGrammar(gram);
            sp.RecognizeAsync(RecognizeMode.Multiple);
        }

        private void SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (e.Result.Semantics != null)
            {
                lblResult.Text = (e.Result.Text);
            }//Semantics                
        }

        private void SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {

            if (synth.State == SynthesizerState.Speaking)
            {
                synth.SpeakAsyncCancelAll();
            }
        }

        private void btnSwitch_Click(object sender, EventArgs e)
        {
            MessageBox.Show(sp.Grammars.ToString());
        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            tmrCall.Enabled = true;
            nbRing = 3;
        }

        private void tmrCall_Tick(object sender, EventArgs e)
        {
            if (nbRing > 0)
            {
                snd.Play("Media\\nokia-tune.mp3");
                nbRing--;
            }
            else
            {
                //save missed call
                tmrCall.Enabled = false;
            }

        }

    }
}
