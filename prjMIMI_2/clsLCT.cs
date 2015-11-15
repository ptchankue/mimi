using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prjMIMI_2
{
    class clsLCT
    {
        public string simSteer, simSpeed, simTime, simXPos, simYPos;
        string strTab = "\t";
        public void extractData(string msg)
        {
            //validate data
            string tmp = msg; int j = 0;
            for (int i = 0; i < msg.Length; i++)
            {
                if (tmp.Substring(i, 1) == "\t")
                {
                    j++;
                }

            }
            if (j == 8)
            {
                int end = msg.IndexOf("\t");

                //Time (msec)
                //find 1st delineator
                int startPtr = 0;
                int endPtr = msg.IndexOf(strTab);
                simTime = msg.Substring(0, endPtr);

                //lateral position (m)
                //find 2nd delineator
                startPtr = endPtr + 1;
                msg = msg.Substring(startPtr);
                endPtr = msg.IndexOf(strTab);
                simXPos = msg.Substring(0, endPtr);

                //longitudinal position (m)
                //find 3rd delineator
                startPtr = endPtr + 1;
                msg = msg.Substring(startPtr);
                endPtr = msg.IndexOf(strTab);
                simYPos = msg.Substring(0, endPtr);

                //speed (km/hr)
                //find 4th delineator
                startPtr = endPtr + 1;
                msg = msg.Substring(startPtr);
                endPtr = msg.IndexOf(strTab);
                simSpeed = msg.Substring(0, endPtr);

                //Steering Wheel Angular Position (grads)
                //find 5th delineator
                startPtr = endPtr + 1;
                msg = msg.Substring(startPtr);
                endPtr = msg.IndexOf(strTab);
                simSteer = msg.Substring(0, endPtr);
            }
        }
    }
}
