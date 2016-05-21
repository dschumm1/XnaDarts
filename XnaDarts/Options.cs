using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts
{
    [Serializable]
    public class Options
    {
        public const string TermChar = "\n";
        public const string OptionsFilename = "options.bin";

        public int BaudRate = 9600;
        // Serial Port Settings
        public int ComPort = 3;
        public bool Debug = false;

        public bool FullScreen = true;


        public bool PlayAwards = true;
        public int PlayerChangeTimeout = 8;


        public int ResolutionIndex;

        public Resolution[] Resolutions;

        /// <summary>
        ///     SegmentMap holds which dartboard coordinates that correspond to which segment.
        ///     The dartboard coordinates may change depending on how you configure the cables running
        ///     from the dartboard matrix to the circuit.
        ///     The key pair contains the segment, multiplier and the value pair contains the x, y coordinates which the key is
        ///     mapped to.
        ///     If for example the single 20 segment is pressed on the dartboard, and the coordinates 4, 18 are sent, the segment
        ///     map should
        ///     contain a key pair (20, 1) with value (4, 18).
        /// </summary>
        public Dictionary<IntPair, IntPair> SegmentMap = new Dictionary<IntPair, IntPair>();


        // Theme Settings
        public string Theme = "Dark";
        public float Volume = 0.05f;

        private Options()
        {
            _getResolutions();
        }

        private void _getResolutions()
        {
            var numberOfSupportedDisplayModes = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.Count();

            Resolutions = new Resolution[numberOfSupportedDisplayModes];

            for (var i = 0; i < numberOfSupportedDisplayModes; i++)
            {
                var mode = GraphicsAdapter.DefaultAdapter.SupportedDisplayModes.ElementAt(i);

                Resolutions[i] = new Resolution(mode.Width, mode.Height);
            }

            // default to max resolution
            ResolutionIndex = numberOfSupportedDisplayModes - 1;
        }

        public static Options Load()
        {
            try
            {
                if (File.Exists(OptionsFilename))
                {
                    var info = new FileInfo(OptionsFilename);
                    if (info.Length > 0)
                    {
                        System.Diagnostics.Debug.WriteLine("Options loaded: " + OptionsFilename);
                        var binaryFormatter = new BinaryFormatter();
                        var fileStream = new FileStream(OptionsFilename, FileMode.Open);
                        var loadedOptions = (Options) binaryFormatter.Deserialize(fileStream);
                        fileStream.Close();

                        return loadedOptions;
                    }
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Error loading options file: " + e.Message);
            }

            System.Diagnostics.Debug.WriteLine("Options file not found, using default");

            return new Options();
        }

        public bool Save()
        {
            try
            {
                var bf = new BinaryFormatter();
                var fs = new FileStream(OptionsFilename, FileMode.Create);
                bf.Serialize(fs, this);
                fs.Close();
                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("Could not save options: " + e.Message);
                return false;
            }
        }
    }
}