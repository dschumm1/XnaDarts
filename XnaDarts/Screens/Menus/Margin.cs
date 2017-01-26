namespace XnaDarts.Screens.Menus
{
    public class Margin
    {
        public int Bottom;
        public int Left;
        public int Right;
        public int Top;

        public Margin(int top, int right, int bottom, int left)
        {
            Top = top;
            Right = right;
            Bottom = bottom;
            Left = left;
        }

        public static Margin Zero
        {
            get { return new Margin(0, 0, 0, 0); }
        }
    }
}