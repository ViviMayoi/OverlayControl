using System.Collections.Generic;

namespace OverlayControl
{
    public class Match
    {
        #region Properties
        public string Player1 { get; set; }
        public string Player2 { get; set; }
        public List<string> Characters1 { get; set; }
        public List<string> Characters2 { get; set; }
        public string Round { get; set; }
        public System.DateTime StartTime { get; set; }
        #endregion

        // Constructor
        public Match(string p1, string p2, string c1, string c2, string r)
        {
            Player1 = p1;
            Player2 = p2;
            Characters1 = new List<string>() { c1 };
            Characters2 = new List<string>() { c2 };
            Round = r;
            StartTime = System.DateTime.Now;
        }

        #region Public methods
        // Check if given players are different to this match
        public bool IsNewMatch(string p1, string p2)
        {
            if (p1 == this.Player1 && p2 == this.Player2)
                return false;
            return !(p2 == this.Player1) || !(p1 == this.Player2);
        }

        // Check if submitted players are in a different position compared to this match
        public bool? IsReversed(string p1, string p2)
        {
            if (p1 == Player1 && p2 == Player2)
                return false;
            else if (p2 == Player1 && p1 == Player2)
                return true;

            // If they are not the same players, return null
            else
                return null;
        }

        // Used for timestamps
        public override string ToString()
        {
            // Construct a string, starting with round and first player
            string str1 = this.Round + " - " + this.Player1;

            // Check P1's character count
            if (this.Characters1.Count == 1)
                // Close parenthesis immediately if only one character was played
                str1 = str1 + " (" + this.Characters1[0] + ")";
            else if (this.Characters1.Count != 0)
            {
                // If multiple characters were played, format appropriately
                string str2 = str1 + " (";
                for (int index = 0; index < this.Characters1.Count; ++index)
                {
                    str2 += this.Characters1[index];
                    if (index != this.Characters1.Count - 1)
                        str2 += ", ";
                }
                str1 = str2 + ")";
            }

            // Add the second player
            string str3 = str1 + " vs " + this.Player2;

            // Check P2's character count
            if (this.Characters2.Count == 1)
                // Close parenthesis immediately if only one character was played
                str3 = str3 + " (" + this.Characters2[0] + ")";
            else if (this.Characters2.Count != 0)
            {
                // If multiple characters were played, format appropriately
                string str2 = str3 + " (";
                for (int index = 0; index < this.Characters2.Count; ++index)
                {
                    str2 += this.Characters2[index];
                    if (index != this.Characters2.Count - 1)
                        str2 += ", ";
                }
                str3 = str2 + ")";
            }

            // Return finalized string
            return str3;
        }
        #endregion
    }
}
