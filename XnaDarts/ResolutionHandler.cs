using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XnaDarts
{
    internal static class ResolutionHandler
    {
        private static GraphicsDeviceManager _device;

        private static int _width = 800;
        private static int _height = 600;
        private static int _vWidth = 1024;
        private static int _vHeight = 768;
        private static Matrix _scaleMatrix;
        private static bool _fullScreen;
        private static bool _dirtyMatrix = true;
        public static int ViewportX { get; set; }
        public static int ViewportY { get; set; }

        public static int VWidth
        {
            get { return _vWidth; }
            set { _vWidth = value; }
        }

        public static int VHeight
        {
            get { return _vHeight; }
            set { _vHeight = value; }
        }

        public static void Init(ref GraphicsDeviceManager device)
        {
            _width = device.PreferredBackBufferWidth;
            _height = device.PreferredBackBufferHeight;
            _device = device;
            _dirtyMatrix = true;
            _applyResolutionSettings();
        }


        public static Matrix GetTransformationMatrix()
        {
            if (_dirtyMatrix) _recreateScaleMatrix();

            return _scaleMatrix;
        }

        public static void SetResolution(int width, int height, bool fullScreen)
        {
            _width = width;
            _height = height;

            _fullScreen = fullScreen;

            _applyResolutionSettings();
        }

        public static void SetVirtualResolution(int width, int height)
        {
            _vWidth = width;
            _vHeight = height;

            _dirtyMatrix = true;
        }

        private static void _applyResolutionSettings()
        {
#if XBOX360
           _FullScreen = true;
#endif

            // If we aren't using a full screen mode, the height and width of the window can
            // be set to anything equal to or smaller than the actual screen size.
            if (_fullScreen == false)
            {
                if (_width <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width
                    && _height <= GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                {
                    _device.PreferredBackBufferWidth = _width;
                    _device.PreferredBackBufferHeight = _height;
                    _device.IsFullScreen = _fullScreen;
                    _device.ApplyChanges();
                }
            }
            else
            {
                // If we are using full screen mode, we should check to make sure that the display
                // adapter can handle the video mode we are trying to set.  To do this, we will
                // iterate through the display modes supported by the adapter and check them against
                // the mode we want to set.
                foreach (var dm in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
                    if (dm.Width == _width && dm.Height == _height)
                    {
                        // The mode is supported, so set the buffer formats, apply changes and return
                        _device.PreferredBackBufferWidth = _width;
                        _device.PreferredBackBufferHeight = _height;
                        _device.IsFullScreen = _fullScreen;
                        _device.ApplyChanges();
                    }
            }

            _dirtyMatrix = true;

            _width = _device.PreferredBackBufferWidth;
            _height = _device.PreferredBackBufferHeight;
        }

        /// <summary>
        ///     Sets the device to use the draw pump
        ///     Sets correct aspect ratio
        /// </summary>
        public static void BeginDraw()
        {
            // Start by reseting viewport to (0,0,1,1)
            FullViewport();
            // Clear to Black
            _device.GraphicsDevice.Clear(Color.Black);
            // Calculate Proper Viewport according to Aspect Ratio
            ResetViewport();
            // and clear that
            // This way we are gonna have black bars if aspect ratio requires it and
            // the clear color on the rest
            _device.GraphicsDevice.Clear(Color.Black);
        }

        private static void _recreateScaleMatrix()
        {
            _dirtyMatrix = false;
            _scaleMatrix = Matrix.CreateScale(
                (float) _device.GraphicsDevice.Viewport.Width / _vWidth,
                (float) _device.GraphicsDevice.Viewport.Width / _vWidth,
                1f);
        }


        public static void FullViewport()
        {
            var vp = new Viewport();
            vp.X = vp.Y = 0;
            vp.Width = _width;
            vp.Height = _height;
            _device.GraphicsDevice.Viewport = vp;
        }

        /// <summary>
        ///     Get virtual Mode Aspect Ratio
        /// </summary>
        /// <returns>aspect ratio</returns>
        public static float GetVirtualAspectRatio()
        {
            return _vWidth / (float) _vHeight;
        }

        public static void ResetViewport()
        {
            var targetAspectRatio = GetVirtualAspectRatio();
            // figure out the largest area that fits in this resolution at the desired aspect ratio
            var width = _device.PreferredBackBufferWidth;
            var height = (int) (width / targetAspectRatio + .5f);
            var changed = false;

            if (height > _device.PreferredBackBufferHeight)
            {
                height = _device.PreferredBackBufferHeight;
                // PillarBox
                width = (int) (height * targetAspectRatio + .5f);
                changed = true;
            }

            // set up the new viewport centered in the backbuffer
            ViewportX = _device.PreferredBackBufferWidth / 2 - width / 2;
            ViewportY = _device.PreferredBackBufferHeight / 2 - height / 2;
            var viewport = new Viewport
            {
                X = ViewportX,
                Y = ViewportY,
                Width = width,
                Height = height,
                MinDepth = 0,
                MaxDepth = 1
            };


            if (changed)
                _dirtyMatrix = true;

            _device.GraphicsDevice.Viewport = viewport;
        }
    }
}