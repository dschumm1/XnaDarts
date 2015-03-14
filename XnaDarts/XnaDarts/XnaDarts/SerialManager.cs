using System;
using System.IO.Ports;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaDarts.Screens;
using XnaDarts.Screens.Menus;

namespace XnaDarts
{
    [Serializable]
    public class IntPair : IComparable
    {
        public int X;
        public int Y;

        public IntPair(int a, int b)
        {
            X = a;
            Y = b;
        }

        #region IComparable Members

        public int CompareTo(object obj)
        {
            var temp = obj as IntPair;

            if (temp != null)
            {
                if (temp.X == X && temp.Y == Y)
                {
                    return 0;
                }
            }

            return -1;
        }

        public override bool Equals(object obj)
        {
            return CompareTo(obj) == 0;
        }

        public override int GetHashCode()
        {
            return X.GetHashCode();
        }

        public override string ToString()
        {
            return "X: " + X + ", " + "Y: " + Y;
        }

        #endregion
    }

    public class SerialManager
    {
        public delegate void DartHitDelegate(IntPair coords);

        /// <summary>
        ///     TODO: Instead of having a dart trigger an event, handle it through InputHandler which would make it easier manage
        /// </summary>
        /// <param name="segment"></param>
        /// <param name="multiplier"></param>
        public delegate void DartRegisteredDelegate(int segment, int multiplier);

        private static SerialManager _instance;
        private bool[] _buttonStates = {false, false, false, false, false};
        public DartHitDelegate OnDartHit;
        public DartRegisteredDelegate OnDartRegistered;
        private readonly SerialPort _serialPort;

        private SerialManager()
        {
            _serialPort = new SerialPort();
            UpdateSerialPortPropertiesFromOptions();
            _serialPort.DataReceived += SerialPort_DataReceived;
        }

        public bool IsPortOpen
        {
            get { return _serialPort.IsOpen; }
        }

        public bool[] ButtonStates
        {
            get
            {
                var temp = _buttonStates;
                _buttonStates = new[] {false, false, false, false, false};
                // Reset the buttonStates every time the current buttonsStates gets used
                return temp;
            }
        }

        public void UpdateSerialPortPropertiesFromOptions()
        {
            if (IsPortOpen)
            {
                ClosePort();
            }

            _serialPort.BaudRate = XnaDartsGame.Options.BaudRate;
            _serialPort.PortName = "COM" + XnaDartsGame.Options.ComPort;
            _serialPort.NewLine = Options.TermChar;

            OpenPort();
        }

        public static SerialManager Instance()
        {
            if (_instance == null)
            {
                _instance = new SerialManager();
            }

            return _instance;
        }

        public void OpenPort()
        {
            if (_serialPort.IsOpen)
            {
                ClosePort();
            }

            try
            {
                _serialPort.Open();
            }
            catch (Exception ex)
            {
                var sb = new StringBuilder();

                sb.AppendLine("Could not establish a connection with the dart board:");
                sb.AppendLine("\"" + ex.Message + "\"");
                sb.AppendLine();
                sb.AppendLine("Please make sure that the dart board is connected and");
                sb.AppendLine("that the correct settings have been configured in the options.");

                var mb = new MessageBoxScreen("Error", sb.ToString(), MessageBoxButtons.Ok);

                var messageBoxTitle = ((TextBlock) mb.StackPanel.Items[0]);
                messageBoxTitle.Color = Color.Red;

                XnaDartsGame.ScreenManager.AddScreen(mb);
            }
        }

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            var sp = (SerialPort) sender;
            var indata = sp.ReadLine();

            if (indata.StartsWith("B")) // Button messages are prefixed with "B:"
            {
                parseButton(indata);
            }
            else
            {
                parseScore(indata);
            }
        }

        private void parseButton(string indata)
        {
            // A button message is in the format B: X, where X is the index of the pressed button
            var temp = indata.Substring(2); // temp now holds X
            var buttonIndex = int.Parse(temp);

            _buttonStates[buttonIndex] = true;
        }

        private void parseScore(string indata)
        {
            // A dart hit is in the format H: X, Y, where X, Y is the coordinate of the hit segment
            var inCoordinates = indata.Substring(2).Split(',');

            if (inCoordinates.Length == 2)
            {
                var coords = new IntPair(int.Parse(inCoordinates[0]), int.Parse(inCoordinates[1]));

                if (OnDartHit != null)
                {
                    OnDartHit(coords);
                }

                // Check if the given coordinates are mapped to a segment
                if (XnaDartsGame.Options.SegmentMap.ContainsValue(coords))
                {
                    //Find the key (segment, multiplier) which contains the value of the received X, Y coordinates
                    var segmentInfo = XnaDartsGame.Options.SegmentMap.First(p => coords.Equals(p.Value)).Key;

                    if (segmentInfo != null && OnDartRegistered != null)
                    {
                        OnDartRegistered(segmentInfo.X, segmentInfo.Y);
                    }
                }
            }
        }

        public void ClosePort()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }
    }
}