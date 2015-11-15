using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace prjMIMI_2
{
    class clsInput
    {

        string modality;       // Speech, Manual, Click, Gesture,...etc
        string info;           // Actual information carried by the variable
        DateTime time;         // Time when the input was captured

        string part_of_speech; // action, name, number, digit,...
        double confidence;     // Confidence of the input (Speech + Steering)

        public clsInput  next;            // Pointer to the next input

        public clsInput()
        {
        }
        public clsInput(string inf, string pos, string mode, DateTime t, double conf)
        {
            //managing synonyms

            switch (inf.ToLower())
            {
                case "call":
                case "phone":
                case "dial":

                    info = "call"; break;

                case "callback":
                case "call_last_incoming":
                    info = "callback";
                    break;

                case "redial":
                case "call_last_outgoing":
                    info = "redial";
                    break;

                case "pair":
                case "pairing":
                case "connect":
                    info = "pairing"; break;

                case "send":
                case "message":
                case "text":
                case "sms":
                case "reply":
                    info = "message"; break;

                case "browse":
                case "find":
                case "lookup":
                case "browsing":
                case "adressbook":
                    info= "addressbook"; break;
                
                case "accept":
                    info = "accept"; break;

                case "read":
                    info = "read"; break;

                case "add":
                case "contact":
                case "save":
                    info = "add"; break;

                case "time":
                    info = "time"; break;
                
                case "repeat":case "say again":
                    info = "repeat"; break;

                case "play":
                    info = "play"; break;

                default:
                    info = inf; break;
            }

            // split recognised phrase
            //info = inf;
            modality = mode;
            part_of_speech = (pos != "" ? pos : SetPartOfSpeech());
            time = t;
            confidence = conf;
        }
        public DateTime GetTime()
        {
            return time;
        }
        public string GetInfo()
        {
            return info;
        }
        public string GetModality()
        {
            return modality;
        }
        public string GetPartOfSpeech()
        {
            return part_of_speech;
        }
        public double GetConfidence()
        {
            return confidence;
        }
        public string SetPartOfSpeech()
        {
            switch (info.ToLower())
            {
                case "call":
                case "redial":
                case "callback":
                case "call_last_incoming":
                case "call_last_outgoing":
                case "pairing":
                case "play":
                case "stop":
                case "pause":
                case "add":
                case "new":
                case "save":
                case "edit":
                case "remove":
                case "modify":
                case "delete":
                case "exit":
                case "close":
                case "time":
                case "say":
                case "message":
                case "read":
                case "type":case "repeat":
                case "incoming_sms":case "incoming_call":
                case "addressbook": case "lookup":case "browse":case "browsing":
                    return "action";

                case "him":
                case "there":
                case "here":
                case "this":
                case "that":
                case "mobile":
                case "to":

                    return "deitic";//anaphora

                case "good night":
                case "bonjour":
                case "hello":
                case "hi":
                case "morning":

                    return "greeting";

                case "hot":
                case "tamtam":
                case "les lionnes":

                    return "title";

                case "zero": case "one": case "two": case "three": case "four":case "five":case "six":
                case "seven": case "eight": case "nine":
                case "o":case "naught":case "oh":
                case "ten":
                case "eleven": case "twelve": case "thirteen": case "fourteen": case "fifteen": case "sixteen": case "seventeen": case "eighteen": case"nineteen": case 
                 "twenty": case "twenty-one": case "twenty-two": case "twenty-three": case "twenty-four": case "twenty-five": case "twenty-six": case "twenty-seven": case "twenty-eight": case "twenty-nine": case 
                 "thirty": case "thirty-one": case "thirty-two": case "thirty-three": case "thirty-four": case "thirty-five": case "thirty-six": case "thirty-seven": case "thirty-eight": case "thirty-nine": case
                 "forty": case "forty-one": case "forty-two": case "forty-three": case "forty-four": case "forty-five": case "forty-six": case "forty-seven": case "forty-eight": case "forty-nine": case
                 "fifty": case "fifty-one": case "fifty-two": case "fifty-three": case "fifty-four": case "fifty-five": case "fifty-six": case "fifty-seven": case "fifty-eight": case "fifty-nine": case
                 "sixty": case "sixty-one": case "sixty-two": case "sixty-three": case "sixty-four": case "sixty-five": case "sixty-six": case "sixty-seven": case "sixty-eight": case "sixty-nine": case
                 "seventy": case"seventy-one": case "seventy-two": case "seventy-three": case "seventy-four": case "seventy-five": case "seventy-six": case "seventy-seven": case "seventy-eight": case "seventy-nine": case
                 "eighty": case"eighty-one": case "eighty-two": case "eighty-three": case "eighty-four": case "eighty-five": case "eighty-six": case "eighty-seven": case "eighty-eight": case "eighty-nine": case
                 "ninety": case"ninety-one": case "ninety-two": case "ninety-three": case "ninety-four": case "ninety-five": case "ninety-six": case "ninety-seven": case "ninety-eight": case "ninety-nine": 
                 
                case "double zero":case "double one": case "double two": case "double three":
                case "double four":case "double five":case "double six":case "double seven":
                case "double eight":case "double nine":case "double o":
                case "triple zero":case "triple one":case "triple two":case "triple three":
                case "triple four":case "triple five":case "triple six":case "triple seven":
                case "triple eight":case "triple nine":case "triple o":
                    return "digit";

                case "ok":
                case "yes":
                case "no":
                case "yeah":
                case "yebo":
                case "nope":
                case "oui":
                case "non":

                    return "yn";

                case "patrick":case "bradley":case "evan":case "emile":case "janet":
                case "felix":case "ryan":case "meredith":case "michelle":case "christiaan":
                case "chris":case "Pat":case "peter":case "toto":case "vuyo":case "simone":
                case "ivan":case "fabrice":case "sanele":case "themba":case "hyacinthe":
                case "reine":case "dumisani":case "victor":case "jean":case "charmain":case "john doe":
                case "andre":case "dieter":case "lester": case "lynette":

                    return "name";

                default: //List all the possible name

                    return "other";
            }
        }

    }
}
