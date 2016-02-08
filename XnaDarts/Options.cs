using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.Xna.Framework;

namespace XnaDarts
{
    [Serializable]
    public class Resolution
    {
        public int Height;
        public int Width;

        public Resolution(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return Width + ", " + Height;
        }
    }

    [Serializable]
    public class Options
    {
        public const string TermChar = "\n";
        public const string OptionsFilename = "options.bin";

        public readonly Resolution[] Resolutions =
        {
            new Resolution(1920, 1200),
            new Resolution(1920, 1080),
            new Resolution(1680, 1050),
            new Resolution(1200, 800),
            new Resolution(1024, 768)
        };

        public int BaudRate = 9600;
        // Serial Port Settings
        public int ComPort = 3;
        public bool Debug = false;

        [NonSerialized] public Color DisabledMenuItemForeground = Color.DarkGray;

        public bool FullScreen = false;

        [NonSerialized] public Color MenuItemForeground = Color.White;

        public bool PlayAwards = true;
        public int PlayerChangeTimeout = 8;

        [NonSerialized] public Color[] PlayerColors =
        {
            Color.Red,
            Color.Blue,
            Color.Yellow,
            Color.Lime,
            Color.Teal,
            Color.Gold,
            Color.Orange,
            Color.Firebrick,
            Color.Lime
        };

        public int ResolutionIndex = 0;

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


        [NonSerialized] public Color SelectedMenuItemForeground = Color.Yellow;

        // Theme Settings
        public string Theme = "Dark";
        public float Volume = 0.05f;

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
                        var bf = new BinaryFormatter();
                        var fs = new FileStream(OptionsFilename, FileMode.Open);
                        var temp = (Options) bf.Deserialize(fs);
                        fs.Close();

                        return temp;
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