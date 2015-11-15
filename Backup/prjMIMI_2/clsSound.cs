using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
        
namespace prjMIMI_2
{
   
    class clsSound
    {

        [DllImport("winmm.dll")]

        private static extern long mciSendString(string strCommand, StringBuilder strReturn, int iReturnLength, IntPtr hwndCallback);

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

        }
    }
}
