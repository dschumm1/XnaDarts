using System;
using XnaDarts.ScreenManagement;

namespace XnaDarts.Screens.Menus
{
    public class BindSegmentScreen : MessageBoxScreen
    {
        public IntPair SegmentCoordinates;
        public IntPair SelectedSegment;
        private readonly MenuEntry _clear = new MenuEntry("Clear");

        public BindSegmentScreen(string text, IntPair selectedSegment)
            : base("Bind Segments", text, MessageBoxButtons.Cancel)
        {
            _clear.OnSelected += clear_OnSelected;

            SelectedSegment = selectedSegment;

            MenuItems.Items.Add(_clear);

            MenuItems.Items.Reverse(); //Want clear to end up at the top of the list so reverse

            SerialManager.Instance().OnDartRegistered = null;
            SerialManager.Instance().OnDartHit = handleDart;
        }

        public event EventHandler OnDartHit;
        public event EventHandler OnClear;

        private void clear_OnSelected(object sender, EventArgs e)
        {
            if (OnClear != null)
            {
                OnClear(this, null);
            }

            CancelScreen();
        }

        private void handleDart(IntPair coords)
        {
            SegmentCoordinates = coords;

            if (OnDartHit != null)
            {
                OnDartHit(this, null);
            }

            CancelScreen();
        }
    }
}