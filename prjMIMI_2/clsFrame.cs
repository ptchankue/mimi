using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prjMIMI_2
{
    class clsFrame
    {
        public string name;
        public clsSlot slots; // should be seen by Dialogue()

        public clsFrame() { }

        public clsFrame(string name_)
        {
            name = name_;
            switch (name)
            {
                case "call":
                    slots = new clsSlot();
                    slots.name = "name";
                    slots.type = "number";
                    slots.value = "";
                    slots.next = null;
                    break;
                case "message":
                case "reply":
                    slots = new clsSlot();
                    slots.name = "name";
                    slots.type = "number";
                    slots.value = "";
                    slots.next = null;
                    break;
                case "pairing":
                    slots = new clsSlot();
                    /*slots.name = "phone";
                    slots.type = "name";
                    slots.value = "";
                    Slot temp = new clsSlot();
                    temp.name = "pin";
                    temp.type= "number";
                    temp.value = "1234"; temp.next = null;
                    slots.next = temp; temp = null;*/
                    break;
                case "time":
                    break;

                case "repeat":
                    break;

                case "stop":
                    break;

                case "pause":
                    break;

                case "accept":
                    break;

                case "read":
                    break;

                case "redial":
                    break;
                
                case "addressbook":
                    break;

                case "callback":
                    break; // no slot, the number will be retrieved by the system

                case "incoming_call":

                    break;

                case "incoming_sms":

                    break;

                case "play":
                    slots = new clsSlot();
                    slots.name = "title";
                    slots.type = "title";
                    slots.value = "";
                    slots.next = null;
                    break;

                case "add":
                    slots = new clsSlot();
                    clsSlot temp = slots;
                    slots.name = "number";
                    slots.type = "number";
                    slots.value = "";

                    clsSlot nom = new clsSlot();
                    nom.name = "name";
                    nom.type = "name";
                    nom.value = "";
                    nom.next = null;
                    slots.next = nom;
                    break;

                case "remove":
                    slots = new clsSlot();
                    slots.name = "title";
                    slots.type = "title";
                    slots.value = "";
                    slots.next = null;
                    break;
            }
        }
        /*public clsFrame(string nom, slotType s)
        {
            name = n; 
            slots = s.Clone ;
        }*/

        public bool isFilled()
        {
            bool ok = true;
            clsSlot sl = new clsSlot();
            sl = this.slots;
            while (sl != null)
            {
                if (sl.value == "") ok = false;

                sl = sl.next;
            }
            return ok;
        }
    }
}
