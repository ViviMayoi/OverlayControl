using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace OverlayControl
{
    /// <summary>
    /// Singleton class managing the application's user settings.
    /// </summary>
    public sealed class Settings
    {
        public static Settings Instance { get { return _lazy.Value; } }

        #region Private variables
        private static readonly Lazy<Settings> _lazy =
            new Lazy<Settings>(() => new Settings());

        private XmlDocument _file = new XmlDocument();
        private string _fileLocation;

        private bool _resetScores = true;
        private bool _catchScoresUp = true;
        #endregion

        #region Properties
        public bool ResetScores
        {
            get => _resetScores;
            set
            {
                _file.SelectSingleNode("/Settings/ResetScores").InnerText = value.ToString();
                _file.Save(_fileLocation);
                _resetScores = value;
            }
        }

        public bool CatchScoresUp
        {
            get => _catchScoresUp;
            set
            {
                _file.SelectSingleNode("/Settings/CatchScoresUp").InnerText = value.ToString();
                _file.Save(_fileLocation);
                _catchScoresUp = value;
            }
        }

        /// <summary>
        /// Boolean representing whether the settings have been fully loaded into the application. Only to be set to true within the main window.
        /// </summary>
        public bool AreLoaded { get; set; }
        #endregion

        /// <summary>
        /// Private constructor for the Settings singleton class.
        /// </summary>
        private Settings()
        {
            AreLoaded = false;
        }

        #region Methods
        /// <summary>
        /// Loads the application's settings into memory by reading them from the XML configuration file.
        /// </summary>
        /// <param name="filename">The configuration file's location.</param>
        /// <returns>Whether every setting was loaded properly.</returns>
        public bool Load(string filename)
        {
            try
            {
                bool success = true;

                // Load the file into memory
                _file.Load(filename);
                _fileLocation = filename;

                // Load each setting from the file
                if (!bool.TryParse(_file.SelectSingleNode("/Settings/ResetScores").InnerText, out _resetScores))
                    success = false;

                if (bool.TryParse(_file.SelectSingleNode("/Settings/CatchScoresUp").InnerText, out _catchScoresUp))
                    success = false;

                return success;
            }
            catch
            {
                // There was an error; return false
                return false;
            }
        }

        /// <summary>
        /// Saves the application's settings into a file named config.txt.
        /// </summary>
        /// <returns>Whether the operation was successful or not.</returns>
        public bool Save()
        {
            // Check if file is loaded first
            if (AreLoaded)
                try
                {
                    _file.Save(_fileLocation);
                    return true;
                }
                catch
                {
                    return false;
                }

            // If no file is loaded, return false
            return false;
        }
        #endregion
    }
}
