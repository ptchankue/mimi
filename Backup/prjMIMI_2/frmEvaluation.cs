using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Data.OleDb;
using System.IO;

namespace prjMIMI_2
{
    public partial class frmEvaluation : Form
    {
        //Keep track of tasks

        //current participant
        public string participant;
        //current task
        public int iTask = 1;
        string task;
        public string iLastTask = "13";

        OleDbDataReader reader;

        public void Initialise()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql = "SELECT * FROM task ORDER BY id";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                reader = cmd.ExecuteReader();
                reader.Read();
                
                lblTask.Text = reader.GetString(1).ToString();
                txtTask.Text = reader.GetString(2).ToString();
                
                reader.Close();
                conn.Close();
            }
            catch 
            {
                //MessageBox.Show(e.Message);
            }
        }

        public void Log(string who, string what)
        {
            StreamWriter SW;

            SW = File.AppendText("log.csv");
            SW.WriteLine();
            SW.WriteLine(DateTime.Now.ToString() + ", " + who + ", " + what);
            SW.Close();
        }

        public frmEvaluation()
        {
            InitializeComponent();

            //Initialise();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please! Choose your participant code");
            }
            else
            {
                //Log(participant, task)
                Log(comboBox1.Text, lblTask.Text);

                //Application.OpenForms from Stackoverflow.com
                foreach (Form f in Application.OpenForms)
                {
                    if (f.Name == "frmCar")
                    {
                        f.Show();
                        return;
                    }
                }
                frmCar   mimi = new frmCar();
                mimi.Show();
               
                this.Hide();
            }
        }

        private void frmEvaluation_Load(object sender, EventArgs e)
        {
            frmSplash Splash = new frmSplash();
            Splash.Show();

            LoadTask();
            //MessageBox.Show(iTask.ToString ());
            
        }

        private void LoadTask()
        {
            StreamReader sr = new StreamReader("task.txt");
            task = sr.ReadLine();
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
                    lblTask.Text = reader.GetString(1).ToString();
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
            catch 
            {

            }
        }

        
        private void frmEvaluation_Activated(object sender, EventArgs e)
        {
            LoadTask();
        }

        private void frmEvaluation_FormClosing(object sender, FormClosingEventArgs e)
        {
            // go back to the first task
            StreamWriter sw = new StreamWriter("task.txt");
            sw.WriteLine("1");
            sw.Close();
        }

        

       
    }
}
