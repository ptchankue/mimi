using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using System.Data.OleDb;
using System.Runtime.InteropServices;

namespace prjMIMI_2
{
    class clsPhone
    {
        [DllImport("winmm.dll")]
        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

        public void Call(string number)
        {
            // AT commands
            //MessageBox.Show("Opening virtual serial port (bluetooth)\nATD" + number);

            #region Updating the call list
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql = "INSERT INTO Call_List (type, phone_number, call_date) VALUES ('O', '" + number + "', #" + DateTime.Now + "#)";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch 
            {
                //MessageBox.Show(e.Message);
            }

            #endregion


            // update dialogue history

        }

        public void Accept(string number)
        {
            // AT commands
            MessageBox.Show("Opening virtual serial port (bluetooth)\nATA" + number);

            #region Updating the call list
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);

            string sql = "INSERT INTO Call_List (type, phone_number, call_date) VALUES ('I', '" + number + "', #" + DateTime.Now + "#)";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch 
            {
               // MessageBox.Show(e.Message);
            }

            #endregion


            // update dialogue history

        }

        public void MissedCall(string number)
        {

        }
        public string SayTime()
        {
            DateTime time = DateTime.Now;

            int h = (time.Hour > 12 ? time.Hour - 12 : time.Hour);

            string min = (time.Minute == 0 ? "O Clock " : time.Minute.ToString());

            return "the time is " + h.ToString() + "  " + min;

        }

        public void Text(string number, string sms)
        {
            // AT commands
           // MessageBox.Show("Opening virtual serial port (bluetooth)\nAT+CMGF=1\nAT+CMGS=" + number + "\n " + sms);
        }

        public void Play(string track)
        {
            string _command = "Stop MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);

            _command = "Close MediaFile";
            mciSendString(_command, null, 0, IntPtr.Zero);

            _command = "open \"" + track + "\" type mpegvideo alias MediaFile";

            mciSendString(_command, null, 0, IntPtr.Zero);

            _command = "play MediaFile";

            // _command = "play MediaFile repeat";

            mciSendString(_command, null, 0, IntPtr.Zero);
            /*
            
            
                         //System.Media;
                        SoundPlayer player = new SoundPlayer();
                        player.SoundLocation = track;
                        player.Play();*/

        }

        public void Stop()
        {
            string Command = "close Mp3File";
            mciSendString(Command, null, 0, IntPtr.Zero);

        }

        public void Pause()
        {
            string Command = "pause Mp3File";
            mciSendString(Command, null, 0, IntPtr.Zero);
        }

        public void Time(string number)
        {
            // AT commands
            MessageBox.Show("Time");
        }

        public string[] FindContact()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string[] ret = { };
            //string sql = "SELECT phone_number FROM contact WHERE contact_name LIKE '*" + name + "*'";
            string sql = "SELECT contact_name FROM contact ORDER BY contact_name";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();

                // check the end of the list

                // be able to check by keywords and starting by a specific letter :: adjust criteria

                //create an array and return it
                while (reader.Read())
                {
                    string[] tmp = new string[ret.Length + 1];
                    ret.CopyTo(tmp, 0);
                    tmp.SetValue(reader.GetString(0), ret.Length);
                    ret = tmp;

                }

                reader.Close();
                conn.Close();
            }
            catch 
            {
                //MessageBox.Show(e.Message);
                return ret;
            }
            return ret;
        }
        public string CheckSANumber(string nb)
        {
            // 10 digits starting by 0 or 0027 or +27
            string result = " is not valid ";

            if ((nb.StartsWith("0027") && nb.Length == 13))
            {
                result = "ok";
            }
            else
                if (nb.StartsWith("0") && nb.Length == 10)
                    result = "ok";
                else
                {
                    if (nb.Length < 10)
                        result = " too short ";
                    if (nb.Length > 13)
                        result = " too long ";
                }
            return result;
        }

        public void Saving(string name, string number)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "INSERT INTO Call_List (phone_number, contact_name, date) VALUES ('" + number + "', '" + name + "', #" + DateTime.Now + "#)";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                cmd.ExecuteNonQuery();

                conn.Close();
            }
            catch
            {

            }

        }

        public string GetLastOutgoingContact()
        {
            return "";
        }

        //change the database so that call_list store both the name and the number

        public string[] GetLastOutgoingCall()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "SELECT TOP 1 phone_number FROM call_list WHERE type = 'O' GROUP BY phone_number, call_date ORDER BY call_date DESC";
            string[] ret = new string[] { "", "" }; //number and name
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)//reader.HasRows()
                    ret[0] = reader.GetString(0);

                reader.Close();
                conn.Close();
                ret[1] = RetrieveName(ret[0]);
            }
            catch 
            {
                return ret;
            }
            return ret;

        }
        public string[] GetLastIncomingCall()
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string sql = "SELECT TOP 1 phone_number FROM call_list WHERE type = 'I' GROUP BY phone_number, call_date ORDER BY call_date DESC";
            string[] ret = new string[] { "", "" }; //number and name
            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)//reader.HasRows()
                    ret[0] = reader.GetString(0);

                reader.Close();
                conn.Close();
                ret[1] = RetrieveName(ret[0]);
            }
            catch 
            {
                return ret;
            }
            return ret;

        }
        public string RetrieveName(string number)
        {

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string ret = "";
            //string sql = "SELECT phone_number FROM contact WHERE contact_name LIKE '*" + name + "*'";
            string sql = "SELECT contact_name FROM contact WHERE phone_number = '" + number + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)//reader.HasRows()
                    ret = reader.GetString(0);

                reader.Close();
                conn.Close();
            }
            catch 
            {
                return ret;
            }
            return ret;

        }
        public string RetrieveNumber(string name)
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            string ret = "";
            //string sql = "SELECT phone_number FROM contact WHERE contact_name LIKE '*" + name + "*'";
            string sql = "SELECT phone_number FROM contact WHERE contact_name = '" + name + "'";

            OleDbCommand cmd = new OleDbCommand(sql, conn);

            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)//reader.HasRows()
                    ret = reader.GetString(0);

                reader.Close();
                conn.Close();
            }
            catch 
            {
                return ret;
            }
            return ret;
        }

        public string RetrieveSong(string title)//DBQuery
        {
            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);

            //string sql = "SELECT path FROM media WHERE title LIKE '*" + title  + "*'";
            string sql = "SELECT path FROM media WHERE title = '" + title + "'";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            string ret;
            try
            {
                conn.Open();

                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                if (reader.HasRows)//reader.HasRows()
                    ret = reader.GetString(0);
                else
                    ret = "";
                reader.Close();
                conn.Close();

                return ret;
            }
            catch 
            {
                return "";
            }
        }

        public string SayAsTelephone(string nb)
        {
            string tmp = "";
            for (int i = 0; i < nb.Length; i++)
            {
                tmp += nb.Substring(i, 1) + " ";
            }
            return tmp;
        }

        public string GetDigit(string nb)
        {
            string ret;

            switch (nb.ToLower())
            {
                case "o":case "oh":
                case "zero": case "naught": ret = "0"; break;
                case "one": ret = "1"; break;
                case "two": ret = "2"; break;
                case "three": ret = "3"; break;
                case "four": ret = "4"; break;
                case "five": ret = "5"; break;
                case "six": ret = "6"; break;
                case "seven": ret = "7"; break;
                case "eight": ret = "8"; break;
                case "nine": ret = "9"; break;
                case "ten": ret = "10"; break;
                case "eleven": ret = "11"; break;
                case "twelve": ret = "12"; break;
                case "thirteen": ret = "13"; break;
                case "fourteen": ret = "14"; break;
                case "fifteen": ret = "15"; break;
                case "sixteen": ret = "16"; break;
                case "Seventeen": ret = "17"; break;
                case "eighteen": ret = "18"; break;
                case "nineteen": ret = "19"; break;
                case
                    "twenty": ret = "20"; break;
                case "twenty-one": ret = "21"; break;
                case "twenty-two": ret = "22"; break;
                case "twenty-three": ret = "23"; break;
                case "twenty-four": ret = "24"; break;
                case "twenty-five": ret = "25"; break;
                case "twenty-six": ret = "26"; break;
                case "twenty-seven": ret = "27"; break;
                case "twenty-eight": ret = "28"; break;
                case "twenty-nine": ret = "29"; break;
                case
                    "thirty": ret = "30"; break;
                case "thirty-one": ret = "31"; break;
                case "thirty-two": ret = "32"; break;
                case "thirty-three": ret = "33"; break;
                case "thirty-four": ret = "34"; break;
                case "thirty-five": ret = "35"; break;
                case "thirty-six": ret = "36"; break;
                case "thirty-seven": ret = "37"; break;
                case "thirty-eight": ret = "38"; break;
                case "thirty-nine": ret = "39"; break;
                case
                    "forty": ret = "40"; break;
                case "forty-one": ret = "41"; break;
                case "forty-two": ret = "42"; break;
                case "forty-three": ret = "43"; break;
                case "forty-four": ret = "44"; break;
                case "forty-five": ret = "45"; break;
                case "forty-six": ret = "46"; break;
                case "forty-seven": ret = "47"; break;
                case "forty-eight": ret = "48"; break;
                case "forty-nine": ret = "49"; break;
                case
                    "fifty": ret = "50"; break;
                case "fifty-one": ret = "51"; break;
                case "fifty-two": ret = "52"; break;
                case "fifty-three": ret = "53"; break;
                case "fifty-four": ret = "54"; break;
                case "fifty-five": ret = "55"; break;
                case "fifty-six": ret = "56"; break;
                case "fifty-seven": ret = "57"; break;
                case "fifty-eight": ret = "58"; break;
                case "fifty-nine": ret = "59"; break;
                case
                    "sixty": ret = "60"; break;
                case "sixty-one": ret = "61"; break;
                case "sixty-two": ret = "62"; break;
                case "sixty-three": ret = "63"; break;
                case "sixty-four": ret = "64"; break;
                case "sixty-five": ret = "65"; break;
                case "sixty-six": ret = "66"; break;
                case "sixty-seven": ret = "67"; break;
                case "sixty-eight": ret = "68"; break;
                case "sixty-nine": ret = "69"; break;
                case
                    "seventy": ret = "70"; break;
                case "seventy-one": ret = "71"; break;
                case "seventy-two": ret = "72"; break;
                case "seventy-three": ret = "73"; break;
                case "seventy-four": ret = "74"; break;
                case "seventy-five": ret = "75"; break;
                case "seventy-six": ret = "76"; break;
                case "seventy-seven": ret = "77"; break;
                case "seventy-eight": ret = "78"; break;
                case "seventy-nine": ret = "79"; break;
                case
                    "eighty": ret = "80"; break;
                case "eighty-one": ret = "81"; break;
                case "eighty-two": ret = "82"; break;
                case "eighty-three": ret = "83"; break;
                case "eighty-four": ret = "84"; break;
                case "eighty-five": ret = "85"; break;
                case "eighty-six": ret = "86"; break;
                case "eighty-seven": ret = "87"; break;
                case "eighty-eight": ret = "88"; break;
                case "eighty-nine": ret = "89"; break;
                case
                    "ninety": ret = "90"; break;
                case "ninety-one": ret = "91"; break;
                case "ninety-two": ret = "92"; break;
                case "ninety-three": ret = "93"; break;
                case "ninety-four": ret = "94"; break;
                case "ninety-five": ret = "95"; break;
                case "ninety-six": ret = "96"; break;
                case "ninety-seven": ret = "97"; break;
                case "ninety-eight": ret = "98"; break;
                case "ninety-nine": ret = "99"; break; 
                 
                case "double o": ret = "00"; break;
                case "double one": ret = "11"; break;
                case "double two": ret = "22"; break;
                case "double three": ret = "33"; break;
                case "double four": ret = "44"; break;
                case "double five": ret = "55"; break;
                case "double six": ret = "66"; break;
                case "double seven": ret = "77"; break;
                case "double eight": ret = "88"; break;
                case "double nine": ret = "99"; break;
                case "triple o": ret = "000"; break;
                case "triple one": ret = "111"; break;
                case "triple two": ret = "222"; break;
                case "triple three": ret = "333"; break;
                case "triple four": ret = "444"; break;
                case "triple five": ret = "555"; break;
                case "triple six": ret = "666"; break;
                case "triple seven": ret = "777"; break;
                case "triple eight": ret = "888"; break;
                case "triple nine": ret = "999"; break;

                default: ret = ""; break;
            }

            return ret;
        }
        public string ProcessSMS(string sms)
        {
            sms = sms.ToLower();
            string result = sms;
            //open database

            string connectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=phonebook.accdb";
            OleDbConnection conn = new OleDbConnection(connectionString);
            //string sql = "SELECT phone_number FROM contact WHERE contact_name LIKE '*" + name + "*'";
            string sql = "SELECT * FROM Abbreviations;";
            OleDbCommand cmd = new OleDbCommand(sql, conn);
            try
            {
                conn.Open();
                OleDbDataReader reader = cmd.ExecuteReader();
                reader.Read();
                //parse the database and correct the sms
                while (reader.Read())
                {
                    result = sms.Replace(reader["Abbrev"].ToString(),
                        reader["meaning"].ToString());
                    sms = result;
                }

                //sms.Replace(old, n);

                //return reader.GetString(0);

                reader.Close();
                conn.Close();
            }
            catch 
            {

            }

            return result;
        }
    }//class
}
