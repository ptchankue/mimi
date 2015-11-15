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

using System.Data.OleDb;

using System.IO;
using System.IO.Ports;

using System.Net.Sockets;
using System.Net;

using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Sockets;

using System.Threading;

namespace prjMIMI_2
{
    public partial class frmCar : Form
    {

        bool dialogue = false;
        bool acceptSpeech = false;
        byte state;
        string nb = "";
        string nbvalid = "";
        string response = "";
        string selection = "";
        string msg = ""; string total = "12", semantic = "";
        int counter;
        int iContact = 1, nbdigit = 0;
        string[] result;

        string strScrollText = "M I M I : Multimodal Interface for Mobile Info-communication";
        int intPosition = 0;

        string savedText = "";
        string[] message = { "1- I am running late", "2- Appointment postponed", "3- I will call you later" };

        string[] digits ={"Zero", "O", "Oh","One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",
                 "double o", "double one", "double two", "double three", "double four", "double five", "double six", "double seven", "double eight", "double nine", 
                 "triple o","triple one", "triple two", "triple three", "triple four", "triple five", "triple six", "triple seven", "triple eight", "triple nine" 
                 };
        clsInput InputPool = new clsInput(); // Stores all driver's inputs

        clsFrame frame;

        clsPhone phone = new clsPhone();

        SpeechRecognitionEngine sp; //= new SpeechRecognitionEngine();

        SpeechSynthesizer synth = new SpeechSynthesizer();

        private BluetoothClient bluetoothClient;

        BluetoothDeviceInfo[] bluetoothDeviceInfo = { };

        DateTime t0, t1;

        TimeSpan diff; bool musicOn;


        //Training variables
        string timeAsk, timeAns, speed, angle;

        //string[] words = InitializeGrammar();

        string[] r = { };

        byte[] bytes = new byte[256];
        string data = "";
        TcpClient clientLCT = new TcpClient();
        TcpClient clientSteering = new TcpClient();
        TcpListener listLCT;
        TcpListener listSteering;

        private void CursorPos()
        {
            //cursor

            Point ptt = new Point(btnPTT.Width / 2, btnPTT.Height / 2);
            Cursor.Position = btnPTT.PointToScreen(ptt);
        }


        private void CursorNextTask()
        {
            Point ptt = new Point(btnStop.Width / 2, btnStop.Height / 2);
            Cursor.Position = btnStop.PointToScreen(ptt);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Setting of the  Recogniseer and Synthesiser

            CursorPos();
            //strScrollText = PadString(strScrollText, 50);
            LoadTask();

            synth.Rate = -2;
            synth.Volume = 100;
            //synth.Speak("Welcome to my application");

            sp = new SpeechRecognitionEngine();
            sp.SetInputToDefaultAudioDevice();
            sp.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognised);
            sp.SpeechDetected += new EventHandler<SpeechDetectedEventArgs>(SpeechDetected);
            LoadGrammar("Calltext.grxml");
            sp.RecognizeAsync(RecognizeMode.Multiple);

            listLCT = new TcpListener(IPAddress.Any, 4955);
            listLCT.Start();
            //clientLCT = listLCT.AcceptTcpClient();
            listSteering = new TcpListener(IPAddress.Any, 5000);
            listSteering.Start();
            //clientLCT = listLCT.AcceptTcpClient();

            Thread jobLCT = new Thread(new ThreadStart(ReadLCT));
            //jobLCT.Start();


            // txtTask.Text=frmEvaluation.t
            Tts("Welcome to the usability evaluation");
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Save the dialogue in a file
            StreamWriter SW;
            SW = File.AppendText("fileDialogue.txt");
            SW.WriteLine("");
            SW.WriteLine("************ New Dialogue **************");
            SW.WriteLine("Start: " + DateTime.Now.ToString());

            SW.WriteLine("End " + DateTime.Now.ToString());
            SW.Close();

            //MessageBox.Show("End of the user testing \n Thank you for your participation!! \n Please fill the questionnaire");
            int i = 1;
            //updating the current task
            StreamWriter sw = new StreamWriter("task.txt");
            sw.WriteLine(i.ToString());
            sw.Close();
            this.Hide();

            foreach (Form frm in Application.OpenForms)
            {
                if (frm.Name == "frmEvaluation")
                {
                    frm.Activate();
                    frm.Show();
                }
            }
        }

        private void AddInput(clsInput cur)
        {

            if (InputPool == null)
            {
                InputPool = cur;
            }
            else
            {
                clsInput temp = new clsInput();
                temp = InputPool;
                while (temp.next != null)
                {
                    temp = temp.next;
                }
                cur.next = null;
                temp.next = cur;
                temp = cur = null; //frees memory
            }

            ShowInput();

        }

        public frmCar()
        {
            InitializeComponent();

            InputPool = null;

            //r = phone.FindContact();

        }


        public void InitialiseSAPI()
        {
            //string[] words = InitializeGrammar();
            //Initialise speech
            try
            {
                sp = new SpeechRecognitionEngine();
                sp.SetInputToDefaultAudioDevice();                
                sp.SpeechRecognized += new EventHandler<SpeechRecognizedEventArgs>(SpeechRecognised);
                LoadGrammar("CallText.grxml");
                
            }
            catch (ExecutionEngineException e)
            {
                MessageBox.Show(e.Message);
            }

        }


        private void Record(string text)
        {
            ListViewItem item = new ListViewItem();
            item.Text = "Driver: " + text + ".";

            Log("Driver", text);
        }

        public void Log(string who, string what)
        {
            StreamWriter SW;
            try
            {
                SW = File.AppendText("log.csv");
                SW.WriteLine(DateTime.Now.ToLongTimeString() + ", " + who + ", " + what);//does not give seconds
                SW.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DateTime.Now.
        }

        public bool isDigit(string d)
        {
            switch (d)
            {
                case "Zero": case "O":               case "Oh":
                case "One":
                case "Two":
                case "Three":
                case "Four":
                case "Five":
                case "Six":
                case "Seven":
                case "Eight":
                case "Nine":
                case
                    "double o":
                case "double one":
                case "double two":
                case "double three":
                case "double four":
                case "double five":
                case "double six":
                case "double seven":
                case "double eight":
                case "double nine":
                case
                    "triple o":
                case "triple one":
                case "triple two":
                case "triple three":
                case "triple four":
                case "triple five":
                case "triple six":
                case "triple seven":
                case "triple eight":
                case "triple nine":
                    return true;

            }

            return false;
        }

        public string ConvertDigit(string d)
        {
            switch (d.ToLower())
            {
                case "zero":
                case "o":
                case "oh": return "0";
                case "one": return "1";
                case "two": return "2";
                case "three": return "3";
                case "four": return "4";
                case "five": return "5";
                case "six": return "6";
                case "seven": return "7";
                case "eight": return "8";
                case "nine": return "9";
                case
                    "double o": return "00";
                case "double one": return "11";
                case "double two": return "22";
                case "double three": return "33";
                case "double four": return "44";
                case "double five": return "55";
                case "double six": return "66";
                case "double seven": return "77";
                case "double eight": return "88";
                case "double nine": return "99";
                case
                    "triple o": return "000";
                case "triple one": return "111";
                case "triple two": return "222";
                case "triple three": return "333";
                case "triple four": return "444";
                case "triple five": return "555";
                case "triple six": return "666";
                case "triple seven": return "777";
                case "triple eight": return "888";
                case "triple nine": return "999";
                default: return "";
            }

        }

        private void SpeechDetected(object sender, SpeechDetectedEventArgs e)
        {
            if (acceptSpeech && synth.State == SynthesizerState.Speaking)
            {
                synth.SpeakAsyncCancelAll();
            }
        }

        private void SpeechRecognised(object sender, SpeechRecognizedEventArgs e)
        {
            if (acceptSpeech)
            {
                // can be done with XML grammar sgxml
                string[] words;

                Beep();

                //call number sms number ?

                // look for homophones, Result.Semantics

                //if (e.Result.Semantics != null)
                //{
                    if (e.Result.Text.Contains("double") || e.Result.Text.Contains("triple") || e.Result.Text.Contains("re dial"))
                    {
                        words = new string[] { "" };
                        words[0] = e.Result.Text;
                    }
                    else
                    {
                        words = e.Result.Text.Split(' ');
                    }

                    if (e.Result.Text.ToLower() == "repeat")
                        Tts(savedText);
                    //tmrSilence.Enabled = false;

                    if (e.Result.Text.ToLower() == "call" ||
                        e.Result.Text.ToLower() == "phone" ||
                        e.Result.Text.ToLower() == "dial")// nb.Length==10) //check also 10-digits
                    {
                        btnCall_Click(sender, e);
                    }

                    if (e.Result.Text.ToLower() == "send") //check also 10-digits
                    {
                        // btnSend_Click(sender, e);
                    }

                    Record(e.Result.Text);
                    
                    if (e.Result.Semantics.ContainsKey("answer") && state == 31)
                    {
                        response = e.Result.Semantics["answer"].Value.ToString();
                    }

                    if (e.Result.Semantics.ContainsKey("Number"))
                    {
                        nb += e.Result.Semantics["Number"].Value.ToString();
                    }

                    if (e.Result.Semantics.ContainsKey("option") && state == 25)
                    {
                        selection = e.Result.Semantics["option"].Value.ToString();
                    }

                    if (e.Result.Semantics.ContainsKey("cmd") && e.Result.Semantics.ContainsKey("name"))
                    {
                        if (e.Result.Semantics["name"].Value.ToString() == "number")
                        {
                            //ask for the number in grounding()

                        }
                        semantic = "name";
                    }

                    if (e.Result.Semantics.ContainsKey("cmd"))
                    {
                        semantic = "action";
                    }/**/

                    foreach (string word in words)
                    {
                        clsInput inp = new clsInput(word, "", "Speech", DateTime.Now, e.Result.Confidence);

                        AddInput(inp);
                    }
                    
                    if (e.Result.Semantics.ContainsKey("cancel"))
                    {
                        StopDialogue();
                        Tts("Cancelling...");
                        LoadGrammar("calltext.grxml");
                    }

                    else
                        Fusion();
                //}//Semantics

                // }  //call numbers
                // handling digits with adjectives

            //}
            //else
            //{
                //lvwDialogue.Items.Add("Not used...");
            }
        }

        private void PhoneNumber()
        {

        }

        private void btnCall_Click(object sender, EventArgs e)
        {
            if (state == 31)
                response = "Yes";

            if (nb != "" && frame != null && frame.name == "call" && frame.isFilled())
            {
                //check teh number
                frame.slots.value = nb;
                state = 3;
            }

            if (frame != null && !frame.isFilled())
            {
                // try to get a number in the input pool
                //if (frame.name == "call" || frame.name == "message")
                //GetNumber();                
            }
            else
            {
                clsInput inp = new clsInput("call", "action", "Click", DateTime.Now, 1);
                Record("<Button> Call");
                AddInput(inp);
                Fusion();
            }

            //when incoming call accept it
        }

        private void btnReject_Click(object sender, EventArgs e)
        {
            if (state == 31)
                response = "No";
            else
            {

                if (frame != null && !frame.isFilled())
                {
                    // try to get a number in the input pool
                    if (frame.name == "call" || frame.name == "message")
                        GetNumber();

                }
                else
                {
                    clsInput inp = new clsInput("reject", "action", "Click", DateTime.Now, 1);
                    //InputPool 
                    AddInput(inp);
                    Record("<Button> Reject or end a call");
                    Fusion();
                }
            }
        }

        private void bntPairing_Click(object sender, EventArgs e)
        {
            clsInput inp = new clsInput("pairing", "action", "Click", DateTime.Now, 1);

            AddInput(inp);
            Record("<Button> Pairing");
            Fusion();

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (state == 31)
                response = "Yes";
            if (frame != null && !frame.isFilled())
            {
                // try to get a number in the input pool
                if (frame.name == "call" || frame.name == "message")
                    GetNumber();


            }
            else
            {
                clsInput inp = new clsInput("message", "action", "Click", DateTime.Now, 1);
                //InputPool 
                AddInput(inp);
                Record("<Button> Send Message");
                Fusion();
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            clsInput inp = new clsInput("read", "action", "Click", DateTime.Now, 1);
            //InputPool 
            AddInput(inp);
            Record("<Button> Read message");
            Fusion();
        }

        private void ShowInput()
        {

        }


        private void Fusion()
        {
            clsInput temp = new clsInput();
            temp = InputPool;

            DeleteExpiredData();

            if (!dialogue) StartDialogue();

            while (temp != null)
            {
                if ((temp.GetPartOfSpeech() == "action") && frame == null)
                {
                    SelectFrame(temp.GetInfo());

                    state = 1; //  tries to fill that frame

                    //start task's timer
                    //tmrTimeFrame.Enabled = true;
                    counter = 0; //cancel after 3 attempts
                    RemoveInput(temp);
                }
                else
                {
                    /*if (frame != null)

                        if (frame.name == "call" && state == 31)
                            response = "Yes";
                    if (temp.GetPartOfSpeech() == "digit")
                        nbdigit++;
                    if (frame != null && !frame.isFilled())
                    {
                        if ((frame.name == "call" || frame.name == "message" )&& nbdigit>=10)
                            GetNumber();
                                                    
                    }*/
                }
                if (frame != null && frame.name == "addressbook")
                {
                    if (temp.GetInfo().ToLower() == "next")
                    {
                        iContact++;
                        state = 4;
                    }
                    if (temp.GetInfo().ToLower() == "previous" || temp.GetInfo().ToLower() == "back")
                    {
                        iContact--;
                        state = 4;
                    }

                }
                temp = temp.next;
            }
            temp = null;

            //Dialogue();

        }//fusion

        private void SelectFrame(string input)
        {
            switch (input.ToLower())
            {
                case "call":
                    frame = new clsFrame("call");
                    lblFrame.Text = frame.name; break;
                //Only Load Names and Numbers Grammars
                case "callback":
                    frame = new clsFrame("callback");
                    lblFrame.Text = frame.name; break;
                case "re dial":
                    frame = new clsFrame("redial");
                    lblFrame.Text = frame.name; break;

                case "message":
                    frame = new clsFrame("message");
                    lblFrame.Text = frame.name; break;

                case "pairing":
                    frame = new clsFrame("pairing");
                    lblFrame.Text = frame.name; break;
                case "time":
                    frame = new clsFrame("time");
                    lblFrame.Text = frame.name; break;
                case "play":
                    frame = new clsFrame("play");
                    lblFrame.Text = frame.name; break;
                case "stop":
                    frame = new clsFrame("stop");
                    lblFrame.Text = frame.name; break;
                case "pause":
                    frame = new clsFrame("pause");
                    lblFrame.Text = frame.name; break;
                case "accept":
                    frame = new clsFrame("accept");
                    lblFrame.Text = frame.name; break;
                case "read":
                    frame = new clsFrame("read");
                    lblFrame.Text = frame.name; break;
                case "addressbook":
                    frame = new clsFrame("addressbook");
                    lblFrame.Text = frame.name; break;

                case "incoming_sms":
                    frame = new clsFrame("incoming_sms");
                    lblFrame.Text = frame.name; break;

                case "incoming_call":
                    frame = new clsFrame("incoming_call");
                    lblFrame.Text = frame.name;
                    break;
            }
        }
        private void FillingSlots()
        {

            clsInput temp = new clsInput();
            temp = InputPool;

            //reverse the parsing
            while (temp != null && frame != null)
            {
                clsSlot sl = new clsSlot();
                sl = frame.slots;
                while (sl != null && frame != null)
                {
                    if ((sl.name == temp.GetPartOfSpeech()))
                    {
                        sl.value = temp.GetInfo();
                        //tmrTimeFrame.Enabled = false;
                    }
                    sl = sl.next;
                }
                if (frame.name == "call" && temp.GetInfo() == "number")
                    state = 26;
                temp = temp.next;
            }
            temp = null;

            if (frame != null && frame.isFilled())
                state = 3; // check for number state 21

            //
        }//FillingSlots



        void GenerateCallEvents()
        {
            double prob = 0.2;
            // if number > call probability generate call
            Random x = new Random();
            double y = x.NextDouble();
            if (y < prob)
            {
                // Incoming call

                // Ringing

                Ringing();

                // Pause current dialogue
                // save frame and state
                StopDialogue();
                Tts(" You have a call from John");

                //Tts(" You have a text message from ");
            }

        }

        private void StartDialogue()
        {
            dialogue = true;
            state = 0;

        }
        private void PauseDialogue()
        {
            /* stop sounds
             * save state and frame in a queue
             */

        }
        private void StopDialogue()
        {
            dialogue = false; // the dialogue is not completed yet
            InputPool = null; // clear it properly
            frame = null;
            lblFrame.Text = "";
            state = 0; // return to the idle state

        }

        private void RequestingInput()
        {
            //parse frame and ask for empty slot

            // When user silent for 10 seconds


            clsSlot sl = new clsSlot();
            sl = frame.slots;
            while (sl != null)
            {
                if (sl.value == "")
                {
                    Tts("Give a value for " + sl.name);
                    counter++;
                    break;
                }
                sl = sl.next;
            }

            if (counter == 3)
            {
                StopDialogue();
                //tmrTimeFrame.Enabled = false;
            }

            // DeleteExpiredData();
        }

        private void Grounding()
        {
            //string nb;
            switch (frame.name)
            {
                case "pairing":
                    Tts("Do you want to pair a new mobile phone? Say 'Yes' or 'No'");
                    Answering();
                    break;
                case "call":
                    /*call number
                    if (nb != "")
                    {
                        Tts("Do you want to dial " + nb + ". Say 'Yes' or 'No'");
                        Answering();
                    }
                    */
                    if (phone.CheckSANumber(nbvalid) == "ok")
                    {
                        Tts("Do you want to dial " + phone.SayAsTelephone(nbvalid) + ". Say 'Yes' or 'No'");
                        Answering();// get the answer
                        state = 31;

                    }
                    else
                    {
                        Tts("Do you want to call " + frame.slots.value + ". Say 'Yes' or 'No'");
                        Answering();// get the answer

                    }
                    break;
                case "message":

                    if (phone.CheckSANumber(nbvalid) == "ok")
                    {
                        Tts("Do you want to send a text message to "
                              + phone.SayAsTelephone(nbvalid) + ". Say 'Yes' or 'No'");
                        state = 31;
                        Answering();// get the answer

                    }
                    else
                    {
                        Tts("Do you want to  send a text message to " + frame.slots.value + ". Say 'Yes' or 'No'");
                        Answering();// get the answer

                    }

                    break;
                //no grounding for the time
                case "time":
                    Tts("Do you want to know the time? Say 'Yes' or 'No'");
                    Answering();
                    break;

                case "play": // Pause, Stop, Play
                    string path = phone.RetrieveSong(frame.slots.value);
                    if (path != "")
                    {
                        Tts("Do you want to play " + frame.slots.value + "? Say 'Yes' or 'No'");
                        Answering();
                    }
                    else
                    {
                        Tts(frame.slots.value + " is not in your song's library");
                        state = 0;
                    }
                    break;
                case "add":

                    Tts("Do you want to add the name" + frame.slots.value + " with the number"
                    + frame.slots.next.value + "to your phone book? Say 'Yes' or 'No'");
                    Answering();

                    break;
                case "edit":
                    break;
                case "remove":
                    break;

                case "addressbook":
                    //make sure that the song is playing

                    Tts("Do you want to find a contact? Say 'Yes' or 'No'");
                    Answering();

                    break;

                case "stop":
                    //make sure that the song is playing

                    Tts("Do you want to stop the conversation? Say 'Yes' or 'No'");
                    Answering();

                    break;

                case "read":

                    Tts("Do you want to read the last text message? Say 'Yes' or 'No'");
                    //Say who sent it

                    Answering();
                    break;

                case "redial":
                    //retrieve number and name
                    result = phone.GetLastOutgoingCall();
                    if (result[1] != "")
                    {
                        Tts("Do you want to redial " + result[1] + " ? Say 'Yes' or 'No' ");
                    }
                    else
                    {
                        //number
                        Tts("Do you want to redial the last number " + phone.SayAsTelephone(result[0]) + " ? Say 'Yes' or 'No'");
                    }
                    Answering();
                    break;

                case "callback":
                    result = phone.GetLastIncomingCall();
                    if (result[1] != "")
                    {
                        Tts("Do you want to call back the last incoming contact " + result[1] + " ? Say 'Yes' or 'No' ");
                    }
                    else
                    {
                        Tts("Do you want to dial the last incoming number " + phone.SayAsTelephone(result[0]) + " ? Say 'Yes' or 'No'");
                    }
                    Answering();
                    break;

                case "incoming_call":

                    Tts("You have a call from John Doe. Do you want to answer. Say 'Yes' or 'No'");
                    Answering();
                    break;

                case "incoming_sms":
                    //retrieve name and number
                    Tts("Do you want to call ");
                    Answering();
                    break;

            }


        }

        private void DialogueControl()
        {

            switch (state)
            {
                case 0: //we are waiting for an input :: idle state
                    StopDialogue();
                    response = "";
                    break;

                case 1: // Input handling
                    FillingSlots();
                    break;
                case 11: // handle phone number
                    if (phone.CheckSANumber(nb) == "ok")
                    {
                        // Tts("You have entered a SA valid number  :)");
                        nbvalid = nb;
                        state = 3; // in case of a call
                    }
                    else
                    {
                        Tts("Sorry, your number " + phone.SayAsTelephone(nb) + " is " + phone.CheckSANumber(nb) + ". Cancelling the command...");
                        state = 0; // StopDialogue();
                    }
                    break;

                case 12: // Browse contact

                    // state = 4;

                    break;

                case 13: // collect the name for the contact
                    if (!frame.isFilled())
                    {

                    }
                    else
                    {
                        state = 3;
                    }
                    break;
                case 15:

                    break;
                case 14:
                    if (nb != "")
                        state = 11;
                    break;
                case 2: //check frame

                    break;
                case 21: //filled?

                    break;
                case 22: //phone number ?

                    break;
                case 23: //db query

                    break;
                case 24: //clarification

                    break;
                case 25: //selection options

                    if (selection != "")
                    {
                        switch (selection.ToLower())
                        {
                            case "one":
                            case "first":
                                // select the options corresponding to the slot we are filling
                                msg = message[0];
                                break;
                            case "two":
                                msg = message[1];
                                break;
                            case "three":
                                msg = message[2];
                                break;
                        }
                        state = 4;
                        selection = "";
                    }
                    break;
                case 26:
                    //initialise phone number
                    Tts("Please, dictate the telephone number and press call");
                    //load grammar
                    LoadGrammar("telephone.grxml");
                    state = 14;
                    break;
                case 3: //grounding
                    if (frame != null && frame.name != "time")
                        Grounding();// RunFrame(); 
                    else
                        state = 4;
                    break;
                case 31:

                    switch (response.ToLower())
                    {
                        case "yes":
                            state = 4;
                            response = "";
                            break;
                        case "no":
                            Tts("Cancelling the " + frame.name + " request.");
                            state = 0;
                            StopDialogue();
                            LoadGrammar("calltext.grxml");
                            break;
                    }
                    break;
                case 4:
                    RunFrame();
                    break;
            }
        }//dialoguecontrol

        private void RunFrame()
        {
            //Run frame phone.Run(frame)
            //tmrTimeFrame.Enabled = false;

            string nb; string sms = msg;
            switch (frame.name)
            {
                case "pairing":
                    // move it to the grounding function

                    Tts("Discovering mobile phones....");
                    Discovering();
                    if (bluetoothDeviceInfo.Length > 0)
                    {
                        Tts("I have found " + bluetoothDeviceInfo.Length + " Mobile phones.");
                        //give the name
                        /*
                        Tts("Which one do you want to connect to MIMI?");

                        // Go and get the answer

                        Tts("Enter the PIN code 1 2 3 4");

                        //Get the PIN code

                        Tts("the Mobile phone has been successfully paired to MIMI");
                        //state: AskPIN, check it and pair
                        */
                    }
                    else
                    {
                        Tts("No mobile phone discovered, make sure that your phone is discoverable");

                    }
                    state = 0;
                    break;

                case "redial":
                case "callback":
                    if (result[1] != "")
                    {
                        Tts("Calling " + result[1]);
                        //Tts("Please, go to the next task"); 
                    }
                    else
                    {
                        Tts("Dialling " + phone.SayAsTelephone(result[0]));
                        //Tts("Please, go to the next task"); 
                    }
                    state = 0;
                    acceptSpeech = false;
                    phone.Call(result[0]);
                    break;

                case "call":
                    // look for number

                    // checkif a number has been given
                    if (nbvalid != "")
                    {
                        Tts("Dialling " + phone.SayAsTelephone(nbvalid));
                        //Tts("Please, go to the next task"); 
                        state = 0;
                        phone.Call(nbvalid);
                        nbvalid = "";
                    }
                    else
                    {
                        nb = phone.RetrieveNumber(frame.slots.value);
                        if (nb != "")
                        {
                            Tts("Calling " + frame.slots.value + " on " + nb);
                            // Tts("Please, go to the next task"); 
                            state = 0;
                            phone.Call(nb);
                            nb = "";
                            StopDialogue();

                        }
                        else
                        {
                            Tts("Sorry, " + frame.slots.value + " number has not been found");
                            StopDialogue();

                        }
                        acceptSpeech = false;
                    }
                    break;

                case "message":
                    // look for number
                    nb = phone.RetrieveNumber(frame.slots.value);
                    if (nbvalid != "" || nb != "")
                    {
                        if (sms == "")
                        {
                            //Selecting the content of the message
                            Tts("Choose between the following options");
                            Tts(message[0]); Tts(message[1]); Tts(message[2]);
                            //Taking answer
                            selection = ""; msg = "";
                            // Do this in the grounding function
                            LoadGrammar("Option.grxml");
                            state = 25;
                        }
                        else
                        {
                            Tts("Sending the text message " + sms);
                            //Tts("Please, go to the next task"); 
                            state = 0;
                            phone.Text(nb, sms);
                            selection = ""; msg = ""; nb = ""; nbvalid = ""; sms = "";
                            acceptSpeech = false;
                        }

                    }
                    else
                    {
                        Tts("name or phone number not found.");
                        state = 0;
                    }
                    break;

                case "time":
                    Tts(phone.SayTime());
                    StopDialogue();
                    break;

                case "repeat":
                    Tts(savedText);
                    // The dialogue context should be saved before that

                    break;

                case "addressbook":

                    if (iContact <= r.Length)
                    {
                        Tts(r[iContact]);
                    }
                    else

                        Tts("End of the contact list");
                    state = 12;
                    break;

                case "play": // Pause, Stop, Play
                    string path = phone.RetrieveSong(frame.slots.value);
                    if (path != "")
                    {
                        Tts("Playing " + frame.slots.value);
                        state = 0;
                        phone.Play(path);
                        musicOn = true;
                    }
                    else
                    {
                        Tts(frame.slots.value + " is not in your song's library");
                    }
                    state = 0;
                    break;
                case "stop":
                    //make sure that the song is playing
                    if (musicOn)
                    {
                        phone.Stop();
                    }
                    else
                    {
                        Tts(" No song is playing");
                    }
                    state = 0;
                    break;
                case "read": // read text message
                    //retrieve the last SMS
                    msg = "call me b4 u get there.";
                    Tts(phone.ProcessSMS(msg));
                    //AT command
                    StopDialogue();
                    break;
                case "incoming_call"://accept call
                    state = 0;
                    phone.Accept(phone.RetrieveNumber("John Doe"));
                    //StopDialogue();
                    break;

            }//switch

            CursorPos();
            if (state != 25) LoadGrammar("CallText.grxml");

        } //RunFrame
        public void Answering()
        {
            state = 31;
            CursorPos();
            LoadGrammar("Answer.grxml");
        }

        public void Tts(string text)
        {
            savedText = text;
            Log("System", text);
            synth.SpeakAsync(text);

        }

        public void TTS(string text)
        {
            PromptBuilder pb = new PromptBuilder();
            pb.AppendText("This is a date");
            pb.AppendBreak();
            pb.AppendTextWithHint("31-12-2007", SayAs.DayMonthYear);
            //PromptBuilder(pb);

        }
        private void Discovering()
        {
            if (!BluetoothRadio.IsSupported)
            {
                //synth.Speak ("No Bluetooth Adapter found!!!");
            }
            else
            {
                if (BluetoothRadio.PrimaryRadio.Mode == RadioMode.PowerOff)
                    BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;

                try
                {
                    BluetoothRadio.PrimaryRadio.Mode = RadioMode.Discoverable;
                    bluetoothClient = new BluetoothClient();
                    Cursor.Current = Cursors.WaitCursor;

                    bluetoothDeviceInfo = bluetoothClient.DiscoverDevices(10);


                    Cursor.Current = Cursors.Default;
                }
                catch (Exception ex)
                {
                    // synth.Speak ("Error: Bluetooth device isn't active (" + ex.Message + ").");
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DeleteExpiredData()
        {

            clsInput temp = InputPool;
            if (InputPool != null)
            {
                while (temp.next != null)
                {
                    t0 = temp.next.GetTime();
                    t1 = DateTime.Now;
                    diff = t1.Subtract(t0);
                    if (diff.TotalSeconds > 90) // 1.5 minutes                    
                    {
                        temp = temp.next.next;
                    }
                    else
                        temp = temp.next;
                }
                temp = null;
                // ShowInput();
            }
        }




        private void tmrTimeFrame_Tick(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            DeleteExpiredData();
        }
        private void LoadGrammar(string file)
        {
            Grammar gram = new Grammar(file);
            sp.UnloadAllGrammars();
            sp.LoadGrammar(gram);

        }


        // Grammar *******************************************************************
        public void SetGrammar(string[] words)
        {

        }


        void RemoveInput(clsInput val)
        {
            clsInput temp = new clsInput();
            temp = InputPool;
            if (temp.next == null) temp = null;
            else

                while (temp.next != null)
                {
                    string info = temp.GetInfo(); DateTime time = temp.GetTime();
                    if ((info == val.GetInfo()) && (time == val.GetTime()))
                    {
                        temp = temp.next.next;
                        break;
                    }
                    else
                        temp = temp.next;
                }
        }

        void ClearingDigits()
        {
            clsInput temp = new clsInput();
            temp = InputPool;
            if (temp.next == null) temp = null;
            else
                while (temp.next != null)
                {
                    string info = temp.GetPartOfSpeech();
                    //DateTime time = temp.GetTime();
                    if (info == "digit")
                    {
                        temp = temp.next.next;
                        break;
                    }
                    else
                        temp = temp.next;
                }
        }

        public void Ringing()
        {
            phone.Play("Media\\Nokia_Tune.mid");

        }


        ~frmCar()
        {


        }

        private void timer1_Tick(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Ringing();
            clsInput inp = new clsInput("incoming_call", "action", "Event", DateTime.Now, 1);
            AddInput(inp);
            Record("<Event> Incoming call");
            Fusion();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            phone.Play("Media\\nokiasms.mp3");
            clsInput inp = new clsInput("incoming_sms", "action", "Event", DateTime.Now, 1);
            AddInput(inp);
            Record("<Event> Incoming text message");
            Fusion();
        }


        private void CaptureTCP(object sender, EventArgs e)
        {
            //Receive Steerin wheel events

            if (clientSteering.Connected)
            {
                NetworkStream net = clientSteering.GetStream();

                msg = new BinaryReader(net).ReadString();

                switch (msg)
                {
                    case "<CALL>":
                        btnCall_Click(sender, e);
                        break;

                    case "<SMS>":
                        btnSend_Click(sender, e);
                        break;

                    case "<READ>":
                        btnRead_Click(sender, e);
                        break;

                    case "<PTT>":
                        //accept speech
                        acceptSpeech = true;
                        break;

                    case "<PTT RELEASED>":
                        acceptSpeech = false;
                        break;
                }
                //
                string old = msg;
            }
        }

        private void tmrDialogueMonitor_Tick(object sender, EventArgs e)
        {
            DialogueControl();
            CaptureTCP(sender, e);

        }

        // List of similar names

        //List of phones
        public string PadString(string utility, int num)
        {
            try
            {
                //string to hold our spaces
                string spaces = "";
                //loop the number of times the user requests
                for (int i = 1; i <= num; i++)
                {
                    //concat spaces to our string
                    spaces += " ";
                }
                //return the encoded spaces
                return spaces + utility;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ScrollText()
        {
            if (intPosition > 50)
                intPosition = 0;
            this.Text = strScrollText.Substring(intPosition);
            intPosition = intPosition + 1;
        }

        private void LoadTask()
        {
            StreamReader sr = new StreamReader("task.txt");
            string task = sr.ReadLine();
            sr.Close();

            //Load  task 1
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "select * from task where id = " + task;
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)
                {
                    lblStatus.Text = reader.GetString(1).ToString().Substring(4) + " / " + total;
                    txtTask.Text = reader.GetString(2);
                }
                else
                {
                    //reset the task file
                    StreamWriter sw = new StreamWriter("task.txt");
                    sw.WriteLine("1");
                    sw.Close();
                    MessageBox.Show("End of the user testing \n Thank you for your participation!!");
                }
                reader.Close();
                conn.Close();

            }
            catch (OleDbException ex)
            {
                MessageBox.Show(ex.Message);
            }
            CursorNextTask();
        }


        private void timer2_Tick(object sender, EventArgs e)
        {
            DeleteExpiredData();
        }

        private void tmrSilence_Tick(object sender, EventArgs e)
        {
            //when a silence is detected try to see if the input pool has a number
            clsInput temp = new clsInput();
            temp = InputPool;
            //check if the last number is a digit

            if (frame != null && !frame.isFilled())
            {
                if (frame.name == "call" || frame.name == "add" || frame.name == "message")
                {

                    //parse input pool from the end and

                    nb = "";

                    while (temp != null)
                    {
                        clsSlot sl = new clsSlot();
                        sl = frame.slots;
                        if ("digit" == temp.GetPartOfSpeech())
                        {
                            nb += phone.GetDigit(temp.GetInfo());
                        }

                        temp = temp.next;
                    }
                    temp = null;

                    // check the number a perform the call
                    state = 11;

                    //this.Enabled = false;
                }

                if (frame.name == "add")
                {
                    if (!frame.isFilled())
                    {
                        if (frame.slots.next.value == "")
                        {
                            Tts("Please enter the phone number of your new contact");
                            //ASRGrammar("digits");
                            state = 13;
                            //check number
                        }
                        else
                        {
                            if (frame.slots.value == "")
                            {
                                Tts("Please enter the name of your new contact");

                            }
                        }
                    }
                    else
                    {
                        state = 3;
                    }
                }
            }
        }

        /*
         * Any time a digit is detected, treat it apart (method)
         * enable the timer, 
         * concatenate with the number till it react 10 digit/ make a beep
         * Cancel/Restart => reinitialise the number
         */

        public void GetNumber()
        {
            //parse input pool from the end and
            clsInput temp = new clsInput();
            temp = InputPool;

            nb = "";

            while (temp != null)
            {
                clsSlot sl = new clsSlot();
                sl = frame.slots;
                if ("digit" == temp.GetPartOfSpeech())
                {
                    nb += phone.GetDigit(temp.GetInfo());
                }

                temp = temp.next;
            }
            temp = null;

            ClearingDigits(); //remove all digit to avoid issues

            // check the number a perform the call
            state = 11;
        }


        private void btnPTT_KeyDown(object sender, KeyEventArgs e)
        {

            acceptSpeech = true;
            //barge in if the system is speeking and the user  speaks

        }

        private void btnPTT_KeyUp(object sender, KeyEventArgs e)
        {
            acceptSpeech = false;

        }

        private void btnPTT_MouseDown(object sender, MouseEventArgs e)
        {
            acceptSpeech = true;

        }

        private void btnPTT_MouseUp(object sender, MouseEventArgs e)
        {
            acceptSpeech = false;
        }

        private void tmrFocus_Tick(object sender, EventArgs e)
        {
            btnPTT.Focus();
        }


        private void btnStop_Click(object sender, EventArgs e)
        {
            FiniliseTask();
            CursorPos();
        }

        private void FiniliseTask()
        {
            //increment task
            StreamReader sr = new StreamReader("task.txt");
            int i = Convert.ToInt32(sr.ReadLine());
            sr.Close();

            if (i < Convert.ToInt32(total)) // 12 is the number of task available
            {
                i++;
                StreamWriter sw = new StreamWriter("task.txt");
                sw.WriteLine(i.ToString());
                sw.Close();

                LoadTask();
            }
            else
            {
                MessageBox.Show("End of the user testing \n Thank you for your participation!! \n Please fill the questionnaire", "MIMI");
                Tts("Thank you for your participation. Please, fill in the questionnaire.");
                i = 1;
                //updating the current task
                StreamWriter sw = new StreamWriter("task.txt");
                sw.WriteLine(i.ToString());
                sw.Close();
                this.Hide();

                foreach (Form frm in Application.OpenForms)
                {
                    if (frm.Name == "frmEvaluation")
                    {
                        frm.Activate();
                        frm.Show();
                    }
                }
            }

        }

        private void btnPTT_KeyPress(object sender, KeyPressEventArgs e)
        {

            //HOW TO INVOKE THE EVENTS: techrepublic.com

            switch (e.KeyChar.ToString())
            {
                case "s":
                case "t": //sms
                    Beep();
                    btnSend_Click(sender, e);
                    break;
                case "p": //pairing
                    Beep();
                    //btnPairing_Click(sender, e);
                    break;
                case "c":
                case "d"://call
                    Beep();
                    btnCall_Click(sender, e);
                    break;
                case "r": //call
                    Beep();
                    btnRead_Click(sender, e);
                    break;
            }
        }
        private void Beep()
        {
            phone.Play("Media\\beep-8.mp3");
        }

        private static string[] InitializeGrammar()
        {
            //vocabulary constrained to the evaluation

            string[] words = new string[] { 
                 
                 "Patrick", "Janet", "Felix", "Michelle", "Christiaan", "Pat",
                 "Call", "Call number", "Call contact",
                 /*"Call Oh","Dial Oh", "Call zero", "Dial zero", */

                 "Call Patrick","Call Janet","Call Christiaan","Call Sanele",
                 "Call Felix","Call Peter", "Call Michelle", "Call Andre",
                 "Call Charmain", "Call Fabrice","Call Hyacinthe", "Call Meredith",
                 "Call Simone",  "Dial Simone","Dial Sanele",
                 
                 "Phone Patrick", "Phone Janet", "Phone Dieter", "Dial Patrick", "Dial Janet", "Dial Dieter", "Phone Jean",
                 "Phone Andre", "Dial Felix",

                 "Phone", "Dial","Redial", "Callback","Call_last_incoming","Call_last_outgoing",
                 
                 "Send message to Patrick",
                 "Send message to Peter","Send message to Janet", "Send message to dieter", "Send message to Andre",
                 "Sms Andre", "Sms Patrick", "Sms Janet","Sms dieter","Sms Sanele",

                 "Devices", "Mobile phone","Pair new phone","Read new message","Read text message",
                                 
                 "Send", "Time","What time is it?",
                 
                 "Repeat","Sms",
                 
                 "Restart","Cancel","Reset",
                 
                 "Yes", "No", "Yeah","Nope",
                 
                 "Zero", "O", "Oh",//"naught",
                 "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine",/* "Ten", 
                 "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen","Nineteen", 
                 "Twenty", "Twenty-one", "Twenty-two", "Twenty-three", "Twenty-four", "Twenty-five", "Twenty-six", "Twenty-seven", "Twenty-eight", "Twenty-nine", 
                 "Thirty", "Thirty-one", "Thirty-two", "Thirty-three", "Thirty-four", "Thirty-five", "Thirty-six", "Thirty-seven", "Thirty-eight", "Thirty-nine",
                 "Forty", "Forty-one", "Forty-two", "Forty-three", "Forty-four", "Forty-five", "Forty-six", "Forty-seven", "Forty-eight", "Forty-nine",
                 "Fifty", "Fifty-one", "Fifty-two", "Fifty-three", "Fifty-four", "Fifty-five", "Fifty-six", "Fifty-seven", "Fifty-eight", "Fifty-nine",
                 "Sixty", "Sixty-one", "Sixty-two", "Sixty-three", "Sixty-four", "Sixty-five", "Sixty-six", "Sixty-seven", "Sixty-eight", "Sixty-nine",
                 "Seventy","Seventy-one", "Seventy-two", "Seventy-three", "Seventy-four", "Seventy-five", "Seventy-six", "Seventy-seven", "Seventy-eight", "Seventy-nine",
                 "Eighty","Eighty-one", "Eighty-two", "Eighty-three", "Eighty-four", "Eighty-five", "Eighty-six", "Eighty-seven", "Eighty-eight", "Eighty-nine",
                 "Ninety","Ninety-one", "Ninety-two", "Ninety-three", "Ninety-four", "Ninety-five", "Ninety-six", "Ninety-seven", "Ninety-eight", "Ninety-nine",*/
                 "double o", "double one", "double two", "double three", "double four", "double five", "double six", "double seven", "double eight", "double nine", 
                 "triple o","triple one", "triple two", "triple three", "triple four", "triple five", "triple six", "triple seven", "triple eight", "triple nine", 
                 
                 };

            //GenerateGrammar();

            return words;
        }
        // Grammar *******************************************************************

        private void Form1_Activated(object sender, EventArgs e)
        {
            LoadTask();
            CursorPos();
        }

        private void btnCallIn_Click(object sender, EventArgs e)
        {
            /* generate new call
             * Randomly select a contact from the phonebook
             * Ring a number of time 4  
             */

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            /*if (checkBox1.Checked)
            {
                //Steering wheel
                listSteering = new TcpListener(IPAddress.Any, 5000);
                listSteering.Start();

                clientSteering = listSteering.AcceptTcpClient();

                //Reading LCT data
                clientLCT = new TcpClient();
                clientLCT.Connect("127.0.0.1", 4955);

                Thread job = new Thread(new ThreadStart(read));
                job.Start();
            }*/
        }

        private void read()
        {

            while (true)
            {
                NetworkStream net = clientLCT.GetStream();
                string msg = new BinaryReader(net).ReadString();
                //Split the data - check them first
            }
        }

        private void btnDistraction_Click(object sender, EventArgs e)
        {
            Beep();
            //Record the current distraction level
            state = 12;
            //display form to type level

            //capture all other variable: time, speed, angle

            StreamWriter SW;
            try
            {
                SW = File.AppendText("trainingset.csv");
                SW.WriteLine(DateTime.Now.ToLongTimeString() + ", " + "" + ", " + "");//does not give seconds
                SW.Close();
            }
            catch (IOException ex)
            {
                MessageBox.Show(ex.Message);
            }
            //DateTime.Now.

        }

        private void ReadLCT()
        {
            while (true)
            {
                NetworkStream stream = clientLCT.GetStream();
                int i;
                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                }

            }
        }

        private void ReadSteering()
        {
            while (true)
            {

            }
        }

        private void tmrNumber_Tick(object sender, EventArgs e)
        {

        }

  


    }//project
}//namespace
