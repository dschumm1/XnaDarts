using System;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class OptionsMenuScreen : MenuScreen
    {
        private DialMenuEntry _baudRate;
        private int _baudRateIndex;
        private bool _hasChangedSerialSettings;

        private readonly DialMenuEntry _awards = new DialMenuEntry(XnaDartsGame.Options.PlayAwards ? "Yes" : "No",
            "Play Awards:");

        private readonly MenuEntry _back = new MenuEntry("Back");

        private readonly int[] _baudRates =
        {
            300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 28800, 38400, 57600, 115200
        };

        private readonly DialMenuEntry _comPort = new DialMenuEntry("COM" + XnaDartsGame.Options.ComPort, "Serial Port:");
        private readonly MenuEntry _editSegmentMap = new MenuEntry("Edit Segment Mapping");

        private readonly DialMenuEntry _fullScreen =
            new DialMenuEntry(XnaDartsGame.Options.FullScreen ? "FullScreen" : "Windowed", "Screen Mode:");

        private readonly DialMenuEntry _playerChangeTimeout;

        private readonly DialMenuEntry _resolution =
            new DialMenuEntry(XnaDartsGame.Options.Resolutions[XnaDartsGame.Options.ResolutionIndex].ToString(),
                "Resolution:");

        private readonly DialMenuEntry _volume =
            new DialMenuEntry(((int) (Math.Round(XnaDartsGame.Options.Volume*100))) + "%", "Volume:");

        public OptionsMenuScreen() : base("Options")
        {
            _volume.OnMenuLeft += Volume_OnMenuLeft;
            _volume.OnMenuRight += Volume_OnMenuRight;
            _volume.OnSelected += Volume_OnMenuRight;
            _volume.OnCanceled += Volume_OnMenuLeft;

            _fullScreen.OnSelected += FullScreen_ValueChanged;
            _fullScreen.OnCanceled += FullScreen_ValueChanged;

            _back.OnSelected += Back_OnSelected;

            _playerChangeTimeout = new DialMenuEntry(PlayerChangeTimeoutText, "Player Change Timeout:");
            _playerChangeTimeout.OnMenuLeft += PlayerChangeTimout_OnMenuLeft;
            _playerChangeTimeout.OnMenuRight += PlayerChangeTimout_OnMenuRight;
            _playerChangeTimeout.OnSelected += PlayerChangeTimout_OnMenuRight;
            _playerChangeTimeout.OnCanceled += PlayerChangeTimout_OnMenuLeft;

            _editSegmentMap.OnSelected += EditSegmentMap_OnSelected;

            _comPort.OnMenuLeft += ComPort_OnMenuLeft;
            _comPort.OnMenuRight += ComPort_OnMenuRight;
            _comPort.OnSelected += ComPort_OnMenuRight;
            _comPort.OnCanceled += ComPort_OnMenuLeft;

            createBaudRateMenuEntry();

            _awards.OnSelected += Awards_OnSelected;
            _awards.OnMenuLeft += Awards_OnSelected;
            _awards.OnMenuRight += Awards_OnSelected;
            _awards.OnCanceled += Awards_OnSelected;

            _resolution.OnMenuRight += Resolution_OnMenuRight;
            _resolution.OnMenuLeft += Resolution_OnMenuLeft;
            _resolution.OnSelected += Resolution_OnMenuRight;
            _resolution.OnCanceled += Resolution_OnMenuLeft;

            MenuItems.AddItems(
                _resolution,
                _fullScreen,
                _volume,

                _awards,
                _playerChangeTimeout,

                _comPort,
                _baudRate,
                _editSegmentMap,
                _back
                );
        }

        public string PlayerChangeTimeoutText
        {
            get
            {
                var text = XnaDartsGame.Options.PlayerChangeTimeout + "s";

                if (XnaDartsGame.Options.PlayerChangeTimeout == 0)
                {
                    text = "Disabled";
                }

                return text;
            }
        }

        private void createBaudRateMenuEntry()
        {
            _baudRateIndex = Array.IndexOf(_baudRates, XnaDartsGame.Options.BaudRate);

            if (_baudRateIndex == -1)
            {
                _baudRateIndex = 0;
            }

            _baudRate = new DialMenuEntry(_baudRates[_baudRateIndex], "Baud Rate:");

            _baudRate.OnMenuLeft += BaudRate_OnMenuLeft;
            _baudRate.OnMenuRight += BaudRate_OnMenuRight;
            _baudRate.OnSelected += BaudRate_OnMenuRight;
            _baudRate.OnCanceled += BaudRate_OnMenuLeft;
        }

        private void BaudRate_OnMenuRight(object sender, EventArgs e)
        {
            _baudRateIndex++;

            updateBaudRate();
        }

        private void updateBaudRate()
        {
            if (_baudRateIndex < 0)
            {
                _baudRateIndex = _baudRates.Length - 1;
            }
            else if (_baudRateIndex >= _baudRates.Length)
            {
                _baudRateIndex = 0;
            }

            _hasChangedSerialSettings = true;
            _baudRate.Value = XnaDartsGame.Options.BaudRate = _baudRates[_baudRateIndex];
        }

        private void BaudRate_OnMenuLeft(object sender, EventArgs e)
        {
            _baudRateIndex--;

            updateBaudRate();
        }

        private void Awards_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.Options.PlayAwards = !XnaDartsGame.Options.PlayAwards;
            _awards.Value = XnaDartsGame.Options.PlayAwards ? "Yes" : "No";
        }

        private void updateResolution()
        {
            XnaDartsGame.GraphicsDeviceManager.PreferredBackBufferWidth =
                XnaDartsGame.Options.Resolutions[XnaDartsGame.Options.ResolutionIndex].Width;
            XnaDartsGame.GraphicsDeviceManager.PreferredBackBufferHeight =
                XnaDartsGame.Options.Resolutions[XnaDartsGame.Options.ResolutionIndex].Height;

            XnaDartsGame.GraphicsDeviceManager.ApplyChanges();

            _resolution.Value = XnaDartsGame.Options.Resolutions[XnaDartsGame.Options.ResolutionIndex].ToString();
        }

        private void Resolution_OnMenuLeft(object sender, EventArgs e)
        {
            XnaDartsGame.Options.ResolutionIndex--;
            if (XnaDartsGame.Options.ResolutionIndex < 0)
            {
                XnaDartsGame.Options.ResolutionIndex = XnaDartsGame.Options.Resolutions.Length - 1;
            }

            updateResolution();
        }

        private void Resolution_OnMenuRight(object sender, EventArgs e)
        {
            XnaDartsGame.Options.ResolutionIndex++;
            if (XnaDartsGame.Options.ResolutionIndex >= XnaDartsGame.Options.Resolutions.Length)
            {
                XnaDartsGame.Options.ResolutionIndex = 0;
            }

            updateResolution();
        }

        private void ComPort_OnMenuRight(object sender, EventArgs e)
        {
            updateComPort(+1);
        }

        private void ComPort_OnMenuLeft(object sender, EventArgs e)
        {
            updateComPort(-1);
        }

        private void updateComPort(int direction)
        {
            XnaDartsGame.Options.ComPort += direction;

            _hasChangedSerialSettings = true;

            if (XnaDartsGame.Options.ComPort > 10)
            {
                XnaDartsGame.Options.ComPort = 1;
            }

            if (XnaDartsGame.Options.ComPort < 1)
            {
                XnaDartsGame.Options.ComPort = 10;
            }

            _comPort.Value = "COM" + XnaDartsGame.Options.ComPort;
        }

        private void cancelScreen()
        {
            showOptionsSaveResult();

            if (_hasChangedSerialSettings)
            {
                SerialManager.Instance().UpdateSerialPortPropertiesFromOptions();
            }

            base.CancelScreen();
        }

        private void Back_OnSelected(object sender, EventArgs e)
        {
            cancelScreen();
        }

        private void showOptionsSaveResult()
        {
            if (XnaDartsGame.Options.Save())
            {
                var mb = new MessageBoxScreen("Options saved to file: " + Options.OptionsFilename + "!", string.Empty,
                    MessageBoxButtons.Ok);
                XnaDartsGame.ScreenManager.AddScreen(mb);
            }
            else
            {
                var mb = new MessageBoxScreen("Error", "Options could not be saved!", MessageBoxButtons.Ok);
                XnaDartsGame.ScreenManager.AddScreen(mb);
            }
        }

        public override void CancelScreen()
        {
            cancelScreen();
        }

        private void EditSegmentMap_OnSelected(object sender, EventArgs e)
        {
            XnaDartsGame.ScreenManager.AddScreen(new SegmentMapScreen("Edit Segment Map"));
        }

        private void PlayerChangeTimout_OnMenuRight(object sender, EventArgs e)
        {
            XnaDartsGame.Options.PlayerChangeTimeout += 1;

            if (XnaDartsGame.Options.PlayerChangeTimeout > 30)
            {
                XnaDartsGame.Options.PlayerChangeTimeout = 30;
            }

            _playerChangeTimeout.Value = PlayerChangeTimeoutText;
        }

        private void PlayerChangeTimout_OnMenuLeft(object sender, EventArgs e)
        {
            XnaDartsGame.Options.PlayerChangeTimeout -= 1;

            if (XnaDartsGame.Options.PlayerChangeTimeout < 0)
            {
                XnaDartsGame.Options.PlayerChangeTimeout = 0;
            }

            _playerChangeTimeout.Value = PlayerChangeTimeoutText;
        }

        private void FullScreen_ValueChanged(object sender, EventArgs e)
        {
            XnaDartsGame.GraphicsDeviceManager.ToggleFullScreen();

            XnaDartsGame.Options.FullScreen = XnaDartsGame.GraphicsDeviceManager.IsFullScreen;

            _fullScreen.Value = XnaDartsGame.Options.FullScreen ? "FullScreen" : "Windowed";
        }

        private void Volume_OnMenuRight(object sender, EventArgs e)

        {
            XnaDartsGame.Options.Volume += 0.10f;

            if (XnaDartsGame.Options.Volume > 1.0f)
            {
                XnaDartsGame.Options.Volume = 1.00f;
            }

            updateVolumeValue();
            XnaDartsGame.SoundManager.PlaySound(SoundCue.SingleBull);
        }

        private void updateVolumeValue()
        {
            _volume.Value = ((int) (Math.Round(XnaDartsGame.Options.Volume*100))) + "%";
        }

        private void Volume_OnMenuLeft(object sender, EventArgs e)
        {
            XnaDartsGame.Options.Volume -= 0.10f;

            if (XnaDartsGame.Options.Volume < 0)
            {
                XnaDartsGame.Options.Volume = 0.00f;
            }

            updateVolumeValue();
            XnaDartsGame.SoundManager.PlaySound(SoundCue.SingleBull);
        }
    }
}