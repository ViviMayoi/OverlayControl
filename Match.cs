using System.Collections.Generic;

namespace OverlayControl
{
    /// <summary>
    /// Class representing a set being observed through meltyhook.
    /// </summary>
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

        /// <summary>
        /// Constructor for the Match class, representing a set being observed through meltyhook.
        /// </summary>
        /// <param name="p1">Player 1's tag.</param>
        /// <param name="p2">Player 2's tag.</param>
        /// <param name="c1">Player 1's chosen character and moon style.</param>
        /// <param name="c2">Player 2's chosen character and moon style.</param>
        /// <param name="r">Tournament round where the set is taking place.</param>
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
        /// <summary>
        /// Checks if two given players are the same players as the ones recorded in this match.
        /// </summary>
        /// <param name="p1">Player 1's full tag (with sponsor tag).</param>
        /// <param name="p2">Player 2's full tag (with sponsor tag).</param>
        /// <returns>Whether or not the match contains different players.</returns>
        public bool IsNewMatch(string p1, string p2)
        {
            if (p1 == Player1 && p2 == Player2)
                return false;
            return !(p2 == Player1) || !(p1 == Player2);
        }

        /// <summary>
        /// Checks if two given players have changed the side they are playing on during the set.
        /// </summary>
        /// <param name="p1">Player 1's full tag (with sponsor tag).</param>
        /// <param name="p2">Player 2's full tag (with sponsor tag).</param>
        /// <returns>Whether or not the players have changed sides.</returns>
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

        /// <summary>
        /// Formats a Match object into a string containing the match's round name, both players' full tags (with sponsor tags), and their character and moon style choice(s).
        /// </summary>
        /// <returns>The formatted string.</returns>
        public override string ToString()
        {
            // Construct a string, starting with round and first player
            string str1 = Round + " - " + Player1;

            // Check P1's character count
            if (Characters1.Count == 1)
                // Close parenthesis immediately if only one character was played
                str1 = str1 + " (" + Characters1[0] + ")";
            else if (Characters1.Count != 0)
            {
                // If multiple characters were played, format appropriately
                string str2 = str1 + " (";
                for (int index = 0; index < Characters1.Count; ++index)
                {
                    str2 += Characters1[index];
                    if (index != Characters1.Count - 1)
                        str2 += ", ";
                }
                str1 = str2 + ")";
            }

            // Add the second player
            string str3 = str1 + " vs " + Player2;

            // Check P2's character count
            if (Characters2.Count == 1)
                // Close parenthesis immediately if only one character was played
                str3 = str3 + " (" + Characters2[0] + ")";
            else if (Characters2.Count != 0)
            {
                // If multiple characters were played, format appropriately
                string str2 = str3 + " (";
                for (int index = 0; index < Characters2.Count; ++index)
                {
                    str2 += Characters2[index];
                    if (index != Characters2.Count - 1)
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
